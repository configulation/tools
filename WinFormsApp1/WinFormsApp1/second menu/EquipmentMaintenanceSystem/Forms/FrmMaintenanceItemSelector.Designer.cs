namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    partial class FrmMaintenanceItemSelector
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
            this.lblTitle = new Sunny.UI.UILabel();
            this.clbItems = new System.Windows.Forms.CheckedListBox();
            this.btnSelectAll = new Sunny.UI.UIButton();
            this.btnClearAll = new Sunny.UI.UIButton();
            this.btnConfirm = new Sunny.UI.UIButton();
            this.btnCancel = new Sunny.UI.UIButton();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 45);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(360, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "请选择保养项目（可多选）";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // clbItems
            // 
            this.clbItems.CheckOnClick = true;
            this.clbItems.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.clbItems.FormattingEnabled = true;
            this.clbItems.Location = new System.Drawing.Point(20, 80);
            this.clbItems.Name = "clbItems";
            this.clbItems.Size = new System.Drawing.Size(360, 300);
            this.clbItems.TabIndex = 1;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectAll.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.btnSelectAll.Location = new System.Drawing.Point(20, 395);
            this.btnSelectAll.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(85, 32);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearAll.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.btnClearAll.Location = new System.Drawing.Point(115, 395);
            this.btnClearAll.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(85, 32);
            this.btnClearAll.TabIndex = 3;
            this.btnClearAll.Text = "清空";
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirm.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.btnConfirm.Location = new System.Drawing.Point(210, 395);
            this.btnConfirm.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(80, 32);
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.btnCancel.Location = new System.Drawing.Point(300, 395);
            this.btnCancel.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 32);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmMaintenanceItemSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 450);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnClearAll);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.clbItems);
            this.Controls.Add(this.lblTitle);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMaintenanceItemSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择保养项目";
            this.ResumeLayout(false);
        }

        private Sunny.UI.UILabel lblTitle;
        private System.Windows.Forms.CheckedListBox clbItems;
        private Sunny.UI.UIButton btnSelectAll;
        private Sunny.UI.UIButton btnClearAll;
        private Sunny.UI.UIButton btnConfirm;
        private Sunny.UI.UIButton btnCancel;
    }
}
