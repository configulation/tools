using System.Drawing;
using System.Drawing.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Interfaces;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Models;

namespace WinFormsApp1.second_menu.ChessRecognizer.Core.Services.Detection
{
    /// <summary>
    /// 棋子检测服务
    /// </summary>
    public class PieceDetector
    {
        // 象棋汉字到棋子类型的映射
        private static readonly Dictionary<string, (PieceType type, bool isRed)> ChineseMapping = new()
        {
            // 红方
            ["帅"] = (PieceType.King, true),
            ["仕"] = (PieceType.Advisor, true),
            ["相"] = (PieceType.Elephant, true),
            ["馬"] = (PieceType.Knight, true),
            ["马"] = (PieceType.Knight, true),
            ["車"] = (PieceType.Rook, true),
            ["车"] = (PieceType.Rook, true),
            ["炮"] = (PieceType.Cannon, true),
            ["兵"] = (PieceType.Pawn, true),

            // 黑方
            ["将"] = (PieceType.King, false),
            ["將"] = (PieceType.King, false),
            ["士"] = (PieceType.Advisor, false),
            ["象"] = (PieceType.Elephant, false),
            ["俥"] = (PieceType.Rook, false),
            ["砲"] = (PieceType.Cannon, false),
            ["卒"] = (PieceType.Pawn, false),
        };

        /// <summary>
        /// 从OCR文本解析棋子
        /// </summary>
        public ChessPieceModel ParsePieceFromText(string text, int col, int row, PieceColorType colorHint)
        {
            var piece = new ChessPieceModel
            {
                Column = col,
                Row = row,
                RecognizedText = text
            };

            if (string.IsNullOrWhiteSpace(text))
            {
                piece.Type = PieceType.Empty;
                return piece;
            }

            // 清理文本
            text = text.Trim();

            // 尝试直接匹配
            if (ChineseMapping.TryGetValue(text, out var mapping))
            {
                piece.Type = mapping.type;
                piece.IsRed = mapping.isRed;
                piece.Confidence = 0.95;
                return piece;
            }

            // 尝试模糊匹配
            var fuzzyResult = FuzzyMatchPiece(text);
            if (fuzzyResult.HasValue)
            {
                piece.Type = fuzzyResult.Value.type;
                piece.IsRed = fuzzyResult.Value.isRed;
                piece.Confidence = fuzzyResult.Value.confidence;
                return piece;
            }

            // 如果有颜色提示，尝试根据位置推测
            if (colorHint != PieceColorType.Unknown)
            {
                piece.IsRed = colorHint == PieceColorType.Red;
                piece.Type = GuessPieceTypeByPosition(col, row, piece.IsRed);
                piece.Confidence = 0.5;
                return piece;
            }

            piece.Type = PieceType.Empty;
            return piece;
        }

        /// <summary>
        /// 模糊匹配棋子
        /// </summary>
        private (PieceType type, bool isRed, double confidence)? FuzzyMatchPiece(string text)
        {
            // 相似字符映射
            var similarChars = new Dictionary<char, char[]>
            {
                ['帅'] = new[] { '帥', '師' },
                ['将'] = new[] { '將', '将' },
                ['仕'] = new[] { '士', '仁' },
                ['士'] = new[] { '仕', '土' },
                ['相'] = new[] { '象', '想' },
                ['象'] = new[] { '相', '像' },
                ['马'] = new[] { '馬', '码' },
                ['馬'] = new[] { '马', '瑪' },
                ['车'] = new[] { '車', '东' },
                ['車'] = new[] { '车', '軍' },
                ['炮'] = new[] { '砲', '泡' },
                ['砲'] = new[] { '炮', '跑' },
                ['兵'] = new[] { '乒', '丘' },
                ['卒'] = new[] { '率', '萃' },
            };

            foreach (char c in text)
            {
                // 直接匹配
                string s = c.ToString();
                if (ChineseMapping.TryGetValue(s, out var direct))
                {
                    return (direct.type, direct.isRed, 0.9);
                }

                // 相似字符匹配
                foreach (var kvp in similarChars)
                {
                    if (kvp.Value.Contains(c))
                    {
                        string key = kvp.Key.ToString();
                        if (ChineseMapping.TryGetValue(key, out var similar))
                        {
                            return (similar.type, similar.isRed, 0.7);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 根据位置推测棋子类型
        /// </summary>
        private PieceType GuessPieceTypeByPosition(int col, int row, bool isRed)
        {
            // 红方在下方（row 5-9），黑方在上方（row 0-4）
            if (isRed)
            {
                if (row == 9)
                {
                    if (col == 4) return PieceType.King;
                    if (col == 3 || col == 5) return PieceType.Advisor;
                    if (col == 2 || col == 6) return PieceType.Elephant;
                    if (col == 1 || col == 7) return PieceType.Knight;
                    if (col == 0 || col == 8) return PieceType.Rook;
                }
                else if (row == 7 && (col == 1 || col == 7))
                {
                    return PieceType.Cannon;
                }
                else if (row == 6 && col % 2 == 0)
                {
                    return PieceType.Pawn;
                }
            }
            else
            {
                if (row == 0)
                {
                    if (col == 4) return PieceType.King;
                    if (col == 3 || col == 5) return PieceType.Advisor;
                    if (col == 2 || col == 6) return PieceType.Elephant;
                    if (col == 1 || col == 7) return PieceType.Knight;
                    if (col == 0 || col == 8) return PieceType.Rook;
                }
                else if (row == 2 && (col == 1 || col == 7))
                {
                    return PieceType.Cannon;
                }
                else if (row == 3 && col % 2 == 0)
                {
                    return PieceType.Pawn;
                }
            }

            return PieceType.Pawn; // 默认返回兵/卒
        }

        /// <summary>
        /// 分析棋子颜色
        /// </summary>
        public ColorAnalysisResult AnalyzeColor(Bitmap image)
        {
            var result = new ColorAnalysisResult();

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

                        // 转HSV
                        RgbToHsv(r, g, b, out double h, out double s, out double v);

                        if (s > 0.15 && v > 0.15)
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

            result.AvgR = totalR / totalPixels;
            result.AvgG = totalG / totalPixels;
            result.AvgB = totalB / totalPixels;

            if (colorPixels > 0)
            {
                result.AvgH = totalH / colorPixels;
                result.AvgS = totalS / colorPixels;
                result.AvgV = totalV / colorPixels;
            }

            // 判断红黑
            // 红色：高饱和度 + 红色色相（0-30 或 330-360）
            bool isRed = (result.AvgS > 0.25 && (result.AvgH < 35 || result.AvgH > 325)) ||
                         (result.AvgR > result.AvgB + 45 && result.AvgR > result.AvgG + 25 && result.AvgR > 100);

            result.IsRed = isRed;
            result.ColorType = isRed ? PieceColorType.Red : PieceColorType.Black;
            result.Confidence = Math.Min(0.95, 0.5 + result.AvgS * 0.4);

            return result;
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
        /// 检测是否有棋子
        /// </summary>
        public bool HasPiece(Bitmap image, double threshold = 500)
        {
            double variance = CalculateVariance(image);
            return variance > threshold;
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
    }

    /// <summary>
    /// 颜色分析结果
    /// </summary>
    public class ColorAnalysisResult
    {
        public double AvgR { get; set; }
        public double AvgG { get; set; }
        public double AvgB { get; set; }
        public double AvgH { get; set; }
        public double AvgS { get; set; }
        public double AvgV { get; set; }
        public bool IsRed { get; set; }
        public PieceColorType ColorType { get; set; }
        public double Confidence { get; set; }
    }
}
