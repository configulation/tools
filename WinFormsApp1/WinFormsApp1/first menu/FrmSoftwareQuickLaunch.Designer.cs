namespace WinFormsApp1.first_menu
{
    partial class FrmSoftwareQuickLaunch : Sunny.UI.UIForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblConfig = new Sunny.UI.UILabel();
            this.txtConfigPath = new Sunny.UI.UITextBox();
            this.btnOpenConfig = new Sunny.UI.UISymbolButton();
            this.btnReload = new Sunny.UI.UISymbolButton();
            this.btnSave = new Sunny.UI.UISymbolButton();
            this.gridSoftware = new Sunny.UI.UIDataGridView();
            this.btnLaunchAuto = new Sunny.UI.UISymbolButton();
            this.btnLaunchAll = new Sunny.UI.UISymbolButton();
            this.btnLaunchSelected = new Sunny.UI.UISymbolButton();
            this.rtbLog = new Sunny.UI.UIRichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridSoftware)).BeginInit();
            this.SuspendLayout();
            // 
            // lblConfig
            // 
            this.lblConfig.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblConfig.Location = new System.Drawing.Point(20, 25);
            this.lblConfig.Name = "lblConfig";
            this.lblConfig.Size = new System.Drawing.Size(92, 25);
            this.lblConfig.TabIndex = 0;
            this.lblConfig.Text = "配置文件";
            this.lblConfig.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtConfigPath
            // 
            this.txtConfigPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConfigPath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtConfigPath.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txtConfigPath.Location = new System.Drawing.Point(118, 25);
            this.txtConfigPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConfigPath.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtConfigPath.Name = "txtConfigPath";
            this.txtConfigPath.Padding = new System.Windows.Forms.Padding(5);
            this.txtConfigPath.ReadOnly = true;
            this.txtConfigPath.ShowText = false;
            this.txtConfigPath.Size = new System.Drawing.Size(515, 29);
            this.txtConfigPath.Style = Sunny.UI.UIStyle.Custom;
            this.txtConfigPath.TabIndex = 1;
            this.txtConfigPath.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtConfigPath.Watermark = "";
            // 
            // btnOpenConfig
            // 
            this.btnOpenConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenConfig.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnOpenConfig.Location = new System.Drawing.Point(642, 25);
            this.btnOpenConfig.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnOpenConfig.Name = "btnOpenConfig";
            this.btnOpenConfig.Size = new System.Drawing.Size(116, 29);
            this.btnOpenConfig.Style = Sunny.UI.UIStyle.Custom;
            this.btnOpenConfig.Symbol = 61468;
            this.btnOpenConfig.TabIndex = 2;
            this.btnOpenConfig.Text = "打开配置";
            this.btnOpenConfig.Click += new System.EventHandler(this.btnOpenConfig_Click);
            // 
            // btnReload
            // 
            this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReload.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnReload.Location = new System.Drawing.Point(764, 25);
            this.btnReload.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(116, 29);
            this.btnReload.Style = Sunny.UI.UIStyle.Custom;
            this.btnReload.Symbol = 61473;
            this.btnReload.TabIndex = 3;
            this.btnReload.Text = "重新加载";
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnSave.Location = new System.Drawing.Point(886, 25);
            this.btnSave.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(116, 29);
            this.btnSave.Style = Sunny.UI.UIStyle.Custom;
            this.btnSave.Symbol = 61508;
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存配置";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gridSoftware
            // 
            this.gridSoftware.AllowUserToAddRows = true;
            this.gridSoftware.AllowUserToDeleteRows = true;
            this.gridSoftware.AllowUserToResizeRows = false;
            this.gridSoftware.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSoftware.BackgroundColor = System.Drawing.Color.White;
            this.gridSoftware.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSoftware.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.gridSoftware.Location = new System.Drawing.Point(20, 70);
            this.gridSoftware.MultiSelect = true;
            this.gridSoftware.Name = "gridSoftware";
            this.gridSoftware.RowHeadersWidth = 62;
            this.gridSoftware.RowTemplate.Height = 28;
            this.gridSoftware.Size = new System.Drawing.Size(982, 396);
            this.gridSoftware.TabIndex = 5;
            this.gridSoftware.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridSoftware_DataError);
            this.gridSoftware.CurrentCellDirtyStateChanged += new System.EventHandler(this.gridSoftware_CurrentCellDirtyStateChanged);
            this.gridSoftware.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridSoftware_CellDoubleClick);
            // 
            // btnLaunchAuto
            // 
            this.btnLaunchAuto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLaunchAuto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLaunchAuto.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnLaunchAuto.Location = new System.Drawing.Point(20, 480);
            this.btnLaunchAuto.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnLaunchAuto.Name = "btnLaunchAuto";
            this.btnLaunchAuto.Size = new System.Drawing.Size(162, 35);
            this.btnLaunchAuto.Style = Sunny.UI.UIStyle.Custom;
            this.btnLaunchAuto.Symbol = 61664;
            this.btnLaunchAuto.TabIndex = 6;
            this.btnLaunchAuto.Text = "启动自动项";
            this.btnLaunchAuto.Click += new System.EventHandler(this.btnLaunchAuto_Click);
            // 
            // btnLaunchAll
            // 
            this.btnLaunchAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLaunchAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLaunchAll.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnLaunchAll.Location = new System.Drawing.Point(188, 480);
            this.btnLaunchAll.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnLaunchAll.Name = "btnLaunchAll";
            this.btnLaunchAll.Size = new System.Drawing.Size(162, 35);
            this.btnLaunchAll.Style = Sunny.UI.UIStyle.Custom;
            this.btnLaunchAll.Symbol = 61728;
            this.btnLaunchAll.TabIndex = 7;
            this.btnLaunchAll.Text = "全部启动";
            this.btnLaunchAll.Click += new System.EventHandler(this.btnLaunchAll_Click);
            // 
            // btnLaunchSelected
            // 
            this.btnLaunchSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLaunchSelected.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLaunchSelected.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnLaunchSelected.Location = new System.Drawing.Point(356, 480);
            this.btnLaunchSelected.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnLaunchSelected.Name = "btnLaunchSelected";
            this.btnLaunchSelected.Size = new System.Drawing.Size(162, 35);
            this.btnLaunchSelected.Style = Sunny.UI.UIStyle.Custom;
            this.btnLaunchSelected.Symbol = 61516;
            this.btnLaunchSelected.TabIndex = 8;
            this.btnLaunchSelected.Text = "启动选中";
            this.btnLaunchSelected.Click += new System.EventHandler(this.btnLaunchSelected_Click);
            // 
            // rtbLog
            // 
            this.rtbLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLog.FillColor = System.Drawing.Color.White;
            this.rtbLog.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.rtbLog.Location = new System.Drawing.Point(20, 525);
            this.rtbLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtbLog.MinimumSize = new System.Drawing.Size(1, 1);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Padding = new System.Windows.Forms.Padding(2);
            this.rtbLog.ReadOnly = true;
            this.rtbLog.ShowText = false;
            this.rtbLog.Size = new System.Drawing.Size(982, 120);
            this.rtbLog.Style = Sunny.UI.UIStyle.Custom;
            this.rtbLog.TabIndex = 9;
            this.rtbLog.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmSoftwareQuickLaunch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1022, 664);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.btnLaunchSelected);
            this.Controls.Add(this.btnLaunchAll);
            this.Controls.Add(this.btnLaunchAuto);
            this.Controls.Add(this.gridSoftware);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnOpenConfig);
            this.Controls.Add(this.txtConfigPath);
            this.Controls.Add(this.lblConfig);
            this.Name = "FrmSoftwareQuickLaunch";
            this.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.Style = Sunny.UI.UIStyle.Custom;
            this.Text = "批量软件启动";
            this.Load += new System.EventHandler(this.FrmSoftwareQuickLaunch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridSoftware)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UILabel lblConfig;
        private Sunny.UI.UITextBox txtConfigPath;
        private Sunny.UI.UISymbolButton btnOpenConfig;
        private Sunny.UI.UISymbolButton btnReload;
        private Sunny.UI.UISymbolButton btnSave;
        private Sunny.UI.UIDataGridView gridSoftware;
        private Sunny.UI.UISymbolButton btnLaunchAuto;
        private Sunny.UI.UISymbolButton btnLaunchAll;
        private Sunny.UI.UISymbolButton btnLaunchSelected;
        private Sunny.UI.UIRichTextBox rtbLog;
    }
}

