using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1.first_menu.RemoteControl
{
    /// <summary>
    /// 网络诊断工具
    /// </summary>
    public class NetworkDiagnostics
    {
        /// <summary>
        /// 诊断连接问题
        /// </summary>
        public static async Task<DiagnosticResult> DiagnoseConnection(string targetIP, int port = 8888)
        {
            var result = new DiagnosticResult();
            var sb = new StringBuilder();
            
            sb.AppendLine("========== 网络诊断开始 ==========");
            sb.AppendLine($"时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine();
            
            // 1. 检查本机IP
            string localIP = NetworkHelper.GetLocalIPAddress();
            sb.AppendLine($"✓ 本机IP: {localIP}");
            result.LocalIP = localIP;
            
            // 2. 检查目标IP格式
            if (!IPAddress.TryParse(targetIP, out IPAddress targetAddress))
            {
                sb.AppendLine($"✗ 目标IP格式错误: {targetIP}");
                result.Issues.Add("目标IP格式不正确");
                result.Success = false;
                result.Report = sb.ToString();
                return result;
            }
            sb.AppendLine($"✓ 目标IP: {targetIP}");
            result.TargetIP = targetIP;
            
            // 3. Ping测试
            sb.AppendLine($"\n[Ping测试]");
            bool pingSuccess = await PingTest(targetIP);
            if (pingSuccess)
            {
                sb.AppendLine($"✓ Ping {targetIP} 成功");
                result.PingSuccess = true;
            }
            else
            {
                sb.AppendLine($"✗ Ping {targetIP} 失败");
                sb.AppendLine("  可能原因:");
                sb.AppendLine("  - 目标主机防火墙阻止了ICMP");
                sb.AppendLine("  - 目标主机不在线");
                sb.AppendLine("  - 网络隔离（如路由器AP隔离）");
                result.Issues.Add("无法Ping通目标主机");
            }
            
            // 4. 端口连接测试
            sb.AppendLine($"\n[端口连接测试]");
            bool portOpen = await TestPortConnection(targetIP, port);
            if (portOpen)
            {
                sb.AppendLine($"✓ 端口 {port} 连接成功");
                result.PortOpen = true;
            }
            else
            {
                sb.AppendLine($"✗ 端口 {port} 连接失败");
                sb.AppendLine("  可能原因:");
                sb.AppendLine("  - 受控端程序未运行或未点击\"开始受控\"");
                sb.AppendLine($"  - 防火墙阻止了端口 {port}");
                sb.AppendLine("  - 端口被其他程序占用");
                result.Issues.Add($"无法连接到端口{port}");
            }
            
            // 5. 防火墙检查
            sb.AppendLine($"\n[防火墙建议]");
            sb.AppendLine("Windows防火墙设置命令（管理员运行）:");
            sb.AppendLine($"netsh advfirewall firewall add rule name=\"RemoteControl\" dir=in action=allow protocol=TCP localport={port}");
            sb.AppendLine($"netsh advfirewall firewall add rule name=\"RemoteControl\" dir=out action=allow protocol=TCP remoteport={port}");
            
            // 6. 网络类型检查
            sb.AppendLine($"\n[网络环境]");
            if (localIP.StartsWith("192.168.") && targetIP.StartsWith("192.168."))
            {
                string localSubnet = localIP.Substring(0, localIP.LastIndexOf('.'));
                string targetSubnet = targetIP.Substring(0, targetIP.LastIndexOf('.'));
                
                if (localSubnet == targetSubnet)
                {
                    sb.AppendLine($"✓ 同一子网: {localSubnet}.x");
                }
                else
                {
                    sb.AppendLine($"⚠ 不同子网: 本机{localSubnet}.x, 目标{targetSubnet}.x");
                    sb.AppendLine("  可能需要检查路由设置");
                    result.Issues.Add("不在同一子网");
                }
            }
            
            // 7. 诊断总结
            sb.AppendLine($"\n========== 诊断总结 ==========");
            if (result.PortOpen)
            {
                sb.AppendLine("✓ 网络连接正常，可以进行远程控制");
                result.Success = true;
            }
            else if (result.PingSuccess)
            {
                sb.AppendLine("⚠ 能Ping通但端口不通，请检查:");
                sb.AppendLine("1. 受控端是否已启动并点击\"开始受控\"");
                sb.AppendLine("2. 防火墙是否允许端口访问");
                result.Success = false;
            }
            else
            {
                sb.AppendLine("✗ 网络不通，请检查:");
                sb.AppendLine("1. 两台电脑是否在同一WiFi/局域网");
                sb.AppendLine("2. 路由器是否开启了AP隔离");
                sb.AppendLine("3. IP地址是否正确");
                result.Success = false;
            }
            
            result.Report = sb.ToString();
            return result;
        }
        
        /// <summary>
        /// Ping测试
        /// </summary>
        private static async Task<bool> PingTest(string targetIP)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(targetIP);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// 测试端口连接
        /// </summary>
        private static async Task<bool> TestPortConnection(string targetIP, int port)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var connectTask = client.ConnectAsync(targetIP, port);
                    var timeoutTask = Task.Delay(3000); // 3秒超时
                    
                    var completedTask = await Task.WhenAny(connectTask, timeoutTask);
                    
                    if (completedTask == connectTask)
                    {
                        await connectTask; // 确保没有异常
                        return client.Connected;
                    }
                    
                    return false; // 超时
                }
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// 显示诊断窗口
        /// </summary>
        public static void ShowDiagnosticWindow(string targetIP)
        {
            var form = new Form
            {
                Text = "网络连接诊断",
                Size = new System.Drawing.Size(700, 500),
                StartPosition = FormStartPosition.CenterScreen
            };
            
            var txtResult = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Consolas", 10F),
                ReadOnly = true
            };
            
            var panel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };
            
            var btnDiagnose = new Button
            {
                Text = "开始诊断",
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(100, 30)
            };
            
            var btnCopy = new Button
            {
                Text = "复制报告",
                Location = new System.Drawing.Point(120, 10),
                Size = new System.Drawing.Size(100, 30)
            };
            
            var btnFirewall = new Button
            {
                Text = "防火墙设置",
                Location = new System.Drawing.Point(230, 10),
                Size = new System.Drawing.Size(100, 30)
            };
            
            btnDiagnose.Click += async (s, e) =>
            {
                btnDiagnose.Enabled = false;
                txtResult.Text = "正在诊断，请稍候...\r\n";
                
                var result = await DiagnoseConnection(targetIP);
                txtResult.Text = result.Report;
                
                btnDiagnose.Enabled = true;
            };
            
            btnCopy.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(txtResult.Text))
                {
                    Clipboard.SetText(txtResult.Text);
                    MessageBox.Show("诊断报告已复制到剪贴板", "提示");
                }
            };
            
            btnFirewall.Click += (s, e) =>
            {
                string cmd = $"netsh advfirewall firewall add rule name=\"RemoteControl\" dir=in action=allow protocol=TCP localport=8888";
                MessageBox.Show($"请以管理员身份运行CMD并执行:\n\n{cmd}", "防火墙设置");
            };
            
            panel.Controls.Add(btnDiagnose);
            panel.Controls.Add(btnCopy);
            panel.Controls.Add(btnFirewall);
            
            form.Controls.Add(txtResult);
            form.Controls.Add(panel);
            
            form.Show();
            
            // 自动开始诊断
            btnDiagnose.PerformClick();
        }
    }
    
    /// <summary>
    /// 诊断结果
    /// </summary>
    public class DiagnosticResult
    {
        public bool Success { get; set; }
        public string LocalIP { get; set; }
        public string TargetIP { get; set; }
        public bool PingSuccess { get; set; }
        public bool PortOpen { get; set; }
        public List<string> Issues { get; set; } = new List<string>();
        public string Report { get; set; }
    }
}
