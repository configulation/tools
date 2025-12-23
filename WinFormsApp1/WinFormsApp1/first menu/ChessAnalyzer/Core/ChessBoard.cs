using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormsApp1.first_menu.ChessAnalyzer.Core
{
    /// <summary>
    /// 象棋棋盘类
    /// </summary>
    public class ChessBoard
    {
        // 棋盘状态 [列x, 行y]，9列10行
        public ChessPiece[,] Board { get; private set; }
        
        // 红方走棋
        public bool IsRedTurn { get; set; }

        public ChessBoard()
        {
            Board = new ChessPiece[9, 10];
            InitializeStartPosition();
        }

        /// <summary>
        /// 初始化开局局面
        /// </summary>
        public void InitializeStartPosition()
        {
            // 清空棋盘
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Board[x, y] = ChessPiece.Empty;
                }
            }

            // 黑方（上方，第0-4行）
            Board[0, 0] = ChessPiece.BlackRook;    // 車
            Board[1, 0] = ChessPiece.BlackKnight;  // 馬
            Board[2, 0] = ChessPiece.BlackElephant;// 象
            Board[3, 0] = ChessPiece.BlackAdvisor; // 士
            Board[4, 0] = ChessPiece.BlackKing;    // 將
            Board[5, 0] = ChessPiece.BlackAdvisor; // 士
            Board[6, 0] = ChessPiece.BlackElephant;// 象
            Board[7, 0] = ChessPiece.BlackKnight;  // 馬
            Board[8, 0] = ChessPiece.BlackRook;    // 車

            Board[1, 2] = ChessPiece.BlackCannon;  // 炮
            Board[7, 2] = ChessPiece.BlackCannon;  // 炮

            Board[0, 3] = ChessPiece.BlackPawn;    // 卒
            Board[2, 3] = ChessPiece.BlackPawn;
            Board[4, 3] = ChessPiece.BlackPawn;
            Board[6, 3] = ChessPiece.BlackPawn;
            Board[8, 3] = ChessPiece.BlackPawn;

            // 红方（下方，第5-9行）
            Board[0, 6] = ChessPiece.RedPawn;      // 兵
            Board[2, 6] = ChessPiece.RedPawn;
            Board[4, 6] = ChessPiece.RedPawn;
            Board[6, 6] = ChessPiece.RedPawn;
            Board[8, 6] = ChessPiece.RedPawn;

            Board[1, 7] = ChessPiece.RedCannon;    // 炮
            Board[7, 7] = ChessPiece.RedCannon;    // 炮

            Board[0, 9] = ChessPiece.RedRook;      // 車
            Board[1, 9] = ChessPiece.RedKnight;    // 馬
            Board[2, 9] = ChessPiece.RedElephant;  // 相
            Board[3, 9] = ChessPiece.RedAdvisor;   // 仕
            Board[4, 9] = ChessPiece.RedKing;      // 帥
            Board[5, 9] = ChessPiece.RedAdvisor;   // 仕
            Board[6, 9] = ChessPiece.RedElephant;  // 相
            Board[7, 9] = ChessPiece.RedKnight;    // 馬
            Board[8, 9] = ChessPiece.RedRook;      // 車

            IsRedTurn = true;
        }

        /// <summary>
        /// 从 FEN 字符串加载局面（使用改进的解析器）
        /// </summary>
        public void LoadFromFEN(string fen)
        {
            // 使用专门的FenParser进行解析
            var result = FenParser.Parse(fen);
            
            if (!result.Success)
            {
                // 如果解析失败，记录错误但不抛出异常
                System.Diagnostics.Debug.WriteLine($"FEN解析失败: {result.ErrorMessage}");
                return;
            }

            // 将解析结果转换为当前棋盘格式 [x, y]
            // FenParser返回的是 [row, col] 格式，需要转置
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Board[col, row] = result.Board[row, col];
                }
            }

            // 设置当前走棋方
            IsRedTurn = result.CurrentPlayer == "红方";
        }

        /// <summary>
        /// 转换为 FEN 字符串
        /// </summary>
        public string ToFEN()
        {
            var sb = new StringBuilder();

            // 构建棋盘部分
            for (int y = 0; y < 10; y++)
            {
                int emptyCount = 0;
                for (int x = 0; x < 9; x++)
                {
                    var piece = Board[x, y];
                    if (piece == ChessPiece.Empty)
                    {
                        emptyCount++;
                    }
                    else
                    {
                        if (emptyCount > 0)
                        {
                            sb.Append(emptyCount);
                            emptyCount = 0;
                        }
                        sb.Append(PieceToChar(piece));
                    }
                }

                if (emptyCount > 0)
                {
                    sb.Append(emptyCount);
                }

                if (y < 9)
                {
                    sb.Append('/');
                }
            }

            // 添加走棋方
            sb.Append(IsRedTurn ? " w" : " b");

            // 添加其他默认信息
            sb.Append(" - - 0 1");

            return sb.ToString();
        }

        /// <summary>
        /// 字符转棋子
        /// </summary>
        private ChessPiece CharToPiece(char c)
        {
            return c switch
            {
                'R' => ChessPiece.RedRook,
                'N' => ChessPiece.RedKnight,
                'B' => ChessPiece.RedElephant,
                'A' => ChessPiece.RedAdvisor,
                'K' => ChessPiece.RedKing,
                'C' => ChessPiece.RedCannon,
                'P' => ChessPiece.RedPawn,
                'r' => ChessPiece.BlackRook,
                'n' => ChessPiece.BlackKnight,
                'b' => ChessPiece.BlackElephant,
                'a' => ChessPiece.BlackAdvisor,
                'k' => ChessPiece.BlackKing,
                'c' => ChessPiece.BlackCannon,
                'p' => ChessPiece.BlackPawn,
                _ => ChessPiece.Empty
            };
        }

        /// <summary>
        /// 棋子转字符
        /// </summary>
        private char PieceToChar(ChessPiece piece)
        {
            return piece switch
            {
                ChessPiece.RedRook => 'R',
                ChessPiece.RedKnight => 'N',
                ChessPiece.RedElephant => 'B',
                ChessPiece.RedAdvisor => 'A',
                ChessPiece.RedKing => 'K',
                ChessPiece.RedCannon => 'C',
                ChessPiece.RedPawn => 'P',
                ChessPiece.BlackRook => 'r',
                ChessPiece.BlackKnight => 'n',
                ChessPiece.BlackElephant => 'b',
                ChessPiece.BlackAdvisor => 'a',
                ChessPiece.BlackKing => 'k',
                ChessPiece.BlackCannon => 'c',
                ChessPiece.BlackPawn => 'p',
                _ => '1'
            };
        }

        /// <summary>
        /// 获取指定位置的棋子
        /// </summary>
        public ChessPiece GetPiece(int x, int y)
        {
            if (x < 0 || x >= 9 || y < 0 || y >= 10)
                return ChessPiece.Empty;
            return Board[x, y];
        }

        /// <summary>
        /// 设置指定位置的棋子
        /// </summary>
        public void SetPiece(int x, int y, ChessPiece piece)
        {
            if (x >= 0 && x < 9 && y >= 0 && y < 10)
            {
                Board[x, y] = piece;
            }
        }

        /// <summary>
        /// 复制棋盘
        /// </summary>
        public ChessBoard Clone()
        {
            var newBoard = new ChessBoard();
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    newBoard.Board[x, y] = this.Board[x, y];
                }
            }
            newBoard.IsRedTurn = this.IsRedTurn;
            return newBoard;
        }
    }

    /// <summary>
    /// 棋子类型枚举
    /// </summary>
    public enum ChessPiece
    {
        Empty,          // 空
        RedKing,        // 帅
        RedAdvisor,     // 仕
        RedElephant,    // 相
        RedKnight,      // 马
        RedRook,        // 车
        RedCannon,      // 炮
        RedPawn,        // 兵
        BlackKing,      // 将
        BlackAdvisor,   // 士
        BlackElephant,  // 象
        BlackKnight,    // 马
        BlackRook,      // 车
        BlackCannon,    // 炮
        BlackPawn       // 卒
    }

    /// <summary>
    /// 棋子工具类
    /// </summary>
    public static class ChessPieceHelper
    {
        /// <summary>
        /// 获取棋子显示文本
        /// </summary>
        public static string GetPieceText(ChessPiece piece)
        {
            return piece switch
            {
                ChessPiece.RedKing => "帅",
                ChessPiece.RedAdvisor => "仕",
                ChessPiece.RedElephant => "相",
                ChessPiece.RedKnight => "馬",
                ChessPiece.RedRook => "車",
                ChessPiece.RedCannon => "炮",
                ChessPiece.RedPawn => "兵",
                ChessPiece.BlackKing => "将",
                ChessPiece.BlackAdvisor => "士",
                ChessPiece.BlackElephant => "象",
                ChessPiece.BlackKnight => "马",
                ChessPiece.BlackRook => "车",
                ChessPiece.BlackCannon => "砲",
                ChessPiece.BlackPawn => "卒",
                _ => ""
            };
        }

        /// <summary>
        /// 判断是否红方棋子
        /// </summary>
        public static bool IsRedPiece(ChessPiece piece)
        {
            return piece >= ChessPiece.RedKing && piece <= ChessPiece.RedPawn;
        }

        /// <summary>
        /// 判断是否黑方棋子
        /// </summary>
        public static bool IsBlackPiece(ChessPiece piece)
        {
            return piece >= ChessPiece.BlackKing && piece <= ChessPiece.BlackPawn;
        }
    }
}
