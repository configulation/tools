using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WinFormsApp1.first_menu.RemoteControl
{
    /// <summary>
    /// 专业级网络传输协议
    /// </summary>
    public class NetworkProtocol
    {
        // 协议版本
        private const byte PROTOCOL_VERSION = 2;
        
        // 消息类型
        public enum MessageType : byte
        {
            // 基础消息
            Handshake = 0x01,
            Heartbeat = 0x02,
            Disconnect = 0x03,
            
            // 屏幕传输
            FullFrame = 0x10,
            DiffBlocks = 0x11,
            RequestFullFrame = 0x12,
            
            // 控制消息
            MouseMove = 0x20,
            MouseClick = 0x21,
            KeyPress = 0x22,
            
            // 优化消息
            QualityChange = 0x30,
            FpsChange = 0x31,
            NetworkStats = 0x32
        }
        
        // 消息头结构
        public struct MessageHeader
        {
            public byte Version;
            public MessageType Type;
            public uint SequenceNumber;
            public uint PayloadLength;
            public uint Checksum;
            public long Timestamp;
            
            public byte[] ToBytes()
            {
                using (var ms = new MemoryStream())
                using (var writer = new BinaryWriter(ms))
                {
                    writer.Write(Version);
                    writer.Write((byte)Type);
                    writer.Write(SequenceNumber);
                    writer.Write(PayloadLength);
                    writer.Write(Checksum);
                    writer.Write(Timestamp);
                    return ms.ToArray();
                }
            }
            
            public static MessageHeader FromBytes(byte[] data)
            {
                using (var ms = new MemoryStream(data))
                using (var reader = new BinaryReader(ms))
                {
                    return new MessageHeader
                    {
                        Version = reader.ReadByte(),
                        Type = (MessageType)reader.ReadByte(),
                        SequenceNumber = reader.ReadUInt32(),
                        PayloadLength = reader.ReadUInt32(),
                        Checksum = reader.ReadUInt32(),
                        Timestamp = reader.ReadInt64()
                    };
                }
            }
        }
        
        private NetworkStream stream;
        private int sequenceNumber = 0;  // 改为int类型以支持Interlocked.Increment
        private readonly ConcurrentDictionary<uint, TaskCompletionSource<byte[]>> pendingResponses;
        private readonly ConcurrentQueue<Message> sendQueue;
        private readonly ConcurrentQueue<Message> receiveQueue;
        private CancellationTokenSource cancellationToken;
        private Task sendTask;
        private Task receiveTask;
        
        // 网络统计
        private NetworkStatistics stats = new NetworkStatistics();
        
        public NetworkProtocol(NetworkStream networkStream)
        {
            stream = networkStream;
            pendingResponses = new ConcurrentDictionary<uint, TaskCompletionSource<byte[]>>();
            sendQueue = new ConcurrentQueue<Message>();
            receiveQueue = new ConcurrentQueue<Message>();
            cancellationToken = new CancellationTokenSource();
        }
        
        /// <summary>
        /// 启动协议处理
        /// </summary>
        public void Start()
        {
            sendTask = Task.Run(() => SendLoop(cancellationToken.Token));
            receiveTask = Task.Run(() => ReceiveLoop(cancellationToken.Token));
        }
        
        /// <summary>
        /// 停止协议处理
        /// </summary>
        public void Stop()
        {
            cancellationToken.Cancel();
            sendTask?.Wait(1000);
            receiveTask?.Wait(1000);
        }
        
        /// <summary>
        /// 发送差异块（优化版本）
        /// </summary>
        public async Task SendDiffBlocksAsync(ScreenDiffResult diffResult)
        {
            if (!diffResult.HasChanges)
                return;
            
            // 序列化差异数据
            var payload = SerializeDiffBlocks(diffResult);
            
            // 压缩数据
            var compressedData = CompressData(payload);
            
            // 发送消息
            await SendMessageAsync(MessageType.DiffBlocks, compressedData);
            
            // 更新统计
            stats.BytesSent += compressedData.Length;
            stats.BlocksSent += diffResult.ChangedBlocks.Count;
        }
        
        /// <summary>
        /// 序列化差异块
        /// </summary>
        private byte[] SerializeDiffBlocks(ScreenDiffResult diffResult)
        {
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                // 写入帧信息
                writer.Write(diffResult.FrameNumber);
                writer.Write(diffResult.Timestamp);
                writer.Write(diffResult.ChangedBlocks.Count);
                
                // 写入每个块
                foreach (var block in diffResult.ChangedBlocks)
                {
                    writer.Write(block.X);
                    writer.Write(block.Y);
                    writer.Write(block.Width);
                    writer.Write(block.Height);
                    writer.Write((byte)block.CompressionType);
                    writer.Write(block.Data.Length);
                    writer.Write(block.Data);
                }
                
                return ms.ToArray();
            }
        }
        
        /// <summary>
        /// 使用LZ4压缩数据（极快速度）
        /// </summary>
        private byte[] CompressData(byte[] data)
        {
            using (var output = new MemoryStream())
            {
                // 写入原始长度
                byte[] lengthBytes = BitConverter.GetBytes(data.Length);
                output.Write(lengthBytes, 0, 4);
                
                // 使用GZip压缩（实际项目中可以使用LZ4）
                using (var gzip = new GZipStream(output, CompressionLevel.Fastest))
                {
                    gzip.Write(data, 0, data.Length);
                }
                
                return output.ToArray();
            }
        }
        
        /// <summary>
        /// 解压数据
        /// </summary>
        private byte[] DecompressData(byte[] compressedData)
        {
            using (var input = new MemoryStream(compressedData))
            {
                // 读取原始长度
                byte[] lengthBytes = new byte[4];
                input.Read(lengthBytes, 0, 4);
                int originalLength = BitConverter.ToInt32(lengthBytes, 0);
                
                // 解压数据
                using (var gzip = new GZipStream(input, CompressionMode.Decompress))
                {
                    byte[] result = new byte[originalLength];
                    gzip.Read(result, 0, originalLength);
                    return result;
                }
            }
        }
        
        /// <summary>
        /// 发送消息（带确认机制）
        /// </summary>
        private async Task SendMessageAsync(MessageType type, byte[] data)
        {
            var header = new MessageHeader
            {
                Version = PROTOCOL_VERSION,
                Type = type,
                SequenceNumber = (uint)Interlocked.Increment(ref sequenceNumber),
                PayloadLength = (uint)data.Length,
                Checksum = CalculateChecksum(data),
                Timestamp = DateTime.Now.Ticks
            };
            
            var message = new Message
            {
                Header = header,
                Payload = data
            };
            
            // 加入发送队列
            sendQueue.Enqueue(message);
            
            // 等待确认（可选）
            if (RequiresAcknowledgment(type))
            {
                var tcs = new TaskCompletionSource<byte[]>();
                pendingResponses[header.SequenceNumber] = tcs;
                
                // 设置超时
                var cts = new CancellationTokenSource(5000);
                cts.Token.Register(() => tcs.TrySetCanceled());
                
                try
                {
                    await tcs.Task;
                }
                catch (TaskCanceledException)
                {
                    // 重传逻辑
                    sendQueue.Enqueue(message);
                    stats.RetransmitCount++;
                }
            }
        }
        
        /// <summary>
        /// 发送循环（批量发送优化）
        /// </summary>
        private async Task SendLoop(CancellationToken token)
        {
            var batch = new List<Message>();
            
            while (!token.IsCancellationRequested)
            {
                // 收集批量消息
                batch.Clear();
                while (sendQueue.TryDequeue(out var message) && batch.Count < 10)
                {
                    batch.Add(message);
                }
                
                if (batch.Count > 0)
                {
                    try
                    {
                        // 批量发送
                        await SendBatchAsync(batch);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"发送错误: {ex.Message}");
                    }
                }
                else
                {
                    await Task.Delay(1);
                }
            }
        }
        
        /// <summary>
        /// 批量发送消息
        /// </summary>
        private async Task SendBatchAsync(List<Message> messages)
        {
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                // 写入批量标记和消息数量
                writer.Write((byte)0xFF);
                writer.Write(messages.Count);
                
                // 写入所有消息
                foreach (var msg in messages)
                {
                    var headerBytes = msg.Header.ToBytes();
                    writer.Write(headerBytes.Length);
                    writer.Write(headerBytes);
                    writer.Write(msg.Payload.Length);
                    writer.Write(msg.Payload);
                }
                
                // 发送到网络
                var data = ms.ToArray();
                await stream.WriteAsync(data, 0, data.Length);
                await stream.FlushAsync();
                
                stats.MessagesSent += messages.Count;
            }
        }
        
        /// <summary>
        /// 接收循环
        /// </summary>
        private async Task ReceiveLoop(CancellationToken token)
        {
            byte[] buffer = new byte[65536];
            
            while (!token.IsCancellationRequested && stream.CanRead)
            {
                try
                {
                    // .NET Framework 使用这个重载
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        ProcessReceivedData(buffer, bytesRead);
                        stats.BytesReceived += bytesRead;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"接收错误: {ex.Message}");
                    await Task.Delay(100);
                }
            }
        }
        
        /// <summary>
        /// 处理接收到的数据
        /// </summary>
        private void ProcessReceivedData(byte[] buffer, int length)
        {
            // 这里简化处理，实际需要处理粘包/拆包问题
            using (var ms = new MemoryStream(buffer, 0, length))
            using (var reader = new BinaryReader(ms))
            {
                byte marker = reader.ReadByte();
                if (marker == 0xFF) // 批量消息
                {
                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        // 读取消息并处理
                        ProcessSingleMessage(reader);
                    }
                }
            }
        }
        
        private void ProcessSingleMessage(BinaryReader reader)
        {
            // 实现消息处理逻辑
            stats.MessagesReceived++;
        }
        
        /// <summary>
        /// 计算校验和
        /// </summary>
        private uint CalculateChecksum(byte[] data)
        {
            uint checksum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                checksum = ((checksum << 1) | (checksum >> 31)) ^ data[i];
            }
            return checksum;
        }
        
        /// <summary>
        /// 判断是否需要确认
        /// </summary>
        private bool RequiresAcknowledgment(MessageType type)
        {
            return type == MessageType.FullFrame || type == MessageType.Handshake;
        }
        
        /// <summary>
        /// 获取网络统计信息
        /// </summary>
        public NetworkStatistics GetStatistics()
        {
            return stats.Clone();
        }
        
        private class Message
        {
            public MessageHeader Header { get; set; }
            public byte[] Payload { get; set; }
        }
    }
    
    /// <summary>
    /// 网络统计信息
    /// </summary>
    public class NetworkStatistics
    {
        public long BytesSent { get; set; }
        public long BytesReceived { get; set; }
        public long MessagesSent { get; set; }
        public long MessagesReceived { get; set; }
        public long BlocksSent { get; set; }
        public int RetransmitCount { get; set; }
        public double AverageLatency { get; set; }
        public double PacketLossRate { get; set; }
        
        public NetworkStatistics Clone()
        {
            return (NetworkStatistics)MemberwiseClone();
        }
        
        public string GetFormattedStats()
        {
            return $"发送: {BytesSent / 1024.0:F2} KB | " +
                   $"接收: {BytesReceived / 1024.0:F2} KB | " +
                   $"延迟: {AverageLatency:F1}ms | " +
                   $"丢包率: {PacketLossRate:P1}";
        }
    }
}
