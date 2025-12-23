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
    /// 改进的识别器 - 更准确的网格和棋子检测
    /// </summary>
    public class ImprovedRecognizer : IDisposable
    {
        private bool _disposed = false;

        /// <summary>
        /// 从截图识别棋盘
        /// </summary>
        public RecognitionResult RecognizeToBoard(Bitmap screenshot)
        {
            var result = new RecognitionResult();

            try
            {
                using Mat image = BitmapConverter.ToMat(screenshot);

                // 1. 计算交叉点间距
                // 中国象棋：棋子在交叉点上，不是格子里！
                // 9条竖线 = 8个间隔，10条横线 = 9个间隔
                int width = image.Width;
                int height = image.Height;

                double cellWidth = width / 8.0;   // 竖线间距
                double cellHeight = height / 9.0; // 横线间距

                var board = new ChessBoard();
                var debugInfo = new List<string>();

                // 2. 遍历每个交叉点
                for (int row = 0; row < 10; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        // 计算交叉点位置（棋子在线的交叉点上）
                        int centerX = (int)(col * cellWidth);
                        int centerY = (int)(row * cellHeight);

                        // 提取交叉点周围的小区域
                        int sampleSize = (int)Math.Min(cellWidth * 0.4, cellHeight * 0.4);
                        int rectX = Math.Max(0, centerX - sampleSize / 2);
                        int rectY = Math.Max(0, centerY - sampleSize / 2);
                        int rectW = Math.Min(sampleSize, width - rectX);
                        int rectH = Math.Min(sampleSize, height - rectY);

                        if (rectW <= 0 || rectH <= 0) continue;

                        var rect = new Rect(rectX, rectY, rectW, rectH);
                        using Mat cellImage = new Mat(image, rect);

                        // 识别棋子
                        var piece = AnalyzeCell(cellImage, col, row, debugInfo);
                        board.SetPiece(col, row, piece);
                    }
                }

                result.FEN = board.ToFEN();
                result.Success = true;
                result.Message = "识别完成";
                result.DebugInfo = string.Join("\n", debugInfo);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"识别失败: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// 分析单个格子
        /// </summary>
        private ChessPiece AnalyzeCell(Mat cellImage, int col, int row, List<string> debugInfo)
        {
            try
            {
                // 1. 转换颜色空间到 HSV（更好的颜色检测）
                using Mat hsv = new Mat();
                Cv2.CvtColor(cellImage, hsv, ColorConversionCodes.BGR2HSV);

                // 2. 计算平均颜色和标准差
                Scalar meanBgr = Cv2.Mean(cellImage);
                
                using Mat gray = new Mat();
                Cv2.CvtColor(cellImage, gray, ColorConversionCodes.BGR2GRAY);
                Cv2.MeanStdDev(gray, out Scalar mean, out Scalar stdDev);

                // 3. 判断是否有棋子（基于标准差）
                double colorVariance = stdDev[0];
                
                // 如果颜色变化很小，说明是空格或棋盘背景
                if (colorVariance < 20)
                {
                    return ChessPiece.Empty;
                }

                // 4. 分析颜色确定红黑
                double b = meanBgr[0];
                double g = meanBgr[1];
                double r = meanBgr[2];

                // HSV 分析
                Scalar meanHsv = Cv2.Mean(hsv);
                double hue = meanHsv[0];        // 色相 0-180
                double saturation = meanHsv[1]; // 饱和度 0-255
                double value = meanHsv[2];      // 明度 0-255

                bool isRed = false;
                bool isBlack = false;

                // 红色检测：高饱和度 + 红色色相
                if (saturation > 50 && (hue < 10 || hue > 170))
                {
                    isRed = true;
                }
                // 或者 RGB 中 R 明显大
                else if (r > b + 30 && r > g + 20 && r > 80)
                {
                    isRed = true;
                }

                // 黑色检测：低明度
                if (value < 100 && saturation < 100)
                {
                    isBlack = true;
                }

                // 调试信息
                string pieceType = isRed ? "红" : isBlack ? "黑" : "空";
                debugInfo.Add($"[{col},{row}] V:{colorVariance:F1} RGB:({r:F0},{g:F0},{b:F0}) HSV:({hue:F0},{saturation:F0},{value:F0}) => {pieceType}");

                // 5. 根据位置推测棋子类型
                if (isRed)
                {
                    return GuessPieceType(col, row, true);
                }
                else if (isBlack)
                {
                    return GuessPieceType(col, row, false);
                }

                return ChessPiece.Empty;
            }
            catch
            {
                return ChessPiece.Empty;
            }
        }

        /// <summary>
        /// 根据位置推测棋子类型
        /// </summary>
        private ChessPiece GuessPieceType(int col, int row, bool isRed)
        {
            if (isRed)
            {
                // 红方（下方）
                if (row == 9)
                {
                    // 底线
                    if (col == 4) return ChessPiece.RedKing;
                    if (col == 3 || col == 5) return ChessPiece.RedAdvisor;
                    if (col == 2 || col == 6) return ChessPiece.RedElephant;
                    if (col == 1 || col == 7) return ChessPiece.RedKnight;
                    if (col == 0 || col == 8) return ChessPiece.RedRook;
                }
                else if (row == 7)
                {
                    // 炮的位置
                    if (col == 1 || col == 7) return ChessPiece.RedCannon;
                }
                else if (row == 6)
                {
                    // 兵的位置
                    if (col % 2 == 0) return ChessPiece.RedPawn;
                }
                
                // 默认返回兵（移动后的棋子）
                return ChessPiece.RedPawn;
            }
            else
            {
                // 黑方（上方）
                if (row == 0)
                {
                    // 顶线
                    if (col == 4) return ChessPiece.BlackKing;
                    if (col == 3 || col == 5) return ChessPiece.BlackAdvisor;
                    if (col == 2 || col == 6) return ChessPiece.BlackElephant;
                    if (col == 1 || col == 7) return ChessPiece.BlackKnight;
                    if (col == 0 || col == 8) return ChessPiece.BlackRook;
                }
                else if (row == 2)
                {
                    // 炮的位置
                    if (col == 1 || col == 7) return ChessPiece.BlackCannon;
                }
                else if (row == 3)
                {
                    // 卒的位置
                    if (col % 2 == 0) return ChessPiece.BlackPawn;
                }
                
                // 默认返回卒
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
}
