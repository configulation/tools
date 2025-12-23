using System;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace WinFormsApp1.first_menu.ChessAnalyzer.Core
{
    /// <summary>
    /// 配置管理类
    /// </summary>
    public class ConfigManager
    {
        private const string CONFIG_FILE = "Config/chess_analyzer_config.json";
        private static AnalyzerConfig _config;

        /// <summary>
        /// 加载配置
        /// </summary>
        public static AnalyzerConfig LoadConfig()
        {
            if (_config != null)
                return _config;

            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CONFIG_FILE);

                if (File.Exists(configPath))
                {
                    string json = File.ReadAllText(configPath);
                    _config = JsonSerializer.Deserialize<AnalyzerConfig>(json);
                }
                else
                {
                    _config = CreateDefaultConfig();
                }
            }
            catch
            {
                _config = CreateDefaultConfig();
            }

            return _config;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public static void SaveConfig(AnalyzerConfig config)
        {
            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CONFIG_FILE);
                string directory = Path.GetDirectoryName(configPath);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                string json = JsonSerializer.Serialize(config, options);
                File.WriteAllText(configPath, json);

                _config = config;
            }
            catch (Exception ex)
            {
                throw new Exception($"保存配置失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 创建默认配置
        /// </summary>
        private static AnalyzerConfig CreateDefaultConfig()
        {
            return new AnalyzerConfig
            {
                EnginePath = "engine/Pikafish.2025-10-27/Windows/pikafish-avx2.exe",
                NnuePath = "engine/Pikafish.2025-10-27/pikafish.nnue",
                SearchDepth = 18,
                ScanInterval = 2000,
                EngineThreads = 4,
                EngineHashSize = 256,
                OverlayWindowPosition = new Point(100, 100),
                AutoStartOverlay = true
            };
        }
    }

    /// <summary>
    /// 分析器配置类
    /// </summary>
    public class AnalyzerConfig
    {
        /// <summary>
        /// 引擎路径
        /// </summary>
        public string EnginePath { get; set; }

        /// <summary>
        /// NNUE 文件路径
        /// </summary>
        public string NnuePath { get; set; }

        /// <summary>
        /// 搜索深度
        /// </summary>
        public int SearchDepth { get; set; }

        /// <summary>
        /// 扫描间隔（毫秒）
        /// </summary>
        public int ScanInterval { get; set; }

        /// <summary>
        /// 引擎线程数
        /// </summary>
        public int EngineThreads { get; set; }

        /// <summary>
        /// 引擎哈希表大小（MB）
        /// </summary>
        public int EngineHashSize { get; set; }

        /// <summary>
        /// 悬浮窗位置
        /// </summary>
        public Point OverlayWindowPosition { get; set; }

        /// <summary>
        /// 是否自动启动悬浮窗
        /// </summary>
        public bool AutoStartOverlay { get; set; }

        /// <summary>
        /// 最后选择的棋盘区域
        /// </summary>
        public Rectangle LastSelectedArea { get; set; }
    }
}
