using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Interfaces;

namespace WinFormsApp1.second_menu.ChessRecognizer.Core.Services.Detection
{
    /// <summary>
    /// 棋盘检测服务
    /// </summary>
    public class BoardDetector : IBoardDetector
    {
        private BoardDetectionParameters _parameters;
        private const int NormalizedBoardWidth = 900;
        private const int NormalizedBoardHeight = 1000;

        public BoardDetector()
        {
            _parameters = new BoardDetectionParameters();
        }

        /// <summary>
        /// 检测棋盘
        /// </summary>
        public async Task<BoardDetectionResult> DetectBoardAsync(Bitmap image)
        {
            return await Task.Run(() =>
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var result = new BoardDetectionResult();
                Bitmap normalizedBitmap = null;

                try
                {
                    using Mat mat = BitmapConverter.ToMat(image);

                    using Mat normalized = TryNormalizeBoard(mat, out PointF[] corners);
                    Mat gridMat = normalized ?? mat;
                    if (normalized != null)
                    {
                        normalizedBitmap = BitmapConverter.ToBitmap(gridMat);
                        result.NormalizedBoardImage = normalizedBitmap;
                    }

                    var boardInfo = DetectGrid(gridMat);
                    if (boardInfo == null)
                    {
                        int w = gridMat.Width;
                        int h = gridMat.Height;
                        boardInfo = CreateSimpleGrid(w, h);
                        result.Confidence = 0.6;
                    }
                    else
                    {
                        result.Confidence = 0.9;
                    }

                    if (corners != null)
                    {
                        boardInfo.Corners = corners;
                    }

                    result.Success = true;
                    result.BoardInfo = boardInfo;
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.ErrorMessage = ex.Message;
                    normalizedBitmap?.Dispose();
                }

				sw.Stop();
				result.ProcessTime = sw.Elapsed;

                return result;
            });
        }

        /// <summary>
        /// 检测网格
        /// </summary>
        private BoardInfo DetectGrid(Mat image)
        {
            try
            {
                // 转灰度
                using Mat gray = new Mat();
                Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

                // 边缘检测
                using Mat edges = new Mat();
                Cv2.Canny(gray, edges, _parameters.CannyThreshold1, _parameters.CannyThreshold2);

                // 霍夫线检测
                var lines = Cv2.HoughLinesP(
                    edges,
                    rho: 1,
                    theta: Math.PI / 180,
                    threshold: _parameters.HoughThreshold,
                    minLineLength: _parameters.MinLineLength,
                    maxLineGap: _parameters.MaxLineGap
                );

                if (lines == null || lines.Length < 18)
                {
                    return null;
                }

                // 分离横线和竖线
                var horizontalLines = new List<LineSegmentPoint>();
                var verticalLines = new List<LineSegmentPoint>();

                foreach (var line in lines)
                {
                    double angle = Math.Atan2(line.P2.Y - line.P1.Y, line.P2.X - line.P1.X) * 180 / Math.PI;
                    angle = Math.Abs(angle);

                    if (angle < 15 || angle > 165)
                    {
                        horizontalLines.Add(line);
                    }
                    else if (angle > 75 && angle < 105)
                    {
                        verticalLines.Add(line);
                    }
                }

                // 合并相近的线
                var hLines = MergeLines(horizontalLines, true);
                var vLines = MergeLines(verticalLines, false);

                if (hLines.Count < 10 || vLines.Count < 9)
                {
                    return null;
                }

                // 选择最佳的线
                var selectedH = SelectBestLines(hLines, 10, true);
                var selectedV = SelectBestLines(vLines, 9, false);

                // 构建网格
                var boardInfo = new BoardInfo
                {
                    GridPoints = new PointF[9, 10]
                };

                for (int col = 0; col < 9; col++)
                {
                    for (int row = 0; row < 10; row++)
                    {
                        var intersection = GetIntersection(selectedV[col], selectedH[row]);
                        boardInfo.GridPoints[col, row] = new PointF((float)intersection.X, (float)intersection.Y);
                    }
                }

                // 计算单元格大小
                boardInfo.CellWidth = (boardInfo.GridPoints[8, 0].X - boardInfo.GridPoints[0, 0].X) / 8;
                boardInfo.CellHeight = (boardInfo.GridPoints[0, 9].Y - boardInfo.GridPoints[0, 0].Y) / 9;
                boardInfo.Corners = new[]
                {
                    new PointF(0, 0),
                    new PointF(image.Width - 1, 0),
                    new PointF(image.Width - 1, image.Height - 1),
                    new PointF(0, image.Height - 1)
                };

                return boardInfo;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 创建简单的均匀网格
        /// </summary>
        private BoardInfo CreateSimpleGrid(int width, int height)
        {
            var boardInfo = new BoardInfo
            {
                GridPoints = new PointF[9, 10]
            };

            float padding = Math.Min(width, height) * 0.05f;
            float startX = padding;
            float startY = padding;
            float endX = width - padding;
            float endY = height - padding;
            float cellWidth = (endX - startX) / 8.0f;
            float cellHeight = (endY - startY) / 9.0f;

            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 10; row++)
                {
                    boardInfo.GridPoints[col, row] = new PointF(startX + col * cellWidth, startY + row * cellHeight);
                }
            }

            boardInfo.CellWidth = cellWidth;
            boardInfo.CellHeight = cellHeight;

            return boardInfo;
        }

		private List<LineSegmentPoint> MergeLines(List<LineSegmentPoint> lines, bool isHorizontal)
		{
			if (lines.Count == 0) return lines;

			var merged = new List<LineSegmentPoint>();
			var sorted = isHorizontal
				? lines.OrderBy(l => (l.P1.Y + l.P2.Y) / 2.0).ToList()
				: lines.OrderBy(l => (l.P1.X + l.P2.X) / 2.0).ToList();

			var current = sorted[0];
			for (int i = 1; i < sorted.Count; i++)
			{
				double dist = isHorizontal
					? Math.Abs((sorted[i].P1.Y + sorted[i].P2.Y) / 2.0 - (current.P1.Y + current.P2.Y) / 2.0)
					: Math.Abs((sorted[i].P1.X + sorted[i].P2.X) / 2.0 - (current.P1.X + current.P2.X) / 2.0);

				if (dist < _parameters.LineMergeThreshold)
				{
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

		private List<LineSegmentPoint> SelectBestLines(List<LineSegmentPoint> lines, int count, bool isHorizontal)
		{
			if (lines.Count <= count)
			{
				while (lines.Count < count)
				{
					lines.Add(lines.Last());
				}
				return lines;
			}

			var sorted = isHorizontal
				? lines.OrderBy(l => (l.P1.Y + l.P2.Y) / 2.0).ToList()
				: lines.OrderBy(l => (l.P1.X + l.P2.X) / 2.0).ToList();

			double start = isHorizontal ? (sorted[0].P1.Y + sorted[0].P2.Y) / 2.0 : (sorted[0].P1.X + sorted[0].P2.X) / 2.0;
			double end = isHorizontal ? (sorted.Last().P1.Y + sorted.Last().P2.Y) / 2.0 : (sorted.Last().P1.X + sorted.Last().P2.X) / 2.0;
			double idealSpacing = (end - start) / (count - 1);

			var selected = new List<LineSegmentPoint> { sorted[0] };
			for (int i = 1; i < count - 1; i++)
			{
				double targetPos = start + i * idealSpacing;
				var closest = sorted.OrderBy(l =>
				{
					double pos = isHorizontal ? (l.P1.Y + l.P2.Y) / 2.0 : (l.P1.X + l.P2.X) / 2.0;
					return Math.Abs(pos - targetPos);
				}).First();
				selected.Add(closest);
			}
			selected.Add(sorted.Last());

			return selected.OrderBy(l => isHorizontal ? (l.P1.Y + l.P2.Y) / 2.0 : (l.P1.X + l.P2.X) / 2.0).ToList();
		}

		private PointF GetIntersection(LineSegmentPoint line1, LineSegmentPoint line2)
		{
			double x1 = line1.P1.X, y1 = line1.P1.Y, x2 = line1.P2.X, y2 = line1.P2.Y;
			double x3 = line2.P1.X, y3 = line2.P1.Y, x4 = line2.P2.X, y4 = line2.P2.Y;

			double denom = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
			if (Math.Abs(denom) < 0.001)
			{
				return new PointF((float)x1, (float)y3);
			}

			double x = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / denom;
			double y = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / denom;

			return new PointF((float)x, (float)y);
		}

		public List<PieceRegion> ExtractPieceRegions(Bitmap image, BoardInfo boardInfo)
		{
			var regions = new List<PieceRegion>();

			for (int col = 0; col < 9; col++)
			{
				for (int row = 0; row < 10; row++)
				{
					var center = boardInfo.GridPoints[col, row];
					int size = (int)(Math.Min(boardInfo.CellWidth, boardInfo.CellHeight) * _parameters.PieceRegionScale);
					int halfSize = size / 2;

					int x = Math.Max(0, (int)center.X - halfSize);
					int y = Math.Max(0, (int)center.Y - halfSize);
					int w = Math.Min(size, image.Width - x);
					int h = Math.Min(size, image.Height - y);

					if (w > 10 && h > 10)
					{
						var bounds = new Rectangle(x, y, w, h);
						var pieceImage = image.Clone(bounds, image.PixelFormat);

						var region = new PieceRegion
						{
							Column = col,
							Row = row,
							Bounds = bounds,
							Image = pieceImage,
							HasPiece = HasPieceInRegion(pieceImage),
							ColorType = DetectPieceColor(pieceImage)
						};

						regions.Add(region);
					}
				}
			}

			return regions;
		}

        /// <summary>
        /// 检测区域是否有棋子
        /// </summary>
        private bool HasPieceInRegion(Bitmap image)
        {
            using Mat mat = BitmapConverter.ToMat(image);
            using Mat gray = new Mat();
            Cv2.CvtColor(mat, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.GaussianBlur(gray, gray, new OpenCvSharp.Size(3, 3), 0);
            using Mat edges = new Mat();
            Cv2.Canny(gray, edges, 60, 160);
            int edgeCount = Cv2.CountNonZero(edges);
            if (edgeCount < (image.Width * image.Height) * 0.02)
            {
                return false;
            }

            var circles = Cv2.HoughCircles(
                gray,
                HoughModes.Gradient,
                dp: 1.2,
                minDist: image.Width / 2.0,
                param1: 120,
                param2: 30,
                minRadius: (int)(Math.Min(image.Width, image.Height) * 0.25),
                maxRadius: (int)(Math.Min(image.Width, image.Height) * 0.55)
            );

            return circles != null && circles.Length > 0;
        }

        /// <summary>
        /// 检测棋子颜色
        /// </summary>
        private PieceColorType DetectPieceColor(Bitmap image)
        {
			double totalR = 0, totalG = 0, totalB = 0;
			double totalS = 0;
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
						byte b = ptr[offset];
						byte g = ptr[offset + 1];
						byte r = ptr[offset + 2];

						totalR += r;
						totalG += g;
						totalB += b;

						int max = Math.Max(r, Math.Max(g, b));
						int min = Math.Min(r, Math.Min(g, b));
						if (max > 0)
						{
							totalS += (max - min) / (double)max;
						}
						count++;
					}
				}
			}

			image.UnlockBits(data);

			if (count == 0) return PieceColorType.Unknown;

			double avgR = totalR / count;
			double avgG = totalG / count;
			double avgB = totalB / count;
			double avgS = totalS / count;

			if (avgS > 0.3 && avgR > avgB + 40 && avgR > avgG + 20)
			{
				return PieceColorType.Red;
			}
			else if (avgS < 0.3 || (avgR < 100 && avgG < 100 && avgB < 100))
			{
				return PieceColorType.Black;
			}

			return PieceColorType.Unknown;
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

        private Mat TryNormalizeBoard(Mat image, out PointF[] corners)
        {
            corners = null;
            try
            {
                using Mat gray = new Mat();
                Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
                Cv2.GaussianBlur(gray, gray, new OpenCvSharp.Size(5, 5), 0);
                using Mat edges = new Mat();
                Cv2.Canny(gray, edges, _parameters.CannyThreshold1, _parameters.CannyThreshold2);
                using Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(5, 5));
                Cv2.Dilate(edges, edges, kernel);
                Cv2.Erode(edges, edges, kernel);

                Cv2.FindContours(edges, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                if (contours == null || contours.Length == 0)
                {
                    return null;
                }

                double maxArea = 0;
                OpenCvSharp.Point[] best = null;
                foreach (var c in contours)
                {
                    double area = Cv2.ContourArea(c);
                    if (area < maxArea)
                    {
                        continue;
                    }

                    double peri = Cv2.ArcLength(c, true);
                    var approx = Cv2.ApproxPolyDP(c, 0.02 * peri, true);
                    if (approx.Length == 4)
                    {
                        maxArea = area;
                        best = approx;
                    }
                }

                if (best == null)
                {
                    return null;
                }

                Point2f[] src = OrderPoints(best.Select(p => new Point2f(p.X, p.Y)).ToArray());
                corners = src.Select(p => new PointF(p.X, p.Y)).ToArray();

                Point2f[] dst =
                {
                    new Point2f(0, 0),
                    new Point2f(NormalizedBoardWidth - 1, 0),
                    new Point2f(NormalizedBoardWidth - 1, NormalizedBoardHeight - 1),
                    new Point2f(0, NormalizedBoardHeight - 1)
                };

                using Mat m = Cv2.GetPerspectiveTransform(src, dst);
                var warped = new Mat();
                Cv2.WarpPerspective(image, warped, m, new OpenCvSharp.Size(NormalizedBoardWidth, NormalizedBoardHeight));
                return warped;
            }
            catch
            {
                corners = null;
                return null;
            }
        }

        private static Point2f[] OrderPoints(Point2f[] pts)
        {
            if (pts == null || pts.Length != 4)
            {
                return pts;
            }

            var ordered = new Point2f[4];
            var sum = pts.Select(p => p.X + p.Y).ToArray();
            var diff = pts.Select(p => p.X - p.Y).ToArray();
            ordered[0] = pts[Array.IndexOf(sum, sum.Min())];
            ordered[2] = pts[Array.IndexOf(sum, sum.Max())];
            ordered[1] = pts[Array.IndexOf(diff, diff.Max())];
            ordered[3] = pts[Array.IndexOf(diff, diff.Min())];
            return ordered;
        }

        public void SetParameters(BoardDetectionParameters parameters)
        {
            _parameters = parameters ?? new BoardDetectionParameters();
        }
    }
}
