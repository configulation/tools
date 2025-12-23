using System;
using System.Drawing;

namespace WinFormsApp1.first_menu.ChessAnalyzer.Core
{
    /// <summary>
    /// 走法转换工具类
    /// </summary>
    public static class MoveConverter
    {
        /// <summary>
        /// ICCS 格式转坐标
        /// 例如: "h2e2" -> (7,2) to (4,2)
        /// </summary>
        public static (Point from, Point to) ParseICCS(string move)
        {
            if (string.IsNullOrEmpty(move) || move.Length < 4)
                return (Point.Empty, Point.Empty);

            try
            {
                // ICCS 格式: 列用 a-i (0-8), 行用 0-9
                int fromX = move[0] - 'a';
                int fromY = move[1] - '0';
                int toX = move[2] - 'a';
                int toY = move[3] - '0';

                return (new Point(fromX, fromY), new Point(toX, toY));
            }
            catch
            {
                return (Point.Empty, Point.Empty);
            }
        }

        /// <summary>
        /// 坐标转 ICCS 格式
        /// </summary>
        public static string ToICCS(Point from, Point to)
        {
            char fromX = (char)('a' + from.X);
            char fromY = (char)('0' + from.Y);
            char toX = (char)('a' + to.X);
            char toY = (char)('0' + to.Y);

            return $"{fromX}{fromY}{toX}{toY}";
        }

        /// <summary>
        /// ICCS 转中文记谱（完整版）
        /// 例如：h2e2 -> 炮八平五
        /// </summary>
        public static string ToChineseNotation(string move, ChessBoard board)
        {
            if (string.IsNullOrEmpty(move))
                return "";

            var (from, to) = ParseICCS(move);
            if (from == Point.Empty || to == Point.Empty)
                return move;

            var piece = board.GetPiece(from.X, from.Y);
            if (piece == ChessPiece.Empty)
                return move;

            bool isRed = IsRedPiece(piece);
            
            // 1. 棋子名称
            string pieceName = GetChinesePieceName(piece);
            
            // 2. 起始列（红方从右到左九-一，黑方从左到右1-9）
            string fromCol = isRed ? GetRedColumnName(from.X) : GetBlackColumnName(from.X);
            
            // 3. 移动方向和距离
            string direction = GetMoveDirection(from, to);
            string distance = GetMoveDistance(from, to, piece, isRed);
            
            return $"{pieceName}{fromCol}{direction}{distance}";
        }

        /// <summary>
        /// 判断是否为红方棋子
        /// </summary>
        private static bool IsRedPiece(ChessPiece piece)
        {
            return piece >= ChessPiece.RedKing && piece <= ChessPiece.RedPawn;
        }

        /// <summary>
        /// 获取棋子中文名称
        /// </summary>
        private static string GetChinesePieceName(ChessPiece piece)
        {
            return piece switch
            {
                ChessPiece.RedKing => "帅",
                ChessPiece.RedAdvisor => "仕",
                ChessPiece.RedElephant => "相",
                ChessPiece.RedKnight => "马",
                ChessPiece.RedRook => "车",
                ChessPiece.RedCannon => "炮",
                ChessPiece.RedPawn => "兵",
                ChessPiece.BlackKing => "将",
                ChessPiece.BlackAdvisor => "士",
                ChessPiece.BlackElephant => "象",
                ChessPiece.BlackKnight => "马",
                ChessPiece.BlackRook => "车",
                ChessPiece.BlackCannon => "炮",
                ChessPiece.BlackPawn => "卒",
                _ => "?"
            };
        }

        /// <summary>
        /// 获取红方列名（从右到左：九八七六五四三二一）
        /// </summary>
        private static string GetRedColumnName(int col)
        {
            return (9 - col) switch
            {
                9 => "九",
                8 => "八",
                7 => "七",
                6 => "六",
                5 => "五",
                4 => "四",
                3 => "三",
                2 => "二",
                1 => "一",
                _ => col.ToString()
            };
        }

        /// <summary>
        /// 获取黑方列名（从左到右：1-9）
        /// </summary>
        private static string GetBlackColumnName(int col)
        {
            return (col + 1).ToString();
        }

        /// <summary>
        /// 获取移动距离或目标列
        /// </summary>
        private static string GetMoveDistance(Point from, Point to, ChessPiece piece, bool isRed)
        {
            int dx = Math.Abs(to.X - from.X);
            int dy = Math.Abs(to.Y - from.Y);
            
            // 平移：显示目标列
            if (dy == 0)
            {
                return isRed ? GetRedColumnName(to.X) : GetBlackColumnName(to.X);
            }
            
            // 进退：显示步数
            int distance = dy;
            
            // 马的特殊处理（显示目标列）
            if (piece == ChessPiece.RedKnight || piece == ChessPiece.BlackKnight)
            {
                return isRed ? GetRedColumnName(to.X) : GetBlackColumnName(to.X);
            }
            
            // 其他棋子显示步数
            return isRed ? NumberToChinese(distance) : distance.ToString();
        }

        /// <summary>
        /// 数字转中文
        /// </summary>
        private static string NumberToChinese(int num)
        {
            return num switch
            {
                1 => "一",
                2 => "二",
                3 => "三",
                4 => "四",
                5 => "五",
                6 => "六",
                7 => "七",
                8 => "八",
                9 => "九",
                _ => num.ToString()
            };
        }

        /// <summary>
        /// 获取走法方向描述
        /// </summary>
        public static string GetMoveDirection(Point from, Point to)
        {
            int dx = to.X - from.X;
            int dy = to.Y - from.Y;

            if (dx == 0 && dy < 0) return "进";
            if (dx == 0 && dy > 0) return "退";
            if (dx != 0 && dy == 0) return "平";
            if (dy < 0) return "进";
            if (dy > 0) return "退";

            return "";
        }
    }
}
