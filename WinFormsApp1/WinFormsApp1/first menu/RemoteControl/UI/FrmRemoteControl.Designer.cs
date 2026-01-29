namespace WinFormsApp1.first_menu.RemoteControl
{
    partial class FrmRemoteControl
    {
        private System.ComponentModel.IContainer components = null;
        private Sunny.UI.UIPanel panelTop;
        private Sunny.UI.UIPanel panelLeft;
        private Sunny.UI.UIPanel panelMain;
        private Sunny.UI.UIPanel panelBottom;
        private Sunny.UI.UILabel lblTitle;
        private Sunny.UI.UILabel lblDeviceCodeLabel;
        private Sunny.UI.UILabel lblDeviceCode;
        private Sunny.UI.UIButton btnCopyCode;
        private Sunny.UI.UIButton btnRefreshCode;
        private Sunny.UI.UIComboBox uiComboBoxRemoteCode;
        private Sunny.UI.UIButton btnConnect;
        private Sunny.UI.UIButton btnStartHost;
        private System.Windows.Forms.PictureBox pictureBoxScreen;
        private Sunny.UI.UILabel lblConnectionStatus;
        private Sunny.UI.UITextBox uiTextBoxLog;
        private Sunny.UI.UILabel lblQuality;
        private Sunny.UI.UITrackBar uiTrackBarQuality;
        private Sunny.UI.UILabel lblQualityValue;
        private Sunny.UI.UILabel lblFPS;
        private Sunny.UI.UITrackBar uiTrackBarFPS;
        private Sunny.UI.UILabel lblFPSValue;
        private Sunny.UI.UIGroupBox groupBoxControl;
        private Sunny.UI.UIGroupBox groupBoxSettings;
        private Sunny.UI.UIGroupBox groupBoxLog;
        private Sunny.UI.UIButton btnTestMode;
        private Sunny.UI.UIButton btnFullScreen;
        private Sunny.UI.UIPanel panelTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new Sunny.UI.UIPanel();
            this.btnFullScreen = new Sunny.UI.UIButton();
            this.btnTestMode = new Sunny.UI.UIButton();
            this.panelTitle = new Sunny.UI.UIPanel();
            this.lblTitle = new Sunny.UI.UILabel();
            this.lblConnectionStatus = new Sunny.UI.UILabel();
            this.panelLeft = new Sunny.UI.UIPanel();
            this.groupBoxSettings = new Sunny.UI.UIGroupBox();
            this.lblQuality = new Sunny.UI.UILabel();
            this.uiTrackBarQuality = new Sunny.UI.UITrackBar();
            this.lblQualityValue = new Sunny.UI.UILabel();
            this.lblFPS = new Sunny.UI.UILabel();
            this.uiTrackBarFPS = new Sunny.UI.UITrackBar();
            this.lblFPSValue = new Sunny.UI.UILabel();
            this.groupBoxControl = new Sunny.UI.UIGroupBox();
            this.lblDeviceCodeLabel = new Sunny.UI.UILabel();
            this.lblDeviceCode = new Sunny.UI.UILabel();
            this.btnCopyCode = new Sunny.UI.UIButton();
            this.btnRefreshCode = new Sunny.UI.UIButton();
            this.btnStartHost = new Sunny.UI.UIButton();
            this.uiComboBoxRemoteCode = new Sunny.UI.UIComboBox();
            this.btnConnect = new Sunny.UI.UIButton();
            this.panelMain = new Sunny.UI.UIPanel();
            this.pictureBoxScreen = new System.Windows.Forms.PictureBox();
            this.panelBottom = new Sunny.UI.UIPanel();
            this.groupBoxLog = new Sunny.UI.UIGroupBox();
            this.uiTextBoxLog = new Sunny.UI.UITextBox();
            this.panelTop.SuspendLayout();
            this.panelTitle.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.groupBoxSettings.SuspendLayout();
            this.groupBoxControl.SuspendLayout();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScreen)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.groupBoxLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnFullScreen);
            this.panelTop.Controls.Add(this.btnTestMode);
            this.panelTop.Controls.Add(this.panelTitle);
            this.panelTop.Controls.Add(this.lblConnectionStatus);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelTop.MinimumSize = new System.Drawing.Size(1, 1);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(800, 60);
            this.panelTop.TabIndex = 0;
            this.panelTop.Text = null;
            this.panelTop.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnFullScreen
            // 
            this.btnFullScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFullScreen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFullScreen.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnFullScreen.Location = new System.Drawing.Point(300, 15);
            this.btnFullScreen.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnFullScreen.Name = "btnFullScreen";
            this.btnFullScreen.Size = new System.Drawing.Size(80, 30);
            this.btnFullScreen.TabIndex = 4;
            this.btnFullScreen.Text = "全屏";
            this.btnFullScreen.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFullScreen.Click += new System.EventHandler(this.btnFullScreen_Click);
            // 
            // btnTestMode
            // 
            this.btnTestMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTestMode.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnTestMode.Location = new System.Drawing.Point(562, 15);
            this.btnTestMode.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnTestMode.Name = "btnTestMode";
            this.btnTestMode.Size = new System.Drawing.Size(100, 30);
            this.btnTestMode.TabIndex = 2;
            this.btnTestMode.Text = "测试模式";
            this.btnTestMode.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTestMode.Click += new System.EventHandler(this.btnTestMode_Click);
            // 
            // panelTitle
            // 
            this.panelTitle.Controls.Add(this.lblTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTitle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelTitle.MinimumSize = new System.Drawing.Size(1, 1);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(400, 60);
            this.panelTitle.TabIndex = 0;
            this.panelTitle.Text = null;
            this.panelTitle.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "远程控制工具";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblConnectionStatus.Location = new System.Drawing.Point(1000, 15);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(180, 30);
            this.lblConnectionStatus.TabIndex = 1;
            this.lblConnectionStatus.Text = "未连接";
            this.lblConnectionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelLeft
            // 
            this.panelLeft.AutoScroll = true;
            this.panelLeft.Controls.Add(this.groupBoxSettings);
            this.panelLeft.Controls.Add(this.groupBoxControl);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.panelLeft.Location = new System.Drawing.Point(0, 60);
            this.panelLeft.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelLeft.MinimumSize = new System.Drawing.Size(1, 1);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(5);
            this.panelLeft.Size = new System.Drawing.Size(270, 540);
            this.panelLeft.TabIndex = 1;
            this.panelLeft.Text = null;
            this.panelLeft.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.Controls.Add(this.lblQuality);
            this.groupBoxSettings.Controls.Add(this.uiTrackBarQuality);
            this.groupBoxSettings.Controls.Add(this.lblQualityValue);
            this.groupBoxSettings.Controls.Add(this.lblFPS);
            this.groupBoxSettings.Controls.Add(this.uiTrackBarFPS);
            this.groupBoxSettings.Controls.Add(this.lblFPSValue);
            this.groupBoxSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxSettings.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.groupBoxSettings.Location = new System.Drawing.Point(5, 255);
            this.groupBoxSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSettings.MinimumSize = new System.Drawing.Size(1, 1);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Padding = new System.Windows.Forms.Padding(0, 28, 0, 0);
            this.groupBoxSettings.Size = new System.Drawing.Size(260, 100);
            this.groupBoxSettings.TabIndex = 1;
            this.groupBoxSettings.Text = "传输设置";
            this.groupBoxSettings.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblQuality
            // 
            this.lblQuality.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblQuality.Location = new System.Drawing.Point(10, 35);
            this.lblQuality.Name = "lblQuality";
            this.lblQuality.Size = new System.Drawing.Size(60, 20);
            this.lblQuality.TabIndex = 0;
            this.lblQuality.Text = "画面质量";
            this.lblQuality.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiTrackBarQuality
            // 
            this.uiTrackBarQuality.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiTrackBarQuality.Location = new System.Drawing.Point(75, 35);
            this.uiTrackBarQuality.Minimum = 10;
            this.uiTrackBarQuality.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTrackBarQuality.Name = "uiTrackBarQuality";
            this.uiTrackBarQuality.Size = new System.Drawing.Size(140, 23);
            this.uiTrackBarQuality.TabIndex = 1;
            this.uiTrackBarQuality.Text = "uiTrackBarQuality";
            this.uiTrackBarQuality.Value = 70;
            this.uiTrackBarQuality.ValueChanged += new System.EventHandler(this.uiTrackBarQuality_ValueChanged);
            // 
            // lblQualityValue
            // 
            this.lblQualityValue.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblQualityValue.Location = new System.Drawing.Point(218, 35);
            this.lblQualityValue.Name = "lblQualityValue";
            this.lblQualityValue.Size = new System.Drawing.Size(40, 20);
            this.lblQualityValue.TabIndex = 2;
            this.lblQualityValue.Text = "70%";
            this.lblQualityValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFPS
            // 
            this.lblFPS.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblFPS.Location = new System.Drawing.Point(10, 65);
            this.lblFPS.Name = "lblFPS";
            this.lblFPS.Size = new System.Drawing.Size(60, 20);
            this.lblFPS.TabIndex = 3;
            this.lblFPS.Text = "传输帧率";
            this.lblFPS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiTrackBarFPS
            // 
            this.uiTrackBarFPS.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiTrackBarFPS.Location = new System.Drawing.Point(75, 65);
            this.uiTrackBarFPS.Maximum = 60;
            this.uiTrackBarFPS.Minimum = 5;
            this.uiTrackBarFPS.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTrackBarFPS.Name = "uiTrackBarFPS";
            this.uiTrackBarFPS.Size = new System.Drawing.Size(140, 23);
            this.uiTrackBarFPS.TabIndex = 4;
            this.uiTrackBarFPS.Text = "uiTrackBarFPS";
            this.uiTrackBarFPS.Value = 30;
            this.uiTrackBarFPS.ValueChanged += new System.EventHandler(this.uiTrackBarFPS_ValueChanged);
            // 
            // lblFPSValue
            // 
            this.lblFPSValue.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblFPSValue.Location = new System.Drawing.Point(218, 65);
            this.lblFPSValue.Name = "lblFPSValue";
            this.lblFPSValue.Size = new System.Drawing.Size(40, 20);
            this.lblFPSValue.TabIndex = 5;
            this.lblFPSValue.Text = "30";
            this.lblFPSValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBoxControl
            // 
            this.groupBoxControl.Controls.Add(this.lblDeviceCodeLabel);
            this.groupBoxControl.Controls.Add(this.lblDeviceCode);
            this.groupBoxControl.Controls.Add(this.btnCopyCode);
            this.groupBoxControl.Controls.Add(this.btnRefreshCode);
            this.groupBoxControl.Controls.Add(this.btnStartHost);
            this.groupBoxControl.Controls.Add(this.uiComboBoxRemoteCode);
            this.groupBoxControl.Controls.Add(this.btnConnect);
            this.groupBoxControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxControl.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.groupBoxControl.Location = new System.Drawing.Point(5, 5);
            this.groupBoxControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxControl.MinimumSize = new System.Drawing.Size(1, 1);
            this.groupBoxControl.Name = "groupBoxControl";
            this.groupBoxControl.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.groupBoxControl.Size = new System.Drawing.Size(260, 250);
            this.groupBoxControl.TabIndex = 0;
            this.groupBoxControl.Text = "连接控制";
            this.groupBoxControl.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDeviceCodeLabel
            // 
            this.lblDeviceCodeLabel.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblDeviceCodeLabel.Location = new System.Drawing.Point(15, 45);
            this.lblDeviceCodeLabel.Name = "lblDeviceCodeLabel";
            this.lblDeviceCodeLabel.Size = new System.Drawing.Size(100, 23);
            this.lblDeviceCodeLabel.TabIndex = 0;
            this.lblDeviceCodeLabel.Text = "本机设备码：";
            this.lblDeviceCodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDeviceCode
            // 
            this.lblDeviceCode.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold);
            this.lblDeviceCode.Location = new System.Drawing.Point(15, 70);
            this.lblDeviceCode.Name = "lblDeviceCode";
            this.lblDeviceCode.Size = new System.Drawing.Size(150, 35);
            this.lblDeviceCode.TabIndex = 1;
            this.lblDeviceCode.Text = "000000";
            this.lblDeviceCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCopyCode
            // 
            this.btnCopyCode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCopyCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnCopyCode.Location = new System.Drawing.Point(170, 70);
            this.btnCopyCode.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCopyCode.Name = "btnCopyCode";
            this.btnCopyCode.Size = new System.Drawing.Size(45, 35);
            this.btnCopyCode.TabIndex = 2;
            this.btnCopyCode.Text = "复制";
            this.btnCopyCode.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCopyCode.Click += new System.EventHandler(this.btnCopyCode_Click);
            // 
            // btnRefreshCode
            // 
            this.btnRefreshCode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefreshCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnRefreshCode.Location = new System.Drawing.Point(220, 70);
            this.btnRefreshCode.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnRefreshCode.Name = "btnRefreshCode";
            this.btnRefreshCode.Size = new System.Drawing.Size(45, 35);
            this.btnRefreshCode.TabIndex = 3;
            this.btnRefreshCode.Text = "刷新";
            this.btnRefreshCode.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRefreshCode.Click += new System.EventHandler(this.btnRefreshCode_Click);
            // 
            // btnStartHost
            // 
            this.btnStartHost.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartHost.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnStartHost.Location = new System.Drawing.Point(15, 110);
            this.btnStartHost.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnStartHost.Name = "btnStartHost";
            this.btnStartHost.Size = new System.Drawing.Size(230, 35);
            this.btnStartHost.TabIndex = 4;
            this.btnStartHost.Text = "开始受控";
            this.btnStartHost.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStartHost.Click += new System.EventHandler(this.btnStartHost_Click);
            // 
            // uiComboBoxRemoteCode
            // 
            this.uiComboBoxRemoteCode.DataSource = null;
            this.uiComboBoxRemoteCode.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDown;
            this.uiComboBoxRemoteCode.FillColor = System.Drawing.Color.White;
            this.uiComboBoxRemoteCode.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiComboBoxRemoteCode.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.uiComboBoxRemoteCode.Location = new System.Drawing.Point(15, 155);
            this.uiComboBoxRemoteCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiComboBoxRemoteCode.MinimumSize = new System.Drawing.Size(63, 0);
            this.uiComboBoxRemoteCode.Name = "uiComboBoxRemoteCode";
            this.uiComboBoxRemoteCode.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.uiComboBoxRemoteCode.Size = new System.Drawing.Size(230, 35);
            this.uiComboBoxRemoteCode.SymbolSize = 24;
            this.uiComboBoxRemoteCode.TabIndex = 5;
            this.uiComboBoxRemoteCode.Text = "666666#192.168.1.6";
            this.uiComboBoxRemoteCode.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiComboBoxRemoteCode.Watermark = "输入远程设备码";
            this.uiComboBoxRemoteCode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uiComboBoxRemoteCode_MouseDown);
            // 
            // btnConnect
            // 
            this.btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnect.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnConnect.Location = new System.Drawing.Point(15, 200);
            this.btnConnect.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(230, 35);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "连接";
            this.btnConnect.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // panelMain
            // 
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.panelMain.Location = new System.Drawing.Point(270, 60);
            this.panelMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelMain.MinimumSize = new System.Drawing.Size(1, 1);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(5);
            this.panelMain.Size = new System.Drawing.Size(530, 390);
            this.panelMain.TabIndex = 2;
            this.panelMain.Text = null;
            this.panelMain.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.panelMain.Visible = false;  // 隐藏主面板，使用独立窗口显示
            // 
            // pictureBoxScreen
            // 
            this.pictureBoxScreen.BackColor = System.Drawing.Color.Black;
            this.pictureBoxScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxScreen.Location = new System.Drawing.Point(5, 5);
            this.pictureBoxScreen.Name = "pictureBoxScreen";
            this.pictureBoxScreen.Size = new System.Drawing.Size(520, 380);
            this.pictureBoxScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxScreen.TabIndex = 0;
            this.pictureBoxScreen.TabStop = false;
            this.pictureBoxScreen.KeyUp += new System.Windows.Forms.KeyEventHandler(this.pictureBoxScreen_KeyUp);
            this.pictureBoxScreen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.pictureBoxScreen_KeyDown);
            this.pictureBoxScreen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxScreen_MouseDown);
            this.pictureBoxScreen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxScreen_MouseMove);
            this.pictureBoxScreen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxScreen_MouseUp);
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.groupBoxLog);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.panelBottom.Location = new System.Drawing.Point(270, 60);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelBottom.MinimumSize = new System.Drawing.Size(1, 1);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(530, 390);
            this.panelBottom.TabIndex = 3;
            this.panelBottom.Text = null;
            this.panelBottom.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxLog
            // 
            this.groupBoxLog.Controls.Add(this.uiTextBoxLog);
            this.groupBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxLog.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.groupBoxLog.Location = new System.Drawing.Point(0, 0);
            this.groupBoxLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxLog.MinimumSize = new System.Drawing.Size(1, 1);
            this.groupBoxLog.Name = "groupBoxLog";
            this.groupBoxLog.Padding = new System.Windows.Forms.Padding(5, 32, 5, 5);
            this.groupBoxLog.Size = new System.Drawing.Size(530, 150);
            this.groupBoxLog.TabIndex = 0;
            this.groupBoxLog.Text = "运行日志";
            this.groupBoxLog.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiTextBoxLog
            // 
            this.uiTextBoxLog.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.uiTextBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTextBoxLog.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.uiTextBoxLog.Location = new System.Drawing.Point(5, 32);
            this.uiTextBoxLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTextBoxLog.MinimumSize = new System.Drawing.Size(1, 16);
            this.uiTextBoxLog.Multiline = true;
            this.uiTextBoxLog.Name = "uiTextBoxLog";
            this.uiTextBoxLog.Padding = new System.Windows.Forms.Padding(5);
            this.uiTextBoxLog.ReadOnly = true;
            this.uiTextBoxLog.ShowText = false;
            this.uiTextBoxLog.Size = new System.Drawing.Size(520, 113);
            this.uiTextBoxLog.TabIndex = 0;
            this.uiTextBoxLog.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiTextBoxLog.Watermark = "";
            // 
            // FrmRemoteControl
            // 
            this.AllowShowTitle = false;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelTop);
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.Name = "FrmRemoteControl";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.ShowTitle = false;
            this.Text = "远程控制工具";
            this.ZoomScaleRect = new System.Drawing.Rectangle(19, 19, 1200, 650);
            this.panelTop.ResumeLayout(false);
            this.panelTitle.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxControl.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScreen)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.groupBoxLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
