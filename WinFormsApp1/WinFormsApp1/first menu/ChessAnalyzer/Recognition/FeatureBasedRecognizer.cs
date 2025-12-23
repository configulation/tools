using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WinFormsApp1.first_menu.ChessAnalyzer.Core;

namespace WinFormsApp1.first_menu.ChessAnalyzer.Recognition
{
    /// <summary>
    /// 基于特征分析的识别器 - 最靠谱的无依赖方案
    /// </summary>
    public class FeatureBasedRecognizer : IDisposable
    {
        private bool _disposed = false;

        public RecognitionResult RecognizeToBoard(Bitmap screenshot)
        {
            var result = new RecognitionResult();

            try
            {
                using Mat image = BitmapConverter.ToMat(screenshot);

                int width = image.Width;
                int height = image.Height;

                // 关键修正：棋子在交叉点上
                double cellWidth = width / 8.0;   
                double cellHeight = height / 9.0; 

                var board = new ChessBoard();
                var debugInfo = new List<string>();
                int totalPieces = 0;

                // 遍历每个交叉点
                for (int row = 0; row < 10; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        var piece = AnalyzePosition(image, col, row, cellWidth, cellHeight, debugInfo);
                        board.SetPiece(col, row, piece);
                        
                        if (piece != ChessPiece.Empty)
                            totalPieces++;
                    }
                }

                result.FEN = board.ToFEN();
                result.Success = true;
                result.Message = $"识别到 {totalPieces} 个棋子";
                result.DebugInfo = string.Join("\n", debugInfo);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"错误: {ex.Message}";
            }

            return result;
        }

        private ChessPiece AnalyzePosition(Mat image, int col, int row, double cellWidth, double cellHeight, List<string> debugInfo)
        {
            // 交叉点位置
            int centerX = (int)(col * cellWidth);
            int centerY = (int)(row * cellHeight);

            // 提取区域（80%覆盖整个棋子）
            int size = (int)Math.Min(cellWidth * 0.9, cellHeight * 0.9);
            int rectX = Math.Max(0, Math.Min(centerX - size / 2, image.Width - size));
            int rectY = Math.Max(0, Math.Min(centerY - size / 2, image.Height - size));
            
            if (rectX + size > image.Width) size = image.Width - rectX;
            if (rectY + size > image.Height) size = image.Height - rectY;
            
            if (size <= 10) return ChessPiece.Empty;

            var rect = new Rect(rectX, rectY, size, size);
            using Mat cell = new Mat(image, rect);

            // 多重特征分析
            var features = ExtractFeatures(cell);

            // 判断是否有棋子
            if (!features.HasPiece)
            {
                return ChessPiece.Empty;
            }

            // 判断颜色
            bool isRed = features.IsRed;

            // 根据位置和特征推测类型
            ChessPiece piece = AdvancedGuessPiece(col, row, isRed, features);

            string info = $"[{col},{row}] ";
            info += $"圆度:{features.Circularity:F2} ";
            info += $"密度:{features.Density:F2} ";
            info += $"颜色:{(isRed ? "红" : "黑")} ";
            info += $"=> {ChessPieceHelper.GetPieceText(piece)}";
            
            debugInfo.Add(info);

            return piece;
        }

        /// <summary>
        /// 提取图像特征
        /// </summary>
        private PieceFeatures ExtractFeatures(Mat cell)
        {
            var features = new PieceFeatures();

            // 1. 转灰度
            using Mat gray = new Mat();
            Cv2.CvtColor(cell, gray, ColorConversionCodes.BGR2GRAY);

            // 2. 计算统计信息
            Cv2.MeanStdDev(gray, out Scalar mean, out Scalar stdDev);
            features.Brightness = mean[0];
            features.Contrast = stdDev[0];

            // 3. 检测是否有棋子（对比度判断）
            if (features.Contrast < 20)
            {
                features.HasPiece = false;
                return features;
            }

            features.HasPiece = true;

            // 4. 边缘检测
            using Mat edges = new Mat();
            Cv2.Canny(gray, edges, 50, 150);

            // 5. 轮廓分析
            Cv2.FindContours(edges, out OpenCvSharp.Point[][] contours, out _,
                RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            if (contours.Length > 0)
            {
                // 找最大轮廓
                var maxContour = contours.OrderByDescending(c => Cv2.ContourArea(c)).First();
                double area = Cv2.ContourArea(maxContour);
                double perimeter = Cv2.ArcLength(maxContour, true);

                // 圆度 = 4π * 面积 / 周长²
                if (perimeter > 0)
                {
                    features.Circularity = (4 * Math.PI * area) / (perimeter * perimeter);
                }

                // 密度 = 轮廓面积 / 总面积
                features.Density = area / (cell.Width * cell.Height);
            }

            // 6. 颜色分析
            using Mat hsv = new Mat();
            Cv2.CvtColor(cell, hsv, ColorConversionCodes.BGR2HSV);
            Scalar meanHsv = Cv2.Mean(hsv);
            Scalar meanBgr = Cv2.Mean(cell);

            double hue = meanHsv[0];
            double saturation = meanHsv[1];
            double value = meanHsv[2];

            double b = meanBgr[0];
            double g = meanBgr[1];
            double r = meanBgr[2];

            // 红色判断（多条件）
            features.IsRed = (saturation > 60 && (hue < 15 || hue > 165)) ||
                             (r > b + 40 && r > g + 30 && r > 100) ||
                             (r > 150 && r > b * 1.5 && r > g * 1.3);

            // 保存颜色信息
            features.Hue = hue;
            features.Saturation = saturation;
            features.RedValue = r;

            return features;
        }

        /// <summary>
        /// 高级推测算法
        /// </summary>
        private ChessPiece AdvancedGuessPiece(int col, int row, bool isRed, PieceFeatures features)
        {
            if (isRed)
            {
                // 红方开局位置
                if (row == 9)
                {
                    if (col == 4) return ChessPiece.RedKing;
                    if (col == 3 || col == 5) return ChessPiece.RedAdvisor;
                    if (col == 2 || col == 6) return ChessPiece.RedElephant;
                    if (col == 1 || col == 7) return ChessPiece.RedKnight;
                    if (col == 0 || col == 8) return ChessPiece.RedRook;
                }
                else if (row == 7)
                {
                    if (col == 1 || col == 7) return ChessPiece.RedCannon;
                }
                else if (row == 6)
                {
                    if (col % 2 == 0) return ChessPiece.RedPawn;
                }

                // 根据形状特征推测
                if (features.Circularity > 0.7)
                {
                    return ChessPiece.RedCannon;  // 炮通常比较圆
                }
                else if (features.Density > 0.3)
                {
                    return ChessPiece.RedRook;    // 车密度较高
                }
                
                return ChessPiece.RedPawn;
            }
            else
            {
                // 黑方开局位置
                if (row == 0)
                {
                    if (col == 4) return ChessPiece.BlackKing;
                    if (col == 3 || col == 5) return ChessPiece.BlackAdvisor;
                    if (col == 2 || col == 6) return ChessPiece.BlackElephant;
                    if (col == 1 || col == 7) return ChessPiece.BlackKnight;
                    if (col == 0 || col == 8) return ChessPiece.BlackRook;
                }
                else if (row == 2)
                {
                    if (col == 1 || col == 7) return ChessPiece.BlackCannon;
                }
                else if (row == 3)
                {
                    if (col % 2 == 0) return ChessPiece.BlackPawn;
                }

                // 根据形状特征推测
                if (features.Circularity > 0.7)
                {
                    return ChessPiece.BlackCannon;
                }
                else if (features.Density > 0.3)
                {
                    return ChessPiece.BlackRook;
                }
                
                return ChessPiece.BlackPawn;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// 棋子特征
    /// </summary>
    internal class PieceFeatures
    {
        public bool HasPiece { get; set; }
        public bool IsRed { get; set; }
        public double Brightness { get; set; }
        public double Contrast { get; set; }
        public double Circularity { get; set; }  // 圆度
        public double Density { get; set; }      // 密度
        public double Hue { get; set; }
        public double Saturation { get; set; }
        public double RedValue { get; set; }
    }
}
