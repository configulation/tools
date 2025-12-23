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
    /// 基于颜色的简化识别器（适用于大部分棋盘皮肤）
    /// </summary>
    public class ColorBasedRecognizer : IDisposable
    {
        private bool _disposed = false;

        /// <summary>
        /// 从截图识别棋盘并转换为 FEN
        /// </summary>
        public RecognitionResult RecognizeToFEN(Bitmap screenshot)
        {
            var result = new RecognitionResult();

            try
            {
                using Mat image = BitmapConverter.ToMat(screenshot);

                // 1. 简单网格假设：均匀分割
                int width = image.Width;
                int height = image.Height;

                // 假设棋盘占据整个截图
                double cellWidth = width / 9.0;
                double cellHeight = height / 10.0;

                var board = new ChessBoard();

                // 2. 检测每个格子
                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 9; x++)
                    {
                        // 计算格子中心位置
                        int centerX = (int)((x + 0.5) * cellWidth);
                        int centerY = (int)((y + 0.5) * cellHeight);

                        // 提取格子区域
                        int rectX = (int)(x * cellWidth + cellWidth * 0.2);
                        int rectY = (int)(y * cellHeight + cellHeight * 0.2);
                        int rectW = (int)(cellWidth * 0.6);
                        int rectH = (int)(cellHeight * 0.6);

                        if (rectX + rectW > width || rectY + rectH > height)
                            continue;

                        var rect = new Rect(rectX, rectY, rectW, rectH);
                        using Mat cellImage = new Mat(image, rect);

                        // 识别棋子
                        var piece = RecognizePieceByColor(cellImage);
                        board.SetPiece(x, y, piece);
                    }
                }

                result.FEN = board.ToFEN();
                result.Success = true;
                result.Message = "识别成功";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"识别失败: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// 通过颜色识别棋子
        /// </summary>
        private ChessPiece RecognizePieceByColor(Mat cellImage)
        {
            try
            {
                // 计算平均颜色
                Scalar avgColor = Cv2.Mean(cellImage);

                // 判断是否有棋子（通过颜色变化判断）
                using Mat gray = new Mat();
                Cv2.CvtColor(cellImage, gray, ColorConversionCodes.BGR2GRAY);

                Cv2.MeanStdDev(gray, out Scalar mean, out Scalar stdDev);

                // 如果颜色变化很小，说明是空格
                if (stdDev[0] < 15)
                {
                    return ChessPiece.Empty;
                }

                // 通过颜色判断红黑
                // 红色棋子通常：R > B 且 R > G
                // 黑色棋子通常：R ≈ G ≈ B 且都比较低

                double b = avgColor[0];
                double g = avgColor[1];
                double r = avgColor[2];

                bool isRed = r > b + 20 && r > g + 20 && r > 100;
                bool isBlack = r < 100 && g < 100 && b < 100;

                if (!isRed && !isBlack)
                {
                    return ChessPiece.Empty;
                }

                // 检测轮廓以估计棋子类型
                using Mat edges = new Mat();
                Cv2.Canny(gray, edges, 50, 150);

                var contours = new OpenCvSharp.Point[0][];
                Cv2.FindContours(edges, out contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                if (contours.Length == 0)
                {
                    return ChessPiece.Empty;
                }

                // 根据颜色返回默认棋子（这里简化处理）
                // 实际应用中需要更复杂的特征识别

                // 临时方案：返回常见棋子
                if (isRed)
                {
                    return ChessPiece.RedPawn; // 默认红兵
                }
                else
                {
                    return ChessPiece.BlackPawn; // 默认黑卒
                }
            }
            catch
            {
                return ChessPiece.Empty;
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
}
