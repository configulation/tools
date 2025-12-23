using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinFormsApp1.first_menu.ChessAnalyzer.Utils
{
    /// <summary>
    /// 屏幕截图工具类
    /// </summary>
    public static class ScreenCapture
    {
        /// <summary>
        /// 截取屏幕指定区域
        /// </summary>
        public static Bitmap CaptureScreen(Rectangle area)
        {
            try
            {
                Bitmap bmp = new Bitmap(area.Width, area.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(area.Left, area.Top, 0, 0, area.Size, CopyPixelOperation.SourceCopy);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                throw new Exception($"截图失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 截取全屏
        /// </summary>
        public static Bitmap CaptureFullScreen()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            return CaptureScreen(bounds);
        }

        /// <summary>
        /// 截取窗口
        /// </summary>
        public static Bitmap CaptureWindow(IntPtr hWnd)
        {
            RECT rect;
            GetWindowRect(hWnd, out rect);
            Rectangle area = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            return CaptureScreen(area);
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }

    /// <summary>
    /// 区域选择器窗口
    /// </summary>
    public class AreaSelectorForm : Form
    {
        private Point _startPoint;
        private Rectangle _selectedArea;
        private bool _isSelecting;
        private Bitmap _screenshot;

        public Rectangle SelectedArea => _selectedArea;

        public AreaSelectorForm()
        {
            // 截取当前屏幕
            _screenshot = ScreenCapture.CaptureFullScreen();

            // 窗口设置
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.Cursor = Cursors.Cross;
            this.DoubleBuffered = true;
            this.BackColor = Color.Black;
            this.Opacity = 0.3;

            // 设置背景图
            this.BackgroundImage = _screenshot;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // 事件
            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;
            this.Paint += OnPaint;
            this.KeyDown += OnKeyDown;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isSelecting = true;
                _startPoint = e.Location;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isSelecting)
            {
                int x = Math.Min(_startPoint.X, e.X);
                int y = Math.Min(_startPoint.Y, e.Y);
                int width = Math.Abs(e.X - _startPoint.X);
                int height = Math.Abs(e.Y - _startPoint.Y);

                _selectedArea = new Rectangle(x, y, width, height);
                this.Invalidate();
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _isSelecting)
            {
                _isSelecting = false;

                if (_selectedArea.Width > 10 && _selectedArea.Height > 10)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            if (_selectedArea.Width > 0 && _selectedArea.Height > 0)
            {
                // 绘制选中区域边框
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    e.Graphics.DrawRectangle(pen, _selectedArea);
                }

                // 显示尺寸
                string sizeText = $"{_selectedArea.Width} x {_selectedArea.Height}";
                using (Font font = new Font("Arial", 12))
                using (SolidBrush brush = new SolidBrush(Color.Yellow))
                {
                    e.Graphics.DrawString(sizeText, font, brush, _selectedArea.X, _selectedArea.Y - 20);
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _screenshot?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
