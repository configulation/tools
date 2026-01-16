namespace WinFormsApp1.second_menu.ChessRecognizer.Core.Interfaces
{
    /// <summary>
    /// FEN字符串生成器接口
    /// </summary>
    public interface IFENGenerator
    {
        /// <summary>
        /// 从识别结果生成FEN字符串
        /// </summary>
        FenGenerationResult GenerateFEN(RecognizedBoard board);

        /// <summary>
        /// 验证FEN字符串是否合法
        /// </summary>
        FenValidationResult ValidateFEN(string fen);

        /// <summary>
        /// 解析FEN字符串为棋盘状态
        /// </summary>
        RecognizedBoard ParseFEN(string fen);

        /// <summary>
        /// 设置生成选项
        /// </summary>
        void SetOptions(FenGeneratorOptions options);
    }

    /// <summary>
    /// 识别后的棋盘状态
    /// </summary>
    public class RecognizedBoard
    {
        /// <summary>
        /// 棋盘状态 [列0-8, 行0-9]
        /// </summary>
        public RecognizedPiece[,] Pieces { get; set; } = new RecognizedPiece[9, 10];

        /// <summary>
        /// 当前走棋方（true=红方）
        /// </summary>
        public bool IsRedTurn { get; set; } = true;

        /// <summary>
        /// 识别置信度
        /// </summary>
        public double OverallConfidence { get; set; }

        /// <summary>
        /// 识别的棋子总数
        /// </summary>
        public int TotalPieces { get; set; }
    }

    /// <summary>
    /// 识别的棋子
    /// </summary>
    public class RecognizedPiece
    {
        /// <summary>
        /// 棋子类型
        /// </summary>
        public ChessPieceType PieceType { get; set; } = ChessPieceType.Empty;

        /// <summary>
        /// 识别的文字
        /// </summary>
        public string RecognizedText { get; set; } = string.Empty;

        /// <summary>
        /// 识别置信度
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// 是否红方
        /// </summary>
        public bool IsRed { get; set; }
    }

    /// <summary>
    /// 棋子类型枚举
    /// </summary>
    public enum ChessPieceType
    {
        Empty,          // 空
        King,           // 帅/将
        Advisor,        // 仕/士
        Elephant,       // 相/象
        Knight,         // 马
        Rook,           // 车
        Cannon,         // 炮
        Pawn            // 兵/卒
    }

    /// <summary>
    /// FEN生成结果
    /// </summary>
    public class FenGenerationResult
    {
        public bool Success { get; set; }
        public string FEN { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public List<string> Warnings { get; set; } = new();
    }

    /// <summary>
    /// FEN验证结果
    /// </summary>
    public class FenValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
    }

    /// <summary>
    /// FEN生成器选项
    /// </summary>
    public class FenGeneratorOptions
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
}
