using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormsApp1.first_menu.ChessAnalyzer.Core
{
    /// <summary>
    /// FEN格式解析器 - 基于 xiangqi-tools 的实现
    /// 参考：https://github.com/kuiba1949/xiangqi-tools
    /// </summary>
    public class FenParser
    {
        /// <summary>
        /// 从FEN字符串解析棋盘布局
        /// </summary>
        /// <param name="fen">FEN格式字符串，例如：rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1</param>
        /// <returns>解析结果</returns>
        public static FenParseResult Parse(string fen)
        {
            var result = new FenParseResult();

            try
            {
                if (string.IsNullOrWhiteSpace(fen))
                {
                    result.Success = false;
                    result.ErrorMessage = "FEN字符串为空";
                    return result;
                }

                // 分离FEN的各个部分
                // 完整格式：rnbakabnr/9/1c5c1/... w - - 0 1
                // 我们只需要第一部分（棋盘布局）
                var parts = fen.Trim().Split(' ');
                var boardFen = parts[0];

                // 按 / 分割成10行
                var rows = boardFen.Split('/');

                if (rows.Length != 10)
                {
                    result.Success = false;
                    result.ErrorMessage = $"FEN格式错误：应该有10行数据，实际有{rows.Length}行";
                    return result;
                }

                result.Board = new ChessPiece[10, 9];

                // 逐行解析（从黑方底线到红方底线，即从第0行到第9行）
                for (int row = 0; row < 10; row++)
                {
                    if (row >= rows.Length)
                    {
                        result.Success = false;
                        result.ErrorMessage = $"FEN数据不足10行";
                        return result;
                    }

                    var rowData = rows[row];
                    int col = 0;

                    // 解析每行的每个字符
                    foreach (char c in rowData)
                    {
                        if (col >= 9)
                        {
                            result.Success = false;
                            result.ErrorMessage = $"第{row}行棋子数超过9个";
                            return result;
                        }

                        // 数字表示空格数量
                        if (char.IsDigit(c))
                        {
                            int emptyCount = c - '0';
                            for (int i = 0; i < emptyCount && col < 9; i++)
                            {
                                result.Board[row, col] = ChessPiece.Empty;
                                col++;
                            }
                        }
                        else
                        {
                            // 字母表示具体棋子
                            var piece = CharToPiece(c);
                            if (piece == ChessPiece.Empty && c != '0' && c != 'x' && c != 'X')
                            {
                                result.Success = false;
                                result.ErrorMessage = $"无法识别的棋子代码: '{c}' (位置：第{row}行第{col}列)";
                                return result;
                            }

                            result.Board[row, col] = piece;
                            col++;
                        }
                    }

                    // 检查每行是否正好9列
                    if (col < 9)
                    {
                        // 补齐空格
                        for (; col < 9; col++)
                        {
                            result.Board[row, col] = ChessPiece.Empty;
                        }
                    }
                }

                // 解析其他信息（如果有）
                if (parts.Length > 1)
                {
                    result.CurrentPlayer = parts[1].ToLower() == "w" ? "红方" : "黑方";
                }
                else
                {
                    result.CurrentPlayer = "红方"; // 默认红方先行
                }

                result.Success = true;
                result.ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"解析FEN时发生错误: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// 将字符转换为棋子
        /// 参考 xiangqi-tools 的映射规则
        /// </summary>
        private static ChessPiece CharToPiece(char c)
        {
            switch (c)
            {
                // 红方（大写）
                case 'K': return ChessPiece.RedKing;      // 帅
                case 'A': return ChessPiece.RedAdvisor;   // 仕
                case 'B': 
                case 'E': return ChessPiece.RedElephant;  // 相
                case 'N': 
                case 'H': return ChessPiece.RedKnight;    // 马
                case 'R': return ChessPiece.RedRook;      // 车
                case 'C': return ChessPiece.RedCannon;    // 炮
                case 'P': return ChessPiece.RedPawn;      // 兵

                // 黑方（小写）
                case 'k': return ChessPiece.BlackKing;    // 将
                case 'a': return ChessPiece.BlackAdvisor; // 士
                case 'b': 
                case 'e': return ChessPiece.BlackElephant; // 象
                case 'n': 
                case 'h': return ChessPiece.BlackKnight;   // 马
                case 'r': return ChessPiece.BlackRook;    // 车
                case 'c': return ChessPiece.BlackCannon;  // 炮
                case 'p': return ChessPiece.BlackPawn;    // 卒

                // 扩展：标记位置（非标准FEN）
                case '0': 
                case 'X': return ChessPiece.Empty;        // 红方标记
                case 'x': return ChessPiece.Empty;        // 黑方标记

                default: return ChessPiece.Empty;
            }
        }

        /// <summary>
        /// 将棋盘转换为FEN字符串
        /// </summary>
        public static string ToFen(ChessPiece[,] board, string currentPlayer = "红方")
        {
            if (board == null || board.GetLength(0) != 10 || board.GetLength(1) != 9)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            // 逐行生成FEN
            for (int row = 0; row < 10; row++)
            {
                int emptyCount = 0;

                for (int col = 0; col < 9; col++)
                {
                    var piece = board[row, col];

                    if (piece == ChessPiece.Empty)
                    {
                        emptyCount++;
                    }
                    else
                    {
                        // 先输出累积的空格数
                        if (emptyCount > 0)
                        {
                            sb.Append(emptyCount);
                            emptyCount = 0;
                        }

                        // 输出棋子代码
                        sb.Append(PieceToChar(piece));
                    }
                }

                // 行末的空格
                if (emptyCount > 0)
                {
                    sb.Append(emptyCount);
                }

                // 行分隔符（最后一行不加）
                if (row < 9)
                {
                    sb.Append('/');
                }
            }

            // 添加当前行棋方（w=红方，b=黑方）
            sb.Append(' ');
            sb.Append(currentPlayer == "红方" ? "w" : "b");

            // 添加其他标准FEN字段（中国象棋通常用 - - 0 1）
            sb.Append(" - - 0 1");

            return sb.ToString();
        }

        /// <summary>
        /// 将棋子转换为FEN字符
        /// </summary>
        private static char PieceToChar(ChessPiece piece)
        {
            switch (piece)
            {
                case ChessPiece.RedKing: return 'K';
                case ChessPiece.RedAdvisor: return 'A';
                case ChessPiece.RedElephant: return 'B';
                case ChessPiece.RedKnight: return 'N';
                case ChessPiece.RedRook: return 'R';
                case ChessPiece.RedCannon: return 'C';
                case ChessPiece.RedPawn: return 'P';

                case ChessPiece.BlackKing: return 'k';
                case ChessPiece.BlackAdvisor: return 'a';
                case ChessPiece.BlackElephant: return 'b';
                case ChessPiece.BlackKnight: return 'n';
                case ChessPiece.BlackRook: return 'r';
                case ChessPiece.BlackCannon: return 'c';
                case ChessPiece.BlackPawn: return 'p';

                default: return '1';
            }
        }

        /// <summary>
        /// 生成文本棋盘（类似 xiangqi-tools 的输出）
        /// </summary>
        public static string ToTextBoard(ChessPiece[,] board, bool showRowNumbers = true)
        {
            if (board == null || board.GetLength(0) != 10 || board.GetLength(1) != 9)
            {
                return "棋盘数据无效";
            }

            var sb = new StringBuilder();

            // 标题行
            sb.AppendLine("　１　２　３　４　５　６　７　８　９");
            sb.AppendLine("　 ==　==　==　##　##　##　==　==　==");
            sb.AppendLine("　　　　　　　　　黑方\n");

            // 逐行输出（行号从9到0）
            for (int row = 0; row < 10; row++)
            {
                sb.Append("　");

                for (int col = 0; col < 9; col++)
                {
                    var piece = board[row, col];
                    sb.Append(PieceToText(piece));
                }

                // 行号
                if (showRowNumbers)
                {
                    int displayRow = 9 - row;
                    // 河界用 + 代替数字
                    if (displayRow == 4 || displayRow == 5)
                    {
                        sb.Append("  +");
                    }
                    else
                    {
                        sb.Append($"  {displayRow}");
                    }
                }

                sb.AppendLine();
            }

            // 底部分隔
            sb.AppendLine("\n　　　　　　　　　红方");
            sb.AppendLine("　 ==　==　==　##　##　##　==　==　==");
            sb.AppendLine("　１　２　３　４　５　６　７　８　９");

            return sb.ToString();
        }

        /// <summary>
        /// 将棋子转换为文本显示
        /// </summary>
        private static string PieceToText(ChessPiece piece)
        {
            switch (piece)
            {
                case ChessPiece.RedKing: return "(帅)";
                case ChessPiece.RedAdvisor: return "(仕)";
                case ChessPiece.RedElephant: return "(相)";
                case ChessPiece.RedKnight: return "(马)";
                case ChessPiece.RedRook: return "(车)";
                case ChessPiece.RedCannon: return "(炮)";
                case ChessPiece.RedPawn: return "(兵)";

                case ChessPiece.BlackKing: return "[将]";
                case ChessPiece.BlackAdvisor: return "[士]";
                case ChessPiece.BlackElephant: return "[象]";
                case ChessPiece.BlackKnight: return "[马]";
                case ChessPiece.BlackRook: return "[车]";
                case ChessPiece.BlackCannon: return "[炮]";
                case ChessPiece.BlackPawn: return "[卒]";

                default: return " － ";
            }
        }
    }

    /// <summary>
    /// FEN解析结果
    /// </summary>
    public class FenParseResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public ChessPiece[,] Board { get; set; }
        public string CurrentPlayer { get; set; }
    }
}
