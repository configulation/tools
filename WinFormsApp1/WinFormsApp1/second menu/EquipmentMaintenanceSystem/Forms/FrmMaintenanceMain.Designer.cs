namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    partial class FrmMaintenanceMain
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
            navMenu = new Sunny.UI.UINavMenu();
            pnlContent = new Sunny.UI.UIPanel();
            lblExecuteTip = new Sunny.UI.UILabel();
            SuspendLayout();
            // 
            // navMenu
            // 
            navMenu.BackColor = Color.FromArgb(80, 160, 255);
            navMenu.BorderStyle = BorderStyle.None;
            navMenu.Dock = DockStyle.Left;
            navMenu.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            navMenu.Font = new Font("微软雅黑", 12F);
            navMenu.FullRowSelect = true;
            navMenu.HotTracking = true;
            navMenu.ItemHeight = 50;
            navMenu.Location = new Point(0, 0);
            navMenu.MenuStyle = Sunny.UI.UIMenuStyle.Custom;
            navMenu.Name = "navMenu";
            navMenu.ShowLines = false;
            navMenu.ShowPlusMinus = false;
            navMenu.ShowRootLines = false;
            navMenu.Size = new Size(200, 600);
            navMenu.TabIndex = 0;
            navMenu.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // pnlContent
            // 
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Font = new Font("微软雅黑", 12F);
            pnlContent.Location = new Point(200, 0);
            pnlContent.Margin = new Padding(4, 5, 4, 5);
            pnlContent.MinimumSize = new Size(1, 1);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1000, 600);
            pnlContent.TabIndex = 1;
            pnlContent.Text = null;
            pnlContent.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lblExecuteTip
            // 
            lblExecuteTip.Dock = DockStyle.Fill;
            lblExecuteTip.Font = new Font("微软雅黑", 12F);
            lblExecuteTip.Location = new Point(0, 0);
            lblExecuteTip.Name = "lblExecuteTip";
            lblExecuteTip.Size = new Size(1000, 600);
            lblExecuteTip.TabIndex = 0;
            lblExecuteTip.Text = "请从保养计划或保养记录中选择需要执行保养的设备或工装";
            lblExecuteTip.TextAlign = ContentAlignment.MiddleCenter;
            lblExecuteTip.Visible = false;
            // 
            // FrmMaintenanceMain
            // 
            AllowShowTitle = false;
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1200, 600);
            Controls.Add(pnlContent);
            Controls.Add(navMenu);
            Name = "FrmMaintenanceMain";
            Padding = new Padding(0);
            ShowTitle = false;
            Text = "设备工装保养管理系统";
            ZoomScaleRect = new Rectangle(15, 15, 1200, 600);
            ResumeLayout(false);
        }

        private Sunny.UI.UINavMenu navMenu;
        private Sunny.UI.UIPanel pnlContent;
        private Sunny.UI.UILabel lblExecuteTip;
    }
}
