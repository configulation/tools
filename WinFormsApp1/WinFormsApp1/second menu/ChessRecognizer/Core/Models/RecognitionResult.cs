using System.Drawing;

namespace WinFormsApp1.second_menu.ChessRecognizer.Core.Models
{
    /// <summary>
    /// 完整识别结果
    /// </summary>
    public class FullRecognitionResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 生成的FEN字符串
        /// </summary>
        public string FEN { get; set; } = string.Empty;

        /// <summary>
        /// 棋盘位置
        /// </summary>
        public BoardPosition BoardPosition { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// 警告信息列表
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// 识别的棋子数量
        /// </summary>
        public int PieceCount { get; set; }

        /// <summary>
        /// 红方棋子数量
        /// </summary>
        public int RedPieceCount { get; set; }

        /// <summary>
        /// 黑方棋子数量
        /// </summary>
        public int BlackPieceCount { get; set; }

        /// <summary>
        /// 总体置信度
        /// </summary>
        public double OverallConfidence { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public TimeSpan ProcessTime { get; set; }

        /// <summary>
        /// 各步骤耗时
        /// </summary>
        public Dictionary<string, TimeSpan> StepTimes { get; set; } = new();

        /// <summary>
        /// 使用的OCR引擎
        /// </summary>
        public string OcrEngine { get; set; } = string.Empty;

        /// <summary>
        /// 调试信息
        /// </summary>
        public string DebugInfo { get; set; } = string.Empty;

        /// <summary>
        /// 每个位置的详细识别结果
        /// </summary>
        public List<PieceRecognitionDetail> PieceDetails { get; set; } = new();

        /// <summary>
        /// 原始图像
        /// </summary>
        public Bitmap OriginalImage { get; set; }

        /// <summary>
        /// 处理后的图像
        /// </summary>
        public Bitmap ProcessedImage { get; set; }
    }

    /// <summary>
    /// 单个棋子识别详情
    /// </summary>
    public class PieceRecognitionDetail
    {
        /// <summary>
        /// 列位置
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// 行位置
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 识别的文字
        /// </summary>
        public string RecognizedText { get; set; } = string.Empty;

        /// <summary>
        /// 最终确定的棋子类型
        /// </summary>
        public PieceType FinalPieceType { get; set; }

        /// <summary>
        /// 是否红方
        /// </summary>
        public bool IsRed { get; set; }

        /// <summary>
        /// 置信度
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// 各OCR引擎的识别结果
        /// </summary>
        public Dictionary<string, OcrEngineResult> EngineResults { get; set; } = new();

        /// <summary>
        /// 颜色检测结果
        /// </summary>
        public ColorDetectionResult ColorResult { get; set; }

        /// <summary>
        /// 棋子区域图像
        /// </summary>
        public Bitmap PieceImage { get; set; }
    }

    /// <summary>
    /// 单个OCR引擎结果
    /// </summary>
    public class OcrEngineResult
    {
        public string EngineName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public TimeSpan ProcessTime { get; set; }
    }

    /// <summary>
    /// 颜色检测结果
    /// </summary>
    public class ColorDetectionResult
    {
        public bool IsRed { get; set; }
        public double RedScore { get; set; }
        public double BlackScore { get; set; }
        public double Confidence { get; set; }
        public int DominantHue { get; set; }
        public int Saturation { get; set; }
        public int Value { get; set; }
    }

    /// <summary>
    /// 识别进度事件参数
    /// </summary>
    public class RecognitionProgressEventArgs : EventArgs
    {
        public int CurrentStep { get; set; }
        public int TotalSteps { get; set; }
        public string StepName { get; set; } = string.Empty;
        public double Progress { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
