using System;
using System.Drawing;
using System.Windows.Forms;
using Sunny.UI;

namespace RemoteControl.UI
{
    public partial class FrmTestMode : UIForm
    {
        private Timer testTimer;
        private int testCounter = 0;
        
        public FrmTestMode()
        {
            InitializeComponent();
            InitializeTestMode();
        }
        
        private void InitializeTestMode()
        {
            // 设置窗口属性
            this.Text = "测试模式";
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowTitle = true;
            
            // 初始化测试定时器
            testTimer = new Timer();
            testTimer.Interval = 1000;
            testTimer.Tick += TestTimer_Tick;
            
            // 初始化日志文本框
            if (txtTestLog != null)
            {
                txtTestLog.Text = "测试日志:\r\n";
            }
        }
        
        private void TestTimer_Tick(object sender, EventArgs e)
        {
            testCounter++;
            lblTestStatus.Text = $"测试运行中: {testCounter} 秒";
            
            // 更新进度条
            if (progressBar.Value < progressBar.Maximum)
            {
                progressBar.Value++;
                progressBar.Text = $"{progressBar.Value}%";
            }
            else
            {
                progressBar.Value = 0;
                progressBar.Text = "0%";
            }
        }
        
        private void btnStartTest_Click(object sender, EventArgs e)
        {
            if (testTimer.Enabled)
            {
                // 停止测试
                testTimer.Stop();
                btnStartTest.Text = "开始测试";
                lblTestStatus.Text = "测试已停止";
                testCounter = 0;
                progressBar.Value = 0;
                progressBar.Text = "0%";
                AddLog("测试已停止");
            }
            else
            {
                // 开始测试
                testTimer.Start();
                btnStartTest.Text = "停止测试";
                lblTestStatus.Text = "测试运行中...";
                AddLog("测试开始");
            }
        }
        
        private void AddLog(string message)
        {
            if (txtTestLog != null)
            {
                string logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}\r\n";
                txtTestLog.Text += logEntry;
                // UITextBox会自动滚动到最新内容
            }
        }
        
        private void btnFullScreenTest_Click(object sender, EventArgs e)
        {
            // 测试全屏功能
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                btnFullScreenTest.Text = "全屏测试";
                AddLog("退出全屏模式");
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = FormBorderStyle.None;
                btnFullScreenTest.Text = "退出全屏";
                AddLog("进入全屏模式");
            }
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (testTimer.Enabled)
            {
                testTimer.Stop();
            }
            this.Close();
        }
        
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (testTimer != null)
            {
                testTimer.Stop();
                testTimer.Dispose();
            }
            base.OnFormClosed(e);
        }
        
        // ESC键退出全屏
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                btnFullScreenTest.Text = "全屏测试";
            }
            base.OnKeyDown(e);
        }
    }
}
