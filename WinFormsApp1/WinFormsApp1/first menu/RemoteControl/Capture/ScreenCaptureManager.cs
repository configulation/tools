using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp1.first_menu.RemoteControl
{
    /// <summary>
    /// 屏幕捕获管理器
    /// </summary>
    public class ScreenCaptureManager : IDisposable
    {
        private int quality = 70;
        private Rectangle screenBounds;
        private Bitmap screenBitmap;
        private Graphics graphics;
        private bool disposed = false;

        public int Quality
        {
            get { return quality; }
            set { quality = Math.Max(10, Math.Min(100, value)); }
        }

        public ScreenCaptureManager()
        {
            screenBounds = Screen.PrimaryScreen.Bounds;
            InitializeCapture();
        }

        private void InitializeCapture()
        {
            screenBitmap = new Bitmap(screenBounds.Width, screenBounds.Height, PixelFormat.Format32bppArgb);
            graphics = Graphics.FromImage(screenBitmap);
        }

        private void RefreshScreenBounds()
        {
            var bounds = Screen.PrimaryScreen.Bounds;
            if (bounds == screenBounds)
            {
                return;
            }

            screenBounds = bounds;

            graphics?.Dispose();
            screenBitmap?.Dispose();
            InitializeCapture();
        }

        /// <summary>
        /// 捕获整个屏幕
        /// </summary>
        public byte[] CaptureScreen()
        {
            try
            {
                RefreshScreenBounds();

                // 捕获屏幕
                graphics.CopyFromScreen(
                    screenBounds.X,
                    screenBounds.Y,
                    0,
                    0,
                    screenBounds.Size,
                    CopyPixelOperation.SourceCopy);

                // 转换为字节数组
                return ConvertBitmapToBytes(screenBitmap);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"屏幕捕获失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 捕获指定区域
        /// </summary>
        public byte[] CaptureRegion(Rectangle region)
        {
            try
            {
                using (Bitmap regionBitmap = new Bitmap(region.Width, region.Height))
                {
                    using (Graphics g = Graphics.FromImage(regionBitmap))
                    {
                        g.CopyFromScreen(
                            region.X,
                            region.Y,
                            0,
                            0,
                            region.Size,
                            CopyPixelOperation.SourceCopy);
                    }
                    return ConvertBitmapToBytes(regionBitmap);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"区域捕获失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 捕获指定窗口
        /// </summary>
        public byte[] CaptureWindow(IntPtr windowHandle)
        {
            try
            {
                // 获取窗口矩形
                WinAPI.RECT rect;
                WinAPI.GetWindowRect(windowHandle, out rect);
                
                Rectangle bounds = new Rectangle(
                    rect.Left, 
                    rect.Top, 
                    rect.Right - rect.Left, 
                    rect.Bottom - rect.Top);

                return CaptureRegion(bounds);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"窗口捕获失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 将Bitmap转换为字节数组
        /// </summary>
        private byte[] ConvertBitmapToBytes(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // 设置JPEG编码参数
                ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                // 压缩图像
                bitmap.Save(stream, jpegCodec, encoderParams);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 获取图像编码器
        /// </summary>
        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType == mimeType)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// 优化的屏幕捕获（只捕获变化的部分）
        /// </summary>
        public byte[] CaptureScreenDiff(Bitmap previousScreen)
        {
            if (previousScreen == null)
            {
                return CaptureScreen();
            }

            try
            {
                RefreshScreenBounds();

                // 捕获当前屏幕
                Bitmap currentScreen = new Bitmap(screenBounds.Width, screenBounds.Height);
                using (Graphics g = Graphics.FromImage(currentScreen))
                {
                    g.CopyFromScreen(screenBounds.X, screenBounds.Y, 0, 0, screenBounds.Size);
                }

                // 找出变化区域
                Rectangle diffRegion = FindDifferenceRegion(previousScreen, currentScreen);
                
                if (diffRegion.IsEmpty)
                {
                    return null; // 没有变化
                }

                // 只传输变化的部分
                using (Bitmap diffBitmap = new Bitmap(diffRegion.Width, diffRegion.Height))
                {
                    using (Graphics g = Graphics.FromImage(diffBitmap))
                    {
                        g.DrawImage(currentScreen, 
                            new Rectangle(0, 0, diffRegion.Width, diffRegion.Height),
                            diffRegion, GraphicsUnit.Pixel);
                    }
                    return ConvertBitmapToBytes(diffBitmap);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"差异捕获失败: {ex.Message}");
                return CaptureScreen();
            }
        }

        /// <summary>
        /// 查找差异区域
        /// </summary>
        private Rectangle FindDifferenceRegion(Bitmap bitmap1, Bitmap bitmap2)
        {
            int minX = bitmap1.Width;
            int minY = bitmap1.Height;
            int maxX = 0;
            int maxY = 0;
            bool foundDifference = false;

            // 简单的差异检测（可以优化）
            for (int y = 0; y < bitmap1.Height; y += 10)
            {
                for (int x = 0; x < bitmap1.Width; x += 10)
                {
                    if (bitmap1.GetPixel(x, y) != bitmap2.GetPixel(x, y))
                    {
                        foundDifference = true;
                        minX = Math.Min(minX, x);
                        minY = Math.Min(minY, y);
                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }

            if (!foundDifference)
            {
                return Rectangle.Empty;
            }

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    graphics?.Dispose();
                    screenBitmap?.Dispose();
                }
                disposed = true;
            }
        }
    }
}
