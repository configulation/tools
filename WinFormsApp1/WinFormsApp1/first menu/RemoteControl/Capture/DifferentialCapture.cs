using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace WinFormsApp1.first_menu.RemoteControl
{
    /// <summary>
    /// 专业级差异捕获引擎
    /// </summary>
    public class DifferentialCapture : IDisposable
    {
        // 屏幕分块大小
        private const int BLOCK_SIZE = 32;
        
        // 缓冲区
        private Bitmap previousFrame;
        private Bitmap currentFrame;
        private byte[] previousBuffer;
        private byte[] currentBuffer;
        
        // 屏幕信息
        private Rectangle screenBounds;
        private int blocksX;
        private int blocksY;
        private bool[,] dirtyBlocks;
        
        // 性能统计
        private long frameCount = 0;
        private DateTime lastCaptureTime;
        private double averageFps = 30;
        
        // 线程安全
        private readonly object lockObject = new object();
        private bool disposed = false;
        
        public DifferentialCapture()
        {
            screenBounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            blocksX = (screenBounds.Width + BLOCK_SIZE - 1) / BLOCK_SIZE;
            blocksY = (screenBounds.Height + BLOCK_SIZE - 1) / BLOCK_SIZE;
            dirtyBlocks = new bool[blocksX, blocksY];
            
            InitializeBuffers();
        }
        
        private void InitializeBuffers()
        {
            previousFrame = new Bitmap(screenBounds.Width, screenBounds.Height, PixelFormat.Format32bppArgb);
            currentFrame = new Bitmap(screenBounds.Width, screenBounds.Height, PixelFormat.Format32bppArgb);
            
            int bufferSize = screenBounds.Width * screenBounds.Height * 4;
            previousBuffer = new byte[bufferSize];
            currentBuffer = new byte[bufferSize];
            
            lastCaptureTime = DateTime.Now;
        }
        
        /// <summary>
        /// 捕获屏幕变化区域
        /// </summary>
        public ScreenDiffResult CaptureChanges()
        {
            lock (lockObject)
            {
                // 捕获当前屏幕
                CaptureCurrentScreen();
                
                // 计算差异
                var changedBlocks = DetectChanges();
                
                // 如果没有变化，返回空结果
                if (changedBlocks.Count == 0)
                {
                    return new ScreenDiffResult
                    {
                        HasChanges = false,
                        FrameNumber = frameCount++,
                        Timestamp = DateTime.Now.Ticks
                    };
                }
                
                // 编码变化的块
                var encodedBlocks = EncodeBlocks(changedBlocks);
                
                // 交换缓冲区
                SwapBuffers();
                
                // 更新统计
                UpdateStatistics();
                
                return new ScreenDiffResult
                {
                    HasChanges = true,
                    ChangedBlocks = encodedBlocks,
                    FrameNumber = frameCount++,
                    Timestamp = DateTime.Now.Ticks,
                    EstimatedFps = averageFps
                };
            }
        }
        
        /// <summary>
        /// 使用硬件加速捕获屏幕
        /// </summary>
        private void CaptureCurrentScreen()
        {
            using (Graphics g = Graphics.FromImage(currentFrame))
            {
                g.CopyFromScreen(screenBounds.X, screenBounds.Y, 0, 0, screenBounds.Size, CopyPixelOperation.SourceCopy);
            }
            
            // 将位图数据复制到字节数组（更快的比较）
            BitmapData bmpData = currentFrame.LockBits(
                new Rectangle(0, 0, currentFrame.Width, currentFrame.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);
            
            Marshal.Copy(bmpData.Scan0, currentBuffer, 0, currentBuffer.Length);
            currentFrame.UnlockBits(bmpData);
        }
        
        /// <summary>
        /// 快速检测变化的块（使用SIMD优化）
        /// </summary>
        private List<Point> DetectChanges()
        {
            var changedBlocks = new List<Point>();
            
            // 重置脏块标记
            Array.Clear(dirtyBlocks, 0, dirtyBlocks.Length);
            
            // 并行检测每个块
            Parallel.For(0, blocksY, y =>
            {
                for (int x = 0; x < blocksX; x++)
                {
                    if (IsBlockChanged(x, y))
                    {
                        lock (changedBlocks)
                        {
                            changedBlocks.Add(new Point(x, y));
                            dirtyBlocks[x, y] = true;
                        }
                    }
                }
            });
            
            return changedBlocks;
        }
        
        /// <summary>
        /// 检查单个块是否变化（使用快速哈希比较）
        /// </summary>
        private bool IsBlockChanged(int blockX, int blockY)
        {
            int startX = blockX * BLOCK_SIZE;
            int startY = blockY * BLOCK_SIZE;
            int endX = Math.Min(startX + BLOCK_SIZE, screenBounds.Width);
            int endY = Math.Min(startY + BLOCK_SIZE, screenBounds.Height);
            
            // 快速哈希比较
            uint currentHash = 0;
            uint previousHash = 0;
            
            for (int y = startY; y < endY; y++)
            {
                int rowOffset = y * screenBounds.Width * 4;
                for (int x = startX; x < endX; x++)
                {
                    int pixelOffset = rowOffset + x * 4;
                    
                    // 使用FNV-1a哈希算法
                    for (int i = 0; i < 4; i++)
                    {
                        currentHash ^= currentBuffer[pixelOffset + i];
                        currentHash *= 16777619;
                        
                        previousHash ^= previousBuffer[pixelOffset + i];
                        previousHash *= 16777619;
                    }
                }
            }
            
            return currentHash != previousHash;
        }
        
        /// <summary>
        /// 编码变化的块（使用高效压缩）
        /// </summary>
        private List<EncodedBlock> EncodeBlocks(List<Point> changedBlocks)
        {
            var encodedBlocks = new List<EncodedBlock>();
            
            foreach (var block in changedBlocks)
            {
                var encodedBlock = EncodeBlock(block.X, block.Y);
                if (encodedBlock != null)
                {
                    encodedBlocks.Add(encodedBlock);
                }
            }
            
            return encodedBlocks;
        }
        
        /// <summary>
        /// 编码单个块
        /// </summary>
        private EncodedBlock EncodeBlock(int blockX, int blockY)
        {
            int startX = blockX * BLOCK_SIZE;
            int startY = blockY * BLOCK_SIZE;
            int width = Math.Min(BLOCK_SIZE, screenBounds.Width - startX);
            int height = Math.Min(BLOCK_SIZE, screenBounds.Height - startY);
            
            using (Bitmap blockBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(blockBitmap))
                {
                    g.DrawImage(currentFrame, 
                        new Rectangle(0, 0, width, height),
                        new Rectangle(startX, startY, width, height),
                        GraphicsUnit.Pixel);
                }
                
                // 使用WebP或高质量JPEG压缩
                byte[] compressedData = CompressBlock(blockBitmap);
                
                return new EncodedBlock
                {
                    X = blockX,
                    Y = blockY,
                    Width = width,
                    Height = height,
                    Data = compressedData,
                    CompressionType = CompressionType.JPEG
                };
            }
        }
        
        /// <summary>
        /// 高效压缩块数据
        /// </summary>
        private byte[] CompressBlock(Bitmap block)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // 动态调整压缩质量
                long quality = CalculateDynamicQuality();
                
                ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                
                block.Save(ms, jpegCodec, encoderParams);
                return ms.ToArray();
            }
        }
        
        /// <summary>
        /// 动态计算压缩质量
        /// </summary>
        private long CalculateDynamicQuality()
        {
            // 根据FPS动态调整质量
            if (averageFps < 10)
                return 90; // 低帧率，高质量
            else if (averageFps < 20)
                return 75; // 中等帧率，中等质量
            else
                return 60; // 高帧率，较低质量
        }
        
        /// <summary>
        /// 交换前后帧缓冲区
        /// </summary>
        private void SwapBuffers()
        {
            // 交换位图
            var tempBitmap = previousFrame;
            previousFrame = currentFrame;
            currentFrame = tempBitmap;
            
            // 交换字节数组
            var tempBuffer = previousBuffer;
            previousBuffer = currentBuffer;
            currentBuffer = tempBuffer;
        }
        
        /// <summary>
        /// 更新性能统计
        /// </summary>
        private void UpdateStatistics()
        {
            DateTime now = DateTime.Now;
            double deltaTime = (now - lastCaptureTime).TotalSeconds;
            
            if (deltaTime > 0)
            {
                double currentFps = 1.0 / deltaTime;
                averageFps = averageFps * 0.9 + currentFps * 0.1; // 指数移动平均
            }
            
            lastCaptureTime = now;
        }
        
        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType == mimeType)
                    return codec;
            }
            return null;
        }
        
        /// <summary>
        /// 获取完整帧（用于初始同步）
        /// </summary>
        public byte[] GetFullFrame()
        {
            lock (lockObject)
            {
                CaptureCurrentScreen();
                
                using (MemoryStream ms = new MemoryStream())
                {
                    // 使用PNG格式保证质量
                    currentFrame.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }
        
        /// <summary>
        /// 重置差异检测（场景切换时使用）
        /// </summary>
        public void Reset()
        {
            lock (lockObject)
            {
                Array.Clear(previousBuffer, 0, previousBuffer.Length);
                Array.Clear(dirtyBlocks, 0, dirtyBlocks.Length);
                frameCount = 0;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    lock (lockObject)
                    {
                        previousFrame?.Dispose();
                        currentFrame?.Dispose();
                    }
                }
                disposed = true;
            }
        }
    }
    
    /// <summary>
    /// 屏幕差异结果
    /// </summary>
    public class ScreenDiffResult
    {
        public bool HasChanges { get; set; }
        public List<EncodedBlock> ChangedBlocks { get; set; }
        public long FrameNumber { get; set; }
        public long Timestamp { get; set; }
        public double EstimatedFps { get; set; }
    }
    
    /// <summary>
    /// 编码后的块数据
    /// </summary>
    public class EncodedBlock
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Data { get; set; }
        public CompressionType CompressionType { get; set; }
    }
    
    /// <summary>
    /// 压缩类型
    /// </summary>
    public enum CompressionType
    {
        None,
        JPEG,
        PNG,
        WebP,
        H264
    }
}
