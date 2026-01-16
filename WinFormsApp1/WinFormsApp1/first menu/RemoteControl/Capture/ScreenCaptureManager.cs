using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Management;

namespace WinFormsApp1.first_menu.RemoteControl
{
    /// <summary>
    /// 屏幕捕获管理器 - 支持VMware/虚拟机环境
    /// </summary>
    public class ScreenCaptureManager : IDisposable
    {
        private int quality = 70;
        private Rectangle screenBounds;
        private Bitmap screenBitmap;
        private Graphics graphics;
        private bool disposed = false;
        private bool useWinApiCapture = false;  // 是否使用WinAPI捕获（兼容VMware）
        private int captureFailCount = 0;       // 连续捕获失败次数

        // WinAPI 常量和方法
        private const int SRCCOPY = 0x00CC0020;
        private const int CAPTUREBLT = 0x40000000;

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int width, int height,
            IntPtr hdcSrc, int xSrc, int ySrc, int rop);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int width, int height);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr hdc);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        public int Quality
        {
            get { return quality; }
            set { quality = Math.Max(10, Math.Min(100, value)); }
        }

        /// <summary>
        /// 强制使用WinAPI捕获模式（用于VMware/虚拟机环境）
        /// </summary>
        public void ForceWinApiMode(bool enable)
        {
            useWinApiCapture = enable;
            Console.WriteLine($"[ScreenCapture] WinAPI模式: {(enable ? "启用" : "禁用")}");
        }

        public ScreenCaptureManager()
        {
            screenBounds = Screen.PrimaryScreen.Bounds;
            InitializeCapture();
            
            // 在虚拟机环境下默认使用WinAPI捕获（更稳定）
            useWinApiCapture = IsVirtualMachine();
            if (useWinApiCapture)
            {
                Console.WriteLine("[ScreenCapture] 检测到虚拟机环境，使用WinAPI捕获模式");
            }
        }

        /// <summary>
        /// 检测是否在虚拟机环境中运行
        /// </summary>
        private bool IsVirtualMachine()
        {
            try
            {
                // 检测常见虚拟机特征
                string manufacturer = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") ?? "";
                string model = GetSystemModel();
                
                return manufacturer.Contains("Virtual", StringComparison.OrdinalIgnoreCase) ||
                       model.Contains("VMware", StringComparison.OrdinalIgnoreCase) ||
                       model.Contains("VirtualBox", StringComparison.OrdinalIgnoreCase) ||
                       model.Contains("QEMU", StringComparison.OrdinalIgnoreCase) ||
                       model.Contains("Virtual", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取系统型号
        /// </summary>
        private string GetSystemModel()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj["Model"]?.ToString() ?? "";
                    }
                }
            }
            catch
            {
            }
            return "";
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

                // 优先使用WinAPI捕获（兼容VMware/虚拟机）
                if (useWinApiCapture)
                {
                    return CaptureScreenWinApi();
                }

                // 尝试使用GDI+捕获
                graphics.CopyFromScreen(
                    screenBounds.X,
                    screenBounds.Y,
                    0,
                    0,
                    screenBounds.Size,
                    CopyPixelOperation.SourceCopy);

                // 检查是否捕获到黑屏（VMware环境常见问题）
                if (IsBlackScreen(screenBitmap))
                {
                    captureFailCount++;
                    if (captureFailCount >= 3)
                    {
                        Console.WriteLine("[ScreenCapture] 检测到黑屏，切换到WinAPI捕获模式（兼容VMware）");
                        useWinApiCapture = true;
                        return CaptureScreenWinApi();
                    }
                }
                else
                {
                    captureFailCount = 0;
                }

                return ConvertBitmapToBytes(screenBitmap);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"屏幕捕获失败: {ex.Message}");
                // 出错时尝试WinAPI方式
                if (!useWinApiCapture)
                {
                    Console.WriteLine("[ScreenCapture] GDI+捕获失败，切换到WinAPI模式");
                    useWinApiCapture = true;
                    return CaptureScreenWinApi();
                }
                return null;
            }
        }

        /// <summary>
        /// 使用WinAPI捕获屏幕（兼容VMware/虚拟机环境）
        /// </summary>
        private byte[] CaptureScreenWinApi()
        {
            IntPtr desktopWnd = IntPtr.Zero;
            IntPtr desktopDC = IntPtr.Zero;
            IntPtr memoryDC = IntPtr.Zero;
            IntPtr bitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
                // 获取桌面窗口和DC
                desktopWnd = GetDesktopWindow();
                desktopDC = GetWindowDC(desktopWnd);

                // 创建兼容的内存DC
                memoryDC = CreateCompatibleDC(desktopDC);

                // 创建兼容的位图
                bitmap = CreateCompatibleBitmap(desktopDC, screenBounds.Width, screenBounds.Height);
                oldBitmap = SelectObject(memoryDC, bitmap);

                // 使用BitBlt复制屏幕内容（包含CAPTUREBLT标志以支持分层窗口）
                bool success = BitBlt(
                    memoryDC, 0, 0, screenBounds.Width, screenBounds.Height,
                    desktopDC, screenBounds.X, screenBounds.Y,
                    SRCCOPY | CAPTUREBLT);

                if (!success)
                {
                    Console.WriteLine("[ScreenCapture] BitBlt 失败");
                    return null;
                }

                // 从HBITMAP创建Bitmap对象
                Bitmap capturedBitmap = Image.FromHbitmap(bitmap);
                byte[] result = ConvertBitmapToBytes(capturedBitmap);
                capturedBitmap.Dispose();

                captureFailCount = 0;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ScreenCapture] WinAPI捕获失败: {ex.Message}");
                return null;
            }
            finally
            {
                // 清理资源
                if (oldBitmap != IntPtr.Zero)
                    SelectObject(memoryDC, oldBitmap);
                if (bitmap != IntPtr.Zero)
                    DeleteObject(bitmap);
                if (memoryDC != IntPtr.Zero)
                    DeleteDC(memoryDC);
                if (desktopDC != IntPtr.Zero && desktopWnd != IntPtr.Zero)
                    ReleaseDC(desktopWnd, desktopDC);
            }
        }

        /// <summary>
        /// 检测是否为黑屏
        /// </summary>
        private bool IsBlackScreen(Bitmap bitmap)
        {
            try
            {
                // 采样检测（检测几个点是否都是黑色）
                int sampleCount = 0;
                int blackCount = 0;
                int step = Math.Max(bitmap.Width / 10, 1);

                for (int y = 0; y < bitmap.Height; y += step)
                {
                    for (int x = 0; x < bitmap.Width; x += step)
                    {
                        Color pixel = bitmap.GetPixel(x, y);
                        sampleCount++;
                        if (pixel.R < 10 && pixel.G < 10 && pixel.B < 10)
                        {
                            blackCount++;
                        }
                    }
                }

                // 如果超过90%的采样点都是黑色，认为是黑屏
                return sampleCount > 0 && (blackCount * 100.0 / sampleCount) > 90;
            }
            catch
            {
                return false;
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
