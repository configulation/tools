using System;
using System.Windows.Forms;

namespace WinFormsApp1.first_menu.RemoteControl
{
    /// <summary>
    /// 快速测试局域网连接
    /// </summary>
    public partial class QuickTest : Form
    {
        public QuickTest()
        {
            InitializeComponent();
            TestNetworkInfo();
        }
        
        private void InitializeComponent()
        {
            this.Text = "网络信息测试";
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            
            var txtInfo = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Consolas", 10F)
            };
            
            this.Controls.Add(txtInfo);
            this.txtInfo = txtInfo;
        }
        
        private TextBox txtInfo;
        
        private void TestNetworkInfo()
        {
            txtInfo.AppendText("========== 网络信息测试 ==========\r\n\r\n");
            
            // 测试IP获取
            string localIP = NetworkHelper.GetLocalIPAddress();
            txtInfo.AppendText($"✅ 本机真实IP: {localIP}\r\n\r\n");
            
            // 显示所有IP
            txtInfo.AppendText("📋 所有网络接口:\r\n");
            var allIPs = NetworkHelper.GetAllLocalIPAddresses();
            foreach (var ip in allIPs)
            {
                if (ip == localIP)
                    txtInfo.AppendText($"  • {ip} ← 主要IP\r\n");
                else if (ip.StartsWith("192.168.127") || ip.StartsWith("192.168.195"))
                    txtInfo.AppendText($"  • {ip} (VMware虚拟网卡)\r\n");
                else
                    txtInfo.AppendText($"  • {ip}\r\n");
            }
            
            txtInfo.AppendText($"\r\n========== 使用说明 ==========\r\n\r\n");
            
            // 生成设备码
            Random random = new Random();
            string deviceCode = random.Next(100000, 999999).ToString();
            
            txtInfo.AppendText($"📱 这台电脑作为【受控端】:\r\n");
            txtInfo.AppendText($"   1. 点击\"开始受控\"\r\n");
            txtInfo.AppendText($"   2. 告诉对方以下信息:\r\n");
            txtInfo.AppendText($"      设备码: {deviceCode}\r\n");
            txtInfo.AppendText($"      IP地址: {localIP}\r\n\r\n");
            
            txtInfo.AppendText($"💻 这台电脑作为【控制端】:\r\n");
            txtInfo.AppendText($"   1. 获取对方的设备码和IP\r\n");
            txtInfo.AppendText($"   2. 在\"远程设备码\"输入:\r\n");
            txtInfo.AppendText($"      格式: 设备码#IP地址\r\n");
            txtInfo.AppendText($"      例如: {deviceCode}#{localIP}\r\n");
            txtInfo.AppendText($"   3. 点击\"连接\"\r\n\r\n");
            
            txtInfo.AppendText($"⚠️ 注意事项:\r\n");
            txtInfo.AppendText($"   • 两台电脑必须在同一WiFi\r\n");
            txtInfo.AppendText($"   • 防火墙需要开放端口8888\r\n");
            txtInfo.AppendText($"   • 确保能相互ping通\r\n\r\n");
            
            // 测试端口
            bool portOpen = NetworkHelper.IsPortOpen(8888);
            if (portOpen)
            {
                txtInfo.AppendText($"✅ 端口8888可用\r\n");
            }
            else
            {
                txtInfo.AppendText($"❌ 端口8888被占用或无法访问\r\n");
                txtInfo.AppendText($"   请检查防火墙设置或更换端口\r\n");
            }
        }
    }
    
    /// <summary>
    /// 在主窗体中添加测试按钮
    /// </summary>
    public static class NetworkTestHelper
    {
        public static void ShowNetworkInfo()
        {
            var testForm = new QuickTest();
            testForm.ShowDialog();
        }
    }
}
