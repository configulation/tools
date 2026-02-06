namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    partial class FrmEquipmentManagement
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
            this.pnlTop = new Sunny.UI.UIPanel();
            this.txtSearch = new Sunny.UI.UITextBox();
            this.btnAdd = new Sunny.UI.UIButton();
            this.btnEdit = new Sunny.UI.UIButton();
            this.btnDelete = new Sunny.UI.UIButton();
            this.btnMaintenance = new Sunny.UI.UIButton();
            this.btnRefresh = new Sunny.UI.UIButton();
            this.dgvEquipment = new Sunny.UI.UIDataGridView();
            this.pnlBottom = new Sunny.UI.UIPanel();
            this.lblTotal = new Sunny.UI.UILabel();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipment)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.btnAdd);
            this.pnlTop.Controls.Add(this.btnEdit);
            this.pnlTop.Controls.Add(this.btnDelete);
            this.pnlTop.Controls.Add(this.btnMaintenance);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1000, 60);
            this.pnlTop.TabIndex = 0;
            this.pnlTop.Text = null;
            // 
            // txtSearch
            // 
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtSearch.Location = new System.Drawing.Point(20, 15);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Padding = new System.Windows.Forms.Padding(5);
            this.txtSearch.Size = new System.Drawing.Size(250, 30);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.Watermark = "搜索设备ID、线别、类别...";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnAdd.Location = new System.Drawing.Point(300, 13);
            this.btnAdd.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(90, 35);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "新增";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnEdit.Location = new System.Drawing.Point(410, 13);
            this.btnEdit.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(90, 35);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "编辑";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnDelete.Location = new System.Drawing.Point(520, 13);
            this.btnDelete.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 35);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnMaintenance
            // 
            this.btnMaintenance.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMaintenance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnMaintenance.Location = new System.Drawing.Point(630, 13);
            this.btnMaintenance.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnMaintenance.Name = "btnMaintenance";
            this.btnMaintenance.Size = new System.Drawing.Size(90, 35);
            this.btnMaintenance.TabIndex = 4;
            this.btnMaintenance.Text = "执行保养";
            this.btnMaintenance.Click += new System.EventHandler(this.btnMaintenance_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnRefresh.Location = new System.Drawing.Point(740, 13);
            this.btnRefresh.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 35);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dgvEquipment
            // 
            this.dgvEquipment.AllowUserToAddRows = false;
            this.dgvEquipment.AllowUserToDeleteRows = false;
            this.dgvEquipment.BackgroundColor = System.Drawing.Color.White;
            this.dgvEquipment.ColumnHeadersHeight = 32;
            this.dgvEquipment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvEquipment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEquipment.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.dgvEquipment.Location = new System.Drawing.Point(0, 60);
            this.dgvEquipment.MultiSelect = false;
            this.dgvEquipment.Name = "dgvEquipment";
            this.dgvEquipment.ReadOnly = true;
            this.dgvEquipment.RowTemplate.Height = 35;
            this.dgvEquipment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEquipment.Size = new System.Drawing.Size(1000, 540);
            this.dgvEquipment.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.dgvEquipment.TabIndex = 1;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lblTotal);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.pnlBottom.Location = new System.Drawing.Point(0, 600);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1000, 40);
            this.pnlBottom.TabIndex = 2;
            this.pnlBottom.Text = null;
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblTotal.Location = new System.Drawing.Point(20, 10);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(200, 23);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "共 0 条记录";
            // 
            // FrmEquipmentManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Controls.Add(this.dgvEquipment);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Name = "FrmEquipmentManagement";
            this.Text = "设备管理";
            this.Load += new System.EventHandler(this.FrmEquipmentManagement_Load);
            this.pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipment)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Sunny.UI.UIPanel pnlTop;
        private Sunny.UI.UITextBox txtSearch;
        private Sunny.UI.UIButton btnAdd;
        private Sunny.UI.UIButton btnEdit;
        private Sunny.UI.UIButton btnDelete;
        private Sunny.UI.UIButton btnMaintenance;
        private Sunny.UI.UIButton btnRefresh;
        private Sunny.UI.UIDataGridView dgvEquipment;
        private Sunny.UI.UIPanel pnlBottom;
        private Sunny.UI.UILabel lblTotal;
    }
}
