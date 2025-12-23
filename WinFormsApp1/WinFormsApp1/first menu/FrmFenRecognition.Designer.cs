namespace WinFormsApp1.first_menu
{
    partial class FrmFenRecognition
    {
        /// <summary>
        ///  必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            picturePreview?.Image?.Dispose();
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        ///  设计器支持所需的方法 - 不要修改
        ///  使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblImage = new Sunny.UI.UILabel();
            this.txtImagePath = new Sunny.UI.UITextBox();
            this.btnBrowseImage = new Sunny.UI.UISymbolButton();
            this.btnOpenImageLocation = new Sunny.UI.UISymbolButton();
            this.picturePreview = new System.Windows.Forms.PictureBox();
            this.lblServer = new Sunny.UI.UILabel();
            this.txtServerUrl = new Sunny.UI.UITextBox();
            this.lblParam = new Sunny.UI.UILabel();
            this.txtParam = new Sunny.UI.UITextBox();
            this.btnRecognize = new Sunny.UI.UISymbolButton();
            this.lblBoard = new Sunny.UI.UILabel();
            this.gridBoard = new Sunny.UI.UIDataGridView();
            this.lblFen = new Sunny.UI.UILabel();
            this.txtFen = new Sunny.UI.UITextBox();
            this.btnCopyFen = new Sunny.UI.UISymbolButton();
            this.lblMove = new Sunny.UI.UILabel();
            this.txtMove = new Sunny.UI.UITextBox();
            this.lblChineseMove = new Sunny.UI.UILabel();
            this.txtChineseMove = new Sunny.UI.UITextBox();
            this.lblSide = new Sunny.UI.UILabel();
            this.txtSide = new Sunny.UI.UITextBox();
            this.lblScore = new Sunny.UI.UILabel();
            this.txtScore = new Sunny.UI.UITextBox();
            this.lblWinRate = new Sunny.UI.UILabel();
            this.txtWinRate = new Sunny.UI.UITextBox();
            this.rtbLog = new Sunny.UI.UIRichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // lblImage
            // 
            this.lblImage.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblImage.Location = new System.Drawing.Point(20, 15);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(96, 23);
            this.lblImage.Style = Sunny.UI.UIStyle.Custom;
            this.lblImage.TabIndex = 0;
            this.lblImage.Text = "棋盘图片";
            this.lblImage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtImagePath
            // 
            this.txtImagePath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtImagePath.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txtImagePath.Location = new System.Drawing.Point(20, 40);
            this.txtImagePath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtImagePath.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.Padding = new System.Windows.Forms.Padding(5);
            this.txtImagePath.ReadOnly = true;
            this.txtImagePath.ShowText = false;
            this.txtImagePath.Size = new System.Drawing.Size(260, 29);
            this.txtImagePath.Style = Sunny.UI.UIStyle.Custom;
            this.txtImagePath.TabIndex = 1;
            this.txtImagePath.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtImagePath.Watermark = "请选择棋盘图片";
            // 
            // btnBrowseImage
            // 
            this.btnBrowseImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowseImage.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnBrowseImage.Location = new System.Drawing.Point(288, 40);
            this.btnBrowseImage.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnBrowseImage.Name = "btnBrowseImage";
            this.btnBrowseImage.Size = new System.Drawing.Size(76, 29);
            this.btnBrowseImage.Style = Sunny.UI.UIStyle.Custom;
            this.btnBrowseImage.Symbol = 61564;
            this.btnBrowseImage.TabIndex = 2;
            this.btnBrowseImage.Text = "浏览";
            this.btnBrowseImage.Click += new System.EventHandler(this.btnBrowseImage_Click);
            // 
            // btnOpenImageLocation
            // 
            this.btnOpenImageLocation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenImageLocation.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnOpenImageLocation.Location = new System.Drawing.Point(372, 40);
            this.btnOpenImageLocation.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnOpenImageLocation.Name = "btnOpenImageLocation";
            this.btnOpenImageLocation.Size = new System.Drawing.Size(76, 29);
            this.btnOpenImageLocation.Style = Sunny.UI.UIStyle.Custom;
            this.btnOpenImageLocation.Symbol = 61465;
            this.btnOpenImageLocation.TabIndex = 3;
            this.btnOpenImageLocation.Text = "定位";
            this.btnOpenImageLocation.Click += new System.EventHandler(this.btnOpenImageLocation_Click);
            // 
            // picturePreview
            // 
            this.picturePreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.picturePreview.BackColor = System.Drawing.Color.White;
            this.picturePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picturePreview.Location = new System.Drawing.Point(20, 80);
            this.picturePreview.Name = "picturePreview";
            this.picturePreview.Size = new System.Drawing.Size(428, 500);
            this.picturePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picturePreview.TabIndex = 4;
            this.picturePreview.TabStop = false;
            // 
            // lblServer
            // 
            this.lblServer.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblServer.Location = new System.Drawing.Point(460, 15);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(116, 23);
            this.lblServer.Style = Sunny.UI.UIStyle.Custom;
            this.lblServer.TabIndex = 5;
            this.lblServer.Text = "识别服务地址";
            this.lblServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtServerUrl
            // 
            this.txtServerUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServerUrl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtServerUrl.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txtServerUrl.Location = new System.Drawing.Point(460, 40);
            this.txtServerUrl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtServerUrl.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtServerUrl.Name = "txtServerUrl";
            this.txtServerUrl.Padding = new System.Windows.Forms.Padding(5);
            this.txtServerUrl.ShowText = false;
            this.txtServerUrl.Size = new System.Drawing.Size(580, 29);
            this.txtServerUrl.Style = Sunny.UI.UIStyle.Custom;
            this.txtServerUrl.TabIndex = 6;
            this.txtServerUrl.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtServerUrl.Watermark = "http://127.0.0.1:5000/api/upload";
            // 
            // lblParam
            // 
            this.lblParam.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblParam.Location = new System.Drawing.Point(460, 80);
            this.lblParam.Name = "lblParam";
            this.lblParam.Size = new System.Drawing.Size(146, 23);
            this.lblParam.Style = Sunny.UI.UIStyle.Custom;
            this.lblParam.TabIndex = 7;
            this.lblParam.Text = "识别参数(JSON)";
            this.lblParam.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtParam
            // 
            this.txtParam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtParam.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtParam.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txtParam.Location = new System.Drawing.Point(460, 105);
            this.txtParam.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtParam.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtParam.Multiline = true;
            this.txtParam.Name = "txtParam";
            this.txtParam.Padding = new System.Windows.Forms.Padding(5);
            this.txtParam.ShowScrollBar = true;
            this.txtParam.ShowText = false;
            this.txtParam.Size = new System.Drawing.Size(580, 80);
            this.txtParam.Style = Sunny.UI.UIStyle.Custom;
            this.txtParam.TabIndex = 8;
            this.txtParam.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtParam.Watermark = "{\"platform\": \"JJ\", \"autoModel\": \"Off\"}";
            // 
            // btnRecognize
            // 
            this.btnRecognize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRecognize.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnRecognize.Location = new System.Drawing.Point(460, 195);
            this.btnRecognize.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnRecognize.Name = "btnRecognize";
            this.btnRecognize.Size = new System.Drawing.Size(160, 35);
            this.btnRecognize.Style = Sunny.UI.UIStyle.Custom;
            this.btnRecognize.Symbol = 61639;
            this.btnRecognize.TabIndex = 9;
            this.btnRecognize.Text = "开始识别";
            this.btnRecognize.Click += new System.EventHandler(this.btnRecognize_Click);
            // 
            // lblBoard
            // 
            this.lblBoard.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblBoard.Location = new System.Drawing.Point(460, 235);
            this.lblBoard.Name = "lblBoard";
            this.lblBoard.Size = new System.Drawing.Size(140, 23);
            this.lblBoard.Style = Sunny.UI.UIStyle.Custom;
            this.lblBoard.TabIndex = 10;
            this.lblBoard.Text = "识别结果棋盘";
            this.lblBoard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gridBoard
            // 
            this.gridBoard.AllowUserToAddRows = false;
            this.gridBoard.AllowUserToDeleteRows = false;
            this.gridBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridBoard.BackgroundColor = System.Drawing.Color.White;
            this.gridBoard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBoard.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.gridBoard.Location = new System.Drawing.Point(460, 260);
            this.gridBoard.MultiSelect = false;
            this.gridBoard.Name = "gridBoard";
            this.gridBoard.ReadOnly = true;
            this.gridBoard.RowHeadersVisible = false;
            this.gridBoard.RowHeadersWidth = 62;
            this.gridBoard.RowTemplate.Height = 28;
            this.gridBoard.Size = new System.Drawing.Size(580, 210);
            this.gridBoard.Style = Sunny.UI.UIStyle.Custom;
            this.gridBoard.TabIndex = 11;
            // 
            // lblFen
            // 
            this.lblFen.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblFen.Location = new System.Drawing.Point(460, 480);
            this.lblFen.Name = "lblFen";
            this.lblFen.Size = new System.Drawing.Size(140, 23);
            this.lblFen.Style = Sunny.UI.UIStyle.Custom;
            this.lblFen.TabIndex = 12;
            this.lblFen.Text = "FEN 字符串";
            this.lblFen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFen
            // 
            this.txtFen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFen.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFen.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txtFen.Location = new System.Drawing.Point(460, 505);
            this.txtFen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFen.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtFen.Multiline = true;
            this.txtFen.Name = "txtFen";
            this.txtFen.Padding = new System.Windows.Forms.Padding(5);
            this.txtFen.ReadOnly = true;
            this.txtFen.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.txtFen.ShowScrollBar = true;
            this.txtFen.ShowText = false;
            this.txtFen.Size = new System.Drawing.Size(480, 60);
            this.txtFen.Style = Sunny.UI.UIStyle.Custom;
            this.txtFen.TabIndex = 13;
            this.txtFen.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtFen.Watermark = "等待识别结果";
            // 
            // btnCopyFen
            // 
            this.btnCopyFen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyFen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCopyFen.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnCopyFen.Location = new System.Drawing.Point(950, 505);
            this.btnCopyFen.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCopyFen.Name = "btnCopyFen";
            this.btnCopyFen.Size = new System.Drawing.Size(90, 60);
            this.btnCopyFen.Style = Sunny.UI.UIStyle.Custom;
            this.btnCopyFen.Symbol = 61637;
            this.btnCopyFen.TabIndex = 14;
            this.btnCopyFen.Text = "复制";
            this.btnCopyFen.Click += new System.EventHandler(this.btnCopyFen_Click);
            // 
            // lblMove
            // 
            this.lblMove.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblMove.Location = new System.Drawing.Point(460, 575);
            this.lblMove.Name = "lblMove";
            this.lblMove.Size = new System.Drawing.Size(60, 23);
            this.lblMove.Style = Sunny.UI.UIStyle.Custom;
            this.lblMove.TabIndex = 15;
            this.lblMove.Text = "着法";
            this.lblMove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtMove
            // 
            this.txtMove.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMove.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txtMove.Location = new System.Drawing.Point(520, 573);
            this.txtMove.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMove.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtMove.Name = "txtMove";
            this.txtMove.Padding = new System.Windows.Forms.Padding(5);
            this.txtMove.ReadOnly = true;
            this.txtMove.ShowText = false;
            this.txtMove.Size = new System.Drawing.Size(200, 29);
            this.txtMove.Style = Sunny.UI.UIStyle.Custom;
            this.txtMove.TabIndex = 16;
            this.txtMove.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblChineseMove
            // 
            this.lblChineseMove.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblChineseMove.Location = new System.Drawing.Point(740, 575);
            this.lblChineseMove.Name = "lblChineseMove";
            this.lblChineseMove.Size = new System.Drawing.Size(80, 23);
            this.lblChineseMove.Style = Sunny.UI.UIStyle.Custom;
            this.lblChineseMove.TabIndex = 17;
            this.lblChineseMove.Text = "中文着法";
            this.lblChineseMove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtChineseMove
            // 
            this.txtChineseMove.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtChineseMove.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txtChineseMove.Location = new System.Drawing.Point(820, 573);
            this.txtChineseMove.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChineseMove.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtChineseMove.Name = "txtChineseMove";
            this.txtChineseMove.Padding = new System.Windows.Forms.Padding(5);
            this.txtChineseMove.ReadOnly = true;
            this.txtChineseMove.ShowText = false;
            this.txtChineseMove.Size = new System.Drawing.Size(220, 29);
            this.txtChineseMove.Style = Sunny.UI.UIStyle.Custom;
            this.txtChineseMove.TabIndex = 18;
            this.txtChineseMove.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSide
            // 
            this.lblSide.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblSide.Location = new System.Drawing.Point(460, 610);
            this.lblSide.Name = "lblSide";
            this.lblSide.Size = new System.Drawing.Size(60, 23);
            this.lblSide.Style = Sunny.UI.UIStyle.Custom;
            this.lblSide.TabIndex = 19;
            this.lblSide.Text = "走方";
            this.lblSide.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSide
            // 
            this.txtSide.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSide.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txtSide.Location = new System.Drawing.Point(520, 608);
            this.txtSide.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSide.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtSide.Name = "txtSide";
            this.txtSide.Padding = new System.Windows.Forms.Padding(5);
            this.txtSide.ReadOnly = true;
            this.txtSide.ShowText = false;
            this.txtSide.Size = new System.Drawing.Size(120, 29);
            this.txtSide.Style = Sunny.UI.UIStyle.Custom;
            this.txtSide.TabIndex = 20;
            this.txtSide.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblScore
            // 
            this.lblScore.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblScore.Location = new System.Drawing.Point(660, 610);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(60, 23);
            this.lblScore.Style = Sunny.UI.UIStyle.Custom;
            this.lblScore.TabIndex = 21;
            this.lblScore.Text = "分数";
            this.lblScore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtScore
            // 
            this.txtScore.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtScore.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txtScore.Location = new System.Drawing.Point(720, 608);
            this.txtScore.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtScore.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtScore.Name = "txtScore";
            this.txtScore.Padding = new System.Windows.Forms.Padding(5);
            this.txtScore.ReadOnly = true;
            this.txtScore.ShowText = false;
            this.txtScore.Size = new System.Drawing.Size(120, 29);
            this.txtScore.Style = Sunny.UI.UIStyle.Custom;
            this.txtScore.TabIndex = 22;
            this.txtScore.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWinRate
            // 
            this.lblWinRate.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblWinRate.Location = new System.Drawing.Point(860, 610);
            this.lblWinRate.Name = "lblWinRate";
            this.lblWinRate.Size = new System.Drawing.Size(60, 23);
            this.lblWinRate.Style = Sunny.UI.UIStyle.Custom;
            this.lblWinRate.TabIndex = 23;
            this.lblWinRate.Text = "胜率";
            this.lblWinRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtWinRate
            // 
            this.txtWinRate.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtWinRate.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txtWinRate.Location = new System.Drawing.Point(920, 608);
            this.txtWinRate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtWinRate.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtWinRate.Name = "txtWinRate";
            this.txtWinRate.Padding = new System.Windows.Forms.Padding(5);
            this.txtWinRate.ReadOnly = true;
            this.txtWinRate.ShowText = false;
            this.txtWinRate.Size = new System.Drawing.Size(120, 29);
            this.txtWinRate.Style = Sunny.UI.UIStyle.Custom;
            this.txtWinRate.TabIndex = 24;
            this.txtWinRate.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rtbLog
            // 
            this.rtbLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLog.FillColor = System.Drawing.Color.White;
            this.rtbLog.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.rtbLog.Location = new System.Drawing.Point(20, 640);
            this.rtbLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtbLog.MinimumSize = new System.Drawing.Size(1, 1);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Padding = new System.Windows.Forms.Padding(2);
            this.rtbLog.ReadOnly = true;
            this.rtbLog.ShowText = false;
            this.rtbLog.Size = new System.Drawing.Size(1020, 70);
            this.rtbLog.Style = Sunny.UI.UIStyle.Custom;
            this.rtbLog.TabIndex = 25;
            this.rtbLog.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmFenRecognition
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1060, 730);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.txtWinRate);
            this.Controls.Add(this.lblWinRate);
            this.Controls.Add(this.txtScore);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.txtSide);
            this.Controls.Add(this.lblSide);
            this.Controls.Add(this.txtChineseMove);
            this.Controls.Add(this.lblChineseMove);
            this.Controls.Add(this.txtMove);
            this.Controls.Add(this.lblMove);
            this.Controls.Add(this.btnCopyFen);
            this.Controls.Add(this.txtFen);
            this.Controls.Add(this.lblFen);
            this.Controls.Add(this.gridBoard);
            this.Controls.Add(this.lblBoard);
            this.Controls.Add(this.btnRecognize);
            this.Controls.Add(this.txtParam);
            this.Controls.Add(this.lblParam);
            this.Controls.Add(this.txtServerUrl);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.picturePreview);
            this.Controls.Add(this.btnOpenImageLocation);
            this.Controls.Add(this.btnBrowseImage);
            this.Controls.Add(this.txtImagePath);
            this.Controls.Add(this.lblImage);
            this.Name = "FrmFenRecognition";
            this.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.Style = Sunny.UI.UIStyle.Custom;
            this.Text = "棋盘识别 FEN";
            this.Load += new System.EventHandler(this.FrmFenRecognition_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBoard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UILabel lblImage;
        private Sunny.UI.UITextBox txtImagePath;
        private Sunny.UI.UISymbolButton btnBrowseImage;
        private Sunny.UI.UISymbolButton btnOpenImageLocation;
        private System.Windows.Forms.PictureBox picturePreview;
        private Sunny.UI.UILabel lblServer;
        private Sunny.UI.UITextBox txtServerUrl;
        private Sunny.UI.UILabel lblParam;
        private Sunny.UI.UITextBox txtParam;
        private Sunny.UI.UISymbolButton btnRecognize;
        private Sunny.UI.UILabel lblBoard;
        private Sunny.UI.UIDataGridView gridBoard;
        private Sunny.UI.UILabel lblFen;
        private Sunny.UI.UITextBox txtFen;
        private Sunny.UI.UISymbolButton btnCopyFen;
        private Sunny.UI.UILabel lblMove;
        private Sunny.UI.UITextBox txtMove;
        private Sunny.UI.UILabel lblChineseMove;
        private Sunny.UI.UITextBox txtChineseMove;
        private Sunny.UI.UILabel lblSide;
        private Sunny.UI.UITextBox txtSide;
        private Sunny.UI.UILabel lblScore;
        private Sunny.UI.UITextBox txtScore;
        private Sunny.UI.UILabel lblWinRate;
        private Sunny.UI.UITextBox txtWinRate;
        private Sunny.UI.UIRichTextBox rtbLog;
    }
}



