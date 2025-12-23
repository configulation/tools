using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace WinFormsApp1.first_menu.RemoteControl
{
    /// <summary>
    /// 网络辅助类 - 处理局域网连接
    /// </summary>
    public static class NetworkHelper
    {
        /// <summary>
        /// 获取本机局域网IP地址
        /// </summary>
        public static string GetLocalIPAddress()
        {
            try
            {
                // 获取所有网络接口
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(ni => ni.OperationalStatus == OperationalStatus.Up &&
                                ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .OrderBy(ni => ni.Name); // 按名称排序，优先选择真实网卡
                
                // 优先查找无线网卡（WLAN）和以太网
                foreach (var networkInterface in networkInterfaces)
                {
                    string name = networkInterface.Name.ToLower();
                    string description = networkInterface.Description.ToLower();
                    
                    // 跳过虚拟网卡
                    if (name.Contains("vmware") || 
                        name.Contains("virtualbox") || 
                        name.Contains("hyper-v") ||
                        description.Contains("vmware") ||
                        description.Contains("virtualbox") ||
                        description.Contains("hyper-v") ||
                        description.Contains("virtual"))
                    {
                        continue;
                    }
                    
                    var ipProperties = networkInterface.GetIPProperties();
                    
                    // 检查是否有网关（真实网卡通常有网关）
                    if (ipProperties.GatewayAddresses.Count > 0)
                    {
                        var unicastAddresses = ipProperties.UnicastAddresses;
                        
                        foreach (var address in unicastAddresses)
                        {
                            // 获取IPv4地址
                            if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                string ipStr = address.Address.ToString();
                                
                                // 过滤掉特殊地址
                                if (!ipStr.StartsWith("169.254") &&  // 自动专用地址
                                    !ipStr.StartsWith("0.") &&
                                    !ipStr.StartsWith("255.") &&
                                    !ipStr.EndsWith(".1"))  // 排除网关地址
                                {
                                    // 优先返回常见的局域网地址
                                    if (ipStr.StartsWith("192.168.1.") ||  // 家庭网络最常见
                                        ipStr.StartsWith("192.168.0.") ||
                                        ipStr.StartsWith("10.") || 
                                        ipStr.StartsWith("172."))
                                    {
                                        Console.WriteLine($"[NetworkHelper] 选择网卡: {networkInterface.Name} ({networkInterface.Description})");
                                        Console.WriteLine($"[NetworkHelper] 本机IP: {ipStr}");
                                        return ipStr;
                                    }
                                }
                            }
                        }
                    }
                }
                
                // 如果没找到合适的，再尝试所有非虚拟网卡
                foreach (var networkInterface in networkInterfaces)
                {
                    string name = networkInterface.Name.ToLower();
                    string description = networkInterface.Description.ToLower();
                    
                    // 跳过虚拟网卡
                    if (name.Contains("vmware") || name.Contains("virtualbox") || 
                        description.Contains("vmware") || description.Contains("virtual"))
                    {
                        continue;
                    }
                    
                    var ipProperties = networkInterface.GetIPProperties();
                    var unicastAddresses = ipProperties.UnicastAddresses;
                    
                    foreach (var address in unicastAddresses)
                    {
                        if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            string ipStr = address.Address.ToString();
                            if (!ipStr.StartsWith("169.254") && !ipStr.StartsWith("127."))
                            {
                                return ipStr;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取本地IP失败: {ex.Message}");
            }
            
            return "127.0.0.1";
        }
        
        /// <summary>
        /// 获取所有本机IP地址列表
        /// </summary>
        public static List<string> GetAllLocalIPAddresses()
        {
            var ipList = new List<string>();
            
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        string ipStr = ip.ToString();
                        if (!ipStr.StartsWith("169.254") && !ipStr.StartsWith("0."))
                        {
                            ipList.Add(ipStr);
                        }
                    }
                }
                
                // 添加本地回环地址
                if (!ipList.Contains("127.0.0.1"))
                {
                    ipList.Add("127.0.0.1");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取IP列表失败: {ex.Message}");
                ipList.Add("127.0.0.1");
            }
            
            return ipList;
        }
        
        /// <summary>
        /// 扫描局域网中的可用主机
        /// </summary>
        public static async System.Threading.Tasks.Task<List<string>> ScanLocalNetwork(int port = 8888)
        {
            var availableHosts = new List<string>();
            string localIP = GetLocalIPAddress();
            
            if (localIP == "127.0.0.1")
                return availableHosts;
            
            // 获取网段
            string[] parts = localIP.Split('.');
            if (parts.Length != 4)
                return availableHosts;
            
            string subnet = $"{parts[0]}.{parts[1]}.{parts[2]}";
            
            var tasks = new List<System.Threading.Tasks.Task>();
            
            // 扫描局域网（1-254）
            for (int i = 1; i <= 254; i++)
            {
                string targetIP = $"{subnet}.{i}";
                
                var task = System.Threading.Tasks.Task.Run(async () =>
                {
                    try
                    {
                        using (var client = new TcpClient())
                        {
                            var connectTask = client.ConnectAsync(targetIP, port);
                            if (await System.Threading.Tasks.Task.WhenAny(connectTask, 
                                System.Threading.Tasks.Task.Delay(100)) == connectTask)
                            {
                                if (client.Connected)
                                {
                                    lock (availableHosts)
                                    {
                                        availableHosts.Add(targetIP);
                                    }
                                    client.Close();
                                }
                            }
                        }
                    }
                    catch
                    {
                        // 忽略连接失败
                    }
                });
                
                tasks.Add(task);
            }
            
            await System.Threading.Tasks.Task.WhenAll(tasks);
            
            return availableHosts;
        }
        
        /// <summary>
        /// 检查端口是否开放
        /// </summary>
        public static bool IsPortOpen(int port)
        {
            try
            {
                var listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                listener.Stop();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// 生成包含IP的设备码
        /// </summary>
        public static string GenerateDeviceCodeWithIP()
        {
            string localIP = GetLocalIPAddress();
            Random random = new Random();
            string code = random.Next(100000, 999999).ToString();
            
            // 返回格式：设备码#IP地址
            return $"{code}#{localIP}";
        }
        
        /// <summary>
        /// 解析设备码获取IP
        /// </summary>
        public static (string code, string ip) ParseDeviceCode(string fullCode)
        {
            if (string.IsNullOrEmpty(fullCode))
                return ("", "");
            
            var parts = fullCode.Split('#');
            if (parts.Length == 2)
            {
                return (parts[0], parts[1]);
            }
            
            // 兼容旧格式（只有6位数字）
            if (fullCode.Length == 6 && int.TryParse(fullCode, out _))
            {
                return (fullCode, "127.0.0.1");
            }
            
            return (fullCode, "");
        }
    }
}
