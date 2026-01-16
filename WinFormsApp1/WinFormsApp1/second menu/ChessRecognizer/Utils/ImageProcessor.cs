using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace WinFormsApp1.second_menu.ChessRecognizer.Utils
{
    /// <summary>
    /// 图像处理工具类
    /// </summary>
    public static class ImageProcessor
    {
        /// <summary>
        /// 调整图像大小
        /// </summary>
        public static Bitmap Resize(Bitmap source, int maxWidth, int maxHeight)
        {
            if (source.Width <= maxWidth && source.Height <= maxHeight)
            {
                return new Bitmap(source);
            }

            double ratio = Math.Min((double)maxWidth / source.Width, (double)maxHeight / source.Height);
            int newWidth = (int)(source.Width * ratio);
            int newHeight = (int)(source.Height * ratio);

            var result = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(result))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(source, 0, 0, newWidth, newHeight);
            }

            return result;
        }

        /// <summary>
        /// 转换为灰度图
        /// </summary>
        public static Bitmap ToGrayscale(Bitmap source)
        {
            var result = new Bitmap(source.Width, source.Height, PixelFormat.Format24bppRgb);

            using (var g = Graphics.FromImage(result))
            {
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

            return result;
        }

        /// <summary>
        /// 二值化处理
        /// </summary>
        public static Bitmap Binarize(Bitmap source, int threshold = -1)
        {
            var result = new Bitmap(source.Width, source.Height, PixelFormat.Format24bppRgb);

            var rect = new Rectangle(0, 0, source.Width, source.Height);
            var srcData = source.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            var dstData = result.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* srcPtr = (byte*)srcData.Scan0;
                byte* dstPtr = (byte*)dstData.Scan0;
                int stride = srcData.Stride;

                // 如果没有指定阈值，使用Otsu方法计算
                if (threshold < 0)
                {
                    threshold = CalculateOtsuThreshold(srcPtr, source.Width, source.Height, stride);
                }

                for (int y = 0; y < source.Height; y++)
                {
                    for (int x = 0; x < source.Width; x++)
                    {
                        int offset = y * stride + x * 3;
                        int gray = (srcPtr[offset] + srcPtr[offset + 1] + srcPtr[offset + 2]) / 3;
                        byte value = gray > threshold ? (byte)255 : (byte)0;
                        dstPtr[offset] = dstPtr[offset + 1] = dstPtr[offset + 2] = value;
                    }
                }
            }

            source.UnlockBits(srcData);
            result.UnlockBits(dstData);

            return result;
        }

        /// <summary>
        /// 计算Otsu阈值
        /// </summary>
        private static unsafe int CalculateOtsuThreshold(byte* ptr, int width, int height, int stride)
        {
            int[] histogram = new int[256];
            int totalPixels = width * height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int offset = y * stride + x * 3;
                    int gray = (ptr[offset] + ptr[offset + 1] + ptr[offset + 2]) / 3;
                    histogram[gray]++;
                }
            }

            int sum = 0;
            for (int i = 0; i < 256; i++)
                sum += i * histogram[i];

            int sumB = 0;
            int wB = 0;
            double maxVariance = 0;
            int threshold = 0;

            for (int t = 0; t < 256; t++)
            {
                wB += histogram[t];
                if (wB == 0) continue;

                int wF = totalPixels - wB;
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
        /// 锐化处理
        /// </summary>
        public static Bitmap Sharpen(Bitmap source)
        {
            using Mat mat = BitmapConverter.ToMat(source);
            using Mat result = new Mat();

            // 使用拉普拉斯锐化
            using Mat laplacian = new Mat();
            Cv2.Laplacian(mat, laplacian, MatType.CV_16S, 3);
            Cv2.ConvertScaleAbs(laplacian, laplacian);

            // 叠加原图
            Cv2.AddWeighted(mat, 1.5, laplacian, -0.5, 0, result);

            return BitmapConverter.ToBitmap(result);
        }

        /// <summary>
        /// 去噪处理
        /// </summary>
        public static Bitmap Denoise(Bitmap source)
        {
            using Mat mat = BitmapConverter.ToMat(source);
            using Mat result = new Mat();

            // 使用非局部均值去噪
            Cv2.FastNlMeansDenoisingColored(mat, result, 10, 10, 7, 21);

            return BitmapConverter.ToBitmap(result);
        }

        /// <summary>
        /// 对比度增强
        /// </summary>
        public static Bitmap EnhanceContrast(Bitmap source, double alpha = 1.5, int beta = 0)
        {
            using Mat mat = BitmapConverter.ToMat(source);
            using Mat result = new Mat();

            mat.ConvertTo(result, -1, alpha, beta);

            return BitmapConverter.ToBitmap(result);
        }

        /// <summary>
        /// 自动旋转矫正
        /// </summary>
        public static Bitmap AutoRotate(Bitmap source)
        {
            using Mat mat = BitmapConverter.ToMat(source);
            using Mat gray = new Mat();
            Cv2.CvtColor(mat, gray, ColorConversionCodes.BGR2GRAY);

            // 边缘检测
            using Mat edges = new Mat();
            Cv2.Canny(gray, edges, 50, 150);

            // 霍夫线检测
            var lines = Cv2.HoughLines(edges, 1, Math.PI / 180, 100);

            if (lines == null || lines.Length == 0)
            {
                return new Bitmap(source);
            }

            // 计算主要角度
            var angles = new List<double>();
            foreach (var line in lines)
            {
                double angle = line.Theta * 180 / Math.PI - 90;
                if (Math.Abs(angle) < 45)
                {
                    angles.Add(angle);
                }
            }

            if (angles.Count == 0)
            {
                return new Bitmap(source);
            }

            double avgAngle = angles.Average();

            // 如果角度很小，不需要旋转
            if (Math.Abs(avgAngle) < 0.5)
            {
                return new Bitmap(source);
            }

            // 旋转图像
            var center = new Point2f(mat.Width / 2f, mat.Height / 2f);
            using Mat rotationMatrix = Cv2.GetRotationMatrix2D(center, avgAngle, 1.0);
            using Mat rotated = new Mat();
            Cv2.WarpAffine(mat, rotated, rotationMatrix, mat.Size());

            return BitmapConverter.ToBitmap(rotated);
        }

        /// <summary>
        /// 裁剪图像
        /// </summary>
        public static Bitmap Crop(Bitmap source, Rectangle rect)
        {
            if (rect.X < 0 || rect.Y < 0 ||
                rect.Right > source.Width || rect.Bottom > source.Height)
            {
                // 调整裁剪区域
                rect.X = Math.Max(0, rect.X);
                rect.Y = Math.Max(0, rect.Y);
                rect.Width = Math.Min(rect.Width, source.Width - rect.X);
                rect.Height = Math.Min(rect.Height, source.Height - rect.Y);
            }

            if (rect.Width <= 0 || rect.Height <= 0)
            {
                return new Bitmap(source);
            }

            return source.Clone(rect, source.PixelFormat);
        }

        /// <summary>
        /// 从剪贴板获取图像
        /// </summary>
        public static Bitmap GetFromClipboard()
        {
            if (Clipboard.ContainsImage())
            {
                var image = Clipboard.GetImage();
                if (image != null)
                {
                    return new Bitmap(image);
                }
            }
            return null;
        }

        /// <summary>
        /// 复制图像到剪贴板
        /// </summary>
        public static void CopyToClipboard(Bitmap image)
        {
            if (image != null)
            {
                Clipboard.SetImage(image);
            }
        }

        /// <summary>
        /// 保存图像
        /// </summary>
        public static void Save(Bitmap image, string path, ImageFormat format = null)
        {
            format ??= ImageFormat.Png;
            image.Save(path, format);
        }

        /// <summary>
        /// 加载图像
        /// </summary>
        public static Bitmap Load(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("图像文件不存在", path);
            }

            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return new Bitmap(stream);
        }
    }
}
