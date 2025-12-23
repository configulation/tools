namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.uiMainTabControl = new Sunny.UI.UITabControl();
            this.tabPageDbConfig = new System.Windows.Forms.TabPage();
            this.uiComboBoxDbType = new Sunny.UI.UIComboBox();
            this.uiTextBoxConnString = new Sunny.UI.UITextBox();
            this.uiButtonTestConnection = new Sunny.UI.UIButton();
            this.uiButtonTogglePassword = new Sunny.UI.UIButton();
            this.uiLabelConnStringExample = new Sunny.UI.UILabel();
            this.uiComboBoxTableName = new Sunny.UI.UIComboBox();
            this.uiButtonLoadTable = new Sunny.UI.UIButton();
            this.uiButtonSaveConfig = new Sunny.UI.UIButton();
            this.uiButtonLoadConfig = new Sunny.UI.UIButton();
            this.tabPageMapping = new System.Windows.Forms.TabPage();
            this.uiTextBoxExcelPath = new Sunny.UI.UITextBox();
            this.uiButtonSelectExcel = new Sunny.UI.UIButton();
            this.uiDataGridViewExcel = new Sunny.UI.UIDataGridView();
            this.uiDataGridViewMapping = new Sunny.UI.UIDataGridView();
            this.uiButtonSaveMapping = new Sunny.UI.UIButton();
            this.tabPageImport = new System.Windows.Forms.TabPage();
            this.btnCustomerInput = new Sunny.UI.UIButton();
            this.uiButtonImport = new Sunny.UI.UIButton();
            this.uiProcessBar = new Sunny.UI.UIProcessBar();
            this.uiMainTabControl.SuspendLayout();
            this.tabPageDbConfig.SuspendLayout();
            this.tabPageMapping.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridViewExcel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridViewMapping)).BeginInit();
            this.tabPageImport.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiMainTabControl
            // 
            this.uiMainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiMainTabControl.Controls.Add(this.tabPageDbConfig);
            this.uiMainTabControl.Controls.Add(this.tabPageMapping);
            this.uiMainTabControl.Controls.Add(this.tabPageImport);
            this.uiMainTabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.uiMainTabControl.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMainTabControl.ItemSize = new System.Drawing.Size(150, 40);
            this.uiMainTabControl.Location = new System.Drawing.Point(5, 39);
            this.uiMainTabControl.MainPage = "";
            this.uiMainTabControl.Name = "uiMainTabControl";
            this.uiMainTabControl.SelectedIndex = 0;
            this.uiMainTabControl.Size = new System.Drawing.Size(1677, 1202);
            this.uiMainTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.uiMainTabControl.Style = Sunny.UI.UIStyle.Custom;
            this.uiMainTabControl.TabIndex = 0;
            this.uiMainTabControl.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // tabPageDbConfig
            // 
            this.tabPageDbConfig.BackColor = System.Drawing.Color.White;
            this.tabPageDbConfig.Controls.Add(this.uiComboBoxDbType);
            this.tabPageDbConfig.Controls.Add(this.uiTextBoxConnString);
            this.tabPageDbConfig.Controls.Add(this.uiButtonTestConnection);
            this.tabPageDbConfig.Controls.Add(this.uiButtonTogglePassword);
            this.tabPageDbConfig.Controls.Add(this.uiLabelConnStringExample);
            this.tabPageDbConfig.Controls.Add(this.uiComboBoxTableName);
            this.tabPageDbConfig.Controls.Add(this.uiButtonLoadTable);
            this.tabPageDbConfig.Controls.Add(this.uiButtonSaveConfig);
            this.tabPageDbConfig.Controls.Add(this.uiButtonLoadConfig);
            this.tabPageDbConfig.Location = new System.Drawing.Point(0, 40);
            this.tabPageDbConfig.Name = "tabPageDbConfig";
            this.tabPageDbConfig.Size = new System.Drawing.Size(1677, 1162);
            this.tabPageDbConfig.TabIndex = 0;
            this.tabPageDbConfig.Text = "数据库配置";
            // 
            // uiComboBoxDbType
            // 
            this.uiComboBoxDbType.DataSource = null;
            this.uiComboBoxDbType.FillColor = System.Drawing.Color.White;
            this.uiComboBoxDbType.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiComboBoxDbType.Location = new System.Drawing.Point(20, 30);
            this.uiComboBoxDbType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiComboBoxDbType.MinimumSize = new System.Drawing.Size(63, 0);
            this.uiComboBoxDbType.Name = "uiComboBoxDbType";
            this.uiComboBoxDbType.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.uiComboBoxDbType.Size = new System.Drawing.Size(250, 35);
            this.uiComboBoxDbType.Style = Sunny.UI.UIStyle.Custom;
            this.uiComboBoxDbType.TabIndex = 0;
            this.uiComboBoxDbType.Text = "选择数据库类型";
            this.uiComboBoxDbType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiComboBoxDbType.Watermark = "请选择数据库类型";
            this.uiComboBoxDbType.SelectedIndexChanged += new System.EventHandler(this.uiComboBoxDbType_SelectedIndexChanged);
            // 
            // uiTextBoxConnString
            // 
            this.uiTextBoxConnString.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.uiTextBoxConnString.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiTextBoxConnString.Location = new System.Drawing.Point(20, 90);
            this.uiTextBoxConnString.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTextBoxConnString.MinimumSize = new System.Drawing.Size(1, 16);
            this.uiTextBoxConnString.Name = "uiTextBoxConnString";
            this.uiTextBoxConnString.Padding = new System.Windows.Forms.Padding(5);
            this.uiTextBoxConnString.PasswordChar = '*';
            this.uiTextBoxConnString.ShowText = false;
            this.uiTextBoxConnString.Size = new System.Drawing.Size(500, 35);
            this.uiTextBoxConnString.Style = Sunny.UI.UIStyle.Custom;
            this.uiTextBoxConnString.TabIndex = 1;
            this.uiTextBoxConnString.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiTextBoxConnString.Watermark = "请输入数据库连接字符串";
            // 
            // uiButtonTestConnection
            // 
            this.uiButtonTestConnection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButtonTestConnection.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.uiButtonTestConnection.Location = new System.Drawing.Point(550, 30);
            this.uiButtonTestConnection.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButtonTestConnection.Name = "uiButtonTestConnection";
            this.uiButtonTestConnection.Size = new System.Drawing.Size(120, 35);
            this.uiButtonTestConnection.Style = Sunny.UI.UIStyle.Custom;
            this.uiButtonTestConnection.TabIndex = 2;
            this.uiButtonTestConnection.Text = "测试连接";
            this.uiButtonTestConnection.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButtonTestConnection.Click += new System.EventHandler(this.uiButtonTestConnection_Click);
            // 
            // uiButtonTogglePassword
            // 
            this.uiButtonTogglePassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButtonTogglePassword.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.uiButtonTogglePassword.Location = new System.Drawing.Point(680, 30);
            this.uiButtonTogglePassword.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButtonTogglePassword.Name = "uiButtonTogglePassword";
            this.uiButtonTogglePassword.Size = new System.Drawing.Size(120, 35);
            this.uiButtonTogglePassword.Style = Sunny.UI.UIStyle.Custom;
            this.uiButtonTogglePassword.TabIndex = 3;
            this.uiButtonTogglePassword.Text = "显示密码";
            this.uiButtonTogglePassword.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButtonTogglePassword.Click += new System.EventHandler(this.uiButtonTogglePassword_Click);
            // 
            // uiLabelConnStringExample
            // 
            this.uiLabelConnStringExample.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiLabelConnStringExample.Location = new System.Drawing.Point(20, 140);
            this.uiLabelConnStringExample.Name = "uiLabelConnStringExample";
            this.uiLabelConnStringExample.Size = new System.Drawing.Size(800, 30);
            this.uiLabelConnStringExample.Style = Sunny.UI.UIStyle.Custom;
            this.uiLabelConnStringExample.TabIndex = 4;
            this.uiLabelConnStringExample.Text = "连接字符串示例";
            this.uiLabelConnStringExample.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiComboBoxTableName
            // 
            this.uiComboBoxTableName.DataSource = null;
            this.uiComboBoxTableName.FillColor = System.Drawing.Color.White;
            this.uiComboBoxTableName.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiComboBoxTableName.Location = new System.Drawing.Point(20, 190);
            this.uiComboBoxTableName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiComboBoxTableName.MinimumSize = new System.Drawing.Size(63, 0);
            this.uiComboBoxTableName.Name = "uiComboBoxTableName";
            this.uiComboBoxTableName.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.uiComboBoxTableName.Size = new System.Drawing.Size(250, 35);
            this.uiComboBoxTableName.Style = Sunny.UI.UIStyle.Custom;
            this.uiComboBoxTableName.TabIndex = 5;
            this.uiComboBoxTableName.Text = "GEN_SAMPLE_MASTER_YY";
            this.uiComboBoxTableName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiComboBoxTableName.Watermark = "请输入数据库表名";
            // 
            // uiButtonLoadTable
            // 
            this.uiButtonLoadTable.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButtonLoadTable.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.uiButtonLoadTable.Location = new System.Drawing.Point(300, 190);
            this.uiButtonLoadTable.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButtonLoadTable.Name = "uiButtonLoadTable";
            this.uiButtonLoadTable.Size = new System.Drawing.Size(120, 35);
            this.uiButtonLoadTable.Style = Sunny.UI.UIStyle.Custom;
            this.uiButtonLoadTable.TabIndex = 6;
            this.uiButtonLoadTable.Text = "加载表结构";
            this.uiButtonLoadTable.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButtonLoadTable.Click += new System.EventHandler(this.uiButtonLoadTable_Click);
            // 
            // uiButtonSaveConfig
            // 
            this.uiButtonSaveConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButtonSaveConfig.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.uiButtonSaveConfig.Location = new System.Drawing.Point(20, 336);
            this.uiButtonSaveConfig.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButtonSaveConfig.Name = "uiButtonSaveConfig";
            this.uiButtonSaveConfig.Size = new System.Drawing.Size(120, 35);
            this.uiButtonSaveConfig.Style = Sunny.UI.UIStyle.Custom;
            this.uiButtonSaveConfig.TabIndex = 7;
            this.uiButtonSaveConfig.Text = "保存配置";
            this.uiButtonSaveConfig.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButtonSaveConfig.Click += new System.EventHandler(this.uiButtonSaveConfig_Click);
            // 
            // uiButtonLoadConfig
            // 
            this.uiButtonLoadConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButtonLoadConfig.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.uiButtonLoadConfig.Location = new System.Drawing.Point(150, 336);
            this.uiButtonLoadConfig.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButtonLoadConfig.Name = "uiButtonLoadConfig";
            this.uiButtonLoadConfig.Size = new System.Drawing.Size(120, 35);
            this.uiButtonLoadConfig.Style = Sunny.UI.UIStyle.Custom;
            this.uiButtonLoadConfig.TabIndex = 8;
            this.uiButtonLoadConfig.Text = "加载配置";
            this.uiButtonLoadConfig.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButtonLoadConfig.Click += new System.EventHandler(this.uiButtonLoadConfig_Click);
            // 
            // tabPageMapping
            // 
            this.tabPageMapping.BackColor = System.Drawing.Color.White;
            this.tabPageMapping.Controls.Add(this.uiTextBoxExcelPath);
            this.tabPageMapping.Controls.Add(this.uiButtonSelectExcel);
            this.tabPageMapping.Controls.Add(this.uiDataGridViewExcel);
            this.tabPageMapping.Controls.Add(this.uiDataGridViewMapping);
            this.tabPageMapping.Controls.Add(this.uiButtonSaveMapping);
            this.tabPageMapping.Location = new System.Drawing.Point(0, 40);
            this.tabPageMapping.Name = "tabPageMapping";
            this.tabPageMapping.Size = new System.Drawing.Size(1677, 1162);
            this.tabPageMapping.TabIndex = 1;
            this.tabPageMapping.Text = "字段映射";
            // 
            // uiTextBoxExcelPath
            // 
            this.uiTextBoxExcelPath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.uiTextBoxExcelPath.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiTextBoxExcelPath.Location = new System.Drawing.Point(20, 30);
            this.uiTextBoxExcelPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTextBoxExcelPath.MinimumSize = new System.Drawing.Size(1, 16);
            this.uiTextBoxExcelPath.Name = "uiTextBoxExcelPath";
            this.uiTextBoxExcelPath.Padding = new System.Windows.Forms.Padding(5);
            this.uiTextBoxExcelPath.ReadOnly = true;
            this.uiTextBoxExcelPath.ShowText = false;
            this.uiTextBoxExcelPath.Size = new System.Drawing.Size(500, 35);
            this.uiTextBoxExcelPath.Style = Sunny.UI.UIStyle.Custom;
            this.uiTextBoxExcelPath.TabIndex = 0;
            this.uiTextBoxExcelPath.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiTextBoxExcelPath.Watermark = "Excel文件路径";
            // 
            // uiButtonSelectExcel
            // 
            this.uiButtonSelectExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButtonSelectExcel.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.uiButtonSelectExcel.Location = new System.Drawing.Point(550, 30);
            this.uiButtonSelectExcel.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButtonSelectExcel.Name = "uiButtonSelectExcel";
            this.uiButtonSelectExcel.Size = new System.Drawing.Size(120, 35);
            this.uiButtonSelectExcel.Style = Sunny.UI.UIStyle.Custom;
            this.uiButtonSelectExcel.TabIndex = 2;
            this.uiButtonSelectExcel.Text = "选择Excel";
            this.uiButtonSelectExcel.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButtonSelectExcel.Click += new System.EventHandler(this.uiButtonSelectExcel_Click);
            // 
            // uiDataGridViewExcel
            // 
            this.uiDataGridViewExcel.AllowUserToAddRows = false;
            this.uiDataGridViewExcel.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiDataGridViewExcel.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.uiDataGridViewExcel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiDataGridViewExcel.BackgroundColor = System.Drawing.Color.White;
            this.uiDataGridViewExcel.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridViewExcel.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.uiDataGridViewExcel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.uiDataGridViewExcel.EnableHeadersVisualStyles = false;
            this.uiDataGridViewExcel.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiDataGridViewExcel.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.uiDataGridViewExcel.Location = new System.Drawing.Point(20, 80);
            this.uiDataGridViewExcel.Name = "uiDataGridViewExcel";
            this.uiDataGridViewExcel.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridViewExcel.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.uiDataGridViewExcel.RowHeadersWidth = 51;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDataGridViewExcel.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.uiDataGridViewExcel.RowTemplate.Height = 29;
            this.uiDataGridViewExcel.ScrollBarRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.uiDataGridViewExcel.SelectedIndex = -1;
            this.uiDataGridViewExcel.Size = new System.Drawing.Size(1663, 120);
            this.uiDataGridViewExcel.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiDataGridViewExcel.Style = Sunny.UI.UIStyle.Custom;
            this.uiDataGridViewExcel.TabIndex = 3;
            // 
            // uiDataGridViewMapping
            // 
            this.uiDataGridViewMapping.AllowUserToAddRows = false;
            this.uiDataGridViewMapping.AllowUserToDeleteRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiDataGridViewMapping.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.uiDataGridViewMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiDataGridViewMapping.BackgroundColor = System.Drawing.Color.White;
            this.uiDataGridViewMapping.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridViewMapping.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.uiDataGridViewMapping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.uiDataGridViewMapping.EnableHeadersVisualStyles = false;
            this.uiDataGridViewMapping.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiDataGridViewMapping.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.uiDataGridViewMapping.Location = new System.Drawing.Point(14, 237);
            this.uiDataGridViewMapping.Name = "uiDataGridViewMapping";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridViewMapping.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.uiDataGridViewMapping.RowHeadersWidth = 51;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDataGridViewMapping.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.uiDataGridViewMapping.RowTemplate.Height = 29;
            this.uiDataGridViewMapping.ScrollBarRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.uiDataGridViewMapping.SelectedIndex = -1;
            this.uiDataGridViewMapping.Size = new System.Drawing.Size(1663, 1102);
            this.uiDataGridViewMapping.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiDataGridViewMapping.Style = Sunny.UI.UIStyle.Custom;
            this.uiDataGridViewMapping.TabIndex = 4;
            this.uiDataGridViewMapping.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.uiDataGridViewMapping_CellValueChanged);
            // 
            // uiButtonSaveMapping
            // 
            this.uiButtonSaveMapping.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButtonSaveMapping.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.uiButtonSaveMapping.Location = new System.Drawing.Point(20, 570);
            this.uiButtonSaveMapping.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButtonSaveMapping.Name = "uiButtonSaveMapping";
            this.uiButtonSaveMapping.Size = new System.Drawing.Size(120, 35);
            this.uiButtonSaveMapping.Style = Sunny.UI.UIStyle.Custom;
            this.uiButtonSaveMapping.TabIndex = 5;
            this.uiButtonSaveMapping.Text = "保存映射";
            this.uiButtonSaveMapping.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButtonSaveMapping.Click += new System.EventHandler(this.uiButtonSaveMapping_Click);
            // 
            // tabPageImport
            // 
            this.tabPageImport.BackColor = System.Drawing.Color.White;
            this.tabPageImport.Controls.Add(this.btnCustomerInput);
            this.tabPageImport.Controls.Add(this.uiButtonImport);
            this.tabPageImport.Controls.Add(this.uiProcessBar);
            this.tabPageImport.Location = new System.Drawing.Point(0, 40);
            this.tabPageImport.Name = "tabPageImport";
            this.tabPageImport.Size = new System.Drawing.Size(1677, 1162);
            this.tabPageImport.TabIndex = 2;
            this.tabPageImport.Text = "导入数据";
            // 
            // btnCustomerInput
            // 
            this.btnCustomerInput.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCustomerInput.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnCustomerInput.Location = new System.Drawing.Point(194, 30);
            this.btnCustomerInput.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCustomerInput.Name = "btnCustomerInput";
            this.btnCustomerInput.Size = new System.Drawing.Size(120, 35);
            this.btnCustomerInput.Style = Sunny.UI.UIStyle.Custom;
            this.btnCustomerInput.TabIndex = 0;
            this.btnCustomerInput.Text = "自定义sql导入";
            this.btnCustomerInput.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCustomerInput.Click += new System.EventHandler(this.btnCustomerInput_Click);
            // 
            // uiButtonImport
            // 
            this.uiButtonImport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButtonImport.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.uiButtonImport.Location = new System.Drawing.Point(20, 30);
            this.uiButtonImport.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButtonImport.Name = "uiButtonImport";
            this.uiButtonImport.Size = new System.Drawing.Size(120, 35);
            this.uiButtonImport.Style = Sunny.UI.UIStyle.Custom;
            this.uiButtonImport.TabIndex = 0;
            this.uiButtonImport.Text = "开始导入";
            this.uiButtonImport.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButtonImport.Click += new System.EventHandler(this.uiButtonImport_Click);
            // 
            // uiProcessBar
            // 
            this.uiProcessBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiProcessBar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiProcessBar.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiProcessBar.Location = new System.Drawing.Point(20, 90);
            this.uiProcessBar.MinimumSize = new System.Drawing.Size(70, 3);
            this.uiProcessBar.Name = "uiProcessBar";
            this.uiProcessBar.Size = new System.Drawing.Size(1663, 40);
            this.uiProcessBar.Style = Sunny.UI.UIStyle.Custom;
            this.uiProcessBar.TabIndex = 1;
            this.uiProcessBar.Text = "0%";
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(1281, 1100);
            this.Controls.Add(this.uiMainTabControl);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(2, 36, 2, 2);
            this.Style = Sunny.UI.UIStyle.Custom;
            this.StyleCustomMode = true;
            this.Text = "Excel数据导入系统";
            this.ZoomScaleRect = new System.Drawing.Rectangle(19, 19, 800, 450);
            this.uiMainTabControl.ResumeLayout(false);
            this.tabPageDbConfig.ResumeLayout(false);
            this.tabPageMapping.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridViewExcel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridViewMapping)).EndInit();
            this.tabPageImport.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UITabControl uiMainTabControl;
        private System.Windows.Forms.TabPage tabPageDbConfig;
        private System.Windows.Forms.TabPage tabPageMapping;
        private System.Windows.Forms.TabPage tabPageImport;
        private Sunny.UI.UIComboBox uiComboBoxDbType;
        private Sunny.UI.UITextBox uiTextBoxConnString;
        private Sunny.UI.UIButton uiButtonTestConnection;
        private Sunny.UI.UIButton uiButtonTogglePassword;
        private Sunny.UI.UILabel uiLabelConnStringExample;
        private Sunny.UI.UIComboBox uiComboBoxTableName;
        private Sunny.UI.UIButton uiButtonLoadTable;
        private Sunny.UI.UITextBox uiTextBoxExcelPath;
        private Sunny.UI.UIButton uiButtonSelectExcel;
        private Sunny.UI.UIDataGridView uiDataGridViewExcel;
        private Sunny.UI.UIDataGridView uiDataGridViewMapping;
        private Sunny.UI.UIButton uiButtonSaveMapping;
        private Sunny.UI.UIButton uiButtonImport;
        private Sunny.UI.UIProcessBar uiProcessBar;
        private Sunny.UI.UIButton uiButtonSaveConfig;
        private Sunny.UI.UIButton uiButtonLoadConfig;
        private Sunny.UI.UIButton btnCustomerInput;
    }
}
