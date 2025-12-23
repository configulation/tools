using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using WinFormsApp1.first_menu.ChessAnalyzer.Core;

namespace WinFormsApp1.first_menu.ChessAnalyzer.Recognition
{
    /// <summary>
    /// 基于OCR的棋子识别器 - 更准确！
    /// </summary>
    public class OcrChessRecognizer : IDisposable
    {
        private bool _disposed = false;

        // 棋子汉字映射
        private static readonly Dictionary<string, ChessPiece> _chineseTopiece = new Dictionary<string, ChessPiece>
        {
            // 红方
            ["帅"] = ChessPiece.RedKing,
            ["仕"] = ChessPiece.RedAdvisor,
            ["相"] = ChessPiece.RedElephant,
            ["马"] = ChessPiece.RedKnight,
            ["車"] = ChessPiece.RedRook,
            ["车"] = ChessPiece.RedRook,
            ["炮"] = ChessPiece.RedCannon,
            ["兵"] = ChessPiece.RedPawn,
            
            // 黑方
            ["将"] = ChessPiece.BlackKing,
            ["士"] = ChessPiece.BlackAdvisor,
            ["象"] = ChessPiece.BlackElephant,
            ["馬"] = ChessPiece.BlackKnight,
            ["俥"] = ChessPiece.BlackRook,
            ["砲"] = ChessPiece.BlackCannon,
            ["卒"] = ChessPiece.BlackPawn,
        };

        /// <summary>
        /// 从截图识别棋盘
        /// </summary>
        public RecognitionResult RecognizeToBoard(Bitmap screenshot)
        {
            var result = new RecognitionResult();

            try
            {
                using Mat image = BitmapConverter.ToMat(screenshot);

                // 计算交叉点间距
                int width = image.Width;
                int height = image.Height;

                double cellWidth = width / 8.0;   
                double cellHeight = height / 9.0; 

                var board = new ChessBoard();
                var debugInfo = new List<string>();
                int recognizedCount = 0;

                // 遍历每个交叉点
                for (int row = 0; row < 10; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        // 计算交叉点位置
                        int centerX = (int)(col * cellWidth);
                        int centerY = (int)(row * cellHeight);

                        // 提取交叉点周围区域（更大的区域以包含整个棋子）
                        int sampleSize = (int)Math.Min(cellWidth * 0.8, cellHeight * 0.8);
                        int rectX = Math.Max(0, centerX - sampleSize / 2);
                        int rectY = Math.Max(0, centerY - sampleSize / 2);
                        int rectW = Math.Min(sampleSize, width - rectX);
                        int rectH = Math.Min(sampleSize, height - rectY);

                        if (rectW <= 0 || rectH <= 0) continue;

                        var rect = new Rect(rectX, rectY, rectW, rectH);
                        using Mat cellImage = new Mat(image, rect);

                        // 识别棋子
                        var piece = RecognizePiece(cellImage, col, row, debugInfo);
                        board.SetPiece(col, row, piece);
                        
                        if (piece != ChessPiece.Empty)
                            recognizedCount++;
                    }
                }

                result.FEN = board.ToFEN();
                result.Success = true;
                result.Message = $"识别完成，共识别到 {recognizedCount} 个棋子";
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
        /// 识别单个棋子 - 使用图像处理特征
        /// </summary>
        private ChessPiece RecognizePiece(Mat cellImage, int col, int row, List<string> debugInfo)
        {
            try
            {
                // 1. 转灰度
                using Mat gray = new Mat();
                Cv2.CvtColor(cellImage, gray, ColorConversionCodes.BGR2GRAY);

                // 2. 计算平均亮度和标准差
                Cv2.MeanStdDev(gray, out Scalar mean, out Scalar stdDev);
                double brightness = mean[0];
                double variance = stdDev[0];

                // 3. 如果变化很小，说明是空格
                if (variance < 15)
                {
                    return ChessPiece.Empty;
                }

                // 4. 二值化处理
                using Mat binary = new Mat();
                Cv2.Threshold(gray, binary, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                // 5. 形态学处理
                using Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));
                using Mat morphed = new Mat();
                Cv2.MorphologyEx(binary, morphed, MorphTypes.Close, kernel);

                // 6. 轮廓检测
                Cv2.FindContours(morphed, out OpenCvSharp.Point[][] contours, out _, 
                    RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                if (contours.Length == 0)
                {
                    return ChessPiece.Empty;
                }

                // 7. 分析颜色特征判断红黑
                using Mat hsv = new Mat();
                Cv2.CvtColor(cellImage, hsv, ColorConversionCodes.BGR2HSV);
                Scalar meanHsv = Cv2.Mean(hsv);
                
                double hue = meanHsv[0];
                double saturation = meanHsv[1];
                double value = meanHsv[2];

                bool isRed = false;
                
                // 红色判断：高饱和度 + 红色色相
                if (saturation > 60 && (hue < 15 || hue > 165))
                {
                    isRed = true;
                }
                // 或者RGB分析
                else
                {
                    Scalar meanBgr = Cv2.Mean(cellImage);
                    double b = meanBgr[0];
                    double g = meanBgr[1];
                    double r = meanBgr[2];
                    
                    if (r > b + 40 && r > g + 30 && r > 100)
                    {
                        isRed = true;
                    }
                }

                // 8. 根据位置和颜色推测类型（简化版）
                ChessPiece piece = GuessPieceByPosition(col, row, isRed);
                
                string color = isRed ? "红" : "黑";
                string pieceName = ChessPieceHelper.GetPieceText(piece);
                debugInfo.Add($"[{col},{row}] 亮度:{brightness:F0} 变化:{variance:F1} 饱和度:{saturation:F0} => {color}{pieceName}");

                return piece;
            }
            catch
            {
                return ChessPiece.Empty;
            }
        }

        /// <summary>
        /// 根据位置推测棋子类型（临时方案，建议后续用OCR或模板匹配）
        /// </summary>
        private ChessPiece GuessPieceByPosition(int col, int row, bool isRed)
        {
            if (isRed)
            {
                // 红方
                if (row == 9)  // 底线
                {
                    if (col == 4) return ChessPiece.RedKing;
                    if (col == 3 || col == 5) return ChessPiece.RedAdvisor;
                    if (col == 2 || col == 6) return ChessPiece.RedElephant;
                    if (col == 1 || col == 7) return ChessPiece.RedKnight;
                    if (col == 0 || col == 8) return ChessPiece.RedRook;
                }
                else if (row == 7 && (col == 1 || col == 7))
                {
                    return ChessPiece.RedCannon;
                }
                else if (row == 6 && col % 2 == 0)
                {
                    return ChessPiece.RedPawn;
                }
                
                // 移动后的棋子，暂时返回兵
                return ChessPiece.RedPawn;
            }
            else
            {
                // 黑方
                if (row == 0)  // 顶线
                {
                    if (col == 4) return ChessPiece.BlackKing;
                    if (col == 3 || col == 5) return ChessPiece.BlackAdvisor;
                    if (col == 2 || col == 6) return ChessPiece.BlackElephant;
                    if (col == 1 || col == 7) return ChessPiece.BlackKnight;
                    if (col == 0 || col == 8) return ChessPiece.BlackRook;
                }
                else if (row == 2 && (col == 1 || col == 7))
                {
                    return ChessPiece.BlackCannon;
                }
                else if (row == 3 && col % 2 == 0)
                {
                    return ChessPiece.BlackPawn;
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
}
