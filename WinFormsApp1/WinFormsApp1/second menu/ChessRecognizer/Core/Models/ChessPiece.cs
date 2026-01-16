namespace WinFormsApp1.second_menu.ChessRecognizer.Core.Models
{
    /// <summary>
    /// 棋子模型
    /// </summary>
    public class ChessPieceModel
    {
        /// <summary>
        /// 棋子类型
        /// </summary>
        public PieceType Type { get; set; }

        /// <summary>
        /// 是否红方
        /// </summary>
        public bool IsRed { get; set; }

        /// <summary>
        /// 棋盘列位置 (0-8)
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// 棋盘行位置 (0-9)
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 识别置信度
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// 识别的原始文字
        /// </summary>
        public string RecognizedText { get; set; } = string.Empty;

        /// <summary>
        /// 获取FEN字符
        /// </summary>
        public char GetFenChar()
        {
            char c = Type switch
            {
                PieceType.King => 'k',
                PieceType.Advisor => 'a',
                PieceType.Elephant => 'b',
                PieceType.Knight => 'n',
                PieceType.Rook => 'r',
                PieceType.Cannon => 'c',
                PieceType.Pawn => 'p',
                _ => ' '
            };
            return IsRed ? char.ToUpper(c) : c;
        }

        /// <summary>
        /// 获取显示文字
        /// </summary>
        public string GetDisplayText()
        {
            if (IsRed)
            {
                return Type switch
                {
                    PieceType.King => "帅",
                    PieceType.Advisor => "仕",
                    PieceType.Elephant => "相",
                    PieceType.Knight => "馬",
                    PieceType.Rook => "車",
                    PieceType.Cannon => "炮",
                    PieceType.Pawn => "兵",
                    _ => ""
                };
            }
            else
            {
                return Type switch
                {
                    PieceType.King => "将",
                    PieceType.Advisor => "士",
                    PieceType.Elephant => "象",
                    PieceType.Knight => "马",
                    PieceType.Rook => "车",
                    PieceType.Cannon => "砲",
                    PieceType.Pawn => "卒",
                    _ => ""
                };
            }
        }

        /// <summary>
        /// 从汉字创建棋子
        /// </summary>
        public static ChessPieceModel FromChinese(string text, int col, int row)
        {
            var piece = new ChessPieceModel { Column = col, Row = row, RecognizedText = text };

            switch (text)
            {
                // 红方
                case "帅": piece.Type = PieceType.King; piece.IsRed = true; break;
                case "仕": piece.Type = PieceType.Advisor; piece.IsRed = true; break;
                case "相": piece.Type = PieceType.Elephant; piece.IsRed = true; break;
                case "馬": case "马": piece.Type = PieceType.Knight; piece.IsRed = true; break;
                case "車": case "车": piece.Type = PieceType.Rook; piece.IsRed = true; break;
                case "炮": piece.Type = PieceType.Cannon; piece.IsRed = true; break;
                case "兵": piece.Type = PieceType.Pawn; piece.IsRed = true; break;

                // 黑方
                case "将": piece.Type = PieceType.King; piece.IsRed = false; break;
                case "士": piece.Type = PieceType.Advisor; piece.IsRed = false; break;
                case "象": piece.Type = PieceType.Elephant; piece.IsRed = false; break;
                case "俥": piece.Type = PieceType.Rook; piece.IsRed = false; break;
                case "砲": piece.Type = PieceType.Cannon; piece.IsRed = false; break;
                case "卒": piece.Type = PieceType.Pawn; piece.IsRed = false; break;

                default:
                    piece.Type = PieceType.Empty;
                    break;
            }

            return piece;
        }
    }

    /// <summary>
    /// 棋子类型
    /// </summary>
    public enum PieceType
    {
        Empty,
        King,       // 帅/将
        Advisor,    // 仕/士
        Elephant,   // 相/象
        Knight,     // 马
        Rook,       // 车
        Cannon,     // 炮
        Pawn        // 兵/卒
    }
}
