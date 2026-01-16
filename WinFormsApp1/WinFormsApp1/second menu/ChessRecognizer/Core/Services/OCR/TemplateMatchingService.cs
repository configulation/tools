using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Interfaces;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Models;

namespace WinFormsApp1.second_menu.ChessRecognizer.Core.Services.OCR
{
    /// <summary>
    /// 基于模板匹配的棋子识别服务
    /// 通过颜色分析和形状特征识别棋子类型
    /// </summary>
    public class TemplateMatchingService : IChessOCR
    {
        private bool _initialized = false;
        private double _confidenceThreshold = 0.5;
        private int _processedCount = 0;
        private double _totalProcessTime = 0;

        // 棋子特征库 - 基于颜色和形状特征
        private readonly Dictionary<string, PieceFeature> _redPieceFeatures;
        private readonly Dictionary<string, PieceFeature> _blackPieceFeatures;

        public string EngineName => "TemplateMatching";
        public bool IsInitialized => _initialized;

        public TemplateMatchingService()
        {
            // 初始化红方棋子特征
            _redPieceFeatures = new Dictionary<string, PieceFeature>
            {
                ["帅"] = new PieceFeature { Name = "帅", Type = PieceType.King, IsRed = true },
                ["仕"] = new PieceFeature { Name = "仕", Type = PieceType.Advisor, IsRed = true },
                ["相"] = new PieceFeature { Name = "相", Type = PieceType.Elephant, IsRed = true },
                ["马"] = new PieceFeature { Name = "马", Type = PieceType.Knight, IsRed = true },
                ["车"] = new PieceFeature { Name = "车", Type = PieceType.Rook, IsRed = true },
                ["炮"] = new PieceFeature { Name = "炮", Type = PieceType.Cannon, IsRed = true },
                ["兵"] = new PieceFeature { Name = "兵", Type = PieceType.Pawn, IsRed = true },
            };

            // 初始化黑方棋子特征
            _blackPieceFeatures = new Dictionary<string, PieceFeature>
            {
                ["将"] = new PieceFeature { Name = "将", Type = PieceType.King, IsRed = false },
                ["士"] = new PieceFeature { Name = "士", Type = PieceType.Advisor, IsRed = false },
                ["象"] = new PieceFeature { Name = "象", Type = PieceType.Elephant, IsRed = false },
                ["马"] = new PieceFeature { Name = "马", Type = PieceType.Knight, IsRed = false },
                ["车"] = new PieceFeature { Name = "车", Type = PieceType.Rook, IsRed = false },
                ["砲"] = new PieceFeature { Name = "砲", Type = PieceType.Cannon, IsRed = false },
                ["卒"] = new PieceFeature { Name = "卒", Type = PieceType.Pawn, IsRed = false },
            };
        }

        public Task<bool> InitializeAsync()
        {
            _initialized = true;
            return Task.FromResult(true);
        }

        /// <summary>
        /// 识别棋子 - 基于颜色和形状特征分析
        /// </summary>
        public async Task<OcrResult> RecognizePieceAsync(Bitmap pieceImage)
        {
            return await Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    // 分析图像特征
                    var analysis = AnalyzePieceImage(pieceImage);

                    if (!analysis.HasPiece)
                    {
                        return new OcrResult
                        {
                            Text = "",
                            Confidence = 0.9,
                            Success = true,
                            ProcessTime = sw.Elapsed
                        };
                    }

                    // 根据颜色和特征识别棋子
                    var (pieceName, confidence) = IdentifyPieceByFeatures(analysis);

                    sw.Stop();
                    _processedCount++;
                    _totalProcessTime += sw.Elapsed.TotalMilliseconds;

                    return new OcrResult
                    {
                        Text = pieceName,
                        Confidence = confidence,
                        Success = !string.IsNullOrEmpty(pieceName) && confidence >= _confidenceThreshold,
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
        /// 分析棋子图像特征
        /// </summary>
        private PieceAnalysis AnalyzePieceImage(Bitmap image)
        {
            var analysis = new PieceAnalysis();

            int width = image.Width;
            int height = image.Height;
            int totalPixels = width * height;

            // 颜色统计
            double totalR = 0, totalG = 0, totalB = 0;
            double totalH = 0, totalS = 0, totalV = 0;
            int colorPixels = 0;
            int redPixels = 0;
            int blackPixels = 0;
            int whitePixels = 0;

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

                        // 统计颜色像素
                        if (s > 0.2 && v > 0.2)
                        {
                            totalH += h;
                            totalS += s;
                            totalV += v;
                            colorPixels++;

                            // 红色判断
                            if ((h < 20 || h > 340) && s > 0.4)
                            {
                                redPixels++;
                            }
                            // 黑色/深色判断
                            else if (v < 0.4 || (s < 0.2 && v < 0.5))
                            {
                                blackPixels++;
                            }
                        }

                        // 白色/浅色判断
                        if (v > 0.8 && s < 0.2)
                        {
                            whitePixels++;
                        }
                    }
                }
            }

            image.UnlockBits(data);

            // 计算平均值
            analysis.AvgR = totalR / totalPixels;
            analysis.AvgG = totalG / totalPixels;
            analysis.AvgB = totalB / totalPixels;

            if (colorPixels > 0)
            {
                analysis.AvgH = totalH / colorPixels;
                analysis.AvgS = totalS / colorPixels;
                analysis.AvgV = totalV / colorPixels;
            }

            analysis.RedPixelRatio = (double)redPixels / totalPixels;
            analysis.BlackPixelRatio = (double)blackPixels / totalPixels;
            analysis.WhitePixelRatio = (double)whitePixels / totalPixels;
            analysis.ColorPixelRatio = (double)colorPixels / totalPixels;

            // 计算图像方差（用于判断是否有棋子）
            analysis.Variance = CalculateVariance(image);

            // 判断是否有棋子
            analysis.HasPiece = analysis.Variance > 400 && 
                               (analysis.RedPixelRatio > 0.05 || analysis.BlackPixelRatio > 0.05 || 
                                analysis.ColorPixelRatio > 0.15);

            // 判断红黑
            if (analysis.HasPiece)
            {
                // 红色棋子特征：红色像素比例高，或者平均色相在红色范围
                analysis.IsRed = analysis.RedPixelRatio > 0.08 ||
                                (analysis.AvgS > 0.3 && (analysis.AvgH < 30 || analysis.AvgH > 330)) ||
                                (analysis.AvgR > analysis.AvgB + 40 && analysis.AvgR > analysis.AvgG + 20);

                analysis.ColorConfidence = Math.Min(0.95, 0.5 + Math.Max(analysis.RedPixelRatio, analysis.BlackPixelRatio) * 2);
            }

            // 分析形状特征（用于区分不同棋子）
            analysis.ShapeFeatures = AnalyzeShapeFeatures(image);

            return analysis;
        }

        /// <summary>
        /// 分析形状特征
        /// </summary>
        private ShapeFeatures AnalyzeShapeFeatures(Bitmap image)
        {
            var features = new ShapeFeatures();

            try
            {
                using var mat = BitmapConverter.ToMat(image);
                using var gray = new Mat();
                using var binary = new Mat();
                using var edges = new Mat();

                // 转灰度
                Cv2.CvtColor(mat, gray, ColorConversionCodes.BGR2GRAY);

                // 二值化
                Cv2.Threshold(gray, binary, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                // 边缘检测
                Cv2.Canny(gray, edges, 50, 150);

                // 计算边缘密度
                int edgePixels = Cv2.CountNonZero(edges);
                features.EdgeDensity = (double)edgePixels / (image.Width * image.Height);

                // 计算轮廓
                Cv2.FindContours(binary, out var contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                if (contours.Length > 0)
                {
                    // 找最大轮廓
                    var maxContour = contours.OrderByDescending(c => Cv2.ContourArea(c)).First();
                    features.ContourArea = Cv2.ContourArea(maxContour);
                    features.ContourPerimeter = Cv2.ArcLength(maxContour, true);

                    // 计算圆度
                    if (features.ContourPerimeter > 0)
                    {
                        features.Circularity = 4 * Math.PI * features.ContourArea / 
                                              (features.ContourPerimeter * features.ContourPerimeter);
                    }

                    // 计算凸包
                    var hull = Cv2.ConvexHull(maxContour);
                    features.ConvexHullArea = Cv2.ContourArea(hull);

                    // 凸度
                    if (features.ConvexHullArea > 0)
                    {
                        features.Convexity = features.ContourArea / features.ConvexHullArea;
                    }
                }

                // 计算水平和垂直投影
                features.HorizontalProjection = CalculateProjection(binary, true);
                features.VerticalProjection = CalculateProjection(binary, false);
            }
            catch
            {
                // 形状分析失败，使用默认值
            }

            return features;
        }

        /// <summary>
        /// 计算投影
        /// </summary>
        private double[] CalculateProjection(Mat binary, bool horizontal)
        {
            int size = horizontal ? binary.Rows : binary.Cols;
            var projection = new double[size];

            for (int i = 0; i < size; i++)
            {
                int count = 0;
                if (horizontal)
                {
                    for (int j = 0; j < binary.Cols; j++)
                    {
                        if (binary.At<byte>(i, j) > 0) count++;
                    }
                }
                else
                {
                    for (int j = 0; j < binary.Rows; j++)
                    {
                        if (binary.At<byte>(j, i) > 0) count++;
                    }
                }
                projection[i] = count;
            }

            return projection;
        }

        /// <summary>
        /// 根据特征识别棋子
        /// </summary>
        private (string name, double confidence) IdentifyPieceByFeatures(PieceAnalysis analysis)
        {
            if (!analysis.HasPiece)
            {
                return ("", 0.9);
            }

            // 选择对应颜色的特征库
            var features = analysis.IsRed ? _redPieceFeatures : _blackPieceFeatures;

            // 基于形状特征进行匹配
            // 由于没有预训练的模板，我们使用启发式规则

            // 边缘密度可以区分不同复杂度的汉字
            double edgeDensity = analysis.ShapeFeatures.EdgeDensity;
            double circularity = analysis.ShapeFeatures.Circularity;

            string bestMatch;
            double confidence;

            // 根据边缘密度和其他特征进行粗略分类
            // 这是一个简化的启发式方法
            if (edgeDensity > 0.15)
            {
                // 复杂字形 - 可能是 将/帅、象/相
                if (analysis.IsRed)
                {
                    bestMatch = "帅";
                }
                else
                {
                    bestMatch = "将";
                }
                confidence = 0.5;
            }
            else if (edgeDensity > 0.10)
            {
                // 中等复杂度 - 可能是 马、炮/砲
                if (analysis.IsRed)
                {
                    bestMatch = "炮";
                }
                else
                {
                    bestMatch = "砲";
                }
                confidence = 0.5;
            }
            else
            {
                // 简单字形 - 可能是 兵/卒、车、士/仕
                if (analysis.IsRed)
                {
                    bestMatch = "兵";
                }
                else
                {
                    bestMatch = "卒";
                }
                confidence = 0.5;
            }

            // 由于没有真正的模板匹配，置信度较低
            // 返回颜色标记，让上层知道这是基于颜色的猜测
            return ($"[{(analysis.IsRed ? "RED" : "BLACK")}:{bestMatch}]", confidence);
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
            double sum = 0, sumSq = 0;
            int count = 0;

            var rect = new Rectangle(0, 0, image.Width, image.Height);
            var data = image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;

                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        int offset = y * stride + x * 3;
                        double gray = (ptr[offset] + ptr[offset + 1] + ptr[offset + 2]) / 3.0;
                        sum += gray;
                        sumSq += gray * gray;
                        count++;
                    }
                }
            }

            image.UnlockBits(data);

            if (count == 0) return 0;
            double mean = sum / count;
            return (sumSq / count) - (mean * mean);
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
    }

    /// <summary>
    /// 棋子分析结果
    /// </summary>
    internal class PieceAnalysis
    {
        public double AvgR { get; set; }
        public double AvgG { get; set; }
        public double AvgB { get; set; }
        public double AvgH { get; set; }
        public double AvgS { get; set; }
        public double AvgV { get; set; }
        public double RedPixelRatio { get; set; }
        public double BlackPixelRatio { get; set; }
        public double WhitePixelRatio { get; set; }
        public double ColorPixelRatio { get; set; }
        public double Variance { get; set; }
        public bool HasPiece { get; set; }
        public bool IsRed { get; set; }
        public double ColorConfidence { get; set; }
        public ShapeFeatures ShapeFeatures { get; set; } = new();
    }

    /// <summary>
    /// 形状特征
    /// </summary>
    internal class ShapeFeatures
    {
        public double EdgeDensity { get; set; }
        public double ContourArea { get; set; }
        public double ContourPerimeter { get; set; }
        public double Circularity { get; set; }
        public double ConvexHullArea { get; set; }
        public double Convexity { get; set; }
        public double[] HorizontalProjection { get; set; } = Array.Empty<double>();
        public double[] VerticalProjection { get; set; } = Array.Empty<double>();
    }

    /// <summary>
    /// 棋子特征
    /// </summary>
    internal class PieceFeature
    {
        public string Name { get; set; } = "";
        public PieceType Type { get; set; }
        public bool IsRed { get; set; }
    }
}
