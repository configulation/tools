using Newtonsoft.Json;

namespace WinFormsApp1.second_menu.ChessRecognizer.Config
{
    /// <summary>
    /// 应用程序设置
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// OCR设置
        /// </summary>
        public OcrSettings Ocr { get; set; } = new();

        /// <summary>
        /// 棋盘检测设置
        /// </summary>
        public BoardDetectionSettings BoardDetection { get; set; } = new();

        /// <summary>
        /// FEN生成设置
        /// </summary>
        public FenSettings Fen { get; set; } = new();

        /// <summary>
        /// 性能设置
        /// </summary>
        public PerformanceSettings Performance { get; set; } = new();

        /// <summary>
        /// 界面设置
        /// </summary>
        public UISettings UI { get; set; } = new();

        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string ConfigPath => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Common",
            "Data",
            "ChessRecognizer",
            "chess_recognizer_settings.json"
        );

        /// <summary>
        /// 加载设置
        /// </summary>
        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    string json = File.ReadAllText(ConfigPath);
                    return JsonConvert.DeserializeObject<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载设置失败: {ex.Message}");
            }

            return new AppSettings();
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        public void Save()
        {
            try
            {
                string dir = Path.GetDirectoryName(ConfigPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存设置失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 重置为默认值
        /// </summary>
        public void Reset()
        {
            Ocr = new OcrSettings();
            BoardDetection = new BoardDetectionSettings();
            Fen = new FenSettings();
            Performance = new PerformanceSettings();
            UI = new UISettings();
        }
    }

    /// <summary>
    /// OCR设置
    /// </summary>
    public class OcrSettings
    {
        /// <summary>
        /// 默认OCR引擎
        /// </summary>
        public string DefaultEngine { get; set; } = "Tesseract";

        /// <summary>
        /// 置信度阈值
        /// </summary>
        public double ConfidenceThreshold { get; set; } = 0.5;

        /// <summary>
        /// 语言模型
        /// </summary>
        public string Language { get; set; } = "chi_sim";

        /// <summary>
        /// 是否启用预处理
        /// </summary>
        public bool EnablePreprocessing { get; set; } = true;

        /// <summary>
        /// 是否启用二值化
        /// </summary>
        public bool EnableBinarization { get; set; } = true;

        /// <summary>
        /// 是否启用去噪
        /// </summary>
        public bool EnableDenoising { get; set; } = false;

        /// <summary>
        /// 是否启用锐化
        /// </summary>
        public bool EnableSharpening { get; set; } = false;

        /// <summary>
        /// 是否启用多引擎投票
        /// </summary>
        public bool EnableMultiEngineVoting { get; set; } = false;

        /// <summary>
        /// 是否启用位置推测（当OCR无法识别时，根据棋子位置推测类型）
        /// 注意：这会降低识别准确性，仅在初始布局时有效
        /// </summary>
        public bool EnablePositionGuessing { get; set; } = true;
    }

    /// <summary>
    /// 棋盘检测设置
    /// </summary>
    public class BoardDetectionSettings
    {
        /// <summary>
        /// Canny边缘检测阈值下限
        /// </summary>
        public double CannyThreshold1 { get; set; } = 50;

        /// <summary>
        /// Canny边缘检测阈值上限
        /// </summary>
        public double CannyThreshold2 { get; set; } = 150;

        /// <summary>
        /// 霍夫变换阈值
        /// </summary>
        public int HoughThreshold { get; set; } = 100;

        /// <summary>
        /// 最小线段长度
        /// </summary>
        public double MinLineLength { get; set; } = 50;

        /// <summary>
        /// 最大线段间隙
        /// </summary>
        public double MaxLineGap { get; set; } = 10;

        /// <summary>
        /// 线合并距离阈值
        /// </summary>
        public double LineMergeThreshold { get; set; } = 20;

        /// <summary>
        /// 棋子区域扩展比例
        /// </summary>
        public double PieceRegionScale { get; set; } = 0.8;

        /// <summary>
        /// 是否启用透视矫正
        /// </summary>
        public bool EnablePerspectiveCorrection { get; set; } = false;

        /// <summary>
        /// 红色HSV阈值
        /// </summary>
        public ColorThreshold RedColorThreshold { get; set; } = new()
        {
            HueMin = 0, HueMax = 30,
            SaturationMin = 0.3, SaturationMax = 1.0,
            ValueMin = 0.3, ValueMax = 1.0
        };

        /// <summary>
        /// 黑色HSV阈值
        /// </summary>
        public ColorThreshold BlackColorThreshold { get; set; } = new()
        {
            HueMin = 0, HueMax = 360,
            SaturationMin = 0, SaturationMax = 0.3,
            ValueMin = 0, ValueMax = 0.5
        };
    }

    /// <summary>
    /// 颜色阈值
    /// </summary>
    public class ColorThreshold
    {
        public double HueMin { get; set; }
        public double HueMax { get; set; }
        public double SaturationMin { get; set; }
        public double SaturationMax { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
    }

    /// <summary>
    /// FEN设置
    /// </summary>
    public class FenSettings
    {
        /// <summary>
        /// 是否使用中国坐标系
        /// </summary>
        public bool UseChineseCoordinates { get; set; } = false;

        /// <summary>
        /// 是否启用规则验证
        /// </summary>
        public bool EnableRuleValidation { get; set; } = true;

        /// <summary>
        /// 自动纠错级别 (0-3)
        /// </summary>
        public int AutoCorrectionLevel { get; set; } = 1;

        /// <summary>
        /// 默认走棋方
        /// </summary>
        public bool DefaultRedTurn { get; set; } = true;
    }

    /// <summary>
    /// 性能设置
    /// </summary>
    public class PerformanceSettings
    {
        /// <summary>
        /// 并行处理线程数
        /// </summary>
        public int ParallelThreads { get; set; } = 4;

        /// <summary>
        /// 内存缓存大小(MB)
        /// </summary>
        public int CacheSizeMB { get; set; } = 100;

        /// <summary>
        /// 图片最大宽度
        /// </summary>
        public int MaxImageWidth { get; set; } = 2000;

        /// <summary>
        /// 图片最大高度
        /// </summary>
        public int MaxImageHeight { get; set; } = 2000;

        /// <summary>
        /// 超时时间(秒)
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;

        /// <summary>
        /// 是否启用GPU加速
        /// </summary>
        public bool EnableGPU { get; set; } = false;
    }

    /// <summary>
    /// 界面设置
    /// </summary>
    public class UISettings
    {
        /// <summary>
        /// 预览图缩放比例
        /// </summary>
        public double PreviewScale { get; set; } = 1.0;

        /// <summary>
        /// 控制台字体名称
        /// </summary>
        public string ConsoleFontName { get; set; } = "Consolas";

        /// <summary>
        /// 控制台字体大小
        /// </summary>
        public float ConsoleFontSize { get; set; } = 10;

        /// <summary>
        /// 是否自动复制FEN到剪贴板
        /// </summary>
        public bool AutoCopyFEN { get; set; } = true;

        /// <summary>
        /// 是否自动保存结果
        /// </summary>
        public bool AutoSaveResult { get; set; } = false;

        /// <summary>
        /// 最近文件数量
        /// </summary>
        public int RecentFilesCount { get; set; } = 10;

        /// <summary>
        /// 最近文件列表
        /// </summary>
        public List<string> RecentFiles { get; set; } = new();
    }
}
