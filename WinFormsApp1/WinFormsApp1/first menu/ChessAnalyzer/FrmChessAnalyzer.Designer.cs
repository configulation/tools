namespace WinFormsApp1.first_menu.ChessAnalyzer
{
    partial class FrmChessAnalyzer
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.uiGroupBox1 = new Sunny.UI.UIGroupBox();
            this.lblStatus = new Sunny.UI.UILabel();
            this.btnSelectArea = new Sunny.UI.UIButton();
            this.lblAreaInfo = new Sunny.UI.UILabel();
            this.btnStartAnalysis = new Sunny.UI.UISymbolButton();
            this.lblLastScan = new Sunny.UI.UILabel();
            this.uiGroupBox2 = new Sunny.UI.UIGroupBox();
            this.numDepth = new Sunny.UI.UIIntegerUpDown();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.lblDepth = new Sunny.UI.UILabel();
            this.lblScore = new Sunny.UI.UILabel();
            this.lblNodes = new Sunny.UI.UILabel();
            this.lblBestMove = new Sunny.UI.UILabel();
            this.lblPonderMove = new Sunny.UI.UILabel();
            this.uiGroupBox3 = new Sunny.UI.UIGroupBox();
            this.txtFEN = new Sunny.UI.UITextBox();
            this.btnManualFEN = new Sunny.UI.UIButton();
            this.btnTestPosition = new Sunny.UI.UIButton();
            this.btnToggleOverlay = new Sunny.UI.UISymbolButton();
            this.btnRecognizeTest = new Sunny.UI.UIButton();
            this.btnUploadImage = new Sunny.UI.UIButton();
            this.btnPasteImage = new Sunny.UI.UIButton();
            this.uiGroupBox4 = new Sunny.UI.UIGroupBox();
            this.txtLog = new Sunny.UI.UIRichTextBox();
            this.btnClearLog = new Sunny.UI.UIButton();
            this.uiGroupBox1.SuspendLayout();
            this.uiGroupBox2.SuspendLayout();
            this.uiGroupBox3.SuspendLayout();
            this.uiGroupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiGroupBox1.Controls.Add(this.lblStatus);
            this.uiGroupBox1.Controls.Add(this.btnSelectArea);
            this.uiGroupBox1.Controls.Add(this.lblAreaInfo);
            this.uiGroupBox1.Controls.Add(this.btnStartAnalysis);
            this.uiGroupBox1.Controls.Add(this.lblLastScan);
            this.uiGroupBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.uiGroupBox1.Location = new System.Drawing.Point(13, 10);
            this.uiGroupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox1.Size = new System.Drawing.Size(480, 180);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.Text = "实时分析控制";
            this.uiGroupBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(20, 45);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(440, 25);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "引擎状态: 未启动";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSelectArea
            // 
            this.btnSelectArea.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectArea.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnSelectArea.Location = new System.Drawing.Point(20, 80);
            this.btnSelectArea.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSelectArea.Name = "btnSelectArea";
            this.btnSelectArea.Size = new System.Drawing.Size(150, 35);
            this.btnSelectArea.TabIndex = 1;
            this.btnSelectArea.Text = "选择棋盘区域";
            this.btnSelectArea.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectArea.Click += new System.EventHandler(this.btnSelectArea_Click);
            // 
            // lblAreaInfo
            // 
            this.lblAreaInfo.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblAreaInfo.Location = new System.Drawing.Point(180, 80);
            this.lblAreaInfo.Name = "lblAreaInfo";
            this.lblAreaInfo.Size = new System.Drawing.Size(280, 35);
            this.lblAreaInfo.TabIndex = 2;
            this.lblAreaInfo.Text = "未选择区域";
            this.lblAreaInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnStartAnalysis
            // 
            this.btnStartAnalysis.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartAnalysis.Enabled = false;
            this.btnStartAnalysis.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnStartAnalysis.Location = new System.Drawing.Point(20, 125);
            this.btnStartAnalysis.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnStartAnalysis.Name = "btnStartAnalysis";
            this.btnStartAnalysis.Size = new System.Drawing.Size(150, 40);
            this.btnStartAnalysis.Symbol = 61515;
            this.btnStartAnalysis.TabIndex = 3;
            this.btnStartAnalysis.Text = "开始分析";
            this.btnStartAnalysis.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStartAnalysis.Click += new System.EventHandler(this.btnStartAnalysis_Click);
            // 
            // lblLastScan
            // 
            this.lblLastScan.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblLastScan.Location = new System.Drawing.Point(180, 125);
            this.lblLastScan.Name = "lblLastScan";
            this.lblLastScan.Size = new System.Drawing.Size(280, 40);
            this.lblLastScan.TabIndex = 4;
            this.lblLastScan.Text = "上次扫描: --:--:--";
            this.lblLastScan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiGroupBox2.Controls.Add(this.numDepth);
            this.uiGroupBox2.Controls.Add(this.uiLabel1);
            this.uiGroupBox2.Controls.Add(this.lblDepth);
            this.uiGroupBox2.Controls.Add(this.lblScore);
            this.uiGroupBox2.Controls.Add(this.lblNodes);
            this.uiGroupBox2.Controls.Add(this.lblBestMove);
            this.uiGroupBox2.Controls.Add(this.lblPonderMove);
            this.uiGroupBox2.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.uiGroupBox2.Location = new System.Drawing.Point(500, 10);
            this.uiGroupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox2.Size = new System.Drawing.Size(480, 260);
            this.uiGroupBox2.TabIndex = 1;
            this.uiGroupBox2.Text = "分析结果";
            this.uiGroupBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numDepth
            // 
            this.numDepth.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.numDepth.Location = new System.Drawing.Point(120, 45);
            this.numDepth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numDepth.Maximum = 30;
            this.numDepth.Minimum = 10;
            this.numDepth.MinimumSize = new System.Drawing.Size(100, 0);
            this.numDepth.Name = "numDepth";
            this.numDepth.ShowText = false;
            this.numDepth.Size = new System.Drawing.Size(100, 29);
            this.numDepth.TabIndex = 0;
            this.numDepth.Text = "uiIntegerUpDown1";
            this.numDepth.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.numDepth.Value = 18;
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.uiLabel1.Location = new System.Drawing.Point(20, 45);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(90, 29);
            this.uiLabel1.TabIndex = 1;
            this.uiLabel1.Text = "搜索深度:";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDepth
            // 
            this.lblDepth.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.lblDepth.Location = new System.Drawing.Point(20, 85);
            this.lblDepth.Name = "lblDepth";
            this.lblDepth.Size = new System.Drawing.Size(200, 25);
            this.lblDepth.TabIndex = 2;
            this.lblDepth.Text = "深度: 0";
            this.lblDepth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblScore
            // 
            this.lblScore.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblScore.Location = new System.Drawing.Point(20, 115);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(200, 25);
            this.lblScore.TabIndex = 3;
            this.lblScore.Text = "评分: +0.00";
            this.lblScore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNodes
            // 
            this.lblNodes.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblNodes.Location = new System.Drawing.Point(20, 145);
            this.lblNodes.Name = "lblNodes";
            this.lblNodes.Size = new System.Drawing.Size(440, 25);
            this.lblNodes.TabIndex = 4;
            this.lblNodes.Text = "节点: 0";
            this.lblNodes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBestMove
            // 
            this.lblBestMove.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBestMove.Location = new System.Drawing.Point(20, 180);
            this.lblBestMove.Name = "lblBestMove";
            this.lblBestMove.Size = new System.Drawing.Size(440, 30);
            this.lblBestMove.TabIndex = 5;
            this.lblBestMove.Text = "推荐: 等待分析...";
            this.lblBestMove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPonderMove
            // 
            this.lblPonderMove.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.lblPonderMove.Location = new System.Drawing.Point(20, 215);
            this.lblPonderMove.Name = "lblPonderMove";
            this.lblPonderMove.Size = new System.Drawing.Size(440, 30);
            this.lblPonderMove.TabIndex = 6;
            this.lblPonderMove.Text = "应对: 等待分析...";
            this.lblPonderMove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox3
            // 
            this.uiGroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiGroupBox3.Controls.Add(this.txtFEN);
            this.uiGroupBox3.Controls.Add(this.btnManualFEN);
            this.uiGroupBox3.Controls.Add(this.btnTestPosition);
            this.uiGroupBox3.Controls.Add(this.btnToggleOverlay);
            this.uiGroupBox3.Controls.Add(this.btnRecognizeTest);
            this.uiGroupBox3.Controls.Add(this.btnUploadImage);
            this.uiGroupBox3.Controls.Add(this.btnPasteImage);
            this.uiGroupBox3.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.uiGroupBox3.Location = new System.Drawing.Point(13, 200);
            this.uiGroupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox3.Name = "uiGroupBox3";
            this.uiGroupBox3.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox3.Size = new System.Drawing.Size(480, 220);
            this.uiGroupBox3.TabIndex = 2;
            this.uiGroupBox3.Text = "手动测试";
            this.uiGroupBox3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFEN
            // 
            this.txtFEN.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFEN.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtFEN.Location = new System.Drawing.Point(20, 45);
            this.txtFEN.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFEN.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtFEN.Multiline = true;
            this.txtFEN.Name = "txtFEN";
            this.txtFEN.Padding = new System.Windows.Forms.Padding(5);
            this.txtFEN.ShowText = false;
            this.txtFEN.Size = new System.Drawing.Size(440, 50);
            this.txtFEN.TabIndex = 0;
            this.txtFEN.Text = "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1";
            this.txtFEN.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtFEN.Watermark = "输入 FEN 字符串";
            // 
            // btnManualFEN
            // 
            this.btnManualFEN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnManualFEN.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnManualFEN.Location = new System.Drawing.Point(20, 110);
            this.btnManualFEN.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnManualFEN.Name = "btnManualFEN";
            this.btnManualFEN.Size = new System.Drawing.Size(100, 40);
            this.btnManualFEN.TabIndex = 1;
            this.btnManualFEN.Text = "分析FEN";
            this.btnManualFEN.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnManualFEN.Click += new System.EventHandler(this.btnManualFEN_Click);
            // 
            // btnTestPosition
            // 
            this.btnTestPosition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTestPosition.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnTestPosition.Location = new System.Drawing.Point(130, 110);
            this.btnTestPosition.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnTestPosition.Name = "btnTestPosition";
            this.btnTestPosition.Size = new System.Drawing.Size(100, 40);
            this.btnTestPosition.TabIndex = 2;
            this.btnTestPosition.Text = "测试局面";
            this.btnTestPosition.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTestPosition.Click += new System.EventHandler(this.btnTestPosition_Click);
            // 
            // btnToggleOverlay
            // 
            this.btnToggleOverlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToggleOverlay.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnToggleOverlay.Location = new System.Drawing.Point(240, 110);
            this.btnToggleOverlay.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnToggleOverlay.Name = "btnToggleOverlay";
            this.btnToggleOverlay.Size = new System.Drawing.Size(120, 40);
            this.btnToggleOverlay.Symbol = 61461;
            this.btnToggleOverlay.TabIndex = 3;
            this.btnToggleOverlay.Text = "显示悬浮窗";
            this.btnToggleOverlay.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnToggleOverlay.Click += new System.EventHandler(this.btnToggleOverlay_Click);
            // 
            // btnRecognizeTest
            // 
            this.btnRecognizeTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRecognizeTest.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRecognizeTest.Location = new System.Drawing.Point(370, 110);
            this.btnRecognizeTest.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnRecognizeTest.Name = "btnRecognizeTest";
            this.btnRecognizeTest.Size = new System.Drawing.Size(90, 40);
            this.btnRecognizeTest.TabIndex = 4;
            this.btnRecognizeTest.Text = "识别测试";
            this.btnRecognizeTest.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRecognizeTest.Click += new System.EventHandler(this.btnRecognizeTest_Click);
            // 
            // btnUploadImage
            // 
            this.btnUploadImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUploadImage.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnUploadImage.Location = new System.Drawing.Point(20, 160);
            this.btnUploadImage.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnUploadImage.Name = "btnUploadImage";
            this.btnUploadImage.Size = new System.Drawing.Size(120, 40);
            this.btnUploadImage.TabIndex = 5;
            this.btnUploadImage.Text = "上传图片";
            this.btnUploadImage.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUploadImage.Click += new System.EventHandler(this.btnUploadImage_Click);
            // 
            // btnPasteImage
            // 
            this.btnPasteImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPasteImage.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnPasteImage.Location = new System.Drawing.Point(150, 160);
            this.btnPasteImage.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPasteImage.Name = "btnPasteImage";
            this.btnPasteImage.Size = new System.Drawing.Size(120, 40);
            this.btnPasteImage.TabIndex = 6;
            this.btnPasteImage.Text = "粘贴图片";
            this.btnPasteImage.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPasteImage.Click += new System.EventHandler(this.btnPasteImage_Click);
            // 
            // uiGroupBox4
            // 
            this.uiGroupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiGroupBox4.Controls.Add(this.txtLog);
            this.uiGroupBox4.Controls.Add(this.btnClearLog);
            this.uiGroupBox4.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.uiGroupBox4.Location = new System.Drawing.Point(13, 430);
            this.uiGroupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox4.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox4.Name = "uiGroupBox4";
            this.uiGroupBox4.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox4.Size = new System.Drawing.Size(967, 187);
            this.uiGroupBox4.TabIndex = 3;
            this.uiGroupBox4.Text = "运行日志";
            this.uiGroupBox4.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.FillColor = System.Drawing.Color.White;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.Location = new System.Drawing.Point(20, 45);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtLog.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtLog.Name = "txtLog";
            this.txtLog.Padding = new System.Windows.Forms.Padding(2);
            this.txtLog.Radius = 1;
            this.txtLog.ReadOnly = true;
            this.txtLog.ShowText = false;
            this.txtLog.Size = new System.Drawing.Size(927, 97);
            this.txtLog.TabIndex = 0;
            this.txtLog.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClearLog
            // 
            this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearLog.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnClearLog.Location = new System.Drawing.Point(847, 152);
            this.btnClearLog.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(100, 25);
            this.btnClearLog.TabIndex = 1;
            this.btnClearLog.Text = "清空日志";
            this.btnClearLog.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // FrmChessAnalyzer
            // 
            this.AllowShowTitle = false;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(993, 630);
            this.Controls.Add(this.uiGroupBox4);
            this.Controls.Add(this.uiGroupBox3);
            this.Controls.Add(this.uiGroupBox2);
            this.Controls.Add(this.uiGroupBox1);
            this.Name = "FrmChessAnalyzer";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.ShowTitle = false;
            this.Text = "象棋实时分析助手";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 993, 630);
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox2.ResumeLayout(false);
            this.uiGroupBox3.ResumeLayout(false);
            this.uiGroupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIGroupBox uiGroupBox1;
        private Sunny.UI.UILabel lblStatus;
        private Sunny.UI.UIButton btnSelectArea;
        private Sunny.UI.UILabel lblAreaInfo;
        private Sunny.UI.UISymbolButton btnStartAnalysis;
        private Sunny.UI.UILabel lblLastScan;
        private Sunny.UI.UIGroupBox uiGroupBox2;
        private Sunny.UI.UIIntegerUpDown numDepth;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UILabel lblDepth;
        private Sunny.UI.UILabel lblScore;
        private Sunny.UI.UILabel lblNodes;
        private Sunny.UI.UILabel lblBestMove;
        private Sunny.UI.UILabel lblPonderMove;
        private Sunny.UI.UIGroupBox uiGroupBox3;
        private Sunny.UI.UITextBox txtFEN;
        private Sunny.UI.UIButton btnManualFEN;
        private Sunny.UI.UIButton btnTestPosition;
        private Sunny.UI.UISymbolButton btnToggleOverlay;
        private Sunny.UI.UIButton btnRecognizeTest;
        private Sunny.UI.UIButton btnUploadImage;
        private Sunny.UI.UIButton btnPasteImage;
        private Sunny.UI.UIGroupBox uiGroupBox4;
        private Sunny.UI.UIRichTextBox txtLog;
        private Sunny.UI.UIButton btnClearLog;

        #region 事件处理方法

        /// <summary>
        /// 识别测试 - 只识别并显示，不调用引擎
        /// </summary>
        private void btnRecognizeTest_Click(object sender, System.EventArgs e)
        {
            if (_selectedArea == System.Drawing.Rectangle.Empty)
            {
                Sunny.UI.UIMessageBox.ShowWarning("请先选择棋盘区域");
                return;
            }

            try
            {
                AppendLog("开始识别测试...");

                // 1. 截图
                var screenshot = Utils.ScreenCapture.CaptureScreen(_selectedArea);
                AppendLog($"截图成功: {screenshot.Width}x{screenshot.Height}");

                // 2. 识别（使用特征分析识别器）
                var recognizer = new Recognition.FeatureBasedRecognizer();
                var result = recognizer.RecognizeToBoard(screenshot);

                if (result.Success)
                {
                    AppendLog($"识别成功！");
                    AppendLog($"FEN: {result.FEN}");
                    
                    // 显示调试信息
                    if (!string.IsNullOrEmpty(result.DebugInfo))
                    {
                        AppendLog("--- 识别详情 ---");
                        AppendLog(result.DebugInfo);
                    }

                    // 3. 加载到棋盘
                    _currentBoard.LoadFromFEN(result.FEN);

                    // 4. 显示在悬浮窗（不调用引擎）
                    if (_overlayWindow == null || _overlayWindow.IsDisposed)
                    {
                        _overlayWindow = new UI.OverlayForm();
                        _overlayWindow.Show();
                    }

                    // 只显示棋盘，不显示走法
                    _overlayWindow.UpdateBoard(_currentBoard, null, null, 0, 0);
                    AppendLog("已在悬浮窗显示识别结果");
                }
                else
                {
                    AppendLog($"识别失败: {result.Message}");
                    Sunny.UI.UIMessageBox.ShowError($"识别失败:\n{result.Message}");
                }

                screenshot?.Dispose();
                recognizer?.Dispose();
            }
            catch (System.Exception ex)
            {
                AppendLog($"错误: {ex.Message}");
                Sunny.UI.UIMessageBox.ShowError($"识别测试出错:\n{ex.Message}");
            }
        }

        /// <summary>
        /// 上传图片识别
        /// </summary>
        private void btnUploadImage_Click(object sender, System.EventArgs e)
        {
            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp;*.gif|所有文件|*.*";
                openFileDialog.Title = "选择棋盘图片";

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        AppendLog($"加载图片: {System.IO.Path.GetFileName(openFileDialog.FileName)}");
                        
                        using (var image = new System.Drawing.Bitmap(openFileDialog.FileName))
                        {
                            RecognizeAndDisplay(image);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        AppendLog($"加载图片失败: {ex.Message}");
                        Sunny.UI.UIMessageBox.ShowError($"无法加载图片:\n{ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 粘贴图片识别
        /// </summary>
        private void btnPasteImage_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (System.Windows.Forms.Clipboard.ContainsImage())
                {
                    AppendLog("从剪贴板获取图片...");
                    
                    using (var image = (System.Drawing.Bitmap)System.Windows.Forms.Clipboard.GetImage())
                    {
                        RecognizeAndDisplay(image);
                    }
                }
                else
                {
                    Sunny.UI.UIMessageBox.ShowWarning("剪贴板中没有图片\n\n提示：\n1. 截图工具截图后会自动复制到剪贴板\n2. 右键图片→复制图片\n3. Ctrl+C 复制图片");
                    AppendLog("剪贴板中没有图片");
                }
            }
            catch (System.Exception ex)
            {
                AppendLog($"粘贴图片失败: {ex.Message}");
                Sunny.UI.UIMessageBox.ShowError($"无法粘贴图片:\n{ex.Message}");
            }
        }

        /// <summary>
        /// 识别图片并显示
        /// </summary>
        private void RecognizeAndDisplay(System.Drawing.Bitmap image)
        {
            try
            {
                AppendLog($"图片尺寸: {image.Width}x{image.Height}");
                AppendLog("开始识别（特征分析算法）...");

                // 使用特征分析识别器
                var recognizer = new Recognition.FeatureBasedRecognizer();
                var result = recognizer.RecognizeToBoard(image);

                if (result.Success)
                {
                    AppendLog("✓ 识别成功！");
                    AppendLog($"FEN: {result.FEN}");
                    
                    // 显示调试信息
                    if (!string.IsNullOrEmpty(result.DebugInfo))
                    {
                        AppendLog("--- 识别详情 ---");
                        var lines = result.DebugInfo.Split('\n');
                        // 只显示前20行，避免日志过长
                        for (int i = 0; i < System.Math.Min(20, lines.Length); i++)
                        {
                            AppendLog(lines[i]);
                        }
                        if (lines.Length > 20)
                        {
                            AppendLog($"... 还有 {lines.Length - 20} 行详情");
                        }
                    }

                    // 加载到棋盘
                    _currentBoard.LoadFromFEN(result.FEN);

                    // 显示在悬浮窗
                    if (_overlayWindow == null || _overlayWindow.IsDisposed)
                    {
                        _overlayWindow = new UI.OverlayForm();
                        _overlayWindow.Show();
                    }

                    _overlayWindow.UpdateBoard(_currentBoard, null, null, 0, 0);
                    AppendLog("✓ 已在悬浮窗显示棋盘");
                    
                    Sunny.UI.UIMessageBox.ShowSuccess("识别成功！\n已在悬浮窗显示棋盘");
                }
                else
                {
                    AppendLog($"✗ 识别失败: {result.Message}");
                    Sunny.UI.UIMessageBox.ShowError($"识别失败:\n{result.Message}");
                }

                recognizer?.Dispose();
            }
            catch (System.Exception ex)
            {
                AppendLog($"错误: {ex.Message}");
                Sunny.UI.UIMessageBox.ShowError($"识别出错:\n{ex.Message}");
            }
        }

        #endregion
    }
}
