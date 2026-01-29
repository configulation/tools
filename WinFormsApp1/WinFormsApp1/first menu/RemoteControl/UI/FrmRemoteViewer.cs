using System;
using System.Drawing;
using System.Windows.Forms;
using Sunny.UI;

namespace WinFormsApp1.first_menu.RemoteControl
{
    public class FrmRemoteViewer : UIForm
    {
        private readonly UITabControl tabControl;
        private readonly TabPage tabDesktop;
        private readonly TabPage tabLog;
        private readonly PictureBox pictureBox;
        private readonly UITextBox textBoxLog;

        private readonly UIPanel panelTop;
        private readonly UILabel lblAppTitle;
        private readonly UILabel lblLatency;
        private readonly UILabel lblInfo;
        private readonly UIButton btnConnectState;
        private readonly UIButton btnToggleView;
        private readonly UIButton btnFullScreen;

        private bool isFullScreen;
        private FormBorderStyle normalBorderStyle;
        private FormWindowState normalWindowState;
        private Rectangle normalBounds;
        private bool normalTopMost;
        private bool normalShowTitle;
        private Padding normalPadding;

        public PictureBox ScreenPictureBox => pictureBox;

        public FrmRemoteViewer()
        {
            Text = "远程控制";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(900, 600);
            KeyPreview = true;
            AllowShowTitle = true;
            ShowTitle = true;
            Padding = new Padding(0, 35, 0, 0);
            Style = UIStyle.Custom;

            panelTop = new UIPanel
            {
                Dock = DockStyle.Top,
                Height = 48,
                FillColor = Color.FromArgb(34, 36, 40),
                RectColor = Color.FromArgb(34, 36, 40),
                Font = new Font("微软雅黑", 12F),
                Padding = new Padding(12, 8, 12, 8),
                Style = UIStyle.Custom
            };

            lblAppTitle = new UILabel
            {
                Text = "远程",
                Font = new Font("微软雅黑", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(12, 12),
                Size = new Size(60, 24),
                TextAlign = ContentAlignment.MiddleLeft,
                Style = UIStyle.Custom
            };

            lblLatency = new UILabel
            {
                Text = "--ms",
                Font = new Font("微软雅黑", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(108, 117, 125),
                Location = new Point(80, 14),
                Size = new Size(70, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                Style = UIStyle.Custom
            };

            btnFullScreen = new UIButton
            {
                Text = "全屏",
                ForeColor = Color.White,
                Font = new Font("微软雅黑", 9F),
                Size = new Size(80, 28),
                Cursor = Cursors.Hand,
                FillColor = Color.FromArgb(52, 58, 64),
                RectColor = Color.FromArgb(52, 58, 64),
                Style = UIStyle.Custom
            };

            btnConnectState = new UIButton
            {
                Text = "连接",
                ForeColor = Color.White,
                Font = new Font("微软雅黑", 9F, FontStyle.Bold),
                Size = new Size(90, 28),
                Cursor = Cursors.Hand,
                FillColor = Color.FromArgb(0, 123, 255),
                RectColor = Color.FromArgb(0, 123, 255),
                Style = UIStyle.Custom
            };

            btnToggleView = new UIButton
            {
                Text = "日志",
                ForeColor = Color.White,
                Font = new Font("微软雅黑", 9F),
                Size = new Size(80, 28),
                Cursor = Cursors.Hand,
                FillColor = Color.FromArgb(52, 58, 64),
                RectColor = Color.FromArgb(52, 58, 64),
                Style = UIStyle.Custom
            };

            lblInfo = new UILabel
            {
                Text = "分辨率: -  |  FPS: -  |  延迟: -",
                Font = new Font("微软雅黑", 9F),
                ForeColor = Color.FromArgb(210, 210, 210),
                Location = new Point(160, 12),
                Size = new Size(520, 24),
                TextAlign = ContentAlignment.MiddleCenter,
                Style = UIStyle.Custom
            };

            panelTop.Controls.Add(lblAppTitle);
            panelTop.Controls.Add(lblLatency);
            panelTop.Controls.Add(lblInfo);
            panelTop.Controls.Add(btnConnectState);
            panelTop.Controls.Add(btnToggleView);
            panelTop.Controls.Add(btnFullScreen);

            tabControl = new UITabControl
            {
                Dock = DockStyle.Fill,
                Appearance = TabAppearance.FlatButtons,
                DrawMode = TabDrawMode.OwnerDrawFixed,
                ItemSize = new Size(0, 1),
                SizeMode = TabSizeMode.Fixed,
                Font = new Font("微软雅黑", 12F),
                Style = UIStyle.Custom
            };

            tabControl.Multiline = true;
            tabControl.Padding = new Point(0, 0);
            tabControl.Margin = new Padding(0);
            tabControl.BackColor = Color.Black;

            tabDesktop = new TabPage("电脑")
            {
                Padding = new Padding(0)
            };

            tabDesktop.BackColor = Color.Black;

            tabLog = new TabPage("日志")
            {
                Padding = new Padding(6)
            };

            tabLog.BackColor = Color.FromArgb(20, 20, 22);

            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                SizeMode = PictureBoxSizeMode.Zoom,
                TabStop = true
            };

            pictureBox.MouseEnter += (s, e) => pictureBox.Focus();
            pictureBox.MouseDown += (s, e) => pictureBox.Focus();

            textBoxLog = new UITextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                Font = new Font("Consolas", 9F),
                FillColor = Color.FromArgb(20, 20, 22),
                ForeColor = Color.Gainsboro,
                RectColor = Color.FromArgb(20, 20, 22),
                ShowScrollBar = true,
                Style = UIStyle.Custom,
                Padding = new Padding(6),
                ShowText = false,
                TextAlignment = ContentAlignment.TopLeft
            };

            tabDesktop.Controls.Add(pictureBox);
            tabLog.Controls.Add(textBoxLog);
            tabControl.TabPages.Add(tabDesktop);
            tabControl.TabPages.Add(tabLog);

            Controls.Add(tabControl);
            Controls.Add(panelTop);

            btnFullScreen.Click += (s, e) => ToggleFullScreen();
            btnToggleView.Click += (s, e) => ToggleView();

            panelTop.SizeChanged += (s, e) => ApplyTopLayout();
            ApplyTopLayout();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (isFullScreen && keyData == Keys.Escape)
            {
                ToggleFullScreen();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ApplyTopLayout()
        {
            int right = panelTop.Width - 12;

            btnFullScreen.Location = new Point(right - btnFullScreen.Width, 10);
            right -= btnFullScreen.Width + 8;

            btnToggleView.Location = new Point(right - btnToggleView.Width, 10);
            right -= btnToggleView.Width + 8;

            btnConnectState.Location = new Point(right - btnConnectState.Width, 10);
            right -= btnConnectState.Width + 12;

            int infoLeft = lblLatency.Right + 10;
            int infoWidth = Math.Max(120, right - infoLeft);
            lblInfo.Location = new Point(infoLeft, 12);
            lblInfo.Size = new Size(infoWidth, 24);
        }

        private void ToggleView()
        {
            if (tabControl.SelectedTab == tabDesktop)
            {
                tabControl.SelectedTab = tabLog;
                btnToggleView.Text = "电脑";
            }
            else
            {
                tabControl.SelectedTab = tabDesktop;
                btnToggleView.Text = "日志";
                pictureBox.Focus();
            }
        }

        private void ToggleFullScreen()
        {
            if (!isFullScreen)
            {
                isFullScreen = true;
                btnFullScreen.Text = "还原";

                normalBorderStyle = FormBorderStyle;
                normalWindowState = WindowState;
                normalBounds = Bounds;
                normalTopMost = TopMost;
                normalShowTitle = ShowTitle;
                normalPadding = Padding;

                TopMost = true;
                ShowTitle = false;
                Padding = new Padding(0);

                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.None;

                var screenBounds = Screen.FromControl(this).Bounds;
                Bounds = screenBounds;
                WindowState = FormWindowState.Maximized;
                BringToFront();
            }
            else
            {
                isFullScreen = false;
                btnFullScreen.Text = "全屏";

                WindowState = FormWindowState.Normal;
                FormBorderStyle = normalBorderStyle;
                ShowTitle = normalShowTitle;
                Padding = normalPadding;
                TopMost = normalTopMost;

                Bounds = normalBounds;
                WindowState = normalWindowState;
            }

            ApplyTopLayout();
        }

        public void SetStatus(bool connected)
        {
            if (IsDisposed) return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<bool>(SetStatus), connected);
                return;
            }

            btnConnectState.Text = connected ? "已连接" : "连接";
            btnConnectState.FillColor = connected ? Color.FromArgb(40, 167, 69) : Color.FromArgb(0, 123, 255);
            btnConnectState.RectColor = btnConnectState.FillColor;
        }

        public void SetLatency(int ms)
        {
            if (IsDisposed) return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<int>(SetLatency), ms);
                return;
            }

            if (ms < 0)
            {
                lblLatency.Text = "--ms";
                lblLatency.BackColor = Color.FromArgb(108, 117, 125);
                return;
            }

            lblLatency.Text = $"{ms}ms";
            if (ms <= 80)
            {
                lblLatency.BackColor = Color.FromArgb(40, 167, 69);
            }
            else if (ms <= 150)
            {
                lblLatency.BackColor = Color.FromArgb(255, 193, 7);
            }
            else
            {
                lblLatency.BackColor = Color.FromArgb(220, 53, 69);
            }
        }

        public void SetInfo(string info)
        {
            if (IsDisposed) return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(SetInfo), info);
                return;
            }

            lblInfo.Text = info;
        }

        public void AppendLog(string line)
        {
            if (IsDisposed) return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(AppendLog), line);
                return;
            }

            textBoxLog.Text += line;
            try
            {
                textBoxLog.SelectionStart = textBoxLog.Text.Length;
                textBoxLog.ScrollToCaret();
            }
            catch
            {
                // ignore
            }
        }

        public void HideTopPanel()
        {
            if (IsDisposed) return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(HideTopPanel));
                return;
            }

            panelTop.Visible = false;
        }

        public void ShowTopPanel()
        {
            if (IsDisposed) return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(ShowTopPanel));
                return;
            }

            panelTop.Visible = true;
        }
    }
}
