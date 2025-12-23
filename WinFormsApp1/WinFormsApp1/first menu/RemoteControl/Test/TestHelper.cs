using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace WinFormsApp1.first_menu.RemoteControl
{
    /// <summary>
    /// 测试辅助类 - 用于单机调试
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// 启动两个程序实例进行测试
        /// </summary>
        public static void StartDualInstance()
        {
            try
            {
                // 获取当前程序路径
                string appPath = Application.ExecutablePath;
                
                // 启动第二个实例
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = appPath,
                    Arguments = "--second-instance", // 传递参数以区分
                    UseShellExecute = false
                };
                
                Process.Start(startInfo);
                
                MessageBox.Show("已启动第二个实例，您可以：\n" +
                    "1. 在第一个实例中点击'开始受控'\n" +
                    "2. 在第二个实例中输入设备码并连接\n" +
                    "3. 测试远程控制功能",
                    "测试模式",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动第二个实例失败：{ex.Message}",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 创建模拟测试环境
        /// </summary>
        public static void CreateMockEnvironment()
        {
            // 创建两个窗口，模拟两台电脑
            var hostForm = new FrmRemoteControl();
            var clientForm = new FrmRemoteControl();
            
            // 设置窗口标题以区分
            hostForm.Text = "远程控制 - 受控端（主机）";
            clientForm.Text = "远程控制 - 控制端（客户端）";
            
            // 调整窗口位置
            hostForm.StartPosition = FormStartPosition.Manual;
            hostForm.Location = new System.Drawing.Point(0, 0);
            hostForm.Size = new System.Drawing.Size(800, 600);
            
            clientForm.StartPosition = FormStartPosition.Manual;
            clientForm.Location = new System.Drawing.Point(810, 0);
            clientForm.Size = new System.Drawing.Size(800, 600);
            
            // 显示两个窗口
            hostForm.Show();
            clientForm.Show();
        }
    }
    
    /// <summary>
    /// 单机测试窗体
    /// </summary>
    public partial class FrmTestMode : Form
    {
        private FrmRemoteControl hostWindow;
        private FrmRemoteControl clientWindow;
        
        public FrmTestMode()
        {
            InitializeComponent();
        }
        
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // 关闭所有打开的测试窗口
            hostWindow?.Close();
            clientWindow?.Close();
            base.OnFormClosed(e);
        }
        
        private void InitializeComponent()
        {
            this.Text = "远程控制 - 单机测试模式";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // 添加按钮
            var btnDualWindow = new Button
            {
                Text = "双窗口测试",
                Location = new System.Drawing.Point(50, 50),
                Size = new System.Drawing.Size(300, 40)
            };
            btnDualWindow.Click += BtnDualWindow_Click;
            
            var btnDualProcess = new Button
            {
                Text = "双进程测试",
                Location = new System.Drawing.Point(50, 100),
                Size = new System.Drawing.Size(300, 40)
            };
            btnDualProcess.Click += BtnDualProcess_Click;
            
            var btnLocalTest = new Button
            {
                Text = "本地回环测试",
                Location = new System.Drawing.Point(50, 150),
                Size = new System.Drawing.Size(300, 40)
            };
            btnLocalTest.Click += BtnLocalTest_Click;
            
            var lblInfo = new Label
            {
                Text = "选择测试模式：\n" +
                       "• 双窗口：在同一进程中开两个窗口\n" +
                       "• 双进程：启动两个独立的程序实例\n" +
                       "• 本地回环：使用127.0.0.1测试",
                Location = new System.Drawing.Point(50, 200),
                Size = new System.Drawing.Size(300, 80),
                AutoSize = false
            };
            
            this.Controls.Add(btnDualWindow);
            this.Controls.Add(btnDualProcess);
            this.Controls.Add(btnLocalTest);
            this.Controls.Add(lblInfo);
        }
        
        private void BtnDualWindow_Click(object sender, EventArgs e)
        {
            // 创建主机窗口
            hostWindow = new FrmRemoteControl();
            hostWindow.Text = "远程控制 - 受控端";
            hostWindow.Location = new System.Drawing.Point(10, 10);
            hostWindow.Size = new System.Drawing.Size(700, 500);
            hostWindow.Show();
            
            // 创建客户端窗口
            clientWindow = new FrmRemoteControl();
            clientWindow.Text = "远程控制 - 控制端";
            clientWindow.Location = new System.Drawing.Point(720, 10);
            clientWindow.Size = new System.Drawing.Size(700, 500);
            clientWindow.Show();
            
            MessageBox.Show("双窗口已打开！\n" +
                "1. 在左边窗口点击'开始受控'获取设备码\n" +
                "2. 在右边窗口输入设备码并点击'连接'",
                "测试提示");
        }
        
        private void BtnDualProcess_Click(object sender, EventArgs e)
        {
            TestHelper.StartDualInstance();
        }
        
        private void BtnLocalTest_Click(object sender, EventArgs e)
        {
            var testForm = new FrmRemoteControl();
            testForm.Text = "远程控制 - 本地测试模式";
            testForm.Show();
            
            MessageBox.Show("本地测试模式：\n" +
                "1. 先点击'开始受控'作为服务器\n" +
                "2. 再在同一窗口输入设备码并'连接'\n" +
                "3. 这将连接到127.0.0.1（本机）",
                "测试提示");
        }
    }
}
