namespace WinFormsApp1.second_menu.ChessRecognizer.Forms
{
    partial class FrmSettings
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
            this.tabControl = new Sunny.UI.UITabControl();
            this.tabOcr = new System.Windows.Forms.TabPage();
            this.tabBoard = new System.Windows.Forms.TabPage();
            this.tabFen = new System.Windows.Forms.TabPage();
            this.tabPerf = new System.Windows.Forms.TabPage();
            this.tabUI = new System.Windows.Forms.TabPage();
            this.btnPanel = new System.Windows.Forms.Panel();
            this.btnSave = new Sunny.UI.UIButton();
            this.btnCancel = new Sunny.UI.UIButton();
            this.btnReset = new Sunny.UI.UIButton();
            this.btnExport = new Sunny.UI.UIButton();
            this.btnImport = new Sunny.UI.UIButton();
            // OCR Tab Controls
            this.flowOcr = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDefaultEngine = new Sunny.UI.UILabel();
            this.cboDefaultEngine = new Sunny.UI.UIComboBox();
            this.lblConfidence = new Sunny.UI.UILabel();
            this.panelConfidence = new System.Windows.Forms.FlowLayoutPanel();
            this.trkConfidence = new Sunny.UI.UITrackBar();
            this.lblConfidenceValue = new Sunny.UI.UILabel();
            this.lblPreprocess = new Sunny.UI.UILabel();
            this.chkPreprocessing = new Sunny.UI.UICheckBox();
            this.chkBinarization = new Sunny.UI.UICheckBox();
            this.chkDenoising = new Sunny.UI.UICheckBox();
            this.chkSharpening = new Sunny.UI.UICheckBox();
            this.chkMultiEngine = new Sunny.UI.UICheckBox();
            // Board Tab Controls
            this.flowBoard = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCanny1 = new Sunny.UI.UILabel();
            this.trkCanny1 = new Sunny.UI.UITrackBar();
            this.lblCanny2 = new Sunny.UI.UILabel();
            this.trkCanny2 = new Sunny.UI.UITrackBar();
            this.lblHough = new Sunny.UI.UILabel();
            this.trkHough = new Sunny.UI.UITrackBar();
            this.lblPieceScale = new Sunny.UI.UILabel();
            this.trkPieceScale = new Sunny.UI.UITrackBar();
            // FEN Tab Controls
            this.flowFen = new System.Windows.Forms.FlowLayoutPanel();
            this.chkRuleValidation = new Sunny.UI.UICheckBox();
            this.lblAutoCorrection = new Sunny.UI.UILabel();
            this.cboAutoCorrection = new Sunny.UI.UIComboBox();
            this.chkDefaultRedTurn = new Sunny.UI.UICheckBox();
            // Performance Tab Controls
            this.flowPerf = new System.Windows.Forms.FlowLayoutPanel();
            this.lblThreads = new Sunny.UI.UILabel();
            this.trkThreads = new Sunny.UI.UITrackBar();
            this.lblMaxWidth = new Sunny.UI.UILabel();
            this.trkMaxWidth = new Sunny.UI.UITrackBar();
            this.lblTimeout = new Sunny.UI.UILabel();
            this.trkTimeout = new Sunny.UI.UITrackBar();
            // UI Tab Controls
            this.flowUI = new System.Windows.Forms.FlowLayoutPanel();
            this.chkAutoCopyFEN = new Sunny.UI.UICheckBox();
            this.chkAutoSave = new Sunny.UI.UICheckBox();
            this.lblRecentFiles = new Sunny.UI.UILabel();
            this.trkRecentFiles = new Sunny.UI.UITrackBar();

            this.tabControl.SuspendLayout();
            this.tabOcr.SuspendLayout();
            this.tabBoard.SuspendLayout();
            this.tabFen.SuspendLayout();
            this.tabPerf.SuspendLayout();
            this.tabUI.SuspendLayout();
            this.btnPanel.SuspendLayout();
            this.flowOcr.SuspendLayout();
            this.panelConfidence.SuspendLayout();
            this.flowBoard.SuspendLayout();
            this.flowFen.SuspendLayout();
            this.flowPerf.SuspendLayout();
            this.flowUI.SuspendLayout();
            this.SuspendLayout();

            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabOcr);
            this.tabControl.Controls.Add(this.tabBoard);
            this.tabControl.Controls.Add(this.tabFen);
            this.tabControl.Controls.Add(this.tabPerf);
            this.tabControl.Controls.Add(this.tabUI);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tabControl.Location = new System.Drawing.Point(0, 35);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(600, 615);
            this.tabControl.TabIndex = 0;
            // 
            // tabOcr
            // 
            this.tabOcr.Controls.Add(this.flowOcr);
            this.tabOcr.Location = new System.Drawing.Point(0, 40);
            this.tabOcr.Name = "tabOcr";
            this.tabOcr.Padding = new System.Windows.Forms.Padding(3);
            this.tabOcr.Size = new System.Drawing.Size(600, 575);
            this.tabOcr.TabIndex = 0;
            this.tabOcr.Text = "OCR设置";
            this.tabOcr.UseVisualStyleBackColor = true;
            // 
            // flowOcr
            // 
            this.flowOcr.AutoScroll = true;
            this.flowOcr.Controls.Add(this.lblDefaultEngine);
            this.flowOcr.Controls.Add(this.cboDefaultEngine);
            this.flowOcr.Controls.Add(this.lblConfidence);
            this.flowOcr.Controls.Add(this.panelConfidence);
            this.flowOcr.Controls.Add(this.lblPreprocess);
            this.flowOcr.Controls.Add(this.chkPreprocessing);
            this.flowOcr.Controls.Add(this.chkBinarization);
            this.flowOcr.Controls.Add(this.chkDenoising);
            this.flowOcr.Controls.Add(this.chkSharpening);
            this.flowOcr.Controls.Add(this.chkMultiEngine);
            this.flowOcr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowOcr.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowOcr.Location = new System.Drawing.Point(3, 3);
            this.flowOcr.Name = "flowOcr";
            this.flowOcr.Padding = new System.Windows.Forms.Padding(20);
            this.flowOcr.Size = new System.Drawing.Size(594, 569);
            this.flowOcr.TabIndex = 0;
            // 
            // lblDefaultEngine
            // 
            this.lblDefaultEngine.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblDefaultEngine.Location = new System.Drawing.Point(23, 20);
            this.lblDefaultEngine.Name = "lblDefaultEngine";
            this.lblDefaultEngine.Size = new System.Drawing.Size(150, 25);
            this.lblDefaultEngine.TabIndex = 0;
            this.lblDefaultEngine.Text = "默认OCR引擎:";
            this.lblDefaultEngine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboDefaultEngine
            // 
            this.cboDefaultEngine.DataSource = null;
            this.cboDefaultEngine.FillColor = System.Drawing.Color.White;
            this.cboDefaultEngine.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.cboDefaultEngine.Location = new System.Drawing.Point(23, 48);
            this.cboDefaultEngine.MinimumSize = new System.Drawing.Size(63, 0);
            this.cboDefaultEngine.Name = "cboDefaultEngine";
            this.cboDefaultEngine.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cboDefaultEngine.Size = new System.Drawing.Size(200, 32);
            this.cboDefaultEngine.TabIndex = 1;
            this.cboDefaultEngine.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblConfidence
            // 
            this.lblConfidence.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblConfidence.Location = new System.Drawing.Point(23, 93);
            this.lblConfidence.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblConfidence.Name = "lblConfidence";
            this.lblConfidence.Size = new System.Drawing.Size(150, 25);
            this.lblConfidence.TabIndex = 2;
            this.lblConfidence.Text = "置信度阈值:";
            this.lblConfidence.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelConfidence
            // 
            this.panelConfidence.Controls.Add(this.trkConfidence);
            this.panelConfidence.Controls.Add(this.lblConfidenceValue);
            this.panelConfidence.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.panelConfidence.Location = new System.Drawing.Point(23, 121);
            this.panelConfidence.Name = "panelConfidence";
            this.panelConfidence.Size = new System.Drawing.Size(350, 40);
            this.panelConfidence.TabIndex = 3;
            // 
            // trkConfidence
            // 
            this.trkConfidence.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.trkConfidence.Location = new System.Drawing.Point(3, 3);
            this.trkConfidence.Maximum = 99;
            this.trkConfidence.Minimum = 10;
            this.trkConfidence.MinimumSize = new System.Drawing.Size(1, 1);
            this.trkConfidence.Name = "trkConfidence";
            this.trkConfidence.Size = new System.Drawing.Size(250, 30);
            this.trkConfidence.TabIndex = 0;
            this.trkConfidence.Value = 50;
            // 
            // lblConfidenceValue
            // 
            this.lblConfidenceValue.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblConfidenceValue.Location = new System.Drawing.Point(259, 0);
            this.lblConfidenceValue.Name = "lblConfidenceValue";
            this.lblConfidenceValue.Size = new System.Drawing.Size(50, 30);
            this.lblConfidenceValue.TabIndex = 1;
            this.lblConfidenceValue.Text = "50%";
            this.lblConfidenceValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPreprocess
            // 
            this.lblPreprocess.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblPreprocess.Location = new System.Drawing.Point(23, 174);
            this.lblPreprocess.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblPreprocess.Name = "lblPreprocess";
            this.lblPreprocess.Size = new System.Drawing.Size(150, 25);
            this.lblPreprocess.TabIndex = 4;
            this.lblPreprocess.Text = "预处理选项:";
            this.lblPreprocess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkPreprocessing
            // 
            this.chkPreprocessing.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkPreprocessing.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.chkPreprocessing.Location = new System.Drawing.Point(23, 202);
            this.chkPreprocessing.MinimumSize = new System.Drawing.Size(1, 1);
            this.chkPreprocessing.Name = "chkPreprocessing";
            this.chkPreprocessing.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.chkPreprocessing.Size = new System.Drawing.Size(200, 28);
            this.chkPreprocessing.TabIndex = 5;
            this.chkPreprocessing.Text = "启用预处理";
            // 
            // chkBinarization
            // 
            this.chkBinarization.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkBinarization.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.chkBinarization.Location = new System.Drawing.Point(23, 233);
            this.chkBinarization.MinimumSize = new System.Drawing.Size(1, 1);
            this.chkBinarization.Name = "chkBinarization";
            this.chkBinarization.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.chkBinarization.Size = new System.Drawing.Size(200, 28);
            this.chkBinarization.TabIndex = 6;
            this.chkBinarization.Text = "二值化";
            // 
            // chkDenoising
            // 
            this.chkDenoising.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkDenoising.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.chkDenoising.Location = new System.Drawing.Point(23, 264);
            this.chkDenoising.MinimumSize = new System.Drawing.Size(1, 1);
            this.chkDenoising.Name = "chkDenoising";
            this.chkDenoising.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.chkDenoising.Size = new System.Drawing.Size(200, 28);
            this.chkDenoising.TabIndex = 7;
            this.chkDenoising.Text = "去噪";
            // 
            // chkSharpening
            // 
            this.chkSharpening.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkSharpening.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.chkSharpening.Location = new System.Drawing.Point(23, 295);
            this.chkSharpening.MinimumSize = new System.Drawing.Size(1, 1);
            this.chkSharpening.Name = "chkSharpening";
            this.chkSharpening.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.chkSharpening.Size = new System.Drawing.Size(200, 28);
            this.chkSharpening.TabIndex = 8;
            this.chkSharpening.Text = "锐化";
            // 
            // chkMultiEngine
            // 
            this.chkMultiEngine.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkMultiEngine.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.chkMultiEngine.Location = new System.Drawing.Point(23, 336);
            this.chkMultiEngine.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.chkMultiEngine.MinimumSize = new System.Drawing.Size(1, 1);
            this.chkMultiEngine.Name = "chkMultiEngine";
            this.chkMultiEngine.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.chkMultiEngine.Size = new System.Drawing.Size(200, 28);
            this.chkMultiEngine.TabIndex = 9;
            this.chkMultiEngine.Text = "启用多引擎投票";
            // 
            // chkPositionGuessing
            // 
            this.chkPositionGuessing = new Sunny.UI.UICheckBox();
            this.chkPositionGuessing.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkPositionGuessing.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.chkPositionGuessing.Location = new System.Drawing.Point(23, 377);
            this.chkPositionGuessing.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.chkPositionGuessing.MinimumSize = new System.Drawing.Size(1, 1);
            this.chkPositionGuessing.Name = "chkPositionGuessing";
            this.chkPositionGuessing.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.chkPositionGuessing.Size = new System.Drawing.Size(350, 28);
            this.chkPositionGuessing.TabIndex = 10;
            this.chkPositionGuessing.Text = "启用位置推测（OCR失败时根据位置猜测）";
            this.flowOcr.Controls.Add(this.chkPositionGuessing);

            // 
            // tabBoard
            // 
            this.tabBoard.Controls.Add(this.flowBoard);
            this.tabBoard.Location = new System.Drawing.Point(0, 40);
            this.tabBoard.Name = "tabBoard";
            this.tabBoard.Padding = new System.Windows.Forms.Padding(3);
            this.tabBoard.Size = new System.Drawing.Size(600, 575);
            this.tabBoard.TabIndex = 1;
            this.tabBoard.Text = "棋盘检测";
            this.tabBoard.UseVisualStyleBackColor = true;
            // 
            // flowBoard
            // 
            this.flowBoard.AutoScroll = true;
            this.flowBoard.Controls.Add(this.lblCanny1);
            this.flowBoard.Controls.Add(this.trkCanny1);
            this.flowBoard.Controls.Add(this.lblCanny2);
            this.flowBoard.Controls.Add(this.trkCanny2);
            this.flowBoard.Controls.Add(this.lblHough);
            this.flowBoard.Controls.Add(this.trkHough);
            this.flowBoard.Controls.Add(this.lblPieceScale);
            this.flowBoard.Controls.Add(this.trkPieceScale);
            this.flowBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowBoard.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowBoard.Location = new System.Drawing.Point(3, 3);
            this.flowBoard.Name = "flowBoard";
            this.flowBoard.Padding = new System.Windows.Forms.Padding(20);
            this.flowBoard.Size = new System.Drawing.Size(594, 569);
            this.flowBoard.TabIndex = 0;
            // 
            // lblCanny1
            // 
            this.lblCanny1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblCanny1.Location = new System.Drawing.Point(23, 20);
            this.lblCanny1.Name = "lblCanny1";
            this.lblCanny1.Size = new System.Drawing.Size(200, 25);
            this.lblCanny1.TabIndex = 0;
            this.lblCanny1.Text = "Canny边缘检测阈值下限:";
            this.lblCanny1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trkCanny1
            // 
            this.trkCanny1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.trkCanny1.Location = new System.Drawing.Point(23, 48);
            this.trkCanny1.Maximum = 200;
            this.trkCanny1.Minimum = 10;
            this.trkCanny1.MinimumSize = new System.Drawing.Size(1, 1);
            this.trkCanny1.Name = "trkCanny1";
            this.trkCanny1.Size = new System.Drawing.Size(300, 30);
            this.trkCanny1.TabIndex = 1;
            this.trkCanny1.Value = 50;
            // 
            // lblCanny2
            // 
            this.lblCanny2.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblCanny2.Location = new System.Drawing.Point(23, 91);
            this.lblCanny2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblCanny2.Name = "lblCanny2";
            this.lblCanny2.Size = new System.Drawing.Size(200, 25);
            this.lblCanny2.TabIndex = 2;
            this.lblCanny2.Text = "Canny边缘检测阈值上限:";
            this.lblCanny2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trkCanny2
            // 
            this.trkCanny2.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.trkCanny2.Location = new System.Drawing.Point(23, 119);
            this.trkCanny2.Maximum = 300;
            this.trkCanny2.Minimum = 50;
            this.trkCanny2.MinimumSize = new System.Drawing.Size(1, 1);
            this.trkCanny2.Name = "trkCanny2";
            this.trkCanny2.Size = new System.Drawing.Size(300, 30);
            this.trkCanny2.TabIndex = 3;
            this.trkCanny2.Value = 150;
            // 
            // lblHough
            // 
            this.lblHough.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblHough.Location = new System.Drawing.Point(23, 162);
            this.lblHough.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblHough.Name = "lblHough";
            this.lblHough.Size = new System.Drawing.Size(200, 25);
            this.lblHough.TabIndex = 4;
            this.lblHough.Text = "霍夫变换阈值:";
            this.lblHough.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trkHough
            // 
            this.trkHough.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.trkHough.Location = new System.Drawing.Point(23, 190);
            this.trkHough.Maximum = 200;
            this.trkHough.Minimum = 50;
            this.trkHough.MinimumSize = new System.Drawing.Size(1, 1);
            this.trkHough.Name = "trkHough";
            this.trkHough.Size = new System.Drawing.Size(300, 30);
            this.trkHough.TabIndex = 5;
            this.trkHough.Value = 100;
            // 
            // lblPieceScale
            // 
            this.lblPieceScale.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblPieceScale.Location = new System.Drawing.Point(23, 233);
            this.lblPieceScale.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblPieceScale.Name = "lblPieceScale";
            this.lblPieceScale.Size = new System.Drawing.Size(200, 25);
            this.lblPieceScale.TabIndex = 6;
            this.lblPieceScale.Text = "棋子区域比例 (%):";
            this.lblPieceScale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trkPieceScale
            // 
            this.trkPieceScale.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.trkPieceScale.Location = new System.Drawing.Point(23, 261);
            this.trkPieceScale.Maximum = 100;
            this.trkPieceScale.Minimum = 50;
            this.trkPieceScale.MinimumSize = new System.Drawing.Size(1, 1);
            this.trkPieceScale.Name = "trkPieceScale";
            this.trkPieceScale.Size = new System.Drawing.Size(300, 30);
            this.trkPieceScale.TabIndex = 7;
            this.trkPieceScale.Value = 80;
            // 
            // tabFen
            // 
            this.tabFen.Controls.Add(this.flowFen);
            this.tabFen.Location = new System.Drawing.Point(0, 40);
            this.tabFen.Name = "tabFen";
            this.tabFen.Padding = new System.Windows.Forms.Padding(3);
            this.tabFen.Size = new System.Drawing.Size(600, 575);
            this.tabFen.TabIndex = 2;
            this.tabFen.Text = "FEN生成";
            this.tabFen.UseVisualStyleBackColor = true;
            // 
            // flowFen
            // 
            this.flowFen.AutoScroll = true;
            this.flowFen.Controls.Add(this.chkRuleValidation);
            this.flowFen.Controls.Add(this.lblAutoCorrection);
            this.flowFen.Controls.Add(this.cboAutoCorrection);
            this.flowFen.Controls.Add(this.chkDefaultRedTurn);
            this.flowFen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowFen.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowFen.Location = new System.Drawing.Point(3, 3);
            this.flowFen.Name = "flowFen";
            this.flowFen.Padding = new System.Windows.Forms.Padding(20);
            this.flowFen.Size = new System.Drawing.Size(594, 569);
            this.flowFen.TabIndex = 0;
            // 
            // chkRuleValidation
            // 
            this.chkRuleValidation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkRuleValidation.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.chkRuleValidation.Location = new System.Drawing.Point(23, 23);
            this.chkRuleValidation.MinimumSize = new System.Drawing.Size(1, 1);
            this.chkRuleValidation.Name = "chkRuleValidation";
            this.chkRuleValidation.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.chkRuleValidation.Size = new System.Drawing.Size(200, 28);
            this.chkRuleValidation.TabIndex = 0;
            this.chkRuleValidation.Text = "启用规则验证";
            // 
            // lblAutoCorrection
            // 
            this.lblAutoCorrection.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblAutoCorrection.Location = new System.Drawing.Point(23, 64);
            this.lblAutoCorrection.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblAutoCorrection.Name = "lblAutoCorrection";
            this.lblAutoCorrection.Size = new System.Drawing.Size(150, 25);
            this.lblAutoCorrection.TabIndex = 1;
            this.lblAutoCorrection.Text = "自动纠错级别:";
            this.lblAutoCorrection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboAutoCorrection
            // 
            this.cboAutoCorrection.DataSource = null;
            this.cboAutoCorrection.FillColor = System.Drawing.Color.White;
            this.cboAutoCorrection.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.cboAutoCorrection.Location = new System.Drawing.Point(23, 92);
            this.cboAutoCorrection.MinimumSize = new System.Drawing.Size(63, 0);
            this.cboAutoCorrection.Name = "cboAutoCorrection";
            this.cboAutoCorrection.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cboAutoCorrection.Size = new System.Drawing.Size(200, 32);
            this.cboAutoCorrection.TabIndex = 2;
            this.cboAutoCorrection.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkDefaultRedTurn
            // 
            this.chkDefaultRedTurn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkDefaultRedTurn.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.chkDefaultRedTurn.Location = new System.Drawing.Point(23, 137);
            this.chkDefaultRedTurn.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.chkDefaultRedTurn.MinimumSize = new System.Drawing.Size(1, 1);
            this.chkDefaultRedTurn.Name = "chkDefaultRedTurn";
            this.chkDefaultRedTurn.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.chkDefaultRedTurn.Size = new System.Drawing.Size(200, 28);
            this.chkDefaultRedTurn.TabIndex = 3;
            this.chkDefaultRedTurn.Text = "默认红方先走";

            // 
            // tabPerf
            // 
            this.tabPerf.Controls.Add(this.flowPerf);
            this.tabPerf.Location = new System.Drawing.Point(0, 40);
            this.tabPerf.Name = "tabPerf";
            this.tabPerf.Padding = new System.Windows.Forms.Padding(3);
            this.tabPerf.Size = new System.Drawing.Size(600, 575);
            this.tabPerf.TabIndex = 3;
            this.tabPerf.Text = "性能";
            this.tabPerf.UseVisualStyleBackColor = true;
            // 
            // flowPerf
            // 
            this.flowPerf.AutoScroll = true;
            this.flowPerf.Controls.Add(this.lblThreads);
            this.flowPerf.Controls.Add(this.trkThreads);
            this.flowPerf.Controls.Add(this.lblMaxWidth);
            this.flowPerf.Controls.Add(this.trkMaxWidth);
            this.flowPerf.Controls.Add(this.lblTimeout);
            this.flowPerf.Controls.Add(this.trkTimeout);
            this.flowPerf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPerf.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowPerf.Location = new System.Drawing.Point(3, 3);
            this.flowPerf.Name = "flowPerf";
            this.flowPerf.Padding = new System.Windows.Forms.Padding(20);
            this.flowPerf.Size = new System.Drawing.Size(594, 569);
            this.flowPerf.TabIndex = 0;
            // 
            // lblThreads
            // 
            this.lblThreads.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblThreads.Location = new System.Drawing.Point(23, 20);
            this.lblThreads.Name = "lblThreads";
            this.lblThreads.Size = new System.Drawing.Size(150, 25);
            this.lblThreads.TabIndex = 0;
            this.lblThreads.Text = "并行线程数:";
            this.lblThreads.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trkThreads
            // 
            this.trkThreads.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.trkThreads.Location = new System.Drawing.Point(23, 48);
            this.trkThreads.Maximum = 16;
            this.trkThreads.Minimum = 1;
            this.trkThreads.MinimumSize = new System.Drawing.Size(1, 1);
            this.trkThreads.Name = "trkThreads";
            this.trkThreads.Size = new System.Drawing.Size(300, 30);
            this.trkThreads.TabIndex = 1;
            this.trkThreads.Value = 4;
            // 
            // lblMaxWidth
            // 
            this.lblMaxWidth.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblMaxWidth.Location = new System.Drawing.Point(23, 91);
            this.lblMaxWidth.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblMaxWidth.Name = "lblMaxWidth";
            this.lblMaxWidth.Size = new System.Drawing.Size(150, 25);
            this.lblMaxWidth.TabIndex = 2;
            this.lblMaxWidth.Text = "最大图片宽度:";
            this.lblMaxWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trkMaxWidth
            // 
            this.trkMaxWidth.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.trkMaxWidth.Location = new System.Drawing.Point(23, 119);
            this.trkMaxWidth.Maximum = 4000;
            this.trkMaxWidth.Minimum = 500;
            this.trkMaxWidth.MinimumSize = new System.Drawing.Size(1, 1);
            this.trkMaxWidth.Name = "trkMaxWidth";
            this.trkMaxWidth.Size = new System.Drawing.Size(300, 30);
            this.trkMaxWidth.TabIndex = 3;
            this.trkMaxWidth.Value = 2000;
            // 
            // lblTimeout
            // 
            this.lblTimeout.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblTimeout.Location = new System.Drawing.Point(23, 162);
            this.lblTimeout.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(150, 25);
            this.lblTimeout.TabIndex = 4;
            this.lblTimeout.Text = "超时时间 (秒):";
            this.lblTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trkTimeout
            // 
            this.trkTimeout.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.trkTimeout.Location = new System.Drawing.Point(23, 190);
            this.trkTimeout.Maximum = 120;
            this.trkTimeout.Minimum = 5;
            this.trkTimeout.MinimumSize = new System.Drawing.Size(1, 1);
            this.trkTimeout.Name = "trkTimeout";
            this.trkTimeout.Size = new System.Drawing.Size(300, 30);
            this.trkTimeout.TabIndex = 5;
            this.trkTimeout.Value = 30;
            // 
            // tabUI
            // 
            this.tabUI.Controls.Add(this.flowUI);
            this.tabUI.Location = new System.Drawing.Point(0, 40);
            this.tabUI.Name = "tabUI";
            this.tabUI.Padding = new System.Windows.Forms.Padding(3);
            this.tabUI.Size = new System.Drawing.Size(600, 575);
            this.tabUI.TabIndex = 4;
            this.tabUI.Text = "界面";
            this.tabUI.UseVisualStyleBackColor = true;
            // 
            // flowUI
            // 
            this.flowUI.AutoScroll = true;
            this.flowUI.Controls.Add(this.chkAutoCopyFEN);
            this.flowUI.Controls.Add(this.chkAutoSave);
            this.flowUI.Controls.Add(this.lblRecentFiles);
            this.flowUI.Controls.Add(this.trkRecentFiles);
            this.flowUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowUI.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowUI.Location = new System.Drawing.Point(3, 3);
            this.flowUI.Name = "flowUI";
            this.flowUI.Padding = new System.Windows.Forms.Padding(20);
            this.flowUI.Size = new System.Drawing.Size(594, 569);
            this.flowUI.TabIndex = 0;
            // 
            // chkAutoCopyFEN
            // 
            this.chkAutoCopyFEN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkAutoCopyFEN.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.chkAutoCopyFEN.Location = new System.Drawing.Point(23, 23);
            this.chkAutoCopyFEN.MinimumSize = new System.Drawing.Size(1, 1);
            this.chkAutoCopyFEN.Name = "chkAutoCopyFEN";
            this.chkAutoCopyFEN.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.chkAutoCopyFEN.Size = new System.Drawing.Size(250, 28);
            this.chkAutoCopyFEN.TabIndex = 0;
            this.chkAutoCopyFEN.Text = "自动复制FEN到剪贴板";
            // 
            // chkAutoSave
            // 
            this.chkAutoSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkAutoSave.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.chkAutoSave.Location = new System.Drawing.Point(23, 64);
            this.chkAutoSave.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.chkAutoSave.MinimumSize = new System.Drawing.Size(1, 1);
            this.chkAutoSave.Name = "chkAutoSave";
            this.chkAutoSave.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.chkAutoSave.Size = new System.Drawing.Size(200, 28);
            this.chkAutoSave.TabIndex = 1;
            this.chkAutoSave.Text = "自动保存结果";
            // 
            // lblRecentFiles
            // 
            this.lblRecentFiles.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblRecentFiles.Location = new System.Drawing.Point(23, 105);
            this.lblRecentFiles.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblRecentFiles.Name = "lblRecentFiles";
            this.lblRecentFiles.Size = new System.Drawing.Size(150, 25);
            this.lblRecentFiles.TabIndex = 2;
            this.lblRecentFiles.Text = "最近文件数量:";
            this.lblRecentFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trkRecentFiles
            // 
            this.trkRecentFiles.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.trkRecentFiles.Location = new System.Drawing.Point(23, 133);
            this.trkRecentFiles.Maximum = 30;
            this.trkRecentFiles.Minimum = 5;
            this.trkRecentFiles.MinimumSize = new System.Drawing.Size(1, 1);
            this.trkRecentFiles.Name = "trkRecentFiles";
            this.trkRecentFiles.Size = new System.Drawing.Size(300, 30);
            this.trkRecentFiles.TabIndex = 3;
            this.trkRecentFiles.Value = 10;

            // 
            // btnPanel
            // 
            this.btnPanel.Controls.Add(this.btnReset);
            this.btnPanel.Controls.Add(this.btnExport);
            this.btnPanel.Controls.Add(this.btnImport);
            this.btnPanel.Controls.Add(this.btnSave);
            this.btnPanel.Controls.Add(this.btnCancel);
            this.btnPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnPanel.Location = new System.Drawing.Point(0, 650);
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.Size = new System.Drawing.Size(600, 50);
            this.btnPanel.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.btnSave.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(171)))), ((int)(((byte)(155)))));
            this.btnSave.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(109)))));
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnSave.Location = new System.Drawing.Point(420, 8);
            this.btnSave.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.btnSave.Size = new System.Drawing.Size(80, 35);
            this.btnSave.Style = Sunny.UI.UIStyle.Green;
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.TipsFont = new System.Drawing.Font("微软雅黑", 9F);
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnCancel.Location = new System.Drawing.Point(510, 8);
            this.btnCancel.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 35);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.TipsFont = new System.Drawing.Font("微软雅黑", 9F);
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnReset
            // 
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(162)))), ((int)(((byte)(60)))));
            this.btnReset.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(177)))), ((int)(((byte)(95)))));
            this.btnReset.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(130)))), ((int)(((byte)(48)))));
            this.btnReset.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnReset.Location = new System.Drawing.Point(10, 8);
            this.btnReset.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnReset.Name = "btnReset";
            this.btnReset.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(162)))), ((int)(((byte)(60)))));
            this.btnReset.Size = new System.Drawing.Size(80, 35);
            this.btnReset.Style = Sunny.UI.UIStyle.Orange;
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "重置";
            this.btnReset.TipsFont = new System.Drawing.Font("微软雅黑", 9F);
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // btnExport
            // 
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnExport.Location = new System.Drawing.Point(100, 8);
            this.btnExport.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(60, 35);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "导出";
            this.btnExport.TipsFont = new System.Drawing.Font("微软雅黑", 9F);
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImport.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnImport.Location = new System.Drawing.Point(170, 8);
            this.btnImport.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(60, 35);
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "导入";
            this.btnImport.TipsFont = new System.Drawing.Font("微软雅黑", 9F);
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 700);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnPanel);
            this.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "识别设置";
            this.tabControl.ResumeLayout(false);
            this.tabOcr.ResumeLayout(false);
            this.tabBoard.ResumeLayout(false);
            this.tabFen.ResumeLayout(false);
            this.tabPerf.ResumeLayout(false);
            this.tabUI.ResumeLayout(false);
            this.btnPanel.ResumeLayout(false);
            this.flowOcr.ResumeLayout(false);
            this.panelConfidence.ResumeLayout(false);
            this.flowBoard.ResumeLayout(false);
            this.flowFen.ResumeLayout(false);
            this.flowPerf.ResumeLayout(false);
            this.flowUI.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UITabControl tabControl;
        private System.Windows.Forms.TabPage tabOcr;
        private System.Windows.Forms.TabPage tabBoard;
        private System.Windows.Forms.TabPage tabFen;
        private System.Windows.Forms.TabPage tabPerf;
        private System.Windows.Forms.TabPage tabUI;
        private System.Windows.Forms.Panel btnPanel;
        private Sunny.UI.UIButton btnSave;
        private Sunny.UI.UIButton btnCancel;
        private Sunny.UI.UIButton btnReset;
        private Sunny.UI.UIButton btnExport;
        private Sunny.UI.UIButton btnImport;
        // OCR Tab
        private System.Windows.Forms.FlowLayoutPanel flowOcr;
        private Sunny.UI.UILabel lblDefaultEngine;
        private Sunny.UI.UIComboBox cboDefaultEngine;
        private Sunny.UI.UILabel lblConfidence;
        private System.Windows.Forms.FlowLayoutPanel panelConfidence;
        private Sunny.UI.UITrackBar trkConfidence;
        private Sunny.UI.UILabel lblConfidenceValue;
        private Sunny.UI.UILabel lblPreprocess;
        private Sunny.UI.UICheckBox chkPreprocessing;
        private Sunny.UI.UICheckBox chkBinarization;
        private Sunny.UI.UICheckBox chkDenoising;
        private Sunny.UI.UICheckBox chkSharpening;
        private Sunny.UI.UICheckBox chkMultiEngine;
        private Sunny.UI.UICheckBox chkPositionGuessing;
        // Board Tab
        private System.Windows.Forms.FlowLayoutPanel flowBoard;
        private Sunny.UI.UILabel lblCanny1;
        private Sunny.UI.UITrackBar trkCanny1;
        private Sunny.UI.UILabel lblCanny2;
        private Sunny.UI.UITrackBar trkCanny2;
        private Sunny.UI.UILabel lblHough;
        private Sunny.UI.UITrackBar trkHough;
        private Sunny.UI.UILabel lblPieceScale;
        private Sunny.UI.UITrackBar trkPieceScale;
        // FEN Tab
        private System.Windows.Forms.FlowLayoutPanel flowFen;
        private Sunny.UI.UICheckBox chkRuleValidation;
        private Sunny.UI.UILabel lblAutoCorrection;
        private Sunny.UI.UIComboBox cboAutoCorrection;
        private Sunny.UI.UICheckBox chkDefaultRedTurn;
        // Performance Tab
        private System.Windows.Forms.FlowLayoutPanel flowPerf;
        private Sunny.UI.UILabel lblThreads;
        private Sunny.UI.UITrackBar trkThreads;
        private Sunny.UI.UILabel lblMaxWidth;
        private Sunny.UI.UITrackBar trkMaxWidth;
        private Sunny.UI.UILabel lblTimeout;
        private Sunny.UI.UITrackBar trkTimeout;
        // UI Tab
        private System.Windows.Forms.FlowLayoutPanel flowUI;
        private Sunny.UI.UICheckBox chkAutoCopyFEN;
        private Sunny.UI.UICheckBox chkAutoSave;
        private Sunny.UI.UILabel lblRecentFiles;
        private Sunny.UI.UITrackBar trkRecentFiles;
    }
}
