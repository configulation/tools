using System.Drawing;

namespace WinFormsApp1.second_menu.ChessRecognizer.Core.Interfaces
{
    /// <summary>
    /// OCR识别引擎接口
    /// </summary>
    public interface IChessOCR : IDisposable
    {
        /// <summary>
        /// 引擎名称
        /// </summary>
        string EngineName { get; }

        /// <summary>
        /// 是否已初始化
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// 初始化OCR引擎
        /// </summary>
        Task<bool> InitializeAsync();

        /// <summary>
        /// 识别单个棋子图像
        /// </summary>
        /// <param name="pieceImage">棋子图像</param>
        /// <returns>识别结果（汉字）</returns>
        Task<OcrResult> RecognizePieceAsync(Bitmap pieceImage);

        /// <summary>
        /// 批量识别棋子
        /// </summary>
        Task<List<OcrResult>> RecognizePiecesAsync(List<Bitmap> pieceImages);

        /// <summary>
        /// 设置置信度阈值
        /// </summary>
        void SetConfidenceThreshold(double threshold);

        /// <summary>
        /// 获取引擎状态信息
        /// </summary>
        OcrEngineStatus GetStatus();
    }

    /// <summary>
    /// OCR识别结果
    /// </summary>
    public class OcrResult
    {
        public string Text { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public TimeSpan ProcessTime { get; set; }
    }

    /// <summary>
    /// OCR引擎状态
    /// </summary>
    public class OcrEngineStatus
    {
        public string EngineName { get; set; } = string.Empty;
        public bool IsReady { get; set; }
        public long MemoryUsage { get; set; }
        public int ProcessedCount { get; set; }
        public double AverageProcessTime { get; set; }
    }
}
