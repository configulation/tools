namespace WinFormsApp1.second_menu.ChessRecognizer.Core.Models
{
    /// <summary>
    /// 棋盘位置模型
    /// </summary>
    public class BoardPosition
    {
        /// <summary>
        /// 棋盘状态 [列0-8, 行0-9]
        /// </summary>
        public ChessPieceModel[,] Board { get; private set; }

        /// <summary>
        /// 当前走棋方（true=红方）
        /// </summary>
        public bool IsRedTurn { get; set; } = true;

        /// <summary>
        /// 回合数
        /// </summary>
        public int MoveNumber { get; set; } = 1;

        public BoardPosition()
        {
            Board = new ChessPieceModel[9, 10];
            Clear();
        }

        /// <summary>
        /// 清空棋盘
        /// </summary>
        public void Clear()
        {
            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 10; row++)
                {
                    Board[col, row] = new ChessPieceModel
                    {
                        Type = PieceType.Empty,
                        Column = col,
                        Row = row
                    };
                }
            }
        }

        /// <summary>
        /// 设置棋子
        /// </summary>
        public void SetPiece(int col, int row, ChessPieceModel piece)
        {
            if (col >= 0 && col < 9 && row >= 0 && row < 10)
            {
                piece.Column = col;
                piece.Row = row;
                Board[col, row] = piece;
            }
        }

        /// <summary>
        /// 获取棋子
        /// </summary>
        public ChessPieceModel GetPiece(int col, int row)
        {
            if (col >= 0 && col < 9 && row >= 0 && row < 10)
            {
                return Board[col, row];
            }
            return new ChessPieceModel { Type = PieceType.Empty };
        }

        /// <summary>
        /// 获取所有棋子
        /// </summary>
        public List<ChessPieceModel> GetAllPieces()
        {
            var pieces = new List<ChessPieceModel>();
            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 10; row++)
                {
                    if (Board[col, row].Type != PieceType.Empty)
                    {
                        pieces.Add(Board[col, row]);
                    }
                }
            }
            return pieces;
        }

        /// <summary>
        /// 统计棋子数量
        /// </summary>
        public (int red, int black) CountPieces()
        {
            int red = 0, black = 0;
            foreach (var piece in GetAllPieces())
            {
                if (piece.IsRed) red++;
                else black++;
            }
            return (red, black);
        }

        /// <summary>
        /// 转换为FEN字符串
        /// </summary>
        public string ToFEN()
        {
            var sb = new System.Text.StringBuilder();

            for (int row = 0; row < 10; row++)
            {
                int emptyCount = 0;
                for (int col = 0; col < 9; col++)
                {
                    var piece = Board[col, row];
                    if (piece.Type == PieceType.Empty)
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
                        sb.Append(piece.GetFenChar());
                    }
                }

                if (emptyCount > 0)
                {
                    sb.Append(emptyCount);
                }

                if (row < 9)
                {
                    sb.Append('/');
                }
            }

            sb.Append(IsRedTurn ? " w" : " b");
            sb.Append(" - - 0 ");
            sb.Append(MoveNumber);

            return sb.ToString();
        }

        /// <summary>
        /// 从FEN字符串加载
        /// </summary>
        public static BoardPosition FromFEN(string fen)
        {
            var position = new BoardPosition();
            
            if (string.IsNullOrWhiteSpace(fen))
                return position;

            var parts = fen.Split(' ');
            var boardPart = parts[0];
            var rows = boardPart.Split('/');

            for (int row = 0; row < Math.Min(rows.Length, 10); row++)
            {
                int col = 0;
                foreach (char c in rows[row])
                {
                    if (char.IsDigit(c))
                    {
                        col += c - '0';
                    }
                    else if (col < 9)
                    {
                        var piece = CharToPiece(c, col, row);
                        position.SetPiece(col, row, piece);
                        col++;
                    }
                }
            }

            if (parts.Length > 1)
            {
                position.IsRedTurn = parts[1].ToLower() == "w";
            }

            if (parts.Length > 5 && int.TryParse(parts[5], out int moveNum))
            {
                position.MoveNumber = moveNum;
            }

            return position;
        }

        private static ChessPieceModel CharToPiece(char c, int col, int row)
        {
            bool isRed = char.IsUpper(c);
            var type = char.ToLower(c) switch
            {
                'k' => PieceType.King,
                'a' => PieceType.Advisor,
                'b' => PieceType.Elephant,
                'n' => PieceType.Knight,
                'r' => PieceType.Rook,
                'c' => PieceType.Cannon,
                'p' => PieceType.Pawn,
                _ => PieceType.Empty
            };

            return new ChessPieceModel
            {
                Type = type,
                IsRed = isRed,
                Column = col,
                Row = row
            };
        }

        /// <summary>
        /// 克隆棋盘
        /// </summary>
        public BoardPosition Clone()
        {
            var clone = new BoardPosition
            {
                IsRedTurn = this.IsRedTurn,
                MoveNumber = this.MoveNumber
            };

            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 10; row++)
                {
                    var src = Board[col, row];
                    clone.Board[col, row] = new ChessPieceModel
                    {
                        Type = src.Type,
                        IsRed = src.IsRed,
                        Column = col,
                        Row = row,
                        Confidence = src.Confidence,
                        RecognizedText = src.RecognizedText
                    };
                }
            }

            return clone;
        }
    }
}
