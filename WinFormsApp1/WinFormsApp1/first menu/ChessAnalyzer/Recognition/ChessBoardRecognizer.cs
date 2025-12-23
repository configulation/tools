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
    /// 象棋棋盘图像识别器（使用 OpenCV）
    /// </summary>
    public class ChessBoardRecognizer : IDisposable
    {
        private Dictionary<ChessPiece, Mat> _pieceTemplates;
        private bool _disposed = false;

        public ChessBoardRecognizer()
        {
            LoadTemplates();
        }

        /// <summary>
        /// 从截图识别棋盘并转换为 FEN
        /// </summary>
        public string RecognizeToFEN(Bitmap screenshot)
        {
            try
            {
                // 转换为 OpenCV Mat
                using Mat image = BitmapConverter.ToMat(screenshot);

                // 检测棋盘网格
                var grid = DetectChessboardGrid(image);
                if (grid == null)
                {
                    throw new Exception("无法检测到棋盘网格");
                }

                // 识别每个格子的棋子
                var board = new ChessBoard();
                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 9; x++)
                    {
                        var cellRect = GetCellRect(grid, x, y);
                        if (cellRect.Width > 0 && cellRect.Height > 0)
                        {
                            using Mat cellImage = new Mat(image, cellRect);
                            var piece = RecognizePiece(cellImage);
                            board.SetPiece(x, y, piece);
                        }
                    }
                }

                // 转换为 FEN
                return board.ToFEN();
            }
            catch (Exception ex)
            {
                throw new Exception($"棋盘识别失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 检测棋盘网格
        /// </summary>
        private GridInfo DetectChessboardGrid(Mat image)
        {
            try
            {
                // 1. 转灰度
                using Mat gray = new Mat();
                Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

                // 2. 边缘检测
                using Mat edges = new Mat();
                Cv2.Canny(gray, edges, 50, 150, 3);

                // 3. 霍夫线检测
                var lines = Cv2.HoughLinesP(
                    edges,
                    rho: 1,
                    theta: Math.PI / 180,
                    threshold: 100,
                    minLineLength: 50,
                    maxLineGap: 10
                );

                if (lines == null || lines.Length < 20)
                {
                    return null;
                }

                // 4. 分离横线和竖线
                var horizontalLines = new List<LineSegmentPoint>();
                var verticalLines = new List<LineSegmentPoint>();

                foreach (var line in lines)
                {
                    double angle = Math.Atan2(line.P2.Y - line.P1.Y, line.P2.X - line.P1.X) * 180 / Math.PI;
                    angle = Math.Abs(angle);

                    if (angle < 10 || angle > 170) // 横线
                    {
                        horizontalLines.Add(line);
                    }
                    else if (angle > 80 && angle < 100) // 竖线
                    {
                        verticalLines.Add(line);
                    }
                }

                // 5. 合并相近的线并排序
                var hLines = MergeLines(horizontalLines, isHorizontal: true).OrderBy(l => l.P1.Y).ToList();
                var vLines = MergeLines(verticalLines, isHorizontal: false).OrderBy(l => l.P1.X).ToList();

                // 6. 验证线的数量
                if (hLines.Count < 10 || vLines.Count < 9)
                {
                    return null;
                }

                // 7. 选择最有可能的10条横线和9条竖线
                var selectedHLines = SelectBestLines(hLines, 10, isHorizontal: true);
                var selectedVLines = SelectBestLines(vLines, 9, isHorizontal: false);

                // 8. 计算网格交叉点
                var grid = new GridInfo
                {
                    Intersections = new Point2f[9, 10]
                };

                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        var intersection = GetLineIntersection(selectedVLines[x], selectedHLines[y]);
                        grid.Intersections[x, y] = intersection;
                    }
                }

                return grid;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 合并相近的线
        /// </summary>
        private List<LineSegmentPoint> MergeLines(List<LineSegmentPoint> lines, bool isHorizontal)
        {
            if (lines.Count == 0) return lines;

            var merged = new List<LineSegmentPoint>();
            var sorted = isHorizontal
                ? lines.OrderBy(l => (l.P1.Y + l.P2.Y) / 2).ToList()
                : lines.OrderBy(l => (l.P1.X + l.P2.X) / 2).ToList();

            var current = sorted[0];
            for (int i = 1; i < sorted.Count; i++)
            {
                double dist = isHorizontal
                    ? Math.Abs((sorted[i].P1.Y + sorted[i].P2.Y) / 2.0 - (current.P1.Y + current.P2.Y) / 2.0)
                    : Math.Abs((sorted[i].P1.X + sorted[i].P2.X) / 2.0 - (current.P1.X + current.P2.X) / 2.0);

                if (dist < 20) // 距离阈值
                {
                    // 合并线段（取平均）
                    current = new LineSegmentPoint(
                        new OpenCvSharp.Point((current.P1.X + sorted[i].P1.X) / 2, (current.P1.Y + sorted[i].P1.Y) / 2),
                        new OpenCvSharp.Point((current.P2.X + sorted[i].P2.X) / 2, (current.P2.Y + sorted[i].P2.Y) / 2)
                    );
                }
                else
                {
                    merged.Add(current);
                    current = sorted[i];
                }
            }
            merged.Add(current);

            return merged;
        }

        /// <summary>
        /// 选择最佳的 N 条线
        /// </summary>
        private List<LineSegmentPoint> SelectBestLines(List<LineSegmentPoint> lines, int count, bool isHorizontal)
        {
            if (lines.Count <= count)
                return lines;

            // 计算理想间距
            var sorted = isHorizontal
                ? lines.OrderBy(l => (l.P1.Y + l.P2.Y) / 2).ToList()
                : lines.OrderBy(l => (l.P1.X + l.P2.X) / 2).ToList();

            double start = isHorizontal ? (sorted[0].P1.Y + sorted[0].P2.Y) / 2 : (sorted[0].P1.X + sorted[0].P2.X) / 2;
            double end = isHorizontal ? (sorted[sorted.Count - 1].P1.Y + sorted[sorted.Count - 1].P2.Y) / 2 : (sorted[sorted.Count - 1].P1.X + sorted[sorted.Count - 1].P2.X) / 2;
            double idealSpacing = (end - start) / (count - 1);

            // 选择最接近理想间距的线
            var selected = new List<LineSegmentPoint> { sorted[0] };
            for (int i = 1; i < count - 1; i++)
            {
                double targetPos = start + i * idealSpacing;
                var closest = sorted.OrderBy(l =>
                {
                    double pos = isHorizontal ? (l.P1.Y + l.P2.Y) / 2 : (l.P1.X + l.P2.X) / 2;
                    return Math.Abs(pos - targetPos);
                }).First();
                selected.Add(closest);
            }
            selected.Add(sorted[sorted.Count - 1]);

            return selected.OrderBy(l => isHorizontal ? (l.P1.Y + l.P2.Y) / 2 : (l.P1.X + l.P2.X) / 2).ToList();
        }

        /// <summary>
        /// 计算两条线的交点
        /// </summary>
        private Point2f GetLineIntersection(LineSegmentPoint line1, LineSegmentPoint line2)
        {
            double x1 = line1.P1.X, y1 = line1.P1.Y, x2 = line1.P2.X, y2 = line1.P2.Y;
            double x3 = line2.P1.X, y3 = line2.P1.Y, x4 = line2.P2.X, y4 = line2.P2.Y;

            double denom = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (Math.Abs(denom) < 0.001)
            {
                return new Point2f((float)x1, (float)y1);
            }

            double x = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / denom;
            double y = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / denom;

            return new Point2f((float)x, (float)y);
        }

        /// <summary>
        /// 获取格子区域
        /// </summary>
        private Rect GetCellRect(GridInfo grid, int x, int y)
        {
            if (x >= 8 || y >= 9) return new Rect(0, 0, 0, 0);

            var topLeft = grid.Intersections[x, y];
            var bottomRight = grid.Intersections[x + 1, y + 1];

            int left = (int)topLeft.X;
            int top = (int)topLeft.Y;
            int width = (int)(bottomRight.X - topLeft.X);
            int height = (int)(bottomRight.Y - topLeft.Y);

            // 添加边界检查
            if (width <= 0 || height <= 0) return new Rect(0, 0, 0, 0);

            return new Rect(left, top, width, height);
        }

        /// <summary>
        /// 识别单个格子的棋子
        /// </summary>
        private ChessPiece RecognizePiece(Mat cellImage)
        {
            if (_pieceTemplates == null || _pieceTemplates.Count == 0)
            {
                // 如果没有模板，返回空
                return ChessPiece.Empty;
            }

            try
            {
                // 调整大小以匹配模板
                using Mat resized = new Mat();
                Cv2.Resize(cellImage, resized, new OpenCvSharp.Size(50, 50));

                double maxScore = 0;
                ChessPiece bestMatch = ChessPiece.Empty;

                foreach (var kvp in _pieceTemplates)
                {
                    using Mat result = new Mat();
                    Cv2.MatchTemplate(resized, kvp.Value, result, TemplateMatchModes.CCoeffNormed);

                    Cv2.MinMaxLoc(result, out _, out double maxVal);

                    if (maxVal > maxScore)
                    {
                        maxScore = maxVal;
                        bestMatch = kvp.Key;
                    }
                }

                // 阈值过滤
                return maxScore > 0.6 ? bestMatch : ChessPiece.Empty;
            }
            catch
            {
                return ChessPiece.Empty;
            }
        }

        /// <summary>
        /// 加载棋子模板
        /// </summary>
        private void LoadTemplates()
        {
            _pieceTemplates = new Dictionary<ChessPiece, Mat>();

            // TODO: 从文件加载模板图片
            // 示例：
            // string templateDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
            // _pieceTemplates[ChessPiece.RedKing] = Cv2.ImRead(Path.Combine(templateDir, "red_king.png"));
            // ...

            // 当前版本：生成简单的颜色模板（用于演示）
            GenerateSampleTemplates();
        }

        /// <summary>
        /// 生成示例模板（临时方案）
        /// </summary>
        private void GenerateSampleTemplates()
        {
            // 这里生成简单的颜色块作为模板
            // 实际使用时应该替换为真实的棋子图片

            _pieceTemplates[ChessPiece.RedKing] = CreateColorTemplate(new Scalar(0, 0, 200));
            _pieceTemplates[ChessPiece.BlackKing] = CreateColorTemplate(new Scalar(50, 50, 50));
            // ... 其他棋子
        }

        /// <summary>
        /// 创建颜色模板
        /// </summary>
        private Mat CreateColorTemplate(Scalar color)
        {
            Mat template = new Mat(50, 50, MatType.CV_8UC3, color);
            Cv2.Circle(template, new OpenCvSharp.Point(25, 25), 20, Scalar.White, -1);
            return template;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_pieceTemplates != null)
                {
                    foreach (var mat in _pieceTemplates.Values)
                    {
                        mat?.Dispose();
                    }
                    _pieceTemplates.Clear();
                }
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// 网格信息
    /// </summary>
    internal class GridInfo
    {
        public Point2f[,] Intersections { get; set; }
    }
}
