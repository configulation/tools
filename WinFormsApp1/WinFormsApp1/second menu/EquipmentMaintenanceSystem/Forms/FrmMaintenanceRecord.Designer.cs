namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    partial class FrmMaintenanceRecord
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

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTop = new Sunny.UI.UIPanel();
            this.btnStatistics = new Sunny.UI.UIButton();
            this.btnExport = new Sunny.UI.UIButton();
            this.btnViewDetail = new Sunny.UI.UIButton();
            this.btnReset = new Sunny.UI.UIButton();
            this.btnSearch = new Sunny.UI.UIButton();
            this.txtKeyword = new Sunny.UI.UITextBox();
            this.lblKeyword = new Sunny.UI.UILabel();
            this.dtpEndDate = new Sunny.UI.UIDatePicker();
            this.lblTo = new Sunny.UI.UILabel();
            this.dtpStartDate = new Sunny.UI.UIDatePicker();
            this.lblDateRange = new Sunny.UI.UILabel();
            this.cmbType = new Sunny.UI.UIComboBox();
            this.lblType = new Sunny.UI.UILabel();
            this.pnlBottom = new Sunny.UI.UIPanel();
            this.lblTotal = new Sunny.UI.UILabel();
            this.dgvRecords = new Sunny.UI.UIDataGridView();
            this.colRecordId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTargetId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaintenanceTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaintenanceItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOperator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnStatistics);
            this.pnlTop.Controls.Add(this.btnExport);
            this.pnlTop.Controls.Add(this.btnViewDetail);
            this.pnlTop.Controls.Add(this.btnReset);
            this.pnlTop.Controls.Add(this.btnSearch);
            this.pnlTop.Controls.Add(this.txtKeyword);
            this.pnlTop.Controls.Add(this.lblKeyword);
            this.pnlTop.Controls.Add(this.dtpEndDate);
            this.pnlTop.Controls.Add(this.lblTo);
            this.pnlTop.Controls.Add(this.dtpStartDate);
            this.pnlTop.Controls.Add(this.lblDateRange);
            this.pnlTop.Controls.Add(this.cmbType);
            this.pnlTop.Controls.Add(this.lblType);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.pnlTop.Location = new System.Drawing.Point(0, 35);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1000, 120);
            this.pnlTop.TabIndex = 0;
            this.pnlTop.Text = null;
            this.pnlTop.AutoScroll = true;
            // 
            // btnStatistics
            // 
            this.btnStatistics.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStatistics.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnStatistics.Location = new System.Drawing.Point(760, 70);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(100, 35);
            this.btnStatistics.TabIndex = 12;
            this.btnStatistics.Text = "统计分析";
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // btnExport
            // 
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnExport.Location = new System.Drawing.Point(640, 70);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(80, 35);
            this.btnExport.TabIndex = 11;
            this.btnExport.Text = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnViewDetail
            // 
            this.btnViewDetail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnViewDetail.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnViewDetail.Location = new System.Drawing.Point(520, 70);
            this.btnViewDetail.Name = "btnViewDetail";
            this.btnViewDetail.Size = new System.Drawing.Size(100, 35);
            this.btnViewDetail.TabIndex = 10;
            this.btnViewDetail.Text = "查看详情";
            this.btnViewDetail.Click += new System.EventHandler(this.btnViewDetail_Click);
            // 
            // btnReset
            // 
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnReset.Location = new System.Drawing.Point(120, 70);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(80, 35);
            this.btnReset.TabIndex = 9;
            this.btnReset.Text = "重置";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnSearch.Location = new System.Drawing.Point(20, 70);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 35);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtKeyword
            // 
            this.txtKeyword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtKeyword.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtKeyword.Location = new System.Drawing.Point(760, 15);
            this.txtKeyword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtKeyword.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.ShowText = false;
            this.txtKeyword.Size = new System.Drawing.Size(200, 35);
            this.txtKeyword.TabIndex = 7;
            this.txtKeyword.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtKeyword.Watermark = "编号/操作人员/项目";
            // 
            // lblKeyword
            // 
            this.lblKeyword.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblKeyword.Location = new System.Drawing.Point(680, 15);
            this.lblKeyword.Name = "lblKeyword";
            this.lblKeyword.Size = new System.Drawing.Size(80, 35);
            this.lblKeyword.TabIndex = 6;
            this.lblKeyword.Text = "关键词：";
            this.lblKeyword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.FillColor = System.Drawing.Color.White;
            this.dtpEndDate.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.dtpEndDate.Location = new System.Drawing.Point(500, 15);
            this.dtpEndDate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtpEndDate.MaxLength = 10;
            this.dtpEndDate.MinimumSize = new System.Drawing.Size(63, 0);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.dtpEndDate.Size = new System.Drawing.Size(150, 35);
            this.dtpEndDate.SymbolDropDown = 61555;
            this.dtpEndDate.SymbolNormal = 61555;
            this.dtpEndDate.TabIndex = 5;
            this.dtpEndDate.Text = "2026-02-03";
            this.dtpEndDate.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.dtpEndDate.Value = new System.DateTime(2026, 2, 3, 0, 0, 0, 0);
            this.dtpEndDate.Watermark = "";
            // 
            // lblTo
            // 
            this.lblTo.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblTo.Location = new System.Drawing.Point(460, 15);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(40, 35);
            this.lblTo.TabIndex = 4;
            this.lblTo.Text = "至";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.FillColor = System.Drawing.Color.White;
            this.dtpStartDate.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.dtpStartDate.Location = new System.Drawing.Point(300, 15);
            this.dtpStartDate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtpStartDate.MaxLength = 10;
            this.dtpStartDate.MinimumSize = new System.Drawing.Size(63, 0);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.dtpStartDate.Size = new System.Drawing.Size(150, 35);
            this.dtpStartDate.SymbolDropDown = 61555;
            this.dtpStartDate.SymbolNormal = 61555;
            this.dtpStartDate.TabIndex = 3;
            this.dtpStartDate.Text = "2026-01-03";
            this.dtpStartDate.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.dtpStartDate.Value = new System.DateTime(2026, 1, 3, 0, 0, 0, 0);
            this.dtpStartDate.Watermark = "";
            // 
            // lblDateRange
            // 
            this.lblDateRange.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblDateRange.Location = new System.Drawing.Point(200, 15);
            this.lblDateRange.Name = "lblDateRange";
            this.lblDateRange.Size = new System.Drawing.Size(100, 35);
            this.lblDateRange.TabIndex = 2;
            this.lblDateRange.Text = "时间范围：";
            this.lblDateRange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbType
            // 
            this.cmbType.DataSource = null;
            this.cmbType.FillColor = System.Drawing.Color.White;
            this.cmbType.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cmbType.Location = new System.Drawing.Point(80, 15);
            this.cmbType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbType.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbType.Name = "cmbType";
            this.cmbType.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbType.Size = new System.Drawing.Size(100, 35);
            this.cmbType.TabIndex = 1;
            this.cmbType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbType.Watermark = "";
            // 
            // lblType
            // 
            this.lblType.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblType.Location = new System.Drawing.Point(20, 15);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(60, 35);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "类型：";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lblTotal);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.pnlBottom.Location = new System.Drawing.Point(0, 660);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1000, 40);
            this.pnlBottom.TabIndex = 1;
            this.pnlBottom.Text = null;
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblTotal.Location = new System.Drawing.Point(20, 5);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(200, 30);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "共 0 条记录";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvRecords
            // 
            this.dgvRecords.AllowUserToAddRows = false;
            this.dgvRecords.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.dgvRecords.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRecords.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecords.BackgroundColor = System.Drawing.Color.White;
            this.dgvRecords.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRecords.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRecords.ColumnHeadersHeight = 32;
            this.dgvRecords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRecordId,
            this.colType,
            this.colTargetId,
            this.colMaintenanceTime,
            this.colMaintenanceItems,
            this.colOperator,
            this.colResult,
            this.colNotes});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRecords.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecords.EnableHeadersVisualStyles = false;
            this.dgvRecords.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.dgvRecords.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.dgvRecords.Location = new System.Drawing.Point(0, 155);
            this.dgvRecords.Name = "dgvRecords";
            this.dgvRecords.ReadOnly = true;
            this.dgvRecords.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.dgvRecords.RowHeadersVisible = false;
            this.dgvRecords.RowTemplate.Height = 29;
            this.dgvRecords.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecords.Size = new System.Drawing.Size(1000, 505);
            this.dgvRecords.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.dgvRecords.TabIndex = 2;
            this.dgvRecords.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRecords_CellDoubleClick);
            // 
            // colRecordId
            // 
            this.colRecordId.HeaderText = "记录ID";
            this.colRecordId.Name = "colRecordId";
            this.colRecordId.ReadOnly = true;
            this.colRecordId.Visible = false;
            // 
            // colType
            // 
            this.colType.HeaderText = "类型";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            // 
            // colTargetId
            // 
            this.colTargetId.HeaderText = "编号";
            this.colTargetId.Name = "colTargetId";
            this.colTargetId.ReadOnly = true;
            // 
            // colMaintenanceTime
            // 
            this.colMaintenanceTime.HeaderText = "保养时间";
            this.colMaintenanceTime.Name = "colMaintenanceTime";
            this.colMaintenanceTime.ReadOnly = true;
            // 
            // colMaintenanceItems
            // 
            this.colMaintenanceItems.HeaderText = "保养项目";
            this.colMaintenanceItems.Name = "colMaintenanceItems";
            this.colMaintenanceItems.ReadOnly = true;
            // 
            // colOperator
            // 
            this.colOperator.HeaderText = "操作人员";
            this.colOperator.Name = "colOperator";
            this.colOperator.ReadOnly = true;
            // 
            // colResult
            // 
            this.colResult.HeaderText = "保养结果";
            this.colResult.Name = "colResult";
            this.colResult.ReadOnly = true;
            // 
            // colNotes
            // 
            this.colNotes.HeaderText = "备注";
            this.colNotes.Name = "colNotes";
            this.colNotes.ReadOnly = true;
            // 
            // FrmMaintenanceRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.dgvRecords);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Name = "FrmMaintenanceRecord";
            this.Text = "保养记录";
            this.Load += new System.EventHandler(this.FrmMaintenanceRecord_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).EndInit();
            this.ResumeLayout(false);
        }

        private Sunny.UI.UIPanel pnlTop;
        private Sunny.UI.UILabel lblType;
        private Sunny.UI.UIComboBox cmbType;
        private Sunny.UI.UILabel lblDateRange;
        private Sunny.UI.UIDatePicker dtpStartDate;
        private Sunny.UI.UILabel lblTo;
        private Sunny.UI.UIDatePicker dtpEndDate;
        private Sunny.UI.UILabel lblKeyword;
        private Sunny.UI.UITextBox txtKeyword;
        private Sunny.UI.UIButton btnSearch;
        private Sunny.UI.UIButton btnReset;
        private Sunny.UI.UIButton btnViewDetail;
        private Sunny.UI.UIButton btnExport;
        private Sunny.UI.UIButton btnStatistics;
        private Sunny.UI.UIPanel pnlBottom;
        private Sunny.UI.UILabel lblTotal;
        private Sunny.UI.UIDataGridView dgvRecords;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRecordId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTargetId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaintenanceTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaintenanceItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOperator;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNotes;
    }
}
