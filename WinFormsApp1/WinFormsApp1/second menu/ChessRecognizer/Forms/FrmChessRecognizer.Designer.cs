namespace WinFormsApp1.second_menu.ChessRecognizer.Forms
{
    partial class FrmChessRecognizer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelLeft = new Sunny.UI.UIPanel();
            picPreview = new PictureBox();
            panelRight = new Sunny.UI.UIPanel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            grpInput = new Sunny.UI.UIGroupBox();
            flowInput = new FlowLayoutPanel();
            btnPaste = new Sunny.UI.UIButton();
            btnSelectFile = new Sunny.UI.UIButton();
            lblDragTip = new Sunny.UI.UILabel();
            grpOcr = new Sunny.UI.UIGroupBox();
            flowOcr = new FlowLayoutPanel();
            cboOcrEngine = new Sunny.UI.UIComboBox();
            btnTestPerf = new Sunny.UI.UIButton();
            grpControl = new Sunny.UI.UIGroupBox();
            flowControl = new FlowLayoutPanel();
            btnRecognize = new Sunny.UI.UIButton();
            btnStop = new Sunny.UI.UIButton();
            btnSettings = new Sunny.UI.UIButton();
            progressBar = new Sunny.UI.UIProcessBar();
            grpFen = new Sunny.UI.UIGroupBox();
            flowFen = new FlowLayoutPanel();
            txtFEN = new Sunny.UI.UITextBox();
            panelFenButtons = new FlowLayoutPanel();
            btnCopyFEN = new Sunny.UI.UIButton();
            btnClear = new Sunny.UI.UIButton();
            chkAutoCopy = new Sunny.UI.UICheckBox();
            grpConsole = new Sunny.UI.UIGroupBox();
            flowConsole = new FlowLayoutPanel();
            txtConsole = new Sunny.UI.UITextBox();
            btnCopyBoard = new Sunny.UI.UIButton();
            grpStatus = new Sunny.UI.UIGroupBox();
            flowStatus = new FlowLayoutPanel();
            lblStatus = new Sunny.UI.UILabel();
            lblTime = new Sunny.UI.UILabel();
            lblConfidence = new Sunny.UI.UILabel();
            panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPreview).BeginInit();
            panelRight.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            grpInput.SuspendLayout();
            flowInput.SuspendLayout();
            grpOcr.SuspendLayout();
            flowOcr.SuspendLayout();
            grpControl.SuspendLayout();
            flowControl.SuspendLayout();
            grpFen.SuspendLayout();
            flowFen.SuspendLayout();
            panelFenButtons.SuspendLayout();
            grpConsole.SuspendLayout();
            flowConsole.SuspendLayout();
            grpStatus.SuspendLayout();
            flowStatus.SuspendLayout();
            SuspendLayout();
            // 
            // panelLeft
            // 
            panelLeft.Controls.Add(picPreview);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Font = new Font("微软雅黑", 12F);
            panelLeft.Location = new Point(0, 0);
            panelLeft.Margin = new Padding(4, 5, 4, 5);
            panelLeft.MinimumSize = new Size(1, 1);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(550, 700);
            panelLeft.TabIndex = 0;
            panelLeft.Text = "图像预览";
            panelLeft.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // picPreview
            // 
            picPreview.AllowDrop = true;
            picPreview.BackColor = Color.FromArgb(45, 45, 48);
            picPreview.Dock = DockStyle.Fill;
            picPreview.Location = new Point(0, 0);
            picPreview.Name = "picPreview";
            picPreview.Size = new Size(550, 700);
            picPreview.SizeMode = PictureBoxSizeMode.Zoom;
            picPreview.TabIndex = 0;
            picPreview.TabStop = false;
            picPreview.Click += PicPreview_Click;
            picPreview.DragDrop += PicPreview_DragDrop;
            picPreview.DragEnter += PicPreview_DragEnter;
            // 
            // panelRight
            // 
            panelRight.Controls.Add(flowLayoutPanel1);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Font = new Font("微软雅黑", 12F);
            panelRight.Location = new Point(550, 0);
            panelRight.Margin = new Padding(4, 5, 4, 5);
            panelRight.MinimumSize = new Size(1, 1);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(450, 700);
            panelRight.TabIndex = 1;
            panelRight.Text = "控制面板";
            panelRight.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(grpInput);
            flowLayoutPanel1.Controls.Add(grpOcr);
            flowLayoutPanel1.Controls.Add(grpControl);
            flowLayoutPanel1.Controls.Add(progressBar);
            flowLayoutPanel1.Controls.Add(grpFen);
            flowLayoutPanel1.Controls.Add(grpConsole);
            flowLayoutPanel1.Controls.Add(grpStatus);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(10);
            flowLayoutPanel1.Size = new Size(450, 700);
            flowLayoutPanel1.TabIndex = 0;
            flowLayoutPanel1.WrapContents = false;
            // 
            // grpInput
            // 
            grpInput.Controls.Add(flowInput);
            grpInput.Font = new Font("微软雅黑", 12F);
            grpInput.Location = new Point(13, 13);
            grpInput.Margin = new Padding(3, 3, 3, 5);
            grpInput.MinimumSize = new Size(1, 1);
            grpInput.Name = "grpInput";
            grpInput.Padding = new Padding(0, 32, 0, 0);
            grpInput.Size = new Size(400, 130);
            grpInput.TabIndex = 0;
            grpInput.Text = "图片输入";
            grpInput.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // flowInput
            // 
            flowInput.Controls.Add(btnPaste);
            flowInput.Controls.Add(btnSelectFile);
            flowInput.Controls.Add(lblDragTip);
            flowInput.Dock = DockStyle.Fill;
            flowInput.FlowDirection = FlowDirection.TopDown;
            flowInput.Location = new Point(0, 32);
            flowInput.Name = "flowInput";
            flowInput.Padding = new Padding(10, 5, 5, 5);
            flowInput.Size = new Size(400, 98);
            flowInput.TabIndex = 0;
            // 
            // btnPaste
            // 
            btnPaste.Cursor = Cursors.Hand;
            btnPaste.Font = new Font("微软雅黑", 12F);
            btnPaste.Location = new Point(13, 8);
            btnPaste.MinimumSize = new Size(1, 1);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new Size(180, 35);
            btnPaste.TabIndex = 0;
            btnPaste.Text = "粘贴图片 (Ctrl+V)";
            btnPaste.TipsFont = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnPaste.Click += BtnPaste_Click;
            // 
            // btnSelectFile
            // 
            btnSelectFile.Cursor = Cursors.Hand;
            btnSelectFile.Font = new Font("微软雅黑", 12F);
            btnSelectFile.Location = new Point(13, 49);
            btnSelectFile.MinimumSize = new Size(1, 1);
            btnSelectFile.Name = "btnSelectFile";
            btnSelectFile.Size = new Size(180, 35);
            btnSelectFile.TabIndex = 1;
            btnSelectFile.Text = "选择文件...";
            btnSelectFile.TipsFont = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSelectFile.Click += BtnSelectFile_Click;
            // 
            // lblDragTip
            // 
            lblDragTip.Font = new Font("微软雅黑", 10F);
            lblDragTip.ForeColor = Color.Gray;
            lblDragTip.Location = new Point(199, 5);
            lblDragTip.Name = "lblDragTip";
            lblDragTip.Size = new Size(180, 30);
            lblDragTip.TabIndex = 2;
            lblDragTip.Text = "💡 可拖拽图片到左侧";
            lblDragTip.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpOcr
            // 
            grpOcr.Controls.Add(flowOcr);
            grpOcr.Font = new Font("微软雅黑", 12F);
            grpOcr.Location = new Point(13, 151);
            grpOcr.Margin = new Padding(3, 3, 3, 5);
            grpOcr.MinimumSize = new Size(1, 1);
            grpOcr.Name = "grpOcr";
            grpOcr.Padding = new Padding(0, 32, 0, 0);
            grpOcr.Size = new Size(400, 80);
            grpOcr.TabIndex = 1;
            grpOcr.Text = "OCR引擎";
            grpOcr.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // flowOcr
            // 
            flowOcr.Controls.Add(cboOcrEngine);
            flowOcr.Controls.Add(btnTestPerf);
            flowOcr.Dock = DockStyle.Fill;
            flowOcr.Location = new Point(0, 32);
            flowOcr.Name = "flowOcr";
            flowOcr.Padding = new Padding(10, 5, 5, 5);
            flowOcr.Size = new Size(400, 48);
            flowOcr.TabIndex = 0;
            // 
            // cboOcrEngine
            // 
            cboOcrEngine.DataSource = null;
            cboOcrEngine.FillColor = Color.White;
            cboOcrEngine.Font = new Font("微软雅黑", 12F);
            cboOcrEngine.ItemHoverColor = Color.FromArgb(155, 200, 255);
            cboOcrEngine.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            cboOcrEngine.Location = new Point(13, 8);
            cboOcrEngine.Margin = new Padding(3, 3, 10, 3);
            cboOcrEngine.MinimumSize = new Size(63, 0);
            cboOcrEngine.Name = "cboOcrEngine";
            cboOcrEngine.Padding = new Padding(0, 0, 30, 2);
            cboOcrEngine.Size = new Size(200, 32);
            cboOcrEngine.SymbolSize = 24;
            cboOcrEngine.TabIndex = 0;
            cboOcrEngine.TextAlignment = ContentAlignment.MiddleLeft;
            cboOcrEngine.Watermark = "";
            cboOcrEngine.SelectedIndexChanged += CboOcrEngine_SelectedIndexChanged;
            // 
            // btnTestPerf
            // 
            btnTestPerf.Cursor = Cursors.Hand;
            btnTestPerf.Font = new Font("微软雅黑", 12F);
            btnTestPerf.Location = new Point(226, 8);
            btnTestPerf.MinimumSize = new Size(1, 1);
            btnTestPerf.Name = "btnTestPerf";
            btnTestPerf.Size = new Size(100, 32);
            btnTestPerf.TabIndex = 1;
            btnTestPerf.Text = "测试性能";
            btnTestPerf.TipsFont = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnTestPerf.Click += BtnTestPerf_Click;
            // 
            // grpControl
            // 
            grpControl.Controls.Add(flowControl);
            grpControl.Font = new Font("微软雅黑", 12F);
            grpControl.Location = new Point(13, 239);
            grpControl.Margin = new Padding(3, 3, 3, 5);
            grpControl.MinimumSize = new Size(1, 1);
            grpControl.Name = "grpControl";
            grpControl.Padding = new Padding(0, 32, 0, 0);
            grpControl.Size = new Size(400, 85);
            grpControl.TabIndex = 2;
            grpControl.Text = "识别控制";
            grpControl.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // flowControl
            // 
            flowControl.Controls.Add(btnRecognize);
            flowControl.Controls.Add(btnStop);
            flowControl.Controls.Add(btnSettings);
            flowControl.Dock = DockStyle.Fill;
            flowControl.Location = new Point(0, 32);
            flowControl.Name = "flowControl";
            flowControl.Padding = new Padding(10, 5, 5, 5);
            flowControl.Size = new Size(400, 53);
            flowControl.TabIndex = 0;
            // 
            // btnRecognize
            // 
            btnRecognize.Cursor = Cursors.Hand;
            btnRecognize.FillColor = Color.FromArgb(110, 190, 40);
            btnRecognize.FillColor2 = Color.FromArgb(110, 190, 40);
            btnRecognize.FillHoverColor = Color.FromArgb(139, 203, 83);
            btnRecognize.FillPressColor = Color.FromArgb(88, 152, 32);
            btnRecognize.FillSelectedColor = Color.FromArgb(88, 152, 32);
            btnRecognize.Font = new Font("微软雅黑", 12F);
            btnRecognize.LightColor = Color.FromArgb(245, 251, 241);
            btnRecognize.Location = new Point(13, 8);
            btnRecognize.MinimumSize = new Size(1, 1);
            btnRecognize.Name = "btnRecognize";
            btnRecognize.RectColor = Color.FromArgb(110, 190, 40);
            btnRecognize.RectHoverColor = Color.FromArgb(139, 203, 83);
            btnRecognize.RectPressColor = Color.FromArgb(88, 152, 32);
            btnRecognize.RectSelectedColor = Color.FromArgb(88, 152, 32);
            btnRecognize.Size = new Size(120, 40);
            btnRecognize.Style = Sunny.UI.UIStyle.Custom;
            btnRecognize.TabIndex = 0;
            btnRecognize.Text = "开始识别";
            btnRecognize.TipsFont = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnRecognize.Click += BtnRecognize_Click;
            // 
            // btnStop
            // 
            btnStop.Cursor = Cursors.Hand;
            btnStop.Enabled = false;
            btnStop.FillColor = Color.FromArgb(230, 80, 80);
            btnStop.FillColor2 = Color.FromArgb(230, 80, 80);
            btnStop.FillHoverColor = Color.FromArgb(235, 115, 115);
            btnStop.FillPressColor = Color.FromArgb(184, 64, 64);
            btnStop.FillSelectedColor = Color.FromArgb(184, 64, 64);
            btnStop.Font = new Font("微软雅黑", 12F);
            btnStop.LightColor = Color.FromArgb(253, 243, 243);
            btnStop.Location = new Point(139, 8);
            btnStop.MinimumSize = new Size(1, 1);
            btnStop.Name = "btnStop";
            btnStop.RectColor = Color.FromArgb(230, 80, 80);
            btnStop.RectHoverColor = Color.FromArgb(235, 115, 115);
            btnStop.RectPressColor = Color.FromArgb(184, 64, 64);
            btnStop.RectSelectedColor = Color.FromArgb(184, 64, 64);
            btnStop.Size = new Size(80, 40);
            btnStop.Style = Sunny.UI.UIStyle.Custom;
            btnStop.TabIndex = 1;
            btnStop.Text = "停止";
            btnStop.TipsFont = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnStop.Click += BtnStop_Click;
            // 
            // btnSettings
            // 
            btnSettings.Cursor = Cursors.Hand;
            btnSettings.Font = new Font("微软雅黑", 12F);
            btnSettings.Location = new Point(225, 8);
            btnSettings.MinimumSize = new Size(1, 1);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(80, 40);
            btnSettings.TabIndex = 2;
            btnSettings.Text = "⚙ 设置";
            btnSettings.TipsFont = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSettings.Click += BtnSettings_Click;
            // 
            // progressBar
            // 
            progressBar.FillColor = Color.FromArgb(235, 243, 255);
            progressBar.Font = new Font("微软雅黑", 12F);
            progressBar.Location = new Point(13, 332);
            progressBar.MinimumSize = new Size(70, 3);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(380, 25);
            progressBar.TabIndex = 3;
            progressBar.Text = "0%";
            progressBar.Visible = false;
            // 
            // grpFen
            // 
            grpFen.Controls.Add(flowFen);
            grpFen.Font = new Font("微软雅黑", 12F);
            grpFen.Location = new Point(13, 363);
            grpFen.Margin = new Padding(3, 3, 3, 5);
            grpFen.MinimumSize = new Size(1, 1);
            grpFen.Name = "grpFen";
            grpFen.Padding = new Padding(0, 32, 0, 0);
            grpFen.Size = new Size(400, 120);
            grpFen.TabIndex = 4;
            grpFen.Text = "FEN结果";
            grpFen.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // flowFen
            // 
            flowFen.Controls.Add(txtFEN);
            flowFen.Controls.Add(panelFenButtons);
            flowFen.Dock = DockStyle.Fill;
            flowFen.FlowDirection = FlowDirection.TopDown;
            flowFen.Location = new Point(0, 32);
            flowFen.Name = "flowFen";
            flowFen.Padding = new Padding(10, 5, 5, 5);
            flowFen.Size = new Size(400, 88);
            flowFen.TabIndex = 0;
            // 
            // txtFEN
            // 
            txtFEN.Font = new Font("微软雅黑", 10F);
            txtFEN.Location = new Point(13, 8);
            txtFEN.Margin = new Padding(3, 3, 3, 5);
            txtFEN.MinimumSize = new Size(1, 16);
            txtFEN.Multiline = true;
            txtFEN.Name = "txtFEN";
            txtFEN.Padding = new Padding(5);
            txtFEN.ReadOnly = true;
            txtFEN.ShowText = false;
            txtFEN.Size = new Size(370, 40);
            txtFEN.TabIndex = 0;
            txtFEN.TextAlignment = ContentAlignment.MiddleLeft;
            txtFEN.Watermark = "";
            // 
            // panelFenButtons
            // 
            panelFenButtons.Controls.Add(btnCopyFEN);
            panelFenButtons.Controls.Add(btnClear);
            panelFenButtons.Controls.Add(chkAutoCopy);
            panelFenButtons.Location = new Point(389, 8);
            panelFenButtons.Name = "panelFenButtons";
            panelFenButtons.Size = new Size(370, 35);
            panelFenButtons.TabIndex = 1;
            // 
            // btnCopyFEN
            // 
            btnCopyFEN.Cursor = Cursors.Hand;
            btnCopyFEN.Font = new Font("微软雅黑", 10F);
            btnCopyFEN.Location = new Point(3, 3);
            btnCopyFEN.MinimumSize = new Size(1, 1);
            btnCopyFEN.Name = "btnCopyFEN";
            btnCopyFEN.Size = new Size(90, 28);
            btnCopyFEN.TabIndex = 0;
            btnCopyFEN.Text = "复制FEN";
            btnCopyFEN.TipsFont = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnCopyFEN.Click += BtnCopyFEN_Click;
            // 
            // btnClear
            // 
            btnClear.Cursor = Cursors.Hand;
            btnClear.Font = new Font("微软雅黑", 10F);
            btnClear.Location = new Point(99, 3);
            btnClear.MinimumSize = new Size(1, 1);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(60, 28);
            btnClear.TabIndex = 1;
            btnClear.Text = "清空";
            btnClear.TipsFont = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnClear.Click += BtnClear_Click;
            // 
            // chkAutoCopy
            // 
            chkAutoCopy.Checked = true;
            chkAutoCopy.Cursor = Cursors.Hand;
            chkAutoCopy.Font = new Font("微软雅黑", 10F);
            chkAutoCopy.ForeColor = Color.FromArgb(48, 48, 48);
            chkAutoCopy.Location = new Point(165, 3);
            chkAutoCopy.MinimumSize = new Size(1, 1);
            chkAutoCopy.Name = "chkAutoCopy";
            chkAutoCopy.Padding = new Padding(22, 0, 0, 0);
            chkAutoCopy.Size = new Size(150, 28);
            chkAutoCopy.TabIndex = 2;
            chkAutoCopy.Text = "自动复制到剪贴板";
            // 
            // grpConsole
            // 
            grpConsole.Controls.Add(flowConsole);
            grpConsole.Font = new Font("微软雅黑", 12F);
            grpConsole.Location = new Point(13, 491);
            grpConsole.Margin = new Padding(3, 3, 3, 5);
            grpConsole.MinimumSize = new Size(1, 1);
            grpConsole.Name = "grpConsole";
            grpConsole.Padding = new Padding(0, 32, 0, 0);
            grpConsole.Size = new Size(400, 200);
            grpConsole.TabIndex = 5;
            grpConsole.Text = "棋盘图";
            grpConsole.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // flowConsole
            // 
            flowConsole.Controls.Add(txtConsole);
            flowConsole.Controls.Add(btnCopyBoard);
            flowConsole.Dock = DockStyle.Fill;
            flowConsole.FlowDirection = FlowDirection.TopDown;
            flowConsole.Location = new Point(0, 32);
            flowConsole.Name = "flowConsole";
            flowConsole.Padding = new Padding(10, 5, 5, 5);
            flowConsole.Size = new Size(400, 168);
            flowConsole.TabIndex = 0;
            // 
            // txtConsole
            // 
            txtConsole.Font = new Font("Consolas", 9F);
            txtConsole.Location = new Point(13, 8);
            txtConsole.Margin = new Padding(3, 3, 3, 5);
            txtConsole.MinimumSize = new Size(1, 16);
            txtConsole.Multiline = true;
            txtConsole.Name = "txtConsole";
            txtConsole.Padding = new Padding(5);
            txtConsole.ReadOnly = true;
            txtConsole.ShowText = false;
            txtConsole.Size = new Size(370, 120);
            txtConsole.TabIndex = 0;
            txtConsole.TextAlignment = ContentAlignment.TopLeft;
            txtConsole.Watermark = "";
            // 
            // btnCopyBoard
            // 
            btnCopyBoard.Cursor = Cursors.Hand;
            btnCopyBoard.Font = new Font("微软雅黑", 10F);
            btnCopyBoard.Location = new Point(389, 8);
            btnCopyBoard.MinimumSize = new Size(1, 1);
            btnCopyBoard.Name = "btnCopyBoard";
            btnCopyBoard.Size = new Size(100, 28);
            btnCopyBoard.TabIndex = 1;
            btnCopyBoard.Text = "复制棋盘图";
            btnCopyBoard.TipsFont = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnCopyBoard.Click += BtnCopyBoard_Click;
            // 
            // grpStatus
            // 
            grpStatus.Controls.Add(flowStatus);
            grpStatus.Font = new Font("微软雅黑", 12F);
            grpStatus.Location = new Point(13, 699);
            grpStatus.Margin = new Padding(3, 3, 3, 5);
            grpStatus.MinimumSize = new Size(1, 1);
            grpStatus.Name = "grpStatus";
            grpStatus.Padding = new Padding(0, 32, 0, 0);
            grpStatus.Size = new Size(400, 80);
            grpStatus.TabIndex = 6;
            grpStatus.Text = "状态";
            grpStatus.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // flowStatus
            // 
            flowStatus.Controls.Add(lblStatus);
            flowStatus.Controls.Add(lblTime);
            flowStatus.Controls.Add(lblConfidence);
            flowStatus.Dock = DockStyle.Fill;
            flowStatus.FlowDirection = FlowDirection.TopDown;
            flowStatus.Location = new Point(0, 32);
            flowStatus.Name = "flowStatus";
            flowStatus.Padding = new Padding(10, 0, 5, 5);
            flowStatus.Size = new Size(400, 48);
            flowStatus.TabIndex = 0;
            // 
            // lblStatus
            // 
            lblStatus.Font = new Font("微软雅黑", 10F);
            lblStatus.ForeColor = Color.Green;
            lblStatus.Location = new Point(13, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(200, 23);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "就绪";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTime
            // 
            lblTime.Font = new Font("微软雅黑", 9F);
            lblTime.ForeColor = Color.FromArgb(48, 48, 48);
            lblTime.Location = new Point(13, 23);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(120, 20);
            lblTime.TabIndex = 1;
            lblTime.Text = "处理时间: -";
            lblTime.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblConfidence
            // 
            lblConfidence.Font = new Font("微软雅黑", 9F);
            lblConfidence.ForeColor = Color.FromArgb(48, 48, 48);
            lblConfidence.Location = new Point(219, 0);
            lblConfidence.Name = "lblConfidence";
            lblConfidence.Size = new Size(120, 20);
            lblConfidence.TabIndex = 2;
            lblConfidence.Text = "置信度: -";
            lblConfidence.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FrmChessRecognizer
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1000, 700);
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            Font = new Font("微软雅黑", 12F);
            KeyPreview = true;
            Name = "FrmChessRecognizer";
            Text = "中国象棋识别系统";
            KeyDown += FrmChessRecognizer_KeyDown;
            panelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picPreview).EndInit();
            panelRight.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            grpInput.ResumeLayout(false);
            flowInput.ResumeLayout(false);
            grpOcr.ResumeLayout(false);
            flowOcr.ResumeLayout(false);
            grpControl.ResumeLayout(false);
            flowControl.ResumeLayout(false);
            grpFen.ResumeLayout(false);
            flowFen.ResumeLayout(false);
            panelFenButtons.ResumeLayout(false);
            grpConsole.ResumeLayout(false);
            flowConsole.ResumeLayout(false);
            grpStatus.ResumeLayout(false);
            flowStatus.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIPanel panelLeft;
        private System.Windows.Forms.PictureBox picPreview;
        private Sunny.UI.UIPanel panelRight;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Sunny.UI.UIGroupBox grpInput;
        private System.Windows.Forms.FlowLayoutPanel flowInput;
        private Sunny.UI.UIButton btnPaste;
        private Sunny.UI.UIButton btnSelectFile;
        private Sunny.UI.UILabel lblDragTip;
        private Sunny.UI.UIGroupBox grpOcr;
        private System.Windows.Forms.FlowLayoutPanel flowOcr;
        private Sunny.UI.UIComboBox cboOcrEngine;
        private Sunny.UI.UIButton btnTestPerf;
        private Sunny.UI.UIGroupBox grpControl;
        private System.Windows.Forms.FlowLayoutPanel flowControl;
        private Sunny.UI.UIButton btnRecognize;
        private Sunny.UI.UIButton btnStop;
        private Sunny.UI.UIButton btnSettings;
        private Sunny.UI.UIProcessBar progressBar;
        private Sunny.UI.UIGroupBox grpFen;
        private System.Windows.Forms.FlowLayoutPanel flowFen;
        private Sunny.UI.UITextBox txtFEN;
        private System.Windows.Forms.FlowLayoutPanel panelFenButtons;
        private Sunny.UI.UIButton btnCopyFEN;
        private Sunny.UI.UIButton btnClear;
        private Sunny.UI.UICheckBox chkAutoCopy;
        private Sunny.UI.UIGroupBox grpConsole;
        private System.Windows.Forms.FlowLayoutPanel flowConsole;
        private Sunny.UI.UITextBox txtConsole;
        private Sunny.UI.UIButton btnCopyBoard;
        private Sunny.UI.UIGroupBox grpStatus;
        private System.Windows.Forms.FlowLayoutPanel flowStatus;
        private Sunny.UI.UILabel lblStatus;
        private Sunny.UI.UILabel lblTime;
        private Sunny.UI.UILabel lblConfidence;
    }
}
