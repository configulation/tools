namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    partial class FrmToolManagement
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
            pnlTop = new Sunny.UI.UIPanel();
            txtSearch = new Sunny.UI.UITextBox();
            btnAdd = new Sunny.UI.UIButton();
            btnEdit = new Sunny.UI.UIButton();
            btnDelete = new Sunny.UI.UIButton();
            btnMaintenance = new Sunny.UI.UIButton();
            btnRefresh = new Sunny.UI.UIButton();
            dgvTool = new Sunny.UI.UIDataGridView();
            pnlBottom = new Sunny.UI.UIPanel();
            lblTotal = new Sunny.UI.UILabel();
            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTool).BeginInit();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(txtSearch);
            pnlTop.Controls.Add(btnAdd);
            pnlTop.Controls.Add(btnEdit);
            pnlTop.Controls.Add(btnDelete);
            pnlTop.Controls.Add(btnMaintenance);
            pnlTop.Controls.Add(btnRefresh);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Font = new Font("微软雅黑", 12F);
            pnlTop.Location = new Point(0, 35);
            pnlTop.Margin = new Padding(4, 5, 4, 5);
            pnlTop.MinimumSize = new Size(1, 1);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1000, 60);
            pnlTop.TabIndex = 0;
            pnlTop.Text = null;
            pnlTop.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // txtSearch
            // 
            txtSearch.Cursor = Cursors.IBeam;
            txtSearch.Font = new Font("微软雅黑", 12F);
            txtSearch.Location = new Point(20, 15);
            txtSearch.Margin = new Padding(4, 5, 4, 5);
            txtSearch.MinimumSize = new Size(1, 16);
            txtSearch.Name = "txtSearch";
            txtSearch.Padding = new Padding(5);
            txtSearch.ShowText = false;
            txtSearch.Size = new Size(250, 30);
            txtSearch.TabIndex = 0;
            txtSearch.TextAlignment = ContentAlignment.MiddleLeft;
            txtSearch.Watermark = "搜索工装编码、线别、类别...";
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // btnAdd
            // 
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.Font = new Font("微软雅黑", 12F);
            btnAdd.Location = new Point(300, 10);
            btnAdd.MinimumSize = new Size(1, 1);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(90, 35);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "新增";
            btnAdd.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Cursor = Cursors.Hand;
            btnEdit.Font = new Font("微软雅黑", 12F);
            btnEdit.Location = new Point(420, 10);
            btnEdit.MinimumSize = new Size(1, 1);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(90, 35);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "编辑";
            btnEdit.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Font = new Font("微软雅黑", 12F);
            btnDelete.Location = new Point(540, 10);
            btnDelete.MinimumSize = new Size(1, 1);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(90, 35);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "删除";
            btnDelete.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnDelete.Click += btnDelete_Click;
            // 
            // btnMaintenance
            // 
            btnMaintenance.Cursor = Cursors.Hand;
            btnMaintenance.Font = new Font("微软雅黑", 12F);
            btnMaintenance.Location = new Point(660, 10);
            btnMaintenance.MinimumSize = new Size(1, 1);
            btnMaintenance.Name = "btnMaintenance";
            btnMaintenance.Size = new Size(90, 35);
            btnMaintenance.TabIndex = 4;
            btnMaintenance.Text = "执行保养";
            btnMaintenance.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnMaintenance.Click += btnMaintenance_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Font = new Font("微软雅黑", 12F);
            btnRefresh.Location = new Point(780, 10);
            btnRefresh.MinimumSize = new Size(1, 1);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(90, 35);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "刷新";
            btnRefresh.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnRefresh.Click += btnRefresh_Click;
            // 
            // dgvTool
            // 
            dgvTool.AllowUserToAddRows = false;
            dgvTool.AllowUserToDeleteRows = false;
            dgvTool.BackgroundColor = System.Drawing.Color.White;
            dgvTool.ColumnHeadersHeight = 32;
            dgvTool.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvTool.Dock = DockStyle.Fill;
            dgvTool.Font = new Font("微软雅黑", 12F);
            dgvTool.Location = new Point(0, 95);
            dgvTool.MultiSelect = false;
            dgvTool.Name = "dgvTool";
            dgvTool.ReadOnly = true;
            dgvTool.RowTemplate.Height = 35;
            dgvTool.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTool.Size = new Size(1000, 505);
            dgvTool.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            dgvTool.TabIndex = 1;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(lblTotal);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Font = new Font("微软雅黑", 12F);
            pnlBottom.Location = new Point(0, 600);
            pnlBottom.Margin = new Padding(4, 5, 4, 5);
            pnlBottom.MinimumSize = new Size(1, 1);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(1000, 40);
            pnlBottom.TabIndex = 2;
            pnlBottom.Text = null;
            pnlBottom.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lblTotal
            // 
            lblTotal.Font = new Font("微软雅黑", 12F);
            lblTotal.ForeColor = Color.FromArgb(48, 48, 48);
            lblTotal.Location = new Point(20, 10);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(200, 23);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "共 0 条记录";
            // 
            // FrmToolManagement
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1000, 640);
            Controls.Add(dgvTool);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            Name = "FrmToolManagement";
            Text = "工装管理";
            ZoomScaleRect = new Rectangle(15, 15, 1000, 640);
            Load += FrmToolManagement_Load;
            pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTool).EndInit();
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Sunny.UI.UIPanel pnlTop;
        private Sunny.UI.UITextBox txtSearch;
        private Sunny.UI.UIButton btnAdd;
        private Sunny.UI.UIButton btnEdit;
        private Sunny.UI.UIButton btnDelete;
        private Sunny.UI.UIButton btnMaintenance;
        private Sunny.UI.UIButton btnRefresh;
        private Sunny.UI.UIDataGridView dgvTool;
        private Sunny.UI.UIPanel pnlBottom;
        private Sunny.UI.UILabel lblTotal;
    }
}
