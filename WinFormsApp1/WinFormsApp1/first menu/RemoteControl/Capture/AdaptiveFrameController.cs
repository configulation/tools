using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace WinFormsApp1.first_menu.RemoteControl
{
    /// <summary>
    /// 自适应帧率控制器 - 根据场景和网络动态调整帧率
    /// </summary>
    public class AdaptiveFrameController
    {
        // 场景类型
        public enum SceneType
        {
            Static,      // 静态场景（文档、代码编辑）
            Normal,      // 普通活动（浏览网页、操作界面）
            Dynamic,     // 动态场景（视频播放、游戏）
            HighMotion   // 高运动场景（快速滚动、动画）
        }
        
        // 帧率配置
        private readonly Dictionary<SceneType, FrameRateConfig> frameConfigs = new Dictionary<SceneType, FrameRateConfig>
        {
            { SceneType.Static, new FrameRateConfig { MinFps = 2, MaxFps = 10, TargetFps = 5 } },
            { SceneType.Normal, new FrameRateConfig { MinFps = 10, MaxFps = 30, TargetFps = 20 } },
            { SceneType.Dynamic, new FrameRateConfig { MinFps = 20, MaxFps = 60, TargetFps = 30 } },
            { SceneType.HighMotion, new FrameRateConfig { MinFps = 30, MaxFps = 60, TargetFps = 45 } }
        };
        
        // 场景检测参数
        private const int MOTION_HISTORY_SIZE = 30;
        private readonly Queue<MotionData> motionHistory = new Queue<MotionData>();
        
        // 当前状态
        private SceneType currentScene = SceneType.Normal;
        private double currentFps = 30;
        private double targetFps = 30;
        private DateTime lastFrameTime = DateTime.Now;
        
        // 网络自适应
        private double networkBandwidth = 10.0; // Mbps
        private double networkLatency = 10.0;   // ms
        private double packetLoss = 0.0;        // %
        
        // 性能监控
        private PerformanceCounter cpuCounter;
        private PerformanceCounter memoryCounter;
        private double cpuUsage = 0;
        private double memoryUsage = 0;
        
        public AdaptiveFrameController()
        {
            InitializePerformanceCounters();
        }
        
        private void InitializePerformanceCounters()
        {
            try
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"性能计数器初始化失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 分析场景并调整帧率
        /// </summary>
        public double AnalyzeAndAdjust(ScreenDiffResult diffResult)
        {
            // 1. 收集运动数据
            CollectMotionData(diffResult);
            
            // 2. 检测场景类型
            SceneType detectedScene = DetectSceneType();
            
            // 3. 平滑场景过渡
            currentScene = SmoothSceneTransition(currentScene, detectedScene);
            
            // 4. 计算目标帧率
            targetFps = CalculateTargetFps(currentScene);
            
            // 5. 应用网络自适应
            targetFps = ApplyNetworkAdaptation(targetFps);
            
            // 6. 应用性能限制
            targetFps = ApplyPerformanceLimits(targetFps);
            
            // 7. 平滑帧率调整
            currentFps = SmoothFpsTransition(currentFps, targetFps);
            
            return currentFps;
        }
        
        /// <summary>
        /// 收集运动数据
        /// </summary>
        private void CollectMotionData(ScreenDiffResult diffResult)
        {
            var now = DateTime.Now;
            var deltaTime = (now - lastFrameTime).TotalSeconds;
            lastFrameTime = now;
            
            var motionData = new MotionData
            {
                Timestamp = now,
                ChangedBlocks = diffResult.HasChanges ? diffResult.ChangedBlocks?.Count ?? 0 : 0,
                DeltaTime = deltaTime,
                MotionIntensity = CalculateMotionIntensity(diffResult)
            };
            
            motionHistory.Enqueue(motionData);
            
            // 保持历史记录大小
            while (motionHistory.Count > MOTION_HISTORY_SIZE)
            {
                motionHistory.Dequeue();
            }
        }
        
        /// <summary>
        /// 计算运动强度
        /// </summary>
        private double CalculateMotionIntensity(ScreenDiffResult diffResult)
        {
            if (!diffResult.HasChanges || diffResult.ChangedBlocks == null)
                return 0;
            
            // 计算变化面积和分布
            double totalArea = 0;
            double distribution = 0;
            
            foreach (var block in diffResult.ChangedBlocks)
            {
                totalArea += block.Width * block.Height;
            }
            
            // 计算块的分散程度（分布越广，运动越剧烈）
            if (diffResult.ChangedBlocks.Count > 1)
            {
                var centerX = diffResult.ChangedBlocks.Average(b => b.X);
                var centerY = diffResult.ChangedBlocks.Average(b => b.Y);
                
                foreach (var block in diffResult.ChangedBlocks)
                {
                    distribution += Math.Sqrt(Math.Pow(block.X - centerX, 2) + Math.Pow(block.Y - centerY, 2));
                }
                distribution /= diffResult.ChangedBlocks.Count;
            }
            
            // 综合计算运动强度（0-1）
            double intensity = (totalArea / (1920.0 * 1080.0)) * 0.6 + (distribution / 100.0) * 0.4;
            return Math.Min(1.0, intensity);
        }
        
        /// <summary>
        /// 检测场景类型
        /// </summary>
        private SceneType DetectSceneType()
        {
            if (motionHistory.Count < 10)
                return SceneType.Normal;
            
            // 计算最近的运动统计
            var recentMotion = motionHistory.Skip(Math.Max(0, motionHistory.Count - 10)).ToList();
            double avgIntensity = recentMotion.Average(m => m.MotionIntensity);
            double avgChangedBlocks = recentMotion.Average(m => m.ChangedBlocks);
            double changeFrequency = recentMotion.Count(m => m.ChangedBlocks > 0) / (double)recentMotion.Count;
            
            // 基于统计数据判断场景
            if (avgIntensity < 0.05 && avgChangedBlocks < 5 && changeFrequency < 0.3)
            {
                return SceneType.Static;
            }
            else if (avgIntensity < 0.2 && avgChangedBlocks < 20)
            {
                return SceneType.Normal;
            }
            else if (avgIntensity < 0.5 && avgChangedBlocks < 50)
            {
                return SceneType.Dynamic;
            }
            else
            {
                return SceneType.HighMotion;
            }
        }
        
        /// <summary>
        /// 平滑场景过渡（避免频繁切换）
        /// </summary>
        private SceneType SmoothSceneTransition(SceneType current, SceneType detected)
        {
            // 使用迟滞效应避免抖动
            if (current == detected)
                return current;
            
            // 需要连续检测到相同场景多次才切换
            if (!sceneTransitionBuffer.ContainsKey(detected))
                sceneTransitionBuffer[detected] = 0;
            
            sceneTransitionBuffer[detected]++;
            
            // 清理其他场景的计数
            foreach (var key in sceneTransitionBuffer.Keys.ToList())
            {
                if (key != detected)
                    sceneTransitionBuffer[key] = 0;
            }
            
            // 达到阈值才切换
            if (sceneTransitionBuffer[detected] >= 5)
            {
                sceneTransitionBuffer.Clear();
                return detected;
            }
            
            return current;
        }
        
        private Dictionary<SceneType, int> sceneTransitionBuffer = new Dictionary<SceneType, int>();
        
        /// <summary>
        /// 计算目标帧率
        /// </summary>
        private double CalculateTargetFps(SceneType scene)
        {
            var config = frameConfigs[scene];
            return config.TargetFps;
        }
        
        /// <summary>
        /// 应用网络自适应
        /// </summary>
        private double ApplyNetworkAdaptation(double fps)
        {
            // 根据带宽限制帧率
            double maxFpsByBandwidth = networkBandwidth * 1024 * 1024 / (1920 * 1080 * 3); // 粗略估算
            
            // 根据延迟调整
            if (networkLatency > 100)
                maxFpsByBandwidth *= 0.7;
            else if (networkLatency > 50)
                maxFpsByBandwidth *= 0.85;
            
            // 根据丢包率调整
            if (packetLoss > 5)
                maxFpsByBandwidth *= 0.6;
            else if (packetLoss > 2)
                maxFpsByBandwidth *= 0.8;
            
            return Math.Min(fps, maxFpsByBandwidth);
        }
        
        /// <summary>
        /// 应用性能限制
        /// </summary>
        private double ApplyPerformanceLimits(double fps)
        {
            // 更新性能数据
            UpdatePerformanceMetrics();
            
            // CPU限制
            if (cpuUsage > 80)
                fps = Math.Min(fps, 15);
            else if (cpuUsage > 60)
                fps = Math.Min(fps, 25);
            
            // 内存限制
            if (memoryUsage < 500) // MB
                fps = Math.Min(fps, 20);
            
            return fps;
        }
        
        /// <summary>
        /// 平滑帧率过渡
        /// </summary>
        private double SmoothFpsTransition(double current, double target)
        {
            // 使用指数移动平均实现平滑过渡
            double alpha = 0.2; // 平滑因子
            
            // 快速响应场景变化
            if (Math.Abs(target - current) > 20)
                alpha = 0.5;
            
            return current * (1 - alpha) + target * alpha;
        }
        
        /// <summary>
        /// 更新性能指标
        /// </summary>
        private void UpdatePerformanceMetrics()
        {
            try
            {
                if (cpuCounter != null)
                    cpuUsage = cpuCounter.NextValue();
                
                if (memoryCounter != null)
                    memoryUsage = memoryCounter.NextValue();
            }
            catch
            {
                // 忽略性能计数器错误
            }
        }
        
        /// <summary>
        /// 更新网络状态
        /// </summary>
        public void UpdateNetworkStatus(double bandwidth, double latency, double loss)
        {
            networkBandwidth = bandwidth;
            networkLatency = latency;
            packetLoss = loss;
        }
        
        /// <summary>
        /// 获取当前帧率
        /// </summary>
        public double GetCurrentFps()
        {
            return currentFps;
        }
        
        /// <summary>
        /// 获取当前场景类型
        /// </summary>
        public SceneType GetCurrentScene()
        {
            return currentScene;
        }
        
        /// <summary>
        /// 获取性能报告
        /// </summary>
        public string GetPerformanceReport()
        {
            return $"场景: {currentScene} | " +
                   $"FPS: {currentFps:F1} | " +
                   $"CPU: {cpuUsage:F1}% | " +
                   $"内存: {memoryUsage:F0}MB | " +
                   $"带宽: {networkBandwidth:F1}Mbps | " +
                   $"延迟: {networkLatency:F0}ms";
        }
        
        // 内部类
        private class MotionData
        {
            public DateTime Timestamp { get; set; }
            public int ChangedBlocks { get; set; }
            public double DeltaTime { get; set; }
            public double MotionIntensity { get; set; }
        }
        
        private class FrameRateConfig
        {
            public double MinFps { get; set; }
            public double MaxFps { get; set; }
            public double TargetFps { get; set; }
        }
    }
}
