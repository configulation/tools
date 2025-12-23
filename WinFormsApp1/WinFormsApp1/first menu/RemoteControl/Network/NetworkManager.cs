using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WinFormsApp1.first_menu.RemoteControl
{
    /// <summary>
    /// 网络通信管理器
    /// </summary>
    public class NetworkManager : IDisposable
    {
        private TcpListener tcpListener;
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        private Thread listenerThread;
        private Thread receiveThread;
        private bool isRunning = false;
        private string deviceCode;
        private int port;

        private bool relayEnabled;
        private string relayHost;
        private int relayPort;
        private bool waitingRelayPair;

        private readonly SemaphoreSlim sendSemaphore = new SemaphoreSlim(1, 1);
        
        // 事件
        public event Action<bool, string> OnConnectionStatusChanged;
        public event Action<byte[]> OnScreenDataReceived;
        public event Action<Point, MouseButtons, bool> OnMouseEventReceived;
        public event Action<Keys, bool> OnKeyEventReceived;
        public event Action<Size> OnScreenInfoReceived;  // 接收到屏幕信息
        public event Action<int> OnLatencyUpdated;

        private System.Threading.Timer heartbeatTimer;
        private bool isServerMode;
        
        // 消息类型枚举
        private enum MessageType : byte
        {
            ScreenData = 1,
            MouseMove = 2,
            MouseClick = 3,
            KeyPress = 4,
            Disconnect = 5,
            ScreenInfo = 6,      // 屏幕信息（分辨率等）
            Heartbeat = 7,
            ConnectionInfo = 8   // 连接信息（设备码等）
        }

        private const string RelayPairedFlag = "PAIRED";

        private long sentScreenFrames;
        private long receivedScreenFrames;
        private DateTime lastScreenSendAt = DateTime.MinValue;
        private DateTime lastScreenRecvAt = DateTime.MinValue;

        public NetworkManager()
        {
            // 从配置文件获取端口
            port = ConfigManager.Instance.NetworkPort;

            relayEnabled = ConfigManager.Instance.RelayEnabled;
            var relay = ConfigManager.Instance.RelayServer;
            ParseEndpoint(relay, out relayHost, out relayPort);
            if (string.IsNullOrWhiteSpace(relayHost) || relayPort <= 0)
            {
                relayEnabled = false;
                relayHost = string.Empty;
                relayPort = 0;
            }
        }

        public void SetDeviceCode(string code)
        {
            deviceCode = code;
        }

        /// <summary>
        /// 启动主机模式（受控端）
        /// </summary>
        public async Task StartHostAsync(string hostDeviceCode)
        {
            await Task.Run(() =>
            {
                isServerMode = true;

                if (relayEnabled)
                {
                    try
                    {
                        deviceCode = hostDeviceCode;
                        tcpClient = new TcpClient();
                        try
                        {
                            tcpClient.NoDelay = true;
                            tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                        }
                        catch
                        {
                        }
                        tcpClient.Connect(relayHost, relayPort);
                        networkStream = tcpClient.GetStream();
                        isRunning = true;
                        waitingRelayPair = true;

                        SendMessage(MessageType.ConnectionInfo, Encoding.UTF8.GetBytes($"H|{deviceCode}"));

                        receiveThread = new Thread(ReceiveData)
                        {
                            IsBackground = true
                        };
                        receiveThread.Start();

                        OnConnectionStatusChanged?.Invoke(false, "已连接中继，等待控制端...");
                        return;
                    }
                    catch (Exception ex)
                    {
                        relayEnabled = false;
                        relayHost = string.Empty;
                        relayPort = 0;
                        OnConnectionStatusChanged?.Invoke(false, $"连接中继失败: {ex.Message}，已切换为直连监听模式");
                    }
                }

                // 监听所有网络接口（0.0.0.0），这样局域网内的其他电脑可以连接
                tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();
                isRunning = true;
                
                // 显示本机IP信息
                string localIP = NetworkHelper.GetLocalIPAddress();
                Console.WriteLine($"[监听] 本机IP: {localIP}, 端口: {port}");
                
                // 启动监听线程
                listenerThread = new Thread(ListenForClients)
                {
                    IsBackground = true
                };
                listenerThread.Start();
                
                OnConnectionStatusChanged?.Invoke(false, "等待连接...");
            });
        }

        /// <summary>
        /// 连接到主机
        /// </summary>
        public async Task ConnectToHostAsync(string remoteCode)
        {
            await Task.Run(() =>
            {
                try
                {
                    isServerMode = false;
                    tcpClient = new TcpClient();
                    try
                    {
                        tcpClient.NoDelay = true;
                        tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                    }
                    catch
                    {
                    }
                    
                    // 解析设备码，支持格式：纯6位数字 或 设备码#IP地址
                    var (code, targetIP) = NetworkHelper.ParseDeviceCode(remoteCode);

                    bool isDirect = remoteCode.Contains("#");

                    if (!isDirect && !relayEnabled)
                    {
                        OnConnectionStatusChanged?.Invoke(false, "中继未启用或中继地址为空：请输入 设备码#IP 走局域网直连，或在配置中启用 UseRelay 并设置 RelayServer");
                        return;
                    }

                    if (!isDirect && relayEnabled)
                    {
                        waitingRelayPair = true;
                        Console.WriteLine($"[中继] 服务器: {relayHost}:{relayPort}, 设备码: {code}");
                        tcpClient.Connect(relayHost, relayPort);
                        networkStream = tcpClient.GetStream();
                        isRunning = true;

                        SendMessage(MessageType.ConnectionInfo, Encoding.UTF8.GetBytes($"C|{code}"));

                        receiveThread = new Thread(ReceiveData)
                        {
                            IsBackground = true
                        };
                        receiveThread.Start();

                        OnConnectionStatusChanged?.Invoke(false, "已连接中继，等待受控端...");
                        return;
                    }
                    
                    // 如果没有指定IP，使用默认
                    if (string.IsNullOrEmpty(targetIP))
                    {
                        targetIP = "127.0.0.1"; // 默认本地
                    }
                    
                    Console.WriteLine($"[连接] 目标IP: {targetIP}, 端口: {port}, 设备码: {code}");
                    
                    // 连接到目标IP
                    tcpClient.Connect(targetIP, port);
                    
                    networkStream = tcpClient.GetStream();
                    isRunning = true;
                    
                    // 发送连接信息（使用原始设备码）
                    SendMessage(MessageType.ConnectionInfo, Encoding.UTF8.GetBytes(code));
                    
                    // 发送屏幕信息给控制端
                    Thread.Sleep(100); // 等待连接稳定
                    SendScreenInfo();
                    
                    // 启动接收线程
                    receiveThread = new Thread(ReceiveData)
                    {
                        IsBackground = true
                    };
                    receiveThread.Start();

                    StartHeartbeat();
                    
                    OnConnectionStatusChanged?.Invoke(true, "连接成功");
                }
                catch (Exception ex)
                {
                    OnConnectionStatusChanged?.Invoke(false, $"连接失败: {ex.Message}");
                }
            });
        }

        private void StartHeartbeat()
        {
            StopHeartbeat();

            heartbeatTimer = new System.Threading.Timer(_ =>
            {
                try
                {
                    if (!isRunning || isServerMode)
                    {
                        return;
                    }

                    long ticks = DateTime.UtcNow.Ticks;
                    byte[] payload = BitConverter.GetBytes(ticks);
                    _ = SendMessageAsync(MessageType.Heartbeat, payload);
                }
                catch
                {
                    // ignore
                }
            }, null, 1000, 1500);
        }

        private void StopHeartbeat()
        {
            try
            {
                heartbeatTimer?.Dispose();
            }
            catch
            {
                // ignore
            }
            finally
            {
                heartbeatTimer = null;
            }
        }

        /// <summary>
        /// 监听客户端连接
        /// </summary>
        private void ListenForClients()
        {
            while (isRunning)
            {
                try
                {
                    TcpClient client = tcpListener.AcceptTcpClient();

                    try
                    {
                        client.NoDelay = true;
                        client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                    }
                    catch
                    {
                    }
                    
                    // 处理客户端连接
                    tcpClient = client;
                    networkStream = client.GetStream();
                    
                    // 发送屏幕信息给控制端
                    Thread.Sleep(100); // 等待连接稳定
                    SendScreenInfo();
                    
                    // 开始接收数据
                    receiveThread = new Thread(ReceiveData)
                    {
                        IsBackground = true
                    };
                    receiveThread.Start();
                    
                    OnConnectionStatusChanged?.Invoke(true, "客户端已连接");
                }
                catch (Exception ex)
                {
                    if (isRunning)
                    {
                        Console.WriteLine($"监听错误: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        private void ReceiveData()
        {
            while (isRunning && networkStream != null)
            {
                try
                {
                    // 读取消息头（1字节类型 + 4字节长度）
                    byte[] header = new byte[5];
                    if (!ReadExact(networkStream, header, 0, header.Length))
                    {
                        break;
                    }
                    
                    MessageType messageType = (MessageType)header[0];
                    int dataLength = BitConverter.ToInt32(header, 1);

                    if (dataLength < 0 || dataLength > 50 * 1024 * 1024)
                    {
                        Console.WriteLine($"接收错误: 非法数据长度 {dataLength}");
                        break;
                    }
                    
                    // 读取数据
                    byte[] data = new byte[dataLength];
                    if (!ReadExact(networkStream, data, 0, dataLength))
                    {
                        break;
                    }
                    
                    // 处理消息
                    ProcessMessage(messageType, data);
                }
                catch (Exception ex)
                {
                    if (isRunning)
                    {
                        Console.WriteLine($"接收错误: {ex.Message}");
                        break;
                    }
                }
            }

            try
            {
                if (!isServerMode)
                {
                    isRunning = false;
                    StopHeartbeat();
                }

                try
                {
                    networkStream?.Close();
                    tcpClient?.Close();
                }
                catch
                {
                    // ignore
                }

                networkStream = null;
                tcpClient = null;
            }
            finally
            {
                OnConnectionStatusChanged?.Invoke(false, "连接断开");
            }
        }

        private static bool ReadExact(NetworkStream stream, byte[] buffer, int offset, int count)
        {
            int totalRead = 0;
            while (totalRead < count)
            {
                int read = stream.Read(buffer, offset + totalRead, count - totalRead);
                if (read <= 0)
                {
                    return false;
                }
                totalRead += read;
            }
            return true;
        }

        /// <summary>
        /// 处理接收到的消息
        /// </summary>
        private void ProcessMessage(MessageType type, byte[] data)
        {
            switch (type)
            {
                case MessageType.ScreenData:
                    receivedScreenFrames++;
                    lastScreenRecvAt = DateTime.Now;
                    if (receivedScreenFrames == 1 || receivedScreenFrames % 100 == 0)
                    {
                        Console.WriteLine($"[ScreenData] recv frames={receivedScreenFrames} bytes={(data?.Length ?? 0)} at={lastScreenRecvAt:HH:mm:ss.fff}");
                    }
                    OnScreenDataReceived?.Invoke(data);
                    break;
                    
                case MessageType.MouseMove:
                    {
                        string json = Encoding.UTF8.GetString(data);
                        dynamic mouseData = JsonConvert.DeserializeObject(json);
                        
                        // 获取坐标（这些坐标是基于远程屏幕分辨率的）
                        int remoteX = (int)mouseData.X;
                        int remoteY = (int)mouseData.Y;
                        
                        // 不需要缩放，直接使用原始坐标
                        // 因为发送方已经发送的是正确的目标屏幕坐标
                        Point point = new Point(remoteX, remoteY);
                        RemoteControlManager.MoveMouse(point);
                    }
                    break;
                    
                case MessageType.MouseClick:
                    {
                        string json = Encoding.UTF8.GetString(data);
                        dynamic mouseData = JsonConvert.DeserializeObject(json);
                        
                        // 获取坐标
                        int remoteX = (int)mouseData.X;
                        int remoteY = (int)mouseData.Y;
                        Point point = new Point(remoteX, remoteY);
                        
                        MouseButtons button = (MouseButtons)Enum.Parse(typeof(MouseButtons), mouseData.Button.ToString());
                        bool isDown = (bool)mouseData.IsDown;
                        
                        if (isDown)
                            RemoteControlManager.MouseDown(point, button);
                        else
                            RemoteControlManager.MouseUp(point, button);
                    }
                    break;
                    
                case MessageType.KeyPress:
                    {
                        string json = Encoding.UTF8.GetString(data);
                        dynamic keyData = JsonConvert.DeserializeObject(json);
                        Keys key = (Keys)Enum.Parse(typeof(Keys), keyData.Key.ToString());
                        bool isDown = (bool)keyData.IsDown;
                        
                        if (isDown)
                            RemoteControlManager.KeyDown(key);
                        else
                            RemoteControlManager.KeyUp(key);
                    }
                    break;
                    
                case MessageType.ScreenInfo:
                    {
                        string json = Encoding.UTF8.GetString(data);
                        dynamic screenInfo = JsonConvert.DeserializeObject(json);
                        Size screenSize = new Size((int)screenInfo.Width, (int)screenInfo.Height);
                        OnScreenInfoReceived?.Invoke(screenSize);
                    }
                    break;
                    
                case MessageType.ConnectionInfo:
                    {
                        // 接收到连接信息（设备码）
                        string info = Encoding.UTF8.GetString(data);
                        Console.WriteLine($"接收到连接信息: {info}");

                        if (waitingRelayPair && string.Equals(info, RelayPairedFlag, StringComparison.OrdinalIgnoreCase))
                        {
                            waitingRelayPair = false;

                            Console.WriteLine($"[Relay] paired ok role={(isServerMode ? "Host" : "Controller")} at={DateTime.Now:HH:mm:ss.fff}");

                            try
                            {
                                SendScreenInfo();
                            }
                            catch
                            {
                                // ignore
                            }

                            if (!isServerMode)
                            {
                                StartHeartbeat();
                            }

                            OnConnectionStatusChanged?.Invoke(true, "中继配对成功");
                        }
                    }
                    break;
                    
                case MessageType.Heartbeat:
                    if (data != null && data.Length == 8)
                    {
                        long ticks = BitConverter.ToInt64(data, 0);
                        if (isServerMode)
                        {
                            // 受控端：原样回包
                            SendMessage(MessageType.Heartbeat, data);
                        }
                        else
                        {
                            // 控制端：计算往返时延
                            long nowTicks = DateTime.UtcNow.Ticks;
                            int ms = (int)Math.Max(0, (nowTicks - ticks) / TimeSpan.TicksPerMillisecond);
                            OnLatencyUpdated?.Invoke(ms);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 发送屏幕数据
        /// </summary>
        public async Task SendScreenDataAsync(byte[] screenData)
        {
            if (screenData != null && networkStream != null && networkStream.CanWrite)
            {
                sentScreenFrames++;
                lastScreenSendAt = DateTime.Now;
                if (sentScreenFrames == 1 || sentScreenFrames % 100 == 0)
                {
                    Console.WriteLine($"[ScreenData] send frames={sentScreenFrames} bytes={screenData.Length} at={lastScreenSendAt:HH:mm:ss.fff}");
                }
                await SendMessageAsync(MessageType.ScreenData, screenData);
            }
        }

        private async Task SendMessageAsync(MessageType type, byte[] data)
        {
            if (networkStream == null || !networkStream.CanWrite)
            {
                return;
            }

            await sendSemaphore.WaitAsync();
            try
            {
                byte[] header = new byte[5];
                header[0] = (byte)type;
                byte[] lengthBytes = BitConverter.GetBytes(data.Length);
                Array.Copy(lengthBytes, 0, header, 1, 4);

                await networkStream.WriteAsync(header, 0, header.Length);
                await networkStream.WriteAsync(data, 0, data.Length);
                await networkStream.FlushAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送失败: {ex.Message}");
            }
            finally
            {
                sendSemaphore.Release();
            }
        }
        
        /// <summary>
        /// 发送屏幕信息（分辨率）
        /// </summary>
        public void SendScreenInfo()
        {
            var screenBounds = Screen.PrimaryScreen.Bounds;
            var screenInfo = new { Width = screenBounds.Width, Height = screenBounds.Height };
            string json = JsonConvert.SerializeObject(screenInfo);
            SendMessage(MessageType.ScreenInfo, Encoding.UTF8.GetBytes(json));
        }

        /// <summary>
        /// 发送鼠标移动事件
        /// </summary>
        public void SendMouseMove(Point position)
        {
            var data = new { X = position.X, Y = position.Y };
            string json = JsonConvert.SerializeObject(data);
            SendMessage(MessageType.MouseMove, Encoding.UTF8.GetBytes(json));
        }

        /// <summary>
        /// 发送鼠标点击事件
        /// </summary>
        public void SendMouseEvent(Point position, MouseButtons button, bool isDown)
        {
            var data = new { X = position.X, Y = position.Y, Button = button.ToString(), IsDown = isDown };
            string json = JsonConvert.SerializeObject(data);
            SendMessage(MessageType.MouseClick, Encoding.UTF8.GetBytes(json));
        }

        /// <summary>
        /// 发送键盘事件
        /// </summary>
        public void SendKeyEvent(Keys key, bool isDown)
        {
            var data = new { Key = key.ToString(), IsDown = isDown };
            string json = JsonConvert.SerializeObject(data);
            SendMessage(MessageType.KeyPress, Encoding.UTF8.GetBytes(json));
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        private void SendMessage(MessageType type, byte[] data)
        {
            if (networkStream != null && networkStream.CanWrite)
            {
                try
                {
                    sendSemaphore.Wait();
                    try
                    {
                        // 构建消息（1字节类型 + 4字节长度 + 数据）
                        byte[] header = new byte[5];
                        header[0] = (byte)type;
                        byte[] lengthBytes = BitConverter.GetBytes(data.Length);
                        Array.Copy(lengthBytes, 0, header, 1, 4);

                        // 发送头部和数据
                        networkStream.Write(header, 0, header.Length);
                        networkStream.Write(data, 0, data.Length);
                        networkStream.Flush();
                    }
                    finally
                    {
                        sendSemaphore.Release();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"发送失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 停止网络服务
        /// </summary>
        public async Task StopAsync()
        {
            await Task.Run(() =>
            {
                isRunning = false;
                StopHeartbeat();
                
                // 立即关闭网络连接
                try
                {
                    networkStream?.Close();
                    tcpClient?.Close();
                    tcpListener?.Stop();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"关闭网络连接错误: {ex.Message}");
                }
                
                // 尝试等待线程结束，但不要等待太久
                listenerThread?.Join(100);
                receiveThread?.Join(100);
                
                OnConnectionStatusChanged?.Invoke(false, "已断开连接");
            });
        }

        public void Dispose()
        {
            // 直接清理资源，不等待异步操作
            isRunning = false;
            StopHeartbeat();
            
            try
            {
                networkStream?.Dispose();
                tcpClient?.Dispose();
                tcpListener?.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Dispose错误: {ex.Message}");
            }
        }

        private static void ParseEndpoint(string endpoint, out string host, out int endpointPort)
        {
            host = string.Empty;
            endpointPort = 0;

            if (string.IsNullOrWhiteSpace(endpoint))
            {
                return;
            }

            endpoint = endpoint.Trim();
            int idx = endpoint.LastIndexOf(':');
            if (idx <= 0 || idx >= endpoint.Length - 1)
            {
                return;
            }

            string hostPart = endpoint.Substring(0, idx).Trim();
            string portPart = endpoint.Substring(idx + 1).Trim();

            if (string.IsNullOrWhiteSpace(hostPart))
            {
                return;
            }

            if (!int.TryParse(portPart, out var p))
            {
                return;
            }

            if (p <= 0 || p > 65535)
            {
                return;
            }

            host = hostPart;
            endpointPort = p;
        }
    }
}
