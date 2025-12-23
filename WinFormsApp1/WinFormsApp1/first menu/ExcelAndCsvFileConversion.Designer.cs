namespace WinFormsApp1.first_menu
{
    partial class ExcelAndCsvFileConversion
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
            this.lblSource = new Sunny.UI.UILabel();
            this.txtSourceDir = new Sunny.UI.UITextBox();
            this.btnBrowseSource = new Sunny.UI.UISymbolButton();
            this.lblOutput = new Sunny.UI.UILabel();
            this.txtOutputDir = new Sunny.UI.UITextBox();
            this.btnBrowseOutput = new Sunny.UI.UISymbolButton();
            this.lblTargetFormat = new Sunny.UI.UILabel();
            this.cboTargetFormat = new Sunny.UI.UIComboBox();
            this.lblCsvSeparator = new Sunny.UI.UILabel();
            this.cboCsvSeparator = new Sunny.UI.UIComboBox();
            this.lblEncoding = new Sunny.UI.UILabel();
            this.cboEncoding = new Sunny.UI.UIComboBox();
            this.lblExcelFormat = new Sunny.UI.UILabel();
            this.cboExcelFormat = new Sunny.UI.UIComboBox();
            this.chkExcelMergeSheets = new Sunny.UI.UICheckBox();
            this.chkCombineCsvToWorkbook = new Sunny.UI.UICheckBox();
            this.chkIncludeSubfolders = new Sunny.UI.UICheckBox();
            this.btnConvert = new Sunny.UI.UISymbolButton();
            this.btnConvertSingle = new Sunny.UI.UISymbolButton();
            this.rtbLog = new Sunny.UI.UIRichTextBox();
            this.SuspendLayout();
            // 
            // lblSource
            // 
            this.lblSource.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblSource.Location = new System.Drawing.Point(20, 19);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(90, 29);
            this.lblSource.TabIndex = 0;
            this.lblSource.Text = "源文件夹";
            this.lblSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSourceDir
            // 
            this.txtSourceDir.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSourceDir.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txtSourceDir.Location = new System.Drawing.Point(116, 19);
            this.txtSourceDir.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSourceDir.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtSourceDir.Name = "txtSourceDir";
            this.txtSourceDir.Padding = new System.Windows.Forms.Padding(5);
            this.txtSourceDir.ShowText = false;
            this.txtSourceDir.Size = new System.Drawing.Size(520, 29);
            this.txtSourceDir.TabIndex = 1;
            this.txtSourceDir.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSourceDir.Watermark = "";
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowseSource.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnBrowseSource.Location = new System.Drawing.Point(644, 19);
            this.btnBrowseSource.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(96, 29);
            this.btnBrowseSource.TabIndex = 2;
            this.btnBrowseSource.Text = "浏览";
            this.btnBrowseSource.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            // 
            // lblOutput
            // 
            this.lblOutput.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblOutput.Location = new System.Drawing.Point(20, 60);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(90, 29);
            this.lblOutput.TabIndex = 3;
            this.lblOutput.Text = "输出文件夹";
            this.lblOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtOutputDir
            // 
            this.txtOutputDir.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtOutputDir.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txtOutputDir.Location = new System.Drawing.Point(116, 60);
            this.txtOutputDir.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOutputDir.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtOutputDir.Name = "txtOutputDir";
            this.txtOutputDir.Padding = new System.Windows.Forms.Padding(5);
            this.txtOutputDir.ShowText = false;
            this.txtOutputDir.Size = new System.Drawing.Size(520, 29);
            this.txtOutputDir.TabIndex = 4;
            this.txtOutputDir.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtOutputDir.Watermark = "";
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowseOutput.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnBrowseOutput.Location = new System.Drawing.Point(644, 60);
            this.btnBrowseOutput.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(96, 29);
            this.btnBrowseOutput.TabIndex = 5;
            this.btnBrowseOutput.Text = "浏览";
            this.btnBrowseOutput.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // lblTargetFormat
            // 
            this.lblTargetFormat.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblTargetFormat.Location = new System.Drawing.Point(20, 100);
            this.lblTargetFormat.Name = "lblTargetFormat";
            this.lblTargetFormat.Size = new System.Drawing.Size(90, 29);
            this.lblTargetFormat.TabIndex = 6;
            this.lblTargetFormat.Text = "目标格式";
            this.lblTargetFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboTargetFormat
            // 
            this.cboTargetFormat.DataSource = null;
            this.cboTargetFormat.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cboTargetFormat.FillColor = System.Drawing.Color.White;
            this.cboTargetFormat.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cboTargetFormat.Location = new System.Drawing.Point(116, 100);
            this.cboTargetFormat.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboTargetFormat.MinimumSize = new System.Drawing.Size(63, 0);
            this.cboTargetFormat.Name = "cboTargetFormat";
            this.cboTargetFormat.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cboTargetFormat.Size = new System.Drawing.Size(160, 29);
            this.cboTargetFormat.TabIndex = 7;
            this.cboTargetFormat.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cboTargetFormat.Watermark = "";
            this.cboTargetFormat.SelectedIndexChanged += new System.EventHandler(this.cboTargetFormat_SelectedIndexChanged);
            // 
            // lblCsvSeparator
            // 
            this.lblCsvSeparator.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblCsvSeparator.Location = new System.Drawing.Point(292, 100);
            this.lblCsvSeparator.Name = "lblCsvSeparator";
            this.lblCsvSeparator.Size = new System.Drawing.Size(74, 29);
            this.lblCsvSeparator.TabIndex = 8;
            this.lblCsvSeparator.Text = "分隔符";
            this.lblCsvSeparator.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboCsvSeparator
            // 
            this.cboCsvSeparator.DataSource = null;
            this.cboCsvSeparator.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cboCsvSeparator.FillColor = System.Drawing.Color.White;
            this.cboCsvSeparator.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cboCsvSeparator.Location = new System.Drawing.Point(368, 100);
            this.cboCsvSeparator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboCsvSeparator.MinimumSize = new System.Drawing.Size(63, 0);
            this.cboCsvSeparator.Name = "cboCsvSeparator";
            this.cboCsvSeparator.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cboCsvSeparator.Size = new System.Drawing.Size(160, 29);
            this.cboCsvSeparator.TabIndex = 9;
            this.cboCsvSeparator.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cboCsvSeparator.Watermark = "";
            // 
            // lblEncoding
            // 
            this.lblEncoding.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblEncoding.Location = new System.Drawing.Point(540, 100);
            this.lblEncoding.Name = "lblEncoding";
            this.lblEncoding.Size = new System.Drawing.Size(52, 29);
            this.lblEncoding.TabIndex = 10;
            this.lblEncoding.Text = "编码";
            this.lblEncoding.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboEncoding
            // 
            this.cboEncoding.DataSource = null;
            this.cboEncoding.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cboEncoding.FillColor = System.Drawing.Color.White;
            this.cboEncoding.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cboEncoding.Location = new System.Drawing.Point(596, 100);
            this.cboEncoding.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboEncoding.MinimumSize = new System.Drawing.Size(63, 0);
            this.cboEncoding.Name = "cboEncoding";
            this.cboEncoding.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cboEncoding.Size = new System.Drawing.Size(144, 29);
            this.cboEncoding.TabIndex = 11;
            this.cboEncoding.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cboEncoding.Watermark = "";
            // 
            // lblExcelFormat
            // 
            this.lblExcelFormat.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblExcelFormat.Location = new System.Drawing.Point(20, 140);
            this.lblExcelFormat.Name = "lblExcelFormat";
            this.lblExcelFormat.Size = new System.Drawing.Size(90, 29);
            this.lblExcelFormat.TabIndex = 12;
            this.lblExcelFormat.Text = "Excel格式";
            this.lblExcelFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboExcelFormat
            // 
            this.cboExcelFormat.DataSource = null;
            this.cboExcelFormat.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cboExcelFormat.FillColor = System.Drawing.Color.White;
            this.cboExcelFormat.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cboExcelFormat.Location = new System.Drawing.Point(116, 140);
            this.cboExcelFormat.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboExcelFormat.MinimumSize = new System.Drawing.Size(63, 0);
            this.cboExcelFormat.Name = "cboExcelFormat";
            this.cboExcelFormat.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cboExcelFormat.Size = new System.Drawing.Size(160, 29);
            this.cboExcelFormat.TabIndex = 13;
            this.cboExcelFormat.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cboExcelFormat.Watermark = "";
            // 
            // chkExcelMergeSheets
            // 
            this.chkExcelMergeSheets.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkExcelMergeSheets.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.chkExcelMergeSheets.Location = new System.Drawing.Point(292, 140);
            this.chkExcelMergeSheets.MinimumSize = new System.Drawing.Size(1, 1);
            this.chkExcelMergeSheets.Name = "chkExcelMergeSheets";
            this.chkExcelMergeSheets.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.chkExcelMergeSheets.Size = new System.Drawing.Size(222, 29);
            this.chkExcelMergeSheets.TabIndex = 14;
            this.chkExcelMergeSheets.Text = "Excel多Sheet合并为一个CSV";
            // 
            // chkCombineCsvToWorkbook
            // 
            this.chkCombineCsvToWorkbook.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkCombineCsvToWorkbook.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.chkCombineCsvToWorkbook.Location = new System.Drawing.Point(520, 140);
            this.chkCombineCsvToWorkbook.MinimumSize = new System.Drawing.Size(1, 1);
            this.chkCombineCsvToWorkbook.Name = "chkCombineCsvToWorkbook";
            this.chkCombineCsvToWorkbook.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.chkCombineCsvToWorkbook.Size = new System.Drawing.Size(222, 29);
            this.chkCombineCsvToWorkbook.TabIndex = 15;
            this.chkCombineCsvToWorkbook.Text = "所有CSV合并到一个工作簿";
            // 
            // chkIncludeSubfolders
            // 
            this.chkIncludeSubfolders.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkIncludeSubfolders.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.chkIncludeSubfolders.Location = new System.Drawing.Point(20, 180);
            this.chkIncludeSubfolders.MinimumSize = new System.Drawing.Size(1, 1);
            this.chkIncludeSubfolders.Name = "chkIncludeSubfolders";
            this.chkIncludeSubfolders.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.chkIncludeSubfolders.Size = new System.Drawing.Size(182, 29);
            this.chkIncludeSubfolders.TabIndex = 16;
            this.chkIncludeSubfolders.Text = "包含子文件夹";
            // 
            // btnConvert
            // 
            this.btnConvert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConvert.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnConvert.Location = new System.Drawing.Point(596, 176);
            this.btnConvert.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(144, 35);
            this.btnConvert.TabIndex = 17;
            this.btnConvert.Text = "开始转换";
            this.btnConvert.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnConvertSingle
            // 
            this.btnConvertSingle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConvertSingle.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnConvertSingle.Location = new System.Drawing.Point(444, 176);
            this.btnConvertSingle.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnConvertSingle.Name = "btnConvertSingle";
            this.btnConvertSingle.Size = new System.Drawing.Size(144, 35);
            this.btnConvertSingle.TabIndex = 18;
            this.btnConvertSingle.Text = "单个文件转换";
            this.btnConvertSingle.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConvertSingle.Click += new System.EventHandler(this.btnConvertSingle_Click);
            // 
            // rtbLog
            // 
            this.rtbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLog.FillColor = System.Drawing.Color.White;
            this.rtbLog.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.rtbLog.Location = new System.Drawing.Point(20, 219);
            this.rtbLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtbLog.MinimumSize = new System.Drawing.Size(1, 1);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Padding = new System.Windows.Forms.Padding(2);
            this.rtbLog.ShowText = false;
            this.rtbLog.Size = new System.Drawing.Size(720, 252);
            this.rtbLog.TabIndex = 19;
            this.rtbLog.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            // 
            // ExcelAndCsvFileConversion
            // 
            this.AllowShowTitle = false;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 501);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.btnConvertSingle);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.chkIncludeSubfolders);
            this.Controls.Add(this.chkCombineCsvToWorkbook);
            this.Controls.Add(this.chkExcelMergeSheets);
            this.Controls.Add(this.cboExcelFormat);
            this.Controls.Add(this.lblExcelFormat);
            this.Controls.Add(this.cboEncoding);
            this.Controls.Add(this.lblEncoding);
            this.Controls.Add(this.cboCsvSeparator);
            this.Controls.Add(this.lblCsvSeparator);
            this.Controls.Add(this.cboTargetFormat);
            this.Controls.Add(this.lblTargetFormat);
            this.Controls.Add(this.btnBrowseOutput);
            this.Controls.Add(this.txtOutputDir);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.btnBrowseSource);
            this.Controls.Add(this.txtSourceDir);
            this.Controls.Add(this.lblSource);
            this.Name = "ExcelAndCsvFileConversion";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.ShowTitle = false;
            this.Text = "Excel/CSV文件批量转换";
            this.ZoomScaleRect = new System.Drawing.Rectangle(19, 19, 800, 450);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UILabel lblSource;
        private Sunny.UI.UITextBox txtSourceDir;
        private Sunny.UI.UISymbolButton btnBrowseSource;
        private Sunny.UI.UILabel lblOutput;
        private Sunny.UI.UITextBox txtOutputDir;
        private Sunny.UI.UISymbolButton btnBrowseOutput;
        private Sunny.UI.UILabel lblTargetFormat;
        private Sunny.UI.UIComboBox cboTargetFormat;
        private Sunny.UI.UILabel lblCsvSeparator;
        private Sunny.UI.UIComboBox cboCsvSeparator;
        private Sunny.UI.UILabel lblEncoding;
        private Sunny.UI.UIComboBox cboEncoding;
        private Sunny.UI.UILabel lblExcelFormat;
        private Sunny.UI.UIComboBox cboExcelFormat;
        private Sunny.UI.UICheckBox chkExcelMergeSheets;
        private Sunny.UI.UICheckBox chkCombineCsvToWorkbook;
        private Sunny.UI.UICheckBox chkIncludeSubfolders;
        private Sunny.UI.UISymbolButton btnConvert;
        private Sunny.UI.UISymbolButton btnConvertSingle;
        private Sunny.UI.UIRichTextBox rtbLog;
    }
}