namespace RemoteControl.UI
{
    partial class FrmTestMode
    {
        private System.ComponentModel.IContainer components = null;
        private Sunny.UI.UILabel lblTitle;
        private Sunny.UI.UILabel lblTestStatus;
        private Sunny.UI.UIButton btnStartTest;
        private Sunny.UI.UIButton btnFullScreenTest;
        private Sunny.UI.UIButton btnClose;
        private Sunny.UI.UIProcessBar progressBar;
        private Sunny.UI.UIPanel panelMain;
        private Sunny.UI.UITextBox txtTestLog;
        
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
            this.lblTestStatus = new Sunny.UI.UILabel();
            this.btnStartTest = new Sunny.UI.UIButton();
            this.btnFullScreenTest = new Sunny.UI.UIButton();
            this.btnClose = new Sunny.UI.UIButton();
            this.progressBar = new Sunny.UI.UIProcessBar();
            this.panelMain = new Sunny.UI.UIPanel();
            this.txtTestLog = new Sunny.UI.UITextBox();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(30, 50);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "远程控制测试模式";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // 
            // lblTestStatus
            // 
            this.lblTestStatus.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblTestStatus.Location = new System.Drawing.Point(30, 100);
            this.lblTestStatus.Name = "lblTestStatus";
            this.lblTestStatus.Size = new System.Drawing.Size(300, 30);
            this.lblTestStatus.TabIndex = 1;
            this.lblTestStatus.Text = "准备就绪";
            this.lblTestStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // 
            // btnStartTest
            // 
            this.btnStartTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartTest.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnStartTest.Location = new System.Drawing.Point(30, 150);
            this.btnStartTest.Name = "btnStartTest";
            this.btnStartTest.Size = new System.Drawing.Size(100, 35);
            this.btnStartTest.TabIndex = 2;
            this.btnStartTest.Text = "开始测试";
            this.btnStartTest.Click += new System.EventHandler(this.btnStartTest_Click);
            
            // 
            // btnFullScreenTest
            // 
            this.btnFullScreenTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFullScreenTest.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnFullScreenTest.Location = new System.Drawing.Point(150, 150);
            this.btnFullScreenTest.Name = "btnFullScreenTest";
            this.btnFullScreenTest.Size = new System.Drawing.Size(100, 35);
            this.btnFullScreenTest.TabIndex = 3;
            this.btnFullScreenTest.Text = "全屏测试";
            this.btnFullScreenTest.Click += new System.EventHandler(this.btnFullScreenTest_Click);
            
            // 
            // btnClose
            // 
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnClose.Location = new System.Drawing.Point(270, 150);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            
            // 
            // progressBar
            // 
            this.progressBar.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.progressBar.Location = new System.Drawing.Point(30, 200);
            this.progressBar.Maximum = 100;
            this.progressBar.MinimumSize = new System.Drawing.Size(70, 23);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(540, 30);
            this.progressBar.TabIndex = 5;
            this.progressBar.Text = "0%";
            this.progressBar.Value = 0;
            
            // 
            // txtTestLog
            // 
            this.txtTestLog.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTestLog.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtTestLog.Location = new System.Drawing.Point(30, 250);
            this.txtTestLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTestLog.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtTestLog.Multiline = true;
            this.txtTestLog.Name = "txtTestLog";
            this.txtTestLog.Padding = new System.Windows.Forms.Padding(5);
            this.txtTestLog.ReadOnly = true;
            this.txtTestLog.ShowText = false;
            this.txtTestLog.Size = new System.Drawing.Size(540, 200);
            this.txtTestLog.TabIndex = 6;
            this.txtTestLog.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.txtTestLog.Watermark = "";
            this.txtTestLog.ShowButton = false;
            
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.lblTitle);
            this.panelMain.Controls.Add(this.lblTestStatus);
            this.panelMain.Controls.Add(this.btnStartTest);
            this.panelMain.Controls.Add(this.btnFullScreenTest);
            this.panelMain.Controls.Add(this.btnClose);
            this.panelMain.Controls.Add(this.progressBar);
            this.panelMain.Controls.Add(this.txtTestLog);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.panelMain.Location = new System.Drawing.Point(0, 35);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(600, 465);
            this.panelMain.TabIndex = 0;
            this.panelMain.Text = null;
            
            // 
            // FrmTestMode
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(600, 500);
            this.Controls.Add(this.panelMain);
            this.KeyPreview = true;
            this.Name = "FrmTestMode";
            this.ShowTitle = true;
            this.Text = "测试模式";
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
