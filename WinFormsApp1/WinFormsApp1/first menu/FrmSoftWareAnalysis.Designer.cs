namespace WinFormsApp1.first_menu
{
    partial class FrmSoftWareAnalysis
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
            this.panelTop = new Sunny.UI.UIPanel();
            this.btnExportCurrent = new Sunny.UI.UISymbolButton();
            this.btnRun = new Sunny.UI.UISymbolButton();
            this.cboBatchSize = new Sunny.UI.UIComboBox();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.btnSaveConfig = new Sunny.UI.UISymbolButton();
            this.btnLoadConfig = new Sunny.UI.UISymbolButton();
            this.btnImportExcel = new Sunny.UI.UISymbolButton();
            this.txtExcelPath = new Sunny.UI.UITextBox();
            this.gridModels = new Sunny.UI.UIDataGridView();
            this.tabResults = new Sunny.UI.UITabControl();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridModels)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.FillColor = System.Drawing.Color.White;
            this.panelTop.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.panelTop.Location = new System.Drawing.Point(0, 35);
            this.panelTop.MinimumSize = new System.Drawing.Size(1, 1);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.panelTop.RectColor = System.Drawing.Color.Silver;
            this.panelTop.Size = new System.Drawing.Size(1000, 90);
            this.panelTop.Style = Sunny.UI.UIStyle.Custom;
            this.panelTop.TabIndex = 0;
            this.panelTop.Text = null;
            this.panelTop.Controls.Add(this.btnExportCurrent);
            this.panelTop.Controls.Add(this.btnRun);
            this.panelTop.Controls.Add(this.cboBatchSize);
            this.panelTop.Controls.Add(this.uiLabel1);
            this.panelTop.Controls.Add(this.btnSaveConfig);
            this.panelTop.Controls.Add(this.btnLoadConfig);
            this.panelTop.Controls.Add(this.btnImportExcel);
            this.panelTop.Controls.Add(this.txtExcelPath);
            // 
            // btnExportCurrent
            // 
            this.btnExportCurrent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportCurrent.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnExportCurrent.Location = new System.Drawing.Point(880, 48);
            this.btnExportCurrent.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnExportCurrent.Name = "btnExportCurrent";
            this.btnExportCurrent.Size = new System.Drawing.Size(110, 32);
            this.btnExportCurrent.Symbol = 61587;
            this.btnExportCurrent.TabIndex = 7;
            this.btnExportCurrent.Text = "导出当前Tab";
            this.btnExportCurrent.Click += new System.EventHandler(this.btnExportCurrent_Click);
            // 
            // btnRun
            // 
            this.btnRun.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRun.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnRun.Location = new System.Drawing.Point(760, 48);
            this.btnRun.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(110, 32);
            this.btnRun.Symbol = 61473;
            this.btnRun.TabIndex = 6;
            this.btnRun.Text = "开始分析";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // cboBatchSize
            // 
            this.cboBatchSize.DataSource = null;
            this.cboBatchSize.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cboBatchSize.FillColor = System.Drawing.Color.White;
            this.cboBatchSize.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cboBatchSize.Location = new System.Drawing.Point(650, 48);
            this.cboBatchSize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboBatchSize.MinimumSize = new System.Drawing.Size(63, 0);
            this.cboBatchSize.Name = "cboBatchSize";
            this.cboBatchSize.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cboBatchSize.Size = new System.Drawing.Size(90, 32);
            this.cboBatchSize.TabIndex = 5;
            this.cboBatchSize.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.uiLabel1.Location = new System.Drawing.Point(520, 48);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(130, 32);
            this.uiLabel1.TabIndex = 4;
            this.uiLabel1.Text = "批量行数:";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveConfig.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnSaveConfig.Location = new System.Drawing.Point(400, 48);
            this.btnSaveConfig.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(110, 32);
            this.btnSaveConfig.Symbol = 61639;
            this.btnSaveConfig.TabIndex = 3;
            this.btnSaveConfig.Text = "保存配置";
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // btnLoadConfig
            // 
            this.btnLoadConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLoadConfig.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnLoadConfig.Location = new System.Drawing.Point(280, 48);
            this.btnLoadConfig.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnLoadConfig.Name = "btnLoadConfig";
            this.btnLoadConfig.Size = new System.Drawing.Size(110, 32);
            this.btnLoadConfig.Symbol = 61568;
            this.btnLoadConfig.TabIndex = 2;
            this.btnLoadConfig.Text = "载入配置";
            this.btnLoadConfig.Click += new System.EventHandler(this.btnLoadConfig_Click);
            // 
            // btnImportExcel
            // 
            this.btnImportExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImportExcel.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnImportExcel.Location = new System.Drawing.Point(160, 48);
            this.btnImportExcel.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnImportExcel.Name = "btnImportExcel";
            this.btnImportExcel.Size = new System.Drawing.Size(110, 32);
            this.btnImportExcel.Symbol = 61564;
            this.btnImportExcel.TabIndex = 1;
            this.btnImportExcel.Text = "导入Excel";
            this.btnImportExcel.Click += new System.EventHandler(this.btnImportExcel_Click);
            // 
            // txtExcelPath
            // 
            this.txtExcelPath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtExcelPath.FillColor = System.Drawing.Color.White;
            this.txtExcelPath.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txtExcelPath.Location = new System.Drawing.Point(12, 10);
            this.txtExcelPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtExcelPath.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtExcelPath.Name = "txtExcelPath";
            this.txtExcelPath.Padding = new System.Windows.Forms.Padding(5);
            this.txtExcelPath.ReadOnly = true;
            this.txtExcelPath.ShowText = false;
            this.txtExcelPath.Size = new System.Drawing.Size(978, 32);
            this.txtExcelPath.TabIndex = 0;
            this.txtExcelPath.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtExcelPath.Watermark = "请选择Excel路径";
            // 
            // gridModels
            // 
            this.gridModels.AllowUserToAddRows = true;
            this.gridModels.AllowUserToDeleteRows = true;
            this.gridModels.BackgroundColor = System.Drawing.Color.White;
            this.gridModels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridModels.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridModels.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.gridModels.Location = new System.Drawing.Point(0, 125);
            this.gridModels.Name = "gridModels";
            this.gridModels.RowHeadersWidth = 51;
            this.gridModels.RowTemplate.Height = 29;
            this.gridModels.Size = new System.Drawing.Size(1000, 180);
            this.gridModels.TabIndex = 1;
            // 
            // tabResults
            // 
            this.tabResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabResults.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabResults.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tabResults.ItemSize = new System.Drawing.Size(150, 40);
            this.tabResults.Location = new System.Drawing.Point(0, 305);
            this.tabResults.MainPage = "";
            this.tabResults.Name = "tabResults";
            this.tabResults.SelectedIndex = 0;
            this.tabResults.Size = new System.Drawing.Size(1000, 257);
            this.tabResults.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabResults.TabIndex = 2;
            // 
            // FrmSoftWareAnalysis
            // 
            this.AllowShowTitle = false;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1000, 562);
            this.Controls.Add(this.tabResults);
            this.Controls.Add(this.gridModels);
            this.Controls.Add(this.panelTop);
            this.Name = "FrmSoftWareAnalysis";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.ShowTitle = false;
            this.Text = "FrmSoftWareAnalysis";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridModels)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIPanel panelTop;
        private Sunny.UI.UITextBox txtExcelPath;
        private Sunny.UI.UISymbolButton btnImportExcel;
        private Sunny.UI.UISymbolButton btnSaveConfig;
        private Sunny.UI.UISymbolButton btnLoadConfig;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIComboBox cboBatchSize;
        private Sunny.UI.UISymbolButton btnRun;
        private Sunny.UI.UISymbolButton btnExportCurrent;
        private Sunny.UI.UIDataGridView gridModels;
        private Sunny.UI.UITabControl tabResults;
    }
}