using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using Tesseract;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Interfaces;

namespace WinFormsApp1.second_menu.ChessRecognizer.Core.Services.OCR
{
    /// <summary>
    /// Tesseract OCR服务实现
    /// </summary>
    public class TesseractOCRService : IChessOCR
    {
        private TesseractEngine _engine;
        private bool _disposed = false;
        private double _confidenceThreshold = 0.5;
        private int _processedCount = 0;
        private double _totalProcessTime = 0;

        // 象棋汉字白名单
        private static readonly string ChessCharWhitelist = "帅仕相馬車炮兵将士象马车砲卒";

        public string EngineName => "Tesseract";
        public bool IsInitialized => _engine != null;

        /// <summary>
        /// 初始化Tesseract引擎
        /// </summary>
        public async Task<bool> InitializeAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    // 查找tessdata目录
                    string tessDataPath = FindTessDataPath();
                    if (string.IsNullOrEmpty(tessDataPath))
                    {
                        throw new Exception("未找到tessdata目录，请确保已安装Tesseract语言包");
                    }

                    // 初始化引擎，使用中文简体
                    _engine = new TesseractEngine(tessDataPath, "chi_sim", EngineMode.Default);

                    // 设置为单字符识别模式
                    _engine.SetVariable("tessedit_char_whitelist", ChessCharWhitelist);
                    _engine.SetVariable("tessedit_pageseg_mode", "10"); // 单字符模式

                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Tesseract初始化失败: {ex.Message}");
                    return false;
                }
            });
        }

        /// <summary>
        /// 查找tessdata目录
        /// </summary>
        private string FindTessDataPath()
        {
            var possiblePaths = new[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "x64", "tessdata"),
                @"C:\Program Files\Tesseract-OCR\tessdata",
                @"C:\Tesseract-OCR\tessdata",
                Environment.GetEnvironmentVariable("TESSDATA_PREFIX") ?? ""
            };

            foreach (var path in possiblePaths)
            {
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    // 检查是否有中文语言包
                    if (File.Exists(Path.Combine(path, "chi_sim.traineddata")))
                    {
                        return path;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 识别单个棋子
        /// </summary>
        public async Task<OcrResult> RecognizePieceAsync(Bitmap pieceImage)
        {
            if (!IsInitialized)
            {
                return new OcrResult
                {
                    Success = false,
                    ErrorMessage = "OCR引擎未初始化"
                };
            }

            return await Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    // 预处理图像
                    using var processedImage = PreprocessImage(pieceImage);

                    // 转换为Pix格式
                    using var pix = Tesseract.Pix.LoadFromMemory(ImageToBytes(processedImage));

                    // 执行OCR
                    using var page = _engine.Process(pix, PageSegMode.SingleChar);

                    string text = page.GetText().Trim();
                    float confidence = page.GetMeanConfidence();

                    sw.Stop();
                    _processedCount++;
                    _totalProcessTime += sw.Elapsed.TotalMilliseconds;

                    // 清理识别结果
                    text = CleanRecognizedText(text);

                    return new OcrResult
                    {
                        Text = text,
                        Confidence = confidence,
                        Success = !string.IsNullOrEmpty(text) && confidence >= _confidenceThreshold,
                        ProcessTime = sw.Elapsed
                    };
                }
                catch (Exception ex)
                {
                    sw.Stop();
                    return new OcrResult
                    {
                        Success = false,
                        ErrorMessage = ex.Message,
                        ProcessTime = sw.Elapsed
                    };
                }
            });
        }

        /// <summary>
        /// 批量识别
        /// </summary>
        public async Task<List<OcrResult>> RecognizePiecesAsync(List<Bitmap> pieceImages)
        {
            var results = new List<OcrResult>();
            foreach (var image in pieceImages)
            {
                var result = await RecognizePieceAsync(image);
                results.Add(result);
            }
            return results;
        }

        /// <summary>
        /// 图像预处理
        /// </summary>
        private Bitmap PreprocessImage(Bitmap source)
        {
            // 创建灰度图
            var result = new Bitmap(source.Width, source.Height, PixelFormat.Format24bppRgb);

            using (var g = Graphics.FromImage(result))
            {
                // 使用灰度矩阵
                var colorMatrix = new ColorMatrix(new float[][]
                {
                    new float[] {0.299f, 0.299f, 0.299f, 0, 0},
                    new float[] {0.587f, 0.587f, 0.587f, 0, 0},
                    new float[] {0.114f, 0.114f, 0.114f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });

                using var attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);

                g.DrawImage(source,
                    new Rectangle(0, 0, source.Width, source.Height),
                    0, 0, source.Width, source.Height,
                    GraphicsUnit.Pixel, attributes);
            }

            // 二值化处理
            BinarizeImage(result);

            return result;
        }

        /// <summary>
        /// 二值化处理
        /// </summary>
        private void BinarizeImage(Bitmap image)
        {
            var rect = new Rectangle(0, 0, image.Width, image.Height);
            var data = image.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;

                // 计算Otsu阈值
                int[] histogram = new int[256];
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        int offset = y * stride + x * 3;
                        histogram[ptr[offset]]++;
                    }
                }

                int threshold = CalculateOtsuThreshold(histogram, image.Width * image.Height);

                // 应用阈值
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        int offset = y * stride + x * 3;
                        byte value = ptr[offset] > threshold ? (byte)255 : (byte)0;
                        ptr[offset] = ptr[offset + 1] = ptr[offset + 2] = value;
                    }
                }
            }

            image.UnlockBits(data);
        }

        /// <summary>
        /// 计算Otsu阈值
        /// </summary>
        private int CalculateOtsuThreshold(int[] histogram, int totalPixels)
        {
            int sum = 0;
            for (int i = 0; i < 256; i++)
                sum += i * histogram[i];

            int sumB = 0;
            int wB = 0;
            int wF;
            double maxVariance = 0;
            int threshold = 0;

            for (int t = 0; t < 256; t++)
            {
                wB += histogram[t];
                if (wB == 0) continue;

                wF = totalPixels - wB;
                if (wF == 0) break;

                sumB += t * histogram[t];

                double mB = (double)sumB / wB;
                double mF = (double)(sum - sumB) / wF;

                double variance = (double)wB * wF * (mB - mF) * (mB - mF);

                if (variance > maxVariance)
                {
                    maxVariance = variance;
                    threshold = t;
                }
            }

            return threshold;
        }

        /// <summary>
        /// 清理识别文本
        /// </summary>
        private string CleanRecognizedText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // 只保留象棋汉字
            var result = new System.Text.StringBuilder();
            foreach (char c in text)
            {
                if (ChessCharWhitelist.Contains(c))
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        public void SetConfidenceThreshold(double threshold)
        {
            _confidenceThreshold = Math.Clamp(threshold, 0.1, 0.99);
        }

        /// <summary>
        /// 将Bitmap转换为字节数组
        /// </summary>
        private byte[] ImageToBytes(Bitmap image)
        {
            using var ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        public OcrEngineStatus GetStatus()
        {
            return new OcrEngineStatus
            {
                EngineName = EngineName,
                IsReady = IsInitialized,
                ProcessedCount = _processedCount,
                AverageProcessTime = _processedCount > 0 ? _totalProcessTime / _processedCount : 0,
                MemoryUsage = GC.GetTotalMemory(false)
            };
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _engine?.Dispose();
                _disposed = true;
            }
        }
    }
}
