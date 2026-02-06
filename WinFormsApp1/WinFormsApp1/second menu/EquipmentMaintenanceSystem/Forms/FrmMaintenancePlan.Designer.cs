namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    partial class FrmMaintenancePlan
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTop = new Sunny.UI.UIPanel();
            this.btnExport = new Sunny.UI.UIButton();
            this.btnRefresh = new Sunny.UI.UIButton();
            this.btnToday = new Sunny.UI.UIButton();
            this.btnNextMonth = new Sunny.UI.UIButton();
            this.btnPrevMonth = new Sunny.UI.UIButton();
            this.lblMonth = new Sunny.UI.UILabel();
            this.tabControl = new Sunny.UI.UITabControl();
            this.tabCalendar = new System.Windows.Forms.TabPage();
            this.dgvCalendar = new Sunny.UI.UIDataGridView();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDayOfWeek = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEquipmentCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToolCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPlanList = new System.Windows.Forms.TabPage();
            this.dgvPlanList = new Sunny.UI.UIDataGridView();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlanDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDaysLeft = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlTop.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabCalendar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCalendar)).BeginInit();
            this.tabPlanList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlanList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnExport);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.btnToday);
            this.pnlTop.Controls.Add(this.btnNextMonth);
            this.pnlTop.Controls.Add(this.btnPrevMonth);
            this.pnlTop.Controls.Add(this.lblMonth);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.pnlTop.Location = new System.Drawing.Point(0, 35);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1000, 60);
            this.pnlTop.TabIndex = 0;
            this.pnlTop.Text = null;
            this.pnlTop.AutoScroll = true;
            // 
            // btnExport
            // 
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnExport.Location = new System.Drawing.Point(880, 10);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 40);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnRefresh.Location = new System.Drawing.Point(760, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 40);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnToday
            // 
            this.btnToday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToday.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnToday.Location = new System.Drawing.Point(640, 10);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(100, 40);
            this.btnToday.TabIndex = 3;
            this.btnToday.Text = "今天";
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // btnNextMonth
            // 
            this.btnNextMonth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNextMonth.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnNextMonth.Location = new System.Drawing.Point(380, 10);
            this.btnNextMonth.Name = "btnNextMonth";
            this.btnNextMonth.Size = new System.Drawing.Size(100, 40);
            this.btnNextMonth.TabIndex = 2;
            this.btnNextMonth.Text = "下个月 >";
            this.btnNextMonth.Click += new System.EventHandler(this.btnNextMonth_Click);
            // 
            // btnPrevMonth
            // 
            this.btnPrevMonth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrevMonth.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnPrevMonth.Location = new System.Drawing.Point(20, 10);
            this.btnPrevMonth.Name = "btnPrevMonth";
            this.btnPrevMonth.Size = new System.Drawing.Size(100, 40);
            this.btnPrevMonth.TabIndex = 1;
            this.btnPrevMonth.Text = "< 上个月";
            this.btnPrevMonth.Click += new System.EventHandler(this.btnPrevMonth_Click);
            // 
            // lblMonth
            // 
            this.lblMonth.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold);
            this.lblMonth.Location = new System.Drawing.Point(140, 10);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(220, 40);
            this.lblMonth.TabIndex = 0;
            this.lblMonth.Text = "2026年02月";
            this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabCalendar);
            this.tabControl.Controls.Add(this.tabPlanList);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tabControl.Location = new System.Drawing.Point(0, 95);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1000, 605);
            this.tabControl.TabIndex = 1;
            // 
            // tabCalendar
            // 
            this.tabCalendar.Controls.Add(this.dgvCalendar);
            this.tabCalendar.Location = new System.Drawing.Point(4, 30);
            this.tabCalendar.Name = "tabCalendar";
            this.tabCalendar.Padding = new System.Windows.Forms.Padding(3);
            this.tabCalendar.Size = new System.Drawing.Size(992, 571);
            this.tabCalendar.TabIndex = 0;
            this.tabCalendar.Text = "月度日历";
            this.tabCalendar.UseVisualStyleBackColor = true;
            // 
            // dgvCalendar
            // 
            this.dgvCalendar.AllowUserToAddRows = false;
            this.dgvCalendar.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.dgvCalendar.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCalendar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCalendar.BackgroundColor = System.Drawing.Color.White;
            this.dgvCalendar.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCalendar.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCalendar.ColumnHeadersHeight = 32;
            this.dgvCalendar.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDate,
            this.colDayOfWeek,
            this.colEquipmentCount,
            this.colToolCount,
            this.colTotalCount,
            this.colStatus});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCalendar.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCalendar.EnableHeadersVisualStyles = false;
            this.dgvCalendar.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.dgvCalendar.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.dgvCalendar.Location = new System.Drawing.Point(3, 3);
            this.dgvCalendar.Name = "dgvCalendar";
            this.dgvCalendar.ReadOnly = true;
            this.dgvCalendar.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.dgvCalendar.RowHeadersVisible = false;
            this.dgvCalendar.RowTemplate.Height = 29;
            this.dgvCalendar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCalendar.Size = new System.Drawing.Size(986, 565);
            this.dgvCalendar.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.dgvCalendar.TabIndex = 0;
            this.dgvCalendar.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCalendar_CellDoubleClick);
            // 
            // colDate
            // 
            this.colDate.HeaderText = "日期";
            this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            // 
            // colDayOfWeek
            // 
            this.colDayOfWeek.HeaderText = "星期";
            this.colDayOfWeek.Name = "colDayOfWeek";
            this.colDayOfWeek.ReadOnly = true;
            // 
            // colEquipmentCount
            // 
            this.colEquipmentCount.HeaderText = "设备数";
            this.colEquipmentCount.Name = "colEquipmentCount";
            this.colEquipmentCount.ReadOnly = true;
            // 
            // colToolCount
            // 
            this.colToolCount.HeaderText = "工装数";
            this.colToolCount.Name = "colToolCount";
            this.colToolCount.ReadOnly = true;
            // 
            // colTotalCount
            // 
            this.colTotalCount.HeaderText = "合计";
            this.colTotalCount.Name = "colTotalCount";
            this.colTotalCount.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "状态";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // tabPlanList
            // 
            this.tabPlanList.Controls.Add(this.dgvPlanList);
            this.tabPlanList.Location = new System.Drawing.Point(4, 30);
            this.tabPlanList.Name = "tabPlanList";
            this.tabPlanList.Padding = new System.Windows.Forms.Padding(3);
            this.tabPlanList.Size = new System.Drawing.Size(992, 571);
            this.tabPlanList.TabIndex = 1;
            this.tabPlanList.Text = "计划列表";
            this.tabPlanList.UseVisualStyleBackColor = true;
            // 
            // dgvPlanList
            // 
            this.dgvPlanList.AllowUserToAddRows = false;
            this.dgvPlanList.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.dgvPlanList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvPlanList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPlanList.BackgroundColor = System.Drawing.Color.White;
            this.dgvPlanList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPlanList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPlanList.ColumnHeadersHeight = 32;
            this.dgvPlanList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colType,
            this.colCode,
            this.colLocation,
            this.colCategory,
            this.colSubCategory,
            this.colPlanDate,
            this.colDaysLeft});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPlanList.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvPlanList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPlanList.EnableHeadersVisualStyles = false;
            this.dgvPlanList.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.dgvPlanList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.dgvPlanList.Location = new System.Drawing.Point(3, 3);
            this.dgvPlanList.Name = "dgvPlanList";
            this.dgvPlanList.ReadOnly = true;
            this.dgvPlanList.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.dgvPlanList.RowHeadersVisible = false;
            this.dgvPlanList.RowTemplate.Height = 29;
            this.dgvPlanList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPlanList.Size = new System.Drawing.Size(986, 565);
            this.dgvPlanList.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.dgvPlanList.TabIndex = 0;
            // 
            // colType
            // 
            this.colType.HeaderText = "类型";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            // 
            // colCode
            // 
            this.colCode.HeaderText = "编号";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            // 
            // colLocation
            // 
            this.colLocation.HeaderText = "线别储位";
            this.colLocation.Name = "colLocation";
            this.colLocation.ReadOnly = true;
            // 
            // colCategory
            // 
            this.colCategory.HeaderText = "类别";
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            // 
            // colSubCategory
            // 
            this.colSubCategory.HeaderText = "子类别";
            this.colSubCategory.Name = "colSubCategory";
            this.colSubCategory.ReadOnly = true;
            // 
            // colPlanDate
            // 
            this.colPlanDate.HeaderText = "计划日期";
            this.colPlanDate.Name = "colPlanDate";
            this.colPlanDate.ReadOnly = true;
            // 
            // colDaysLeft
            // 
            this.colDaysLeft.HeaderText = "剩余时间";
            this.colDaysLeft.Name = "colDaysLeft";
            this.colDaysLeft.ReadOnly = true;
            // 
            // FrmMaintenancePlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlTop);
            this.Name = "FrmMaintenancePlan";
            this.Text = "保养计划";
            this.Load += new System.EventHandler(this.FrmMaintenancePlan_Load);
            this.pnlTop.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabCalendar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCalendar)).EndInit();
            this.tabPlanList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlanList)).EndInit();
            this.ResumeLayout(false);
        }

        private Sunny.UI.UIPanel pnlTop;
        private Sunny.UI.UILabel lblMonth;
        private Sunny.UI.UIButton btnPrevMonth;
        private Sunny.UI.UIButton btnNextMonth;
        private Sunny.UI.UIButton btnToday;
        private Sunny.UI.UIButton btnRefresh;
        private Sunny.UI.UIButton btnExport;
        private Sunny.UI.UITabControl tabControl;
        private System.Windows.Forms.TabPage tabCalendar;
        private Sunny.UI.UIDataGridView dgvCalendar;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDayOfWeek;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEquipmentCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToolCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.TabPage tabPlanList;
        private Sunny.UI.UIDataGridView dgvPlanList;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlanDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDaysLeft;
    }
}
