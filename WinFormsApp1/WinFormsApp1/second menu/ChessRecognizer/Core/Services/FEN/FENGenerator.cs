using System.Text;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Interfaces;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Models;

namespace WinFormsApp1.second_menu.ChessRecognizer.Core.Services.FEN
{
    /// <summary>
    /// FEN字符串生成器
    /// </summary>
    public class FENGenerator : IFENGenerator
    {
        private FenGeneratorOptions _options;

        public FENGenerator()
        {
            _options = new FenGeneratorOptions();
        }

        /// <summary>
        /// 从识别结果生成FEN
        /// </summary>
        public FenGenerationResult GenerateFEN(RecognizedBoard board)
        {
            var result = new FenGenerationResult();

            try
            {
                var sb = new StringBuilder();

                // 生成棋盘部分
                for (int row = 0; row < 10; row++)
                {
                    int emptyCount = 0;
                    for (int col = 0; col < 9; col++)
                    {
                        var piece = board.Pieces[col, row];
                        if (piece == null || piece.PieceType == ChessPieceType.Empty)
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
                            sb.Append(GetFenChar(piece));
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

                // 添加走棋方
                sb.Append(board.IsRedTurn ? " w" : " b");

                // 添加其他信息
                sb.Append(" - - 0 1");

                result.FEN = sb.ToString();
                result.Success = true;

                // 验证
                if (_options.EnableRuleValidation)
                {
                    var validation = ValidateFEN(result.FEN);
                    result.Warnings.AddRange(validation.Warnings);
                    if (!validation.IsValid)
                    {
                        result.Warnings.AddRange(validation.Errors);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取FEN字符
        /// </summary>
        private char GetFenChar(RecognizedPiece piece)
        {
            char c = piece.PieceType switch
            {
                ChessPieceType.King => 'k',
                ChessPieceType.Advisor => 'a',
                ChessPieceType.Elephant => 'b',
                ChessPieceType.Knight => 'n',
                ChessPieceType.Rook => 'r',
                ChessPieceType.Cannon => 'c',
                ChessPieceType.Pawn => 'p',
                _ => ' '
            };

            return piece.IsRed ? char.ToUpper(c) : c;
        }

        /// <summary>
        /// 验证FEN字符串
        /// </summary>
        public FenValidationResult ValidateFEN(string fen)
        {
            var result = new FenValidationResult { IsValid = true };

            if (string.IsNullOrWhiteSpace(fen))
            {
                result.IsValid = false;
                result.Errors.Add("FEN字符串为空");
                return result;
            }

            var parts = fen.Split(' ');
            if (parts.Length < 1)
            {
                result.IsValid = false;
                result.Errors.Add("FEN格式错误");
                return result;
            }

            var boardPart = parts[0];
            var rows = boardPart.Split('/');

            if (rows.Length != 10)
            {
                result.IsValid = false;
                result.Errors.Add($"棋盘行数错误：期望10行，实际{rows.Length}行");
                return result;
            }

            // 统计棋子
            int redKing = 0, blackKing = 0;
            int redAdvisor = 0, blackAdvisor = 0;
            int redElephant = 0, blackElephant = 0;
            int redKnight = 0, blackKnight = 0;
            int redRook = 0, blackRook = 0;
            int redCannon = 0, blackCannon = 0;
            int redPawn = 0, blackPawn = 0;

            for (int row = 0; row < 10; row++)
            {
                int colCount = 0;
                foreach (char c in rows[row])
                {
                    if (char.IsDigit(c))
                    {
                        colCount += c - '0';
                    }
                    else
                    {
                        colCount++;
                        switch (c)
                        {
                            case 'K': redKing++; break;
                            case 'k': blackKing++; break;
                            case 'A': redAdvisor++; break;
                            case 'a': blackAdvisor++; break;
                            case 'B': redElephant++; break;
                            case 'b': blackElephant++; break;
                            case 'N': redKnight++; break;
                            case 'n': blackKnight++; break;
                            case 'R': redRook++; break;
                            case 'r': blackRook++; break;
                            case 'C': redCannon++; break;
                            case 'c': blackCannon++; break;
                            case 'P': redPawn++; break;
                            case 'p': blackPawn++; break;
                            default:
                                result.Warnings.Add($"未知棋子字符: {c}");
                                break;
                        }
                    }
                }

                if (colCount != 9)
                {
                    result.IsValid = false;
                    result.Errors.Add($"第{row + 1}行列数错误：期望9列，实际{colCount}列");
                }
            }

            // 验证棋子数量
            if (redKing != 1)
            {
                result.Warnings.Add($"红帅数量异常：{redKing}（应为1）");
            }
            if (blackKing != 1)
            {
                result.Warnings.Add($"黑将数量异常：{blackKing}（应为1）");
            }
            if (redAdvisor > 2)
            {
                result.Warnings.Add($"红仕数量异常：{redAdvisor}（最多2）");
            }
            if (blackAdvisor > 2)
            {
                result.Warnings.Add($"黑士数量异常：{blackAdvisor}（最多2）");
            }
            if (redElephant > 2)
            {
                result.Warnings.Add($"红相数量异常：{redElephant}（最多2）");
            }
            if (blackElephant > 2)
            {
                result.Warnings.Add($"黑象数量异常：{blackElephant}（最多2）");
            }
            if (redKnight > 2)
            {
                result.Warnings.Add($"红马数量异常：{redKnight}（最多2）");
            }
            if (blackKnight > 2)
            {
                result.Warnings.Add($"黑马数量异常：{blackKnight}（最多2）");
            }
            if (redRook > 2)
            {
                result.Warnings.Add($"红车数量异常：{redRook}（最多2）");
            }
            if (blackRook > 2)
            {
                result.Warnings.Add($"黑车数量异常：{blackRook}（最多2）");
            }
            if (redCannon > 2)
            {
                result.Warnings.Add($"红炮数量异常：{redCannon}（最多2）");
            }
            if (blackCannon > 2)
            {
                result.Warnings.Add($"黑炮数量异常：{blackCannon}（最多2）");
            }
            if (redPawn > 5)
            {
                result.Warnings.Add($"红兵数量异常：{redPawn}（最多5）");
            }
            if (blackPawn > 5)
            {
                result.Warnings.Add($"黑卒数量异常：{blackPawn}（最多5）");
            }

            return result;
        }

        /// <summary>
        /// 解析FEN字符串
        /// </summary>
        public RecognizedBoard ParseFEN(string fen)
        {
            var board = new RecognizedBoard
            {
                Pieces = new RecognizedPiece[9, 10]
            };

            // 初始化
            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 10; row++)
                {
                    board.Pieces[col, row] = new RecognizedPiece { PieceType = ChessPieceType.Empty };
                }
            }

            if (string.IsNullOrWhiteSpace(fen))
                return board;

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
                        board.Pieces[col, row] = CharToPiece(c);
                        col++;
                    }
                }
            }

            if (parts.Length > 1)
            {
                board.IsRedTurn = parts[1].ToLower() == "w";
            }

            return board;
        }

        /// <summary>
        /// 字符转棋子
        /// </summary>
        private RecognizedPiece CharToPiece(char c)
        {
            bool isRed = char.IsUpper(c);
            var type = char.ToLower(c) switch
            {
                'k' => ChessPieceType.King,
                'a' => ChessPieceType.Advisor,
                'b' => ChessPieceType.Elephant,
                'n' => ChessPieceType.Knight,
                'r' => ChessPieceType.Rook,
                'c' => ChessPieceType.Cannon,
                'p' => ChessPieceType.Pawn,
                _ => ChessPieceType.Empty
            };

            return new RecognizedPiece
            {
                PieceType = type,
                IsRed = isRed,
                Confidence = 1.0
            };
        }

        public void SetOptions(FenGeneratorOptions options)
        {
            _options = options ?? new FenGeneratorOptions();
        }
    }
}
