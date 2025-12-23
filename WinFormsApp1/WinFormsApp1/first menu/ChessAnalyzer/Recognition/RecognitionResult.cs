namespace WinFormsApp1.first_menu.ChessAnalyzer.Recognition
{
    /// <summary>
    /// 识别结果类
    /// </summary>
    public class RecognitionResult
    {
        public bool Success { get; set; }
        public string FEN { get; set; }
        public string Message { get; set; }
        public string DebugInfo { get; set; }
    }
}
