using System;
using System.Drawing;
using System.Windows.Forms;
using Sunny.UI;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WinFormsApp1.first_menu.RemoteControl
{
    public partial class FrmRemoteControl : UIForm
    {
        private NetworkManager networkManager;
        private ScreenCaptureManager screenCapture;
        private DifferentialCapture differentialCapture;  // 专业级差异捕获
        private NetworkProtocol networkProtocol;           // 专业级网络协议
        private AdaptiveFrameController frameController;   // 自适应帧率控制
        private RemoteControlManager remoteControl;
        private ClipboardManager clipboardManager;         // 剪贴板管理器
        private ConnectionHistoryManager historyManager;   // 连接历史管理器
        private bool isHost = false;
        private bool isConnected = false;
        private string localDeviceCode = "";
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Timer statsTimer;     // 统计更新定时器
        private bool isFullScreen = false;                 // 全屏状态
        private FormWindowState previousWindowState;       // 记录全屏前的窗口状态
        private Rectangle previousBounds;                  // 记录全屏前的窗口位置大小
        private Size remoteScreenSize = Screen.PrimaryScreen.Bounds.Size;  // 远程屏幕分辨率

        private FrmRemoteViewer remoteViewerForm;
        private PictureBox remoteViewerPictureBox;
        private bool isClosingRemoteViewer;
        private PictureBox fullScreenSourcePictureBox;

        private bool isCompactMode;
        private Size normalClientSize;
        private Size normalMinimumSize;
        private DockStyle normalPanelTitleDock;
        private int normalPanelTitleWidth;
        private bool normalBtnTestModeVisible;
        private bool normalBtnFullScreenVisible;
        private bool normalLblConnectionStatusVisible;

        private bool isSendingFrame;

        private bool topBarLayoutInited;
        private int latencyMs = -1;

        private bool ShouldCompactLayout()
        {
            if (isHost)
            {
                return false;
            }

            return isConnected || (remoteViewerForm != null && !remoteViewerForm.IsDisposed);
        }

        public FrmRemoteControl()
        {
            InitializeComponent();
            InitializeRemoteControl();

            InitTopBarLayout();

            this.Resize += FrmRemoteControl_Resize;
            this.Load += FrmRemoteControl_Load;
        }

        private void FrmRemoteControl_Load(object sender, EventArgs e)
        {
            AdjustPanelLeftLayout();
            AdjustRightPanelLayout();
        }

        private void FrmRemoteControl_Resize(object sender, EventArgs e)
        {
            AdjustPanelLeftLayout();
            AdjustRightPanelLayout();
        }

        private void AdjustRightPanelLayout()
        {
            // panelBottom 现在填充整个右侧区域，不需要调整高度
            // 日志框会自动填充整个区域
        }

        private void AdjustPanelLeftLayout()
        {
            if (panelLeft == null || groupBoxControl == null || groupBoxSettings == null)
            {
                return;
            }

            int availableHeight = panelLeft.ClientSize.Height - panelLeft.Padding.Top - panelLeft.Padding.Bottom;
            int controlIdealHeight = 250;
            int settingsIdealHeight = 100;
            int totalNeededHeight = controlIdealHeight + settingsIdealHeight;

            if (availableHeight >= totalNeededHeight)
            {
                groupBoxControl.Height = controlIdealHeight;
                groupBoxSettings.Height = settingsIdealHeight;
            }
            else if (availableHeight >= 250)
            {
                groupBoxSettings.Height = settingsIdealHeight;
                groupBoxControl.Height = availableHeight - settingsIdealHeight;
            }
            else if (availableHeight >= 180)
            {
                groupBoxSettings.Height = 80;
                groupBoxControl.Height = availableHeight - 80;
            }
            else
            {
                groupBoxControl.Height = Math.Max(120, availableHeight);
                groupBoxSettings.Height = 0;
                groupBoxSettings.Visible = false;
                return;
            }

            groupBoxSettings.Visible = true;
        }

        private void SafeBeginInvoke(Action action)
        {
            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }

            try
            {
                BeginInvoke(action);
            }
            catch
            {
                // ignore
            }
        }

        private void InitializeRemoteControl()
        {
            // 加载配置
            var config = ConfigManager.Instance;
            
            // 初始化网络管理器
            networkManager = new NetworkManager();
            networkManager.OnConnectionStatusChanged += OnConnectionStatusChanged;
            networkManager.OnScreenDataReceived += OnScreenDataReceived;
            networkManager.OnScreenInfoReceived += OnScreenInfoReceived;
            networkManager.OnLatencyUpdated += OnLatencyUpdated;
            networkManager.OnClipboardDataReceived += OnClipboardDataReceived;
            
            // 初始化剪贴板管理器
            clipboardManager = new ClipboardManager();
            clipboardManager.ClipboardChanged += OnLocalClipboardChanged;
            
            // 初始化历史记录管理器
            historyManager = ConnectionHistoryManager.Instance;
            LoadConnectionHistory();
            
            // 初始化专业级屏幕捕获
            screenCapture = new ScreenCaptureManager();
            screenCapture.Quality = config.ScreenQuality;
            
            // 初始化差异捕获引擎
            differentialCapture = new DifferentialCapture();
            
            // 初始化自适应帧率控制器
            frameController = new AdaptiveFrameController();
            
            // 初始化远程控制
            remoteControl = new RemoteControlManager();
            
            // 生成设备代码
            GenerateDeviceCode();
            
            // 初始化更新定时器
            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 50; // 20fps
            updateTimer.Tick += UpdateTimer_Tick;
            
            // 初始化统计定时器
            statsTimer = new System.Windows.Forms.Timer();
            statsTimer.Interval = 1000; // 每秒更新一次统计
            statsTimer.Tick += StatsTimer_Tick;
            statsTimer.Start();
            
            // 设置UI初始值
            uiTrackBarQuality.Value = config.ScreenQuality;
            uiTrackBarFPS.Value = config.ScreenFPS;
        }
        
        private void LoadConnectionHistory()
        {
            try
            {
                var history = historyManager.GetSortedHistory();
                uiComboBoxRemoteCode.Items.Clear();
                
                foreach (var item in history)
                {
                    uiComboBoxRemoteCode.Items.Add(item.DeviceCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载历史记录失败: {ex.Message}");
            }
        }

        private void uiComboBoxRemoteCode_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = uiComboBoxRemoteCode.SelectedIndex;
                if (index >= 0 && index < uiComboBoxRemoteCode.Items.Count)
                {
                    string deviceCode = uiComboBoxRemoteCode.Items[index].ToString();

                    bool result = UIMessageBox.Show(
                        $"确定要删除历史记录 '{deviceCode}' 吗?",
                        "删除确认",
                        UIStyle.Blue,
                        UIMessageBoxButtons.OKCancel
                    );

                    if (result)  // 用户点击了"确定"
                    {
                        historyManager.RemoveConnection(deviceCode);
                        LoadConnectionHistory();
                        AddLog($"[{DateTime.Now:HH:mm:ss}] 已删除历史记录: {deviceCode}\r\n");
                    }
                }
            }
        }

        private void GenerateDeviceCode()
        {
            // 生成包含IP的设备码
            string localIP = NetworkHelper.GetLocalIPAddress();
            Random random = new Random();
            localDeviceCode = random.Next(100000, 999999).ToString();
              
            // 测试阶段使用固定设备码，方便测试
            #if DEBUG
            string envDeviceCode = Environment.GetEnvironmentVariable("RC_DEVICE_CODE");
            if (!string.IsNullOrWhiteSpace(envDeviceCode) && envDeviceCode.Length == 6 && int.TryParse(envDeviceCode, out _))
            {
                localDeviceCode = envDeviceCode;
            }
            else
            {
                localDeviceCode = random.Next(100000, 999999).ToString();
            }
            #else
            localDeviceCode = random.Next(100000, 999999).ToString();
            #endif
            
            // 显示设备码（使用UILabel控件）
            lblDeviceCode.Text = localDeviceCode;

            try
            {
                networkManager?.SetDeviceCode(localDeviceCode);
            }
            catch
            {
            }
            
            // 显示本机IP信息
            lblTitle.Text = $"远程控制 - 本机IP: {localIP}";
        }

        private void OnConnectionStatusChanged(bool connected, string message)
        {
            SafeBeginInvoke(() =>
            {
                isConnected = connected;
                lblConnectionStatus.Text = connected ? "已连接" : "未连接";
                lblConnectionStatus.ForeColor = connected ? Color.Green : Color.Red;
                AddLog($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");

                remoteViewerForm?.SetStatus(connected);

                if (!connected)
                {
                    latencyMs = -1;
                    remoteViewerForm?.SetLatency(-1);
                    clipboardManager?.StopMonitoring();
                }

                UpdateRemoteViewerInfo();
                
                btnStartHost.Text = isHost ? "停止受控" : "开始受控";
                btnConnect.Text = (!isHost && connected) ? "断开连接" : "连接";

                if (!connected)
                {
                    updateTimer.Stop();
                }
                else
                {
                    if (!isHost)
                    {
                        EnsureRemoteViewerOpened();
                    }

                    if (isHost)
                    {
                        updateTimer.Start();
                    }
                    
                    clipboardManager?.StartMonitoring();
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 剪贴板同步已启动\r\n");
                }

                ApplyCompactLayout(ShouldCompactLayout());
                
                UpdateControlStatus();
            });
        }

        private void OnLatencyUpdated(int ms)
        {
            latencyMs = ms;
            remoteViewerForm?.SetLatency(ms);
            UpdateRemoteViewerInfo();
        }

        private void OnLocalClipboardChanged(object sender, ClipboardChangedEventArgs e)
        {
            if (!isConnected)
                return;

            try
            {
                Task.Run(async () =>
                {
                    await networkManager.SendClipboardDataAsync(e.Data);
                    SafeBeginInvoke(() =>
                    {
                        string typeDesc = e.Data.Type == ClipboardDataType.Text ? "文本" :
                                         e.Data.Type == ClipboardDataType.Image ? "图片" : "文件";
                        AddLog($"[{DateTime.Now:HH:mm:ss}] 发送剪贴板数据: {typeDesc} ({e.Data.GetSizeDescription()})\r\n");
                    });
                });
            }
            catch (Exception ex)
            {
                AddLog($"[{DateTime.Now:HH:mm:ss}] 发送剪贴板数据失败: {ex.Message}\r\n");
            }
        }

        private void OnClipboardDataReceived(ClipboardData data)
        {
            SafeBeginInvoke(() =>
            {
                try
                {
                    string typeDesc = data.Type == ClipboardDataType.Text ? "文本" :
                                     data.Type == ClipboardDataType.Image ? "图片" : "文件";
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 接收剪贴板数据: {typeDesc} ({data.GetSizeDescription()})\r\n");
                    
                    bool success = clipboardManager.SetClipboardData(data);
                    if (success)
                    {
                        AddLog($"[{DateTime.Now:HH:mm:ss}] 剪贴板数据已应用\r\n");
                    }
                    else
                    {
                        AddLog($"[{DateTime.Now:HH:mm:ss}] 剪贴板数据应用失败\r\n");
                    }
                }
                catch (Exception ex)
                {
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 处理剪贴板数据失败: {ex.Message}\r\n");
                }
            });
        }

        private void UpdateRemoteViewerInfo()
        {
            if (remoteViewerForm == null || remoteViewerForm.IsDisposed)
            {
                return;
            }

            string latencyText = latencyMs >= 0 ? $"{latencyMs}ms" : "-";
            remoteViewerForm.SetInfo($"分辨率: {remoteScreenSize.Width}x{remoteScreenSize.Height}  |  帧率: {uiTrackBarFPS.Value}FPS  |  质量: {uiTrackBarQuality.Value}%  |  延迟: {latencyText}");
        }

        private void InitTopBarLayout()
        {
            if (topBarLayoutInited)
            {
                return;
            }

            topBarLayoutInited = true;

            panelTop.SizeChanged += (s, e) => ApplyTopBarLayout();
            panelTitle.SizeChanged += (s, e) => ApplyTopBarLayout();
            ApplyTopBarLayout();
        }

        private void ApplyTopBarLayout()
        {
            if (btnTestMode.Parent != panelTitle)
            {
                btnTestMode.Parent = panelTitle;
            }

            int btnY = Math.Max(10, (panelTitle.Height - btnTestMode.Height) / 2);
            btnTestMode.Location = new Point(10, btnY);

            int titleX = btnTestMode.Right + 10;
            lblTitle.Location = new Point(titleX, lblTitle.Location.Y);
            lblTitle.Width = Math.Max(120, panelTitle.Width - titleX - 10);
        }

        private void ApplyCompactLayout(bool compact)
        {
            if (isCompactMode == compact)
            {
                return;
            }

            if (compact)
            {
                normalClientSize = ClientSize;
                normalMinimumSize = MinimumSize;
                normalPanelTitleDock = panelTitle.Dock;
                normalPanelTitleWidth = panelTitle.Width;
                normalBtnTestModeVisible = btnTestMode.Visible;
                normalBtnFullScreenVisible = btnFullScreen.Visible;
                normalLblConnectionStatusVisible = lblConnectionStatus.Visible;

                // panelMain 已经默认隐藏，不需要再设置
                panelBottom.Visible = false;
                pictureBoxScreen.Visible = false;

                btnTestMode.Visible = false;
                btnFullScreen.Visible = false;
                lblConnectionStatus.Visible = false;

                panelTitle.Dock = DockStyle.Fill;
                panelTitle.Width = panelLeft.Width;

                ClientSize = new Size(panelLeft.Width, normalClientSize.Height);
                MinimumSize = new Size(panelLeft.Width, normalMinimumSize.Height);
            }
            else
            {
                // panelMain 保持隐藏状态
                panelBottom.Visible = true;
                pictureBoxScreen.Visible = false;  // 保持隐藏

                btnTestMode.Visible = normalBtnTestModeVisible;
                btnFullScreen.Visible = normalBtnFullScreenVisible;
                lblConnectionStatus.Visible = normalLblConnectionStatusVisible;

                panelTitle.Dock = normalPanelTitleDock;
                panelTitle.Width = normalPanelTitleWidth;

                ClientSize = normalClientSize;
                MinimumSize = normalMinimumSize;
            }

            isCompactMode = compact;
        }

        private void AddLog(string message)
        {
            string rolePrefix = isHost ? "[HOST] " : "[CTRL] ";
            string finalMessage = rolePrefix + message;

            uiTextBoxLog.AppendText(finalMessage);

            try
            {
                uiTextBoxLog.SelectionStart = uiTextBoxLog.TextLength;
                uiTextBoxLog.ScrollToCaret();
            }
            catch
            {
            }

            remoteViewerForm?.AppendLog(finalMessage);
        }

        private void EnsureRemoteViewerOpened()
        {
            if (remoteViewerForm != null && !remoteViewerForm.IsDisposed)
            {
                remoteViewerForm.Show();
                remoteViewerForm.Activate();
                return;
            }

            remoteViewerForm = new FrmRemoteViewer();
            remoteViewerPictureBox = remoteViewerForm.ScreenPictureBox;

            remoteViewerPictureBox.MouseMove += pictureBoxScreen_MouseMove;
            remoteViewerPictureBox.MouseDown += pictureBoxScreen_MouseDown;
            remoteViewerPictureBox.MouseUp += pictureBoxScreen_MouseUp;
            remoteViewerPictureBox.KeyDown += pictureBoxScreen_KeyDown;
            remoteViewerPictureBox.KeyUp += pictureBoxScreen_KeyUp;

            remoteViewerForm.FormClosed += (s, e) =>
            {
                remoteViewerPictureBox = null;
                remoteViewerForm = null;

                if (!isClosingRemoteViewer && isConnected && !isHost)
                {
                    Task.Run(async () => { await networkManager.StopAsync(); });
                }
            };

            remoteViewerForm.Show(this);
        }

        private void CloseRemoteViewer()
        {
            if (remoteViewerForm == null || remoteViewerForm.IsDisposed)
            {
                remoteViewerForm = null;
                remoteViewerPictureBox = null;
                return;
            }

            try
            {
                isClosingRemoteViewer = true;
                remoteViewerForm.Close();
            }
            finally
            {
                isClosingRemoteViewer = false;
                remoteViewerForm = null;
                remoteViewerPictureBox = null;
            }
        }

        private void OnScreenDataReceived(byte[] data)
        {
            // 在UI线程更新画面
            SafeBeginInvoke(() =>
            {
                try
                {
                    using (var ms = new System.IO.MemoryStream(data))
                    {
                        using (var tmpImage = Image.FromStream(ms))
                        {
                            var newImage = new Bitmap(tmpImage);

                            // 更新远程屏幕分辨率（图片大小即为远程屏幕大小）
                            if (newImage != null)
                            {
                                remoteScreenSize = newImage.Size;
                            }
                            
                            // 更新显示：如果在全屏模式，更新全屏窗口；否则更新主窗口
                            PictureBox targetPictureBox = GetActiveDisplayPictureBox();
                            targetPictureBox.Image?.Dispose();
                            targetPictureBox.Image = newImage;
                        }
                    }
                }
                catch (Exception ex)
                {
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 屏幕显示错误: {ex.Message}\r\n");
                }
            });
        }

        private PictureBox GetActiveDisplayPictureBox()
        {
            if (isFullScreen && fullScreenPictureBox != null)
            {
                return fullScreenPictureBox;
            }

            if (!isHost && remoteViewerPictureBox != null)
            {
                return remoteViewerPictureBox;
            }

            return pictureBoxScreen;
        }
        
        // 接收远程屏幕信息
        private void OnScreenInfoReceived(Size screenSize)
        {
            SafeBeginInvoke(() =>
            {
                remoteScreenSize = screenSize;
                AddLog($"[{DateTime.Now:HH:mm:ss}] 远程屏幕分辨率: {screenSize.Width}x{screenSize.Height}\r\n");

                UpdateRemoteViewerInfo();
                
                // 更新标题栏显示
                if (!isHost)
                {
                    lblTitle.Text = $"远程控制 - 远程屏幕: {screenSize.Width}x{screenSize.Height}";
                }
            });
        }

        private async void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (isHost && isConnected)
            {
                if (isSendingFrame)
                {
                    return;
                }

                isSendingFrame = true;
                try
                {
                    var screenData = screenCapture.CaptureScreen();
                    if (screenData != null)
                    {
                        await networkManager.SendScreenDataAsync(screenData);
                    }
                }
                catch (Exception ex)
                {
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 屏幕发送错误: {ex.Message}\r\n");
                }
                finally
                {
                    isSendingFrame = false;
                }
            }
        }
        
        // 专业版更新方法
        private DateTime lastCaptureTime = DateTime.Now;
        private async void UpdateTimer_Tick_Professional(object sender, EventArgs e)
        {
            if (isHost && isConnected && networkProtocol != null)
            {
                try
                {
                    // 使用差异捕获
                    var diffResult = differentialCapture.CaptureChanges();
                    
                    // 自适应帧率调整
                    double targetFps = frameController.AnalyzeAndAdjust(diffResult);
                    
                    // 检查是否需要发送（根据目标FPS）
                    var now = DateTime.Now;
                    double elapsed = (now - lastCaptureTime).TotalMilliseconds;
                    double targetInterval = 1000.0 / targetFps;
                    
                    if (elapsed >= targetInterval)
                    {
                        // 发送差异数据
                        if (diffResult.HasChanges)
                        {
                            await networkProtocol.SendDiffBlocksAsync(diffResult);
                        }
                        lastCaptureTime = now;
                    }
                }
                catch (Exception ex)
                {
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 专业捕获错误: {ex.Message}\r\n");
                }
            }
        }
        
        // 统计更新
        private void StatsTimer_Tick(object sender, EventArgs e)
        {
            if (isConnected)
            {
                // 获取性能报告
                string performanceReport = frameController?.GetPerformanceReport() ?? "";
                
                // 获取网络统计
                string networkStats = networkProtocol?.GetStatistics()?.GetFormattedStats() ?? "";
                
                // 更新状态显示
                this.Invoke(new Action(() =>
                {
                    // 可以将统计信息显示在状态栏或日志中
                    lblConnectionStatus.Text = $"已连接 | {performanceReport}";
                    
                    // 如果需要，也可以显示在日志中（但不要太频繁）
                    if (DateTime.Now.Second % 10 == 0) // 每10秒记录一次
                    {
                        AddLog($"[{DateTime.Now:HH:mm:ss}] 性能: {performanceReport}\r\n");
                        AddLog($"[{DateTime.Now:HH:mm:ss}] 网络: {networkStats}\r\n");
                    }
                }));
            }
        }

        private void UpdateControlStatus()
        {
            btnStartHost.Enabled = !isConnected || (isConnected && isHost);
            btnConnect.Enabled = !isConnected || (isConnected && !isHost);
            uiComboBoxRemoteCode.Enabled = !isConnected;
            
            // 只有受控端才能调整质量和帧率
            uiTrackBarQuality.Enabled = isHost;
            uiTrackBarFPS.Enabled = isHost;
        }
        
        // 全屏窗口引用
        private Form fullScreenForm = null;
        private PictureBox fullScreenPictureBox = null;
        private Screen currentFullScreen = null;
        
        // 全屏切换功能（真正的屏幕全屏）
        private void ToggleFullScreen()
        {
            if (!isFullScreen)
            {
                // 选择当前窗体所在屏幕
                currentFullScreen = Screen.FromControl(this);
                
                // 创建新的全屏窗口
                fullScreenForm = new Form
                {
                    StartPosition = FormStartPosition.Manual,
                    FormBorderStyle = FormBorderStyle.None,
                    ShowInTaskbar = false,
                    KeyPreview = true,
                    BackColor = Color.Black,
                    Owner = this
                };
                
                // 创建新的PictureBox
                fullScreenPictureBox = new PictureBox();
                fullScreenPictureBox.Dock = DockStyle.Fill;
                fullScreenPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                fullScreenPictureBox.BackColor = Color.Black;
                fullScreenPictureBox.TabStop = false;
                
                // 移动图片到全屏窗口
                fullScreenSourcePictureBox = GetActiveDisplayPictureBox();
                if (fullScreenSourcePictureBox != null && fullScreenSourcePictureBox.Image != null)
                {
                    fullScreenPictureBox.Image = fullScreenSourcePictureBox.Image;
                    fullScreenSourcePictureBox.Image = null;
                }
                
                // 绑定事件
                fullScreenPictureBox.MouseMove += pictureBoxScreen_MouseMove;
                fullScreenPictureBox.MouseDown += pictureBoxScreen_MouseDown;
                fullScreenPictureBox.MouseUp += pictureBoxScreen_MouseUp;
                fullScreenPictureBox.KeyDown += pictureBoxScreen_KeyDown;
                fullScreenPictureBox.KeyUp += pictureBoxScreen_KeyUp;

                // ESC键退出全屏
                fullScreenForm.KeyDown += FullScreenForm_KeyDown;

                // 双击退出全屏
                fullScreenPictureBox.DoubleClick += FullScreenPictureBox_DoubleClick;
                
                fullScreenForm.Controls.Add(fullScreenPictureBox);
                
                // 先隐藏任务栏和工具栏
                HideTaskbar();
                if (remoteViewerForm != null && !remoteViewerForm.IsDisposed)
                {
                    remoteViewerForm.HideTopPanel();
                }
                
                // 设置窗口位置和大小为整个屏幕
                fullScreenForm.Bounds = currentFullScreen.Bounds;
                fullScreenForm.WindowState = FormWindowState.Maximized;
                fullScreenForm.Show();
                fullScreenForm.BringToFront();
                fullScreenPictureBox.Focus();

                isFullScreen = true;
                
                // 显示提示
                ShowFullScreenTip(fullScreenForm);
            }
            else
            {
                ExitFullScreen();
            }
        }
        
        private void FullScreenForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ToggleFullScreen();
            }
        }

        private void FullScreenPictureBox_DoubleClick(object sender, EventArgs e)
        {
            ToggleFullScreen();
        }



        
        private void ExitFullScreen()
        {
            if (fullScreenForm != null)
            {
                // 恢复图片到原窗口
                if (fullScreenPictureBox != null && fullScreenPictureBox.Image != null)
                {
                    var targetPictureBox = fullScreenSourcePictureBox ?? pictureBoxScreen;
                    targetPictureBox.Image = fullScreenPictureBox.Image;
                    fullScreenPictureBox.Image = null;
                }
                
                // 清理事件绑定
                if (fullScreenPictureBox != null)
                {
                    fullScreenPictureBox.MouseMove -= pictureBoxScreen_MouseMove;
                    fullScreenPictureBox.MouseDown -= pictureBoxScreen_MouseDown;
                    fullScreenPictureBox.MouseUp -= pictureBoxScreen_MouseUp;
                    fullScreenPictureBox.KeyDown -= pictureBoxScreen_KeyDown;
                    fullScreenPictureBox.KeyUp -= pictureBoxScreen_KeyUp;
                    fullScreenPictureBox.DoubleClick -= FullScreenPictureBox_DoubleClick;
                }
                
                fullScreenForm.KeyDown -= FullScreenForm_KeyDown;
                fullScreenForm.Close();
                fullScreenForm.Dispose();
            }
            
            fullScreenForm = null;
            fullScreenPictureBox = null;
            currentFullScreen = null;
            fullScreenSourcePictureBox = null;
            isFullScreen = false;
            
            // 显示远程查看器的工具栏
            if (remoteViewerForm != null && !remoteViewerForm.IsDisposed)
            {
                remoteViewerForm.ShowTopPanel();
            }
            
            // 显示Windows任务栏
            ShowTaskbar();
        }
        
        // 隐藏Windows任务栏
        private void HideTaskbar()
        {
            IntPtr taskbarHandle = WinAPI.FindWindow("Shell_TrayWnd", null);
            if (taskbarHandle != IntPtr.Zero)
            {
                WinAPI.ShowWindow(taskbarHandle, WinAPI.SW_HIDE);
            }
        }
        
        // 显示Windows任务栏
        private void ShowTaskbar()
        {
            IntPtr taskbarHandle = WinAPI.FindWindow("Shell_TrayWnd", null);
            if (taskbarHandle != IntPtr.Zero)
            {
                WinAPI.ShowWindow(taskbarHandle, WinAPI.SW_SHOW);
            }
        }
        
        // 显示全屏提示
        private void ShowFullScreenTip(Form fullScreenForm)
        {
            Label tipLabel = new Label();
            tipLabel.Text = "按 ESC 键或双击退出全屏";
            tipLabel.ForeColor = Color.White;
            tipLabel.BackColor = Color.FromArgb(128, 0, 0, 0);
            tipLabel.AutoSize = true;
            tipLabel.Padding = new Padding(10);
            tipLabel.Font = new Font("微软雅黑", 12F);
            tipLabel.Location = new Point(
                (fullScreenForm.Width - 250) / 2,
                20
            );
            
            fullScreenForm.Controls.Add(tipLabel);
            tipLabel.BringToFront();
            
            // 3秒后自动隐藏提示
            System.Windows.Forms.Timer hideTimer = new System.Windows.Forms.Timer();
            hideTimer.Interval = 3000;
            hideTimer.Tick += (s, e) =>
            {
                tipLabel.Dispose();
                hideTimer.Stop();
                hideTimer.Dispose();
            };
            hideTimer.Start();
        }
        
        // 添加全屏按钮点击事件
        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            ToggleFullScreen();
        }
        
        // ESC键退出全屏
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape && isFullScreen)
            {
                ToggleFullScreen();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private async void btnStartHost_Click(object sender, EventArgs e)
        {
            // 防止重复点击
            if (btnStartHost.Enabled == false)
            {
                return;
            }

            btnStartHost.Enabled = false;
            
            try
            {
                if (!isHost)
                {
                    // 确保网络管理器已初始化
                    if (networkManager == null)
                    {
                        AddLog($"[{DateTime.Now:HH:mm:ss}] 初始化网络管理器...\r\n");
                        networkManager = new NetworkManager();
                        networkManager.OnConnectionStatusChanged += OnConnectionStatusChanged;
                        networkManager.OnScreenDataReceived += OnScreenDataReceived;
                        networkManager.OnScreenInfoReceived += OnScreenInfoReceived;
                        networkManager.OnLatencyUpdated += OnLatencyUpdated;
                    }

                    // 确保屏幕捕获已初始化
                    if (screenCapture == null)
                    {
                        AddLog($"[{DateTime.Now:HH:mm:ss}] 初始化屏幕捕获...\r\n");
                        screenCapture = new ScreenCaptureManager();
                        screenCapture.Quality = ConfigManager.Instance.ScreenQuality;
                    }

                    // 设置设备码
                    if (string.IsNullOrEmpty(localDeviceCode) || localDeviceCode == "000000")
                    {
                        GenerateDeviceCode();
                    }

                    isHost = true;
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 正在启动受控模式...\r\n");
                    
                    await networkManager.StartHostAsync(localDeviceCode);

                    string localIP = NetworkHelper.GetLocalIPAddress();
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 已启动受控模式\r\n");
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 设备码：{localDeviceCode}\r\n");
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 本机IP：{localIP}\r\n");
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 端口：{ConfigManager.Instance.NetworkPort}\r\n");
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 连接方式：\r\n");
                    AddLog($"  1. 局域网内其他电脑：输入 {localDeviceCode}#{localIP}\r\n");
                    AddLog($"  2. 公网中继/本机测试：只输入 {localDeviceCode}\r\n");
                }
                else
                {
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 正在停止受控模式...\r\n");
                    await networkManager.StopAsync();
                    isHost = false;
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 已停止受控模式\r\n");
                }
            }
            catch (Exception ex)
            {
                AddLog($"[{DateTime.Now:HH:mm:ss}] 错误: {ex.Message}\r\n");
                UIMessageBox.ShowError($"操作失败: {ex.Message}");
                isHost = false;
            }
            finally
            {
                // 延迟恢复按钮状态，防止快速重复点击
                await Task.Delay(500);
                btnStartHost.Enabled = true;
            }
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isConnected)
                {
                    string remoteCode = uiComboBoxRemoteCode.Text.Trim();
                    if (string.IsNullOrEmpty(remoteCode))
                    {
                        UIMessageBox.ShowWarning("请输入设备码");
                        return;
                    }

                    EnsureRemoteViewerOpened();
                    
                    // 解析设备码，支持两种格式：纯6位数字 或 设备码#IP地址
                    var (code, targetIP) = NetworkHelper.ParseDeviceCode(remoteCode);
                    
                    // 验证设备码格式
                    if (remoteCode.Contains("#"))
                    {
                        // 格式：设备码#IP地址
                        if (code.Length != 6 || string.IsNullOrEmpty(targetIP))
                        {
                            UIMessageBox.ShowWarning("格式错误！请输入：设备码#IP地址\n例如：123456#192.168.1.7");
                            return;
                        }
                        AddLog($"[{DateTime.Now:HH:mm:ss}] 局域网连接模式\r\n");
                        AddLog($"[{DateTime.Now:HH:mm:ss}] 目标IP: {targetIP}\r\n");
                    }
                    else
                    {
                        // 格式：纯6位数字（默认走公网中继，若未启用中继则为本地测试）
                        if (remoteCode.Length != 6)
                        {
                            UIMessageBox.ShowWarning("请输入6位设备码，或使用 设备码#IP地址 格式");
                            return;
                        }
                        AddLog($"[{DateTime.Now:HH:mm:ss}] 公网中继模式（仅设备码）\r\n");
                        AddLog($"[{DateTime.Now:HH:mm:ss}] 💡 局域网直连请使用: {remoteCode}#IP地址\r\n");
                        AddLog($"[{DateTime.Now:HH:mm:ss}] 💡 例如: {remoteCode}#192.168.1.6\r\n");
                    }
                    
                    // 保存到历史记录
                    historyManager.AddOrUpdateConnection(remoteCode);
                    LoadConnectionHistory();
                    
                    isHost = false;
                    await networkManager.ConnectToHostAsync(remoteCode);
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 正在连接到设备: {remoteCode}\r\n");
                }
                else
                {
                    await networkManager.StopAsync();
                }
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError($"连接失败: {ex.Message}");
                
                // 显示诊断选项
                var (code, targetIP) = NetworkHelper.ParseDeviceCode(uiComboBoxRemoteCode.Text.Trim());
                if (!string.IsNullOrEmpty(targetIP))
                {
                    var result = UIMessageBox.Show(
                        "连接失败！是否运行网络诊断工具？",
                        "网络诊断",
                        Sunny.UI.UIStyle.Blue,
                        Sunny.UI.UIMessageBoxButtons.OKCancel
                    );
                    
                    if (result)
                    {
                        NetworkDiagnostics.ShowDiagnosticWindow(targetIP);
                    }
                }
                else
                {
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 提示：局域网连接请使用 设备码#IP地址 格式\r\n");
                    AddLog($"[{DateTime.Now:HH:mm:ss}] 例如：123456#192.168.1.7\r\n");
                }
            }
        }

        private void pictureBoxScreen_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isHost && isConnected)
            {
                PictureBox currentPictureBox = sender as PictureBox ?? GetActiveDisplayPictureBox();
                var relativePos = GetRelativePosition(e.Location, currentPictureBox);
                networkManager.SendMouseEvent(relativePos, e.Button, true);
            }
        }

        private void pictureBoxScreen_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isHost && isConnected)
            {
                PictureBox currentPictureBox = sender as PictureBox ?? GetActiveDisplayPictureBox();
                var relativePos = GetRelativePosition(e.Location, currentPictureBox);
                networkManager.SendMouseEvent(relativePos, e.Button, false);
            }
        }

        // 添加鼠标移动限流（避免发送过于频繁）
        private DateTime lastMouseMoveTime = DateTime.MinValue;
        private const int MOUSE_MOVE_INTERVAL_MS = 20; // 最多50Hz
        
        private void pictureBoxScreen_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isHost && isConnected)
            {
                // 限流：避免发送过于频繁的鼠标移动
                var now = DateTime.Now;
                if ((now - lastMouseMoveTime).TotalMilliseconds < MOUSE_MOVE_INTERVAL_MS)
                    return;
                
                PictureBox currentPictureBox = sender as PictureBox ?? GetActiveDisplayPictureBox();
                
                var remotePos = GetRelativePosition(e.Location, currentPictureBox);
                if (remotePos.X >= 0 && remotePos.Y >= 0)
                {
                    networkManager.SendMouseMove(remotePos);
                    lastMouseMoveTime = now;
                    
                    // 显示坐标信息（调试用）
                    if (!isFullScreen && this.WindowState != FormWindowState.Minimized)
                    {
                        lblTitle.Text = $"远程控制 - 鼠标位置: {remotePos.X}, {remotePos.Y} / {remoteScreenSize.Width}x{remoteScreenSize.Height}";
                    }
                }
            }
        }

        private Point GetRelativePosition(Point localPos, PictureBox pictureBox = null)
        {
            // 如果没有指定PictureBox，使用当前活动的
            if (pictureBox == null)
            {
                pictureBox = isFullScreen && fullScreenPictureBox != null ? 
                             fullScreenPictureBox : pictureBoxScreen;
            }
            
            if (pictureBox.Image == null) return new Point(-1, -1);
            
            // 获取PictureBox的客户区域
            Rectangle clientRect = pictureBox.ClientRectangle;
            
            // 获取图片原始大小（即远程屏幕大小）
            Size imageSize = pictureBox.Image.Size;
            
            // 计算缩放比例和偏移（Zoom模式下图片保持比例居中显示）
            float imageAspect = (float)imageSize.Width / imageSize.Height;
            float clientAspect = (float)clientRect.Width / clientRect.Height;
            
            Rectangle displayRect;
            
            if (imageAspect > clientAspect)
            {
                // 图片更宽，以宽度为准缩放
                int displayHeight = (int)(clientRect.Width / imageAspect);
                int offsetY = (clientRect.Height - displayHeight) / 2;
                displayRect = new Rectangle(0, offsetY, clientRect.Width, displayHeight);
            }
            else
            {
                // 图片更高或相等，以高度为准缩放
                int displayWidth = (int)(clientRect.Height * imageAspect);
                int offsetX = (clientRect.Width - displayWidth) / 2;
                displayRect = new Rectangle(offsetX, 0, displayWidth, clientRect.Height);
            }
            
            // 检查鼠标是否在图片显示区域内
            if (!displayRect.Contains(localPos))
            {
                return new Point(-1, -1);
            }
            
            // 计算相对于显示区域的位置（0.0 - 1.0）
            float relativeX = (float)(localPos.X - displayRect.X) / displayRect.Width;
            float relativeY = (float)(localPos.Y - displayRect.Y) / displayRect.Height;
            
            // 确保在有效范围内
            relativeX = Math.Max(0, Math.Min(1, relativeX));
            relativeY = Math.Max(0, Math.Min(1, relativeY));
            
            // 映射到远程屏幕的实际坐标
            int remoteX = (int)(relativeX * remoteScreenSize.Width);
            int remoteY = (int)(relativeY * remoteScreenSize.Height);
            
            return new Point(remoteX, remoteY);
        }

        private void pictureBoxScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isHost && isConnected)
            {
                networkManager.SendKeyEvent(e.KeyCode, true);
            }
        }

        private void pictureBoxScreen_KeyUp(object sender, KeyEventArgs e)
        {
            if (!isHost && isConnected)
            {
                networkManager.SendKeyEvent(e.KeyCode, false);
            }
        }

        private void uiTrackBarQuality_ValueChanged(object sender, EventArgs e)
        {
            screenCapture.Quality = uiTrackBarQuality.Value;
            lblQualityValue.Text = $"{uiTrackBarQuality.Value}%";
        }

        private void uiTrackBarFPS_ValueChanged(object sender, EventArgs e)
        {
            updateTimer.Interval = 1000 / uiTrackBarFPS.Value;
            lblFPSValue.Text = $"{uiTrackBarFPS.Value} FPS";
        }

        private void btnCopyCode_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(localDeviceCode);
            UIMessageTip.ShowOk("设备码已复制到剪贴板");
        }

        private void btnRefreshCode_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                GenerateDeviceCode();
                UIMessageTip.ShowOk("设备码已刷新");
            }
            else
            {
                UIMessageBox.ShowWarning("连接中不能刷新设备码");
            }
        }
        
        #if DEBUG
        private void btnTestMode_Click(object sender, EventArgs e)
        {
            // 显示测试模式窗口
            var testForm = new FrmTestMode();
            testForm.ShowDialog();
        }
        #endif
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (isConnected)
            {
                // UIMessageBox.Show返回bool值，true表示确定，false表示取消
                bool result = UIMessageBox.Show("确定要断开连接并关闭窗口吗？", "确认", UIStyle.Blue, UIMessageBoxButtons.OKCancel);
                if (!result)  // 如果返回false，表示用户点击了取消
                {
                    e.Cancel = true;
                    return;
                }
                
                // 立即设置为未连接状态
                isConnected = false;
                isHost = false;
            }
            base.OnFormClosing(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // 停止定时器
            updateTimer?.Stop();
            updateTimer?.Dispose();
            statsTimer?.Stop();
            statsTimer?.Dispose();
            
            // 清理网络组件（异步执行，避免阻塞UI）
            Task.Run(async () =>
            {
                try
                {
                    networkProtocol?.Stop();
                    if (networkManager != null)
                    {
                        await networkManager.StopAsync();
                        networkManager.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"清理网络组件错误: {ex.Message}");
                }
            });
            
            // 清理捕获组件
            differentialCapture?.Dispose();
            screenCapture?.Dispose();
            
            base.OnFormClosed(e);
        }
    }
}
