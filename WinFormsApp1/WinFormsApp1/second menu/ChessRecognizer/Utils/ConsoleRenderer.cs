using System.Text;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Models;

namespace WinFormsApp1.second_menu.ChessRecognizer.Utils
{
    /// <summary>
    /// 控制台棋盘渲染器
    /// </summary>
    public static class ConsoleRenderer
    {
        /// <summary>
        /// 渲染ASCII棋盘
        /// </summary>
        public static string RenderBoard(BoardPosition board)
        {
            var sb = new StringBuilder();

            // 顶部边框
            sb.AppendLine("  ┌─────────────────┐");
            sb.AppendLine("  │ ａｂｃｄｅｆｇｈｉ │");
            sb.AppendLine("  ├─────────────────┤");

            for (int row = 0; row < 10; row++)
            {
                // 行号
                sb.Append($"{9 - row} │");

                for (int col = 0; col < 9; col++)
                {
                    var piece = board.GetPiece(col, row);
                    string pieceChar = GetPieceChar(piece);
                    sb.Append(pieceChar);
                }

                sb.AppendLine("│");

                // 楚河汉界
                if (row == 4)
                {
                    sb.AppendLine("  │ ═══楚河  汉界═══ │");
                }
            }

            // 底部边框
            sb.AppendLine("  ├─────────────────┤");
            sb.AppendLine("  │ ａｂｃｄｅｆｇｈｉ │");
            sb.AppendLine("  └─────────────────┘");

            return sb.ToString();
        }

        /// <summary>
        /// 渲染简化ASCII棋盘
        /// </summary>
        public static string RenderSimpleBoard(BoardPosition board)
        {
            var sb = new StringBuilder();

            sb.AppendLine("   a b c d e f g h i");
            sb.AppendLine("  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐");

            for (int row = 0; row < 10; row++)
            {
                sb.Append($"{9 - row} │");

                for (int col = 0; col < 9; col++)
                {
                    var piece = board.GetPiece(col, row);
                    string pieceChar = GetSimplePieceChar(piece);
                    sb.Append(pieceChar);
                    if (col < 8) sb.Append("│");
                }

                sb.AppendLine("│");

                if (row < 9)
                {
                    if (row == 4)
                    {
                        sb.AppendLine("  ├─┴─┴─┴─┴─┴─┴─┴─┴─┤");
                        sb.AppendLine("  │   楚 河   汉 界   │");
                        sb.AppendLine("  ├─┬─┬─┬─┬─┬─┬─┬─┬─┤");
                    }
                    else
                    {
                        sb.AppendLine("  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤");
                    }
                }
            }

            sb.AppendLine("  └─┴─┴─┴─┴─┴─┴─┴─┴─┘");
            sb.AppendLine("   a b c d e f g h i");

            return sb.ToString();
        }

        /// <summary>
        /// 渲染Unicode棋盘（带颜色标记）
        /// </summary>
        public static string RenderUnicodeBoard(BoardPosition board)
        {
            var sb = new StringBuilder();

            sb.AppendLine("┌───┬───┬───┬───┬───┬───┬───┬───┬───┐");

            for (int row = 0; row < 10; row++)
            {
                sb.Append("│");

                for (int col = 0; col < 9; col++)
                {
                    var piece = board.GetPiece(col, row);
                    string pieceStr = GetUnicodePieceChar(piece);
                    sb.Append($" {pieceStr} │");
                }

                sb.AppendLine();

                if (row < 9)
                {
                    if (row == 4)
                    {
                        sb.AppendLine("├───┴───┴───┴───┴───┴───┴───┴───┴───┤");
                        sb.AppendLine("│         楚  河    汉  界          │");
                        sb.AppendLine("├───┬───┬───┬───┬───┬───┬───┬───┬───┤");
                    }
                    else
                    {
                        sb.AppendLine("├───┼───┼───┼───┼───┼───┼───┼───┼───┤");
                    }
                }
            }

            sb.AppendLine("└───┴───┴───┴───┴───┴───┴───┴───┴───┘");

            return sb.ToString();
        }

        /// <summary>
        /// 获取棋子字符（全角）
        /// </summary>
        private static string GetPieceChar(ChessPieceModel piece)
        {
            if (piece.Type == PieceType.Empty)
            {
                return "．";
            }

            return piece.GetDisplayText();
        }

        /// <summary>
        /// 获取简化棋子字符
        /// </summary>
        private static string GetSimplePieceChar(ChessPieceModel piece)
        {
            if (piece.Type == PieceType.Empty)
            {
                return "·";
            }

            // 使用单字符表示
            char c = piece.Type switch
            {
                PieceType.King => piece.IsRed ? '帅' : '将',
                PieceType.Advisor => piece.IsRed ? '仕' : '士',
                PieceType.Elephant => piece.IsRed ? '相' : '象',
                PieceType.Knight => piece.IsRed ? '马' : '馬',
                PieceType.Rook => piece.IsRed ? '车' : '車',
                PieceType.Cannon => piece.IsRed ? '炮' : '砲',
                PieceType.Pawn => piece.IsRed ? '兵' : '卒',
                _ => '·'
            };

            return c.ToString();
        }

        /// <summary>
        /// 获取Unicode棋子字符
        /// </summary>
        private static string GetUnicodePieceChar(ChessPieceModel piece)
        {
            if (piece.Type == PieceType.Empty)
            {
                return "  ";
            }

            string text = piece.GetDisplayText();
            string color = piece.IsRed ? "R" : "B";
            return $"{text}";
        }

        /// <summary>
        /// 渲染FEN字符串的可视化
        /// </summary>
        public static string RenderFEN(string fen)
        {
            var board = BoardPosition.FromFEN(fen);
            return RenderSimpleBoard(board);
        }

        /// <summary>
        /// 生成棋盘统计信息
        /// </summary>
        public static string GenerateStatistics(BoardPosition board)
        {
            var sb = new StringBuilder();

            var (red, black) = board.CountPieces();
            sb.AppendLine($"棋子统计：红方 {red} 子，黑方 {black} 子");

            // 详细统计
            var redPieces = new Dictionary<PieceType, int>();
            var blackPieces = new Dictionary<PieceType, int>();

            foreach (var piece in board.GetAllPieces())
            {
                var dict = piece.IsRed ? redPieces : blackPieces;
                if (!dict.ContainsKey(piece.Type))
                {
                    dict[piece.Type] = 0;
                }
                dict[piece.Type]++;
            }

            sb.AppendLine("\n红方棋子：");
            foreach (var kvp in redPieces.OrderBy(x => x.Key))
            {
                string name = GetPieceTypeName(kvp.Key, true);
                sb.AppendLine($"  {name}: {kvp.Value}");
            }

            sb.AppendLine("\n黑方棋子：");
            foreach (var kvp in blackPieces.OrderBy(x => x.Key))
            {
                string name = GetPieceTypeName(kvp.Key, false);
                sb.AppendLine($"  {name}: {kvp.Value}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取棋子类型名称
        /// </summary>
        private static string GetPieceTypeName(PieceType type, bool isRed)
        {
            return type switch
            {
                PieceType.King => isRed ? "帅" : "将",
                PieceType.Advisor => isRed ? "仕" : "士",
                PieceType.Elephant => isRed ? "相" : "象",
                PieceType.Knight => "马",
                PieceType.Rook => "车",
                PieceType.Cannon => "炮",
                PieceType.Pawn => isRed ? "兵" : "卒",
                _ => "未知"
            };
        }
    }
}
