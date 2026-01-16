using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Interfaces;

namespace WinFormsApp1.second_menu.ChessRecognizer.Core.Services.OCR
{
    /// <summary>
    /// 简单OCR服务 - 基于模板匹配和颜色分析
    /// 当Tesseract不可用时的备选方案
    /// </summary>
    public class SimpleOCRService : IChessOCR
    {
        private bool _initialized = false;
        private double _confidenceThreshold = 0.5;
        private int _processedCount = 0;
        private double _totalProcessTime = 0;

        public string EngineName => "SimpleOCR";
        public bool IsInitialized => _initialized;

        public Task<bool> InitializeAsync()
        {
            _initialized = true;
            return Task.FromResult(true);
        }

        /// <summary>
        /// 识别棋子 - 基于颜色和形状特征
        /// </summary>
        public async Task<OcrResult> RecognizePieceAsync(Bitmap pieceImage)
        {
            return await Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    // 分析图像特征
                    var features = AnalyzeImageFeatures(pieceImage);

                    // 判断是否有棋子
                    if (!features.HasPiece)
                    {
                        return new OcrResult
                        {
                            Text = "",
                            Confidence = 0.9,
                            Success = true,
                            ProcessTime = sw.Elapsed
                        };
                    }

                    // 根据颜色判断红黑
                    string pieceText = features.IsRed ? "红" : "黑";

                    sw.Stop();
                    _processedCount++;
                    _totalProcessTime += sw.Elapsed.TotalMilliseconds;

                    return new OcrResult
                    {
                        Text = pieceText,
                        Confidence = features.Confidence,
                        Success = true,
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
        /// 分析图像特征
        /// </summary>
        private ImageFeatures AnalyzeImageFeatures(Bitmap image)
        {
            var features = new ImageFeatures();

            int width = image.Width;
            int height = image.Height;
            int totalPixels = width * height;

            double totalR = 0, totalG = 0, totalB = 0;
            double totalH = 0, totalS = 0, totalV = 0;
            int colorPixels = 0;

            var rect = new Rectangle(0, 0, width, height);
            var data = image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int offset = y * stride + x * 3;
                        byte b = ptr[offset];
                        byte g = ptr[offset + 1];
                        byte r = ptr[offset + 2];

                        totalR += r;
                        totalG += g;
                        totalB += b;

                        // 转换为HSV
                        RgbToHsv(r, g, b, out double h, out double s, out double v);

                        // 只统计有颜色的像素
                        if (s > 0.2 && v > 0.2)
                        {
                            totalH += h;
                            totalS += s;
                            totalV += v;
                            colorPixels++;
                        }
                    }
                }
            }

            image.UnlockBits(data);

            // 计算平均值
            features.AvgR = totalR / totalPixels;
            features.AvgG = totalG / totalPixels;
            features.AvgB = totalB / totalPixels;

            if (colorPixels > 0)
            {
                features.AvgH = totalH / colorPixels;
                features.AvgS = totalS / colorPixels;
                features.AvgV = totalV / colorPixels;
            }

            // 判断是否有棋子（基于颜色变化）
            double variance = CalculateVariance(image);
            features.HasPiece = variance > 500 && colorPixels > totalPixels * 0.1;

            // 判断红黑
            if (features.HasPiece)
            {
                // 红色判断：高饱和度 + 红色色相
                bool isRed = (features.AvgS > 0.3 && (features.AvgH < 30 || features.AvgH > 330)) ||
                             (features.AvgR > features.AvgB + 50 && features.AvgR > features.AvgG + 30);

                features.IsRed = isRed;
                features.Confidence = Math.Min(0.95, 0.6 + features.AvgS * 0.3);
            }

            return features;
        }

        /// <summary>
        /// RGB转HSV
        /// </summary>
        private void RgbToHsv(byte r, byte g, byte b, out double h, out double s, out double v)
        {
            double rd = r / 255.0;
            double gd = g / 255.0;
            double bd = b / 255.0;

            double max = Math.Max(rd, Math.Max(gd, bd));
            double min = Math.Min(rd, Math.Min(gd, bd));
            double delta = max - min;

            v = max;
            s = max == 0 ? 0 : delta / max;

            if (delta == 0)
            {
                h = 0;
            }
            else if (max == rd)
            {
                h = 60 * (((gd - bd) / delta) % 6);
            }
            else if (max == gd)
            {
                h = 60 * (((bd - rd) / delta) + 2);
            }
            else
            {
                h = 60 * (((rd - gd) / delta) + 4);
            }

            if (h < 0) h += 360;
        }

        /// <summary>
        /// 计算图像方差
        /// </summary>
        private double CalculateVariance(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            int totalPixels = width * height;

            double sum = 0;
            double sumSq = 0;

            var rect = new Rectangle(0, 0, width, height);
            var data = image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int offset = y * stride + x * 3;
                        double gray = (ptr[offset] + ptr[offset + 1] + ptr[offset + 2]) / 3.0;
                        sum += gray;
                        sumSq += gray * gray;
                    }
                }
            }

            image.UnlockBits(data);

            double mean = sum / totalPixels;
            return (sumSq / totalPixels) - (mean * mean);
        }

        public async Task<List<OcrResult>> RecognizePiecesAsync(List<Bitmap> pieceImages)
        {
            var results = new List<OcrResult>();
            foreach (var image in pieceImages)
            {
                results.Add(await RecognizePieceAsync(image));
            }
            return results;
        }

        public void SetConfidenceThreshold(double threshold)
        {
            _confidenceThreshold = Math.Clamp(threshold, 0.1, 0.99);
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

        public void Dispose() { }

        private class ImageFeatures
        {
            public double AvgR { get; set; }
            public double AvgG { get; set; }
            public double AvgB { get; set; }
            public double AvgH { get; set; }
            public double AvgS { get; set; }
            public double AvgV { get; set; }
            public bool HasPiece { get; set; }
            public bool IsRed { get; set; }
            public double Confidence { get; set; }
        }
    }
}
