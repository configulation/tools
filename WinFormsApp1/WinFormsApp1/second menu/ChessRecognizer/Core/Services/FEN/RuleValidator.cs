using WinFormsApp1.second_menu.ChessRecognizer.Core.Models;

namespace WinFormsApp1.second_menu.ChessRecognizer.Core.Services.FEN
{
    /// <summary>
    /// 象棋规则验证器
    /// </summary>
    public class RuleValidator
    {
        /// <summary>
        /// 验证棋盘状态是否合法
        /// </summary>
        public ValidationResult ValidateBoard(BoardPosition board)
        {
            var result = new ValidationResult { IsValid = true };

            // 1. 检查帅将数量
            var (redKing, blackKing) = CountKings(board);
            if (redKing != 1)
            {
                result.IsValid = false;
                result.Errors.Add($"红帅数量错误：{redKing}（应为1）");
            }
            if (blackKing != 1)
            {
                result.IsValid = false;
                result.Errors.Add($"黑将数量错误：{blackKing}（应为1）");
            }

            // 2. 检查帅将位置
            var kingPositions = ValidateKingPositions(board);
            result.Errors.AddRange(kingPositions.Errors);
            result.Warnings.AddRange(kingPositions.Warnings);

            // 3. 检查仕士位置
            var advisorPositions = ValidateAdvisorPositions(board);
            result.Warnings.AddRange(advisorPositions.Warnings);

            // 4. 检查相象位置
            var elephantPositions = ValidateElephantPositions(board);
            result.Warnings.AddRange(elephantPositions.Warnings);

            // 5. 检查兵卒位置
            var pawnPositions = ValidatePawnPositions(board);
            result.Warnings.AddRange(pawnPositions.Warnings);

            // 6. 检查棋子总数
            var pieceCount = ValidatePieceCount(board);
            result.Warnings.AddRange(pieceCount.Warnings);

            // 7. 检查帅将对脸
            if (IsKingsFacing(board))
            {
                result.Warnings.Add("帅将对脸（可能是将军状态）");
            }

            return result;
        }

        /// <summary>
        /// 统计帅将数量
        /// </summary>
        private (int red, int black) CountKings(BoardPosition board)
        {
            int red = 0, black = 0;
            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 10; row++)
                {
                    var piece = board.GetPiece(col, row);
                    if (piece.Type == PieceType.King)
                    {
                        if (piece.IsRed) red++;
                        else black++;
                    }
                }
            }
            return (red, black);
        }

        /// <summary>
        /// 验证帅将位置
        /// </summary>
        private ValidationResult ValidateKingPositions(BoardPosition board)
        {
            var result = new ValidationResult { IsValid = true };

            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 10; row++)
                {
                    var piece = board.GetPiece(col, row);
                    if (piece.Type == PieceType.King)
                    {
                        // 帅将只能在九宫格内
                        bool inPalace = col >= 3 && col <= 5;
                        if (piece.IsRed)
                        {
                            inPalace = inPalace && row >= 7 && row <= 9;
                        }
                        else
                        {
                            inPalace = inPalace && row >= 0 && row <= 2;
                        }

                        if (!inPalace)
                        {
                            result.Errors.Add($"{(piece.IsRed ? "红帅" : "黑将")}位置错误：({col},{row})不在九宫格内");
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 验证仕士位置
        /// </summary>
        private ValidationResult ValidateAdvisorPositions(BoardPosition board)
        {
            var result = new ValidationResult { IsValid = true };

            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 10; row++)
                {
                    var piece = board.GetPiece(col, row);
                    if (piece.Type == PieceType.Advisor)
                    {
                        bool inPalace = col >= 3 && col <= 5;
                        if (piece.IsRed)
                        {
                            inPalace = inPalace && row >= 7 && row <= 9;
                        }
                        else
                        {
                            inPalace = inPalace && row >= 0 && row <= 2;
                        }

                        if (!inPalace)
                        {
                            result.Warnings.Add($"{(piece.IsRed ? "红仕" : "黑士")}位置异常：({col},{row})不在九宫格内");
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 验证相象位置
        /// </summary>
        private ValidationResult ValidateElephantPositions(BoardPosition board)
        {
            var result = new ValidationResult { IsValid = true };

            // 相象只能在己方半场
            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 10; row++)
                {
                    var piece = board.GetPiece(col, row);
                    if (piece.Type == PieceType.Elephant)
                    {
                        bool inOwnHalf;
                        if (piece.IsRed)
                        {
                            inOwnHalf = row >= 5;
                        }
                        else
                        {
                            inOwnHalf = row <= 4;
                        }

                        if (!inOwnHalf)
                        {
                            result.Warnings.Add($"{(piece.IsRed ? "红相" : "黑象")}位置异常：({col},{row})过河了");
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 验证兵卒位置
        /// </summary>
        private ValidationResult ValidatePawnPositions(BoardPosition board)
        {
            var result = new ValidationResult { IsValid = true };

            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 10; row++)
                {
                    var piece = board.GetPiece(col, row);
                    if (piece.Type == PieceType.Pawn)
                    {
                        // 兵卒不能后退到底线
                        if (piece.IsRed && row == 9)
                        {
                            result.Warnings.Add($"红兵位置异常：({col},{row})在底线");
                        }
                        else if (!piece.IsRed && row == 0)
                        {
                            result.Warnings.Add($"黑卒位置异常：({col},{row})在底线");
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 验证棋子数量
        /// </summary>
        private ValidationResult ValidatePieceCount(BoardPosition board)
        {
            var result = new ValidationResult { IsValid = true };

            var counts = new Dictionary<string, int>
            {
                ["红仕"] = 0, ["黑士"] = 0,
                ["红相"] = 0, ["黑象"] = 0,
                ["红马"] = 0, ["黑马"] = 0,
                ["红车"] = 0, ["黑车"] = 0,
                ["红炮"] = 0, ["黑炮"] = 0,
                ["红兵"] = 0, ["黑卒"] = 0,
            };

            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 10; row++)
                {
                    var piece = board.GetPiece(col, row);
                    if (piece.Type == PieceType.Empty) continue;

                    string key = piece.Type switch
                    {
                        PieceType.Advisor => piece.IsRed ? "红仕" : "黑士",
                        PieceType.Elephant => piece.IsRed ? "红相" : "黑象",
                        PieceType.Knight => piece.IsRed ? "红马" : "黑马",
                        PieceType.Rook => piece.IsRed ? "红车" : "黑车",
                        PieceType.Cannon => piece.IsRed ? "红炮" : "黑炮",
                        PieceType.Pawn => piece.IsRed ? "红兵" : "黑卒",
                        _ => null
                    };

                    if (key != null)
                    {
                        counts[key]++;
                    }
                }
            }

            // 检查数量限制
            var limits = new Dictionary<string, int>
            {
                ["红仕"] = 2, ["黑士"] = 2,
                ["红相"] = 2, ["黑象"] = 2,
                ["红马"] = 2, ["黑马"] = 2,
                ["红车"] = 2, ["黑车"] = 2,
                ["红炮"] = 2, ["黑炮"] = 2,
                ["红兵"] = 5, ["黑卒"] = 5,
            };

            foreach (var kvp in counts)
            {
                if (kvp.Value > limits[kvp.Key])
                {
                    result.Warnings.Add($"{kvp.Key}数量异常：{kvp.Value}（最多{limits[kvp.Key]}）");
                }
            }

            return result;
        }

        /// <summary>
        /// 检查帅将是否对脸
        /// </summary>
        private bool IsKingsFacing(BoardPosition board)
        {
            // 找到帅将位置
            int redKingCol = -1, redKingRow = -1;
            int blackKingCol = -1, blackKingRow = -1;

            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 10; row++)
                {
                    var piece = board.GetPiece(col, row);
                    if (piece.Type == PieceType.King)
                    {
                        if (piece.IsRed)
                        {
                            redKingCol = col;
                            redKingRow = row;
                        }
                        else
                        {
                            blackKingCol = col;
                            blackKingRow = row;
                        }
                    }
                }
            }

            // 如果不在同一列，不对脸
            if (redKingCol != blackKingCol || redKingCol < 0 || blackKingCol < 0)
            {
                return false;
            }

            // 检查中间是否有棋子
            int minRow = Math.Min(redKingRow, blackKingRow);
            int maxRow = Math.Max(redKingRow, blackKingRow);

            for (int row = minRow + 1; row < maxRow; row++)
            {
                if (board.GetPiece(redKingCol, row).Type != PieceType.Empty)
                {
                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// 验证结果
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
    }
}
