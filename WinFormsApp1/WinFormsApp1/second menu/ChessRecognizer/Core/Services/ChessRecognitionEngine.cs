using System.Diagnostics;
using System.Drawing;
using WinFormsApp1.second_menu.ChessRecognizer.Config;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Interfaces;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Models;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Services.Detection;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Services.FEN;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Services.OCR;
using WinFormsApp1.second_menu.ChessRecognizer.Utils;

namespace WinFormsApp1.second_menu.ChessRecognizer.Core.Services
{
    /// <summary>
    /// 象棋识别引擎 - 核心服务
    /// </summary>
    public class ChessRecognitionEngine : IDisposable
    {
        private readonly AppSettings _settings;
        private readonly BoardDetector _boardDetector;
        private readonly PieceDetector _pieceDetector;
        private readonly FENGenerator _fenGenerator;
        private readonly RuleValidator _ruleValidator;

        private IChessOCR _currentOcr;
        private readonly Dictionary<string, IChessOCR> _ocrEngines;

        private bool _disposed = false;
        private CancellationTokenSource _cts;

        /// <summary>
        /// 识别进度事件
        /// </summary>
        public event EventHandler<RecognitionProgressEventArgs> ProgressChanged;

        /// <summary>
        /// 当前OCR引擎名称
        /// </summary>
        public string CurrentOcrEngine => _currentOcr?.EngineName ?? "None";

        /// <summary>
        /// 可用的OCR引擎列表
        /// </summary>
        public IReadOnlyList<string> AvailableEngines => _ocrEngines.Keys.ToList();

        public ChessRecognitionEngine(AppSettings settings = null)
        {
            _settings = settings ?? AppSettings.Load();
            _boardDetector = new BoardDetector();
            _pieceDetector = new PieceDetector();
            _fenGenerator = new FENGenerator();
            _ruleValidator = new RuleValidator();

            _ocrEngines = new Dictionary<string, IChessOCR>();

            // 初始化检测参数
            _boardDetector.SetParameters(new BoardDetectionParameters
            {
                CannyThreshold1 = _settings.BoardDetection.CannyThreshold1,
                CannyThreshold2 = _settings.BoardDetection.CannyThreshold2,
                HoughThreshold = _settings.BoardDetection.HoughThreshold,
                MinLineLength = _settings.BoardDetection.MinLineLength,
                MaxLineGap = _settings.BoardDetection.MaxLineGap,
                LineMergeThreshold = _settings.BoardDetection.LineMergeThreshold,
                PieceRegionScale = _settings.BoardDetection.PieceRegionScale
            });

            _fenGenerator.SetOptions(new FenGeneratorOptions
            {
                UseChineseCoordinates = _settings.Fen.UseChineseCoordinates,
                EnableRuleValidation = _settings.Fen.EnableRuleValidation,
                AutoCorrectionLevel = _settings.Fen.AutoCorrectionLevel,
                DefaultRedTurn = _settings.Fen.DefaultRedTurn
            });
        }

        /// <summary>
        /// 初始化OCR引擎
        /// </summary>
        public async Task<bool> InitializeAsync()
        {
            try
            {
                // 初始化Tesseract
                var tesseract = new TesseractOCRService();
                if (await tesseract.InitializeAsync())
                {
                    _ocrEngines["Tesseract"] = tesseract;
                }

                // 初始化简单OCR（备选）
                var simpleOcr = new SimpleOCRService();
                await simpleOcr.InitializeAsync();
                _ocrEngines["SimpleOCR"] = simpleOcr;

                // 设置默认引擎
                string defaultEngine = _settings.Ocr.DefaultEngine;
                if (_ocrEngines.ContainsKey(defaultEngine))
                {
                    _currentOcr = _ocrEngines[defaultEngine];
                }
                else if (_ocrEngines.Count > 0)
                {
                    _currentOcr = _ocrEngines.Values.First();
                }

                return _currentOcr != null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"初始化失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 切换OCR引擎
        /// </summary>
        public bool SwitchOcrEngine(string engineName)
        {
            if (_ocrEngines.TryGetValue(engineName, out var engine))
            {
                _currentOcr = engine;
                _currentOcr.SetConfidenceThreshold(_settings.Ocr.ConfidenceThreshold);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 识别棋盘图像
        /// </summary>
        public async Task<FullRecognitionResult> RecognizeAsync(Bitmap image, CancellationToken cancellationToken = default)
        {
            var result = new FullRecognitionResult();
            var sw = Stopwatch.StartNew();

            try
            {
                _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

                // 保存原始图像
                result.OriginalImage = new Bitmap(image);

                // 步骤1：图像预处理
                ReportProgress(1, 7, "图像预处理", 0);
                var stepSw = Stopwatch.StartNew();

                Bitmap processedImage = PreprocessImage(image);
                result.ProcessedImage = processedImage;

                stepSw.Stop();
                result.StepTimes["预处理"] = stepSw.Elapsed;

                _cts.Token.ThrowIfCancellationRequested();

                // 步骤2：棋盘检测
                ReportProgress(2, 7, "棋盘检测", 15);
                stepSw.Restart();

                var boardResult = await _boardDetector.DetectBoardAsync(processedImage);
                if (!boardResult.Success)
                {
                    result.Success = false;
                    result.ErrorMessage = "棋盘检测失败: " + boardResult.ErrorMessage;
                    return result;
                }

                stepSw.Stop();
                result.StepTimes["棋盘检测"] = stepSw.Elapsed;

                _cts.Token.ThrowIfCancellationRequested();

                // 步骤3：提取棋子区域
                ReportProgress(3, 7, "提取棋子区域", 30);
                stepSw.Restart();

                var pieceRegions = _boardDetector.ExtractPieceRegions(processedImage, boardResult.BoardInfo);

                stepSw.Stop();
                result.StepTimes["提取区域"] = stepSw.Elapsed;

                _cts.Token.ThrowIfCancellationRequested();

                // 步骤4：识别棋子
                ReportProgress(4, 7, "识别棋子", 45);
                stepSw.Restart();

                var boardPosition = new BoardPosition();
                var pieceDetails = new List<PieceRecognitionDetail>();
                int totalRegions = pieceRegions.Count;
                int processedRegions = 0;

                foreach (var region in pieceRegions)
                {
                    _cts.Token.ThrowIfCancellationRequested();

                    var detail = await RecognizePieceRegionAsync(region);
                    pieceDetails.Add(detail);

                    if (detail.FinalPieceType != PieceType.Empty)
                    {
                        var piece = new ChessPieceModel
                        {
                            Type = detail.FinalPieceType,
                            IsRed = detail.IsRed,
                            Column = detail.Column,
                            Row = detail.Row,
                            Confidence = detail.Confidence,
                            RecognizedText = detail.RecognizedText
                        };
                        boardPosition.SetPiece(detail.Column, detail.Row, piece);
                    }

                    processedRegions++;
                    double progress = 45 + (processedRegions / (double)totalRegions) * 35;
                    ReportProgress(4, 7, $"识别棋子 ({processedRegions}/{totalRegions})", progress);
                }

                stepSw.Stop();
                result.StepTimes["棋子识别"] = stepSw.Elapsed;
                result.PieceDetails = pieceDetails;

                // 步骤5：生成FEN
                ReportProgress(5, 7, "生成FEN", 85);
                stepSw.Restart();

                var recognizedBoard = ConvertToRecognizedBoard(boardPosition);
                var fenResult = _fenGenerator.GenerateFEN(recognizedBoard);

                stepSw.Stop();
                result.StepTimes["FEN生成"] = stepSw.Elapsed;

                // 步骤6：规则验证
                ReportProgress(6, 7, "规则验证", 92);
                stepSw.Restart();

                var validation = _ruleValidator.ValidateBoard(boardPosition);
                result.Warnings.AddRange(validation.Warnings);
                if (!validation.IsValid)
                {
                    result.Warnings.AddRange(validation.Errors);
                }

                stepSw.Stop();
                result.StepTimes["规则验证"] = stepSw.Elapsed;

                // 步骤7：完成
                ReportProgress(7, 7, "完成", 100);

                // 填充结果
                result.Success = fenResult.Success;
                result.FEN = fenResult.FEN;
                result.BoardPosition = boardPosition;
                result.Warnings.AddRange(fenResult.Warnings);

                var (red, black) = boardPosition.CountPieces();
                result.PieceCount = red + black;
                result.RedPieceCount = red;
                result.BlackPieceCount = black;
                result.OcrEngine = CurrentOcrEngine;

                // 计算总体置信度
                var validDetails = pieceDetails.Where(d => d.FinalPieceType != PieceType.Empty).ToList();
                result.OverallConfidence = validDetails.Count > 0
                    ? validDetails.Average(d => d.Confidence)
                    : 0;
            }
            catch (OperationCanceledException)
            {
                result.Success = false;
                result.ErrorMessage = "识别已取消";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.DebugInfo = ex.StackTrace;
            }
            finally
            {
                sw.Stop();
                result.ProcessTime = sw.Elapsed;
                _cts?.Dispose();
                _cts = null;
            }

            return result;
        }

        /// <summary>
        /// 取消识别
        /// </summary>
        public void Cancel()
        {
            _cts?.Cancel();
        }

        /// <summary>
        /// 图像预处理
        /// </summary>
        private Bitmap PreprocessImage(Bitmap source)
        {
            Bitmap result = source;

            // 调整大小
            if (source.Width > _settings.Performance.MaxImageWidth ||
                source.Height > _settings.Performance.MaxImageHeight)
            {
                result = ImageProcessor.Resize(source,
                    _settings.Performance.MaxImageWidth,
                    _settings.Performance.MaxImageHeight);
            }
            else
            {
                result = new Bitmap(source);
            }

            // 去噪
            if (_settings.Ocr.EnableDenoising)
            {
                var denoised = ImageProcessor.Denoise(result);
                result.Dispose();
                result = denoised;
            }

            // 锐化
            if (_settings.Ocr.EnableSharpening)
            {
                var sharpened = ImageProcessor.Sharpen(result);
                result.Dispose();
                result = sharpened;
            }

            return result;
        }

        /// <summary>
        /// 识别单个棋子区域
        /// </summary>
        private async Task<PieceRecognitionDetail> RecognizePieceRegionAsync(PieceRegion region)
        {
            var detail = new PieceRecognitionDetail
            {
                Column = region.Column,
                Row = region.Row,
                PieceImage = region.Image
            };

            // 检查是否有棋子
            if (!region.HasPiece && !_pieceDetector.HasPiece(region.Image))
            {
                detail.FinalPieceType = PieceType.Empty;
                detail.Confidence = 0.9;
                return detail;
            }

            // 颜色分析
            var colorResult = _pieceDetector.AnalyzeColor(region.Image);
            detail.ColorResult = new ColorDetectionResult
            {
                IsRed = colorResult.IsRed,
                Confidence = colorResult.Confidence,
                DominantHue = (int)colorResult.AvgH,
                Saturation = (int)(colorResult.AvgS * 100),
                Value = (int)(colorResult.AvgV * 100)
            };

            // OCR识别
            if (_currentOcr != null && _currentOcr.IsInitialized)
            {
                var ocrResult = await _currentOcr.RecognizePieceAsync(region.Image);
                detail.EngineResults[_currentOcr.EngineName] = new OcrEngineResult
                {
                    EngineName = _currentOcr.EngineName,
                    Text = ocrResult.Text,
                    Confidence = ocrResult.Confidence,
                    ProcessTime = ocrResult.ProcessTime
                };

                if (ocrResult.Success && !string.IsNullOrEmpty(ocrResult.Text))
                {
                    detail.RecognizedText = ocrResult.Text;
                }
            }

            // 解析棋子
            var piece = _pieceDetector.ParsePieceFromText(
                detail.RecognizedText,
                region.Column,
                region.Row,
                region.ColorType
            );

            // 如果OCR没有识别出来，但颜色分析显示有棋子，使用颜色结果
            if (piece.Type == PieceType.Empty && colorResult.Confidence > 0.6)
            {
                piece.IsRed = colorResult.IsRed;
                piece.Type = GuessPieceType(region.Column, region.Row, piece.IsRed);
                piece.Confidence = colorResult.Confidence * 0.5;
            }

            detail.FinalPieceType = piece.Type;
            detail.IsRed = piece.IsRed;
            detail.Confidence = piece.Confidence;

            return detail;
        }

        /// <summary>
        /// 根据位置推测棋子类型
        /// </summary>
        private PieceType GuessPieceType(int col, int row, bool isRed)
        {
            if (isRed)
            {
                if (row == 9)
                {
                    if (col == 4) return PieceType.King;
                    if (col == 3 || col == 5) return PieceType.Advisor;
                    if (col == 2 || col == 6) return PieceType.Elephant;
                    if (col == 1 || col == 7) return PieceType.Knight;
                    if (col == 0 || col == 8) return PieceType.Rook;
                }
                else if (row == 7 && (col == 1 || col == 7))
                {
                    return PieceType.Cannon;
                }
                else if (row == 6 && col % 2 == 0)
                {
                    return PieceType.Pawn;
                }
            }
            else
            {
                if (row == 0)
                {
                    if (col == 4) return PieceType.King;
                    if (col == 3 || col == 5) return PieceType.Advisor;
                    if (col == 2 || col == 6) return PieceType.Elephant;
                    if (col == 1 || col == 7) return PieceType.Knight;
                    if (col == 0 || col == 8) return PieceType.Rook;
                }
                else if (row == 2 && (col == 1 || col == 7))
                {
                    return PieceType.Cannon;
                }
                else if (row == 3 && col % 2 == 0)
                {
                    return PieceType.Pawn;
                }
            }

            return PieceType.Pawn;
        }

        /// <summary>
        /// 转换为RecognizedBoard
        /// </summary>
        private RecognizedBoard ConvertToRecognizedBoard(BoardPosition position)
        {
            var board = new RecognizedBoard
            {
                Pieces = new RecognizedPiece[9, 10],
                IsRedTurn = position.IsRedTurn
            };

            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 10; row++)
                {
                    var piece = position.GetPiece(col, row);
                    board.Pieces[col, row] = new RecognizedPiece
                    {
                        PieceType = ConvertPieceType(piece.Type),
                        IsRed = piece.IsRed,
                        Confidence = piece.Confidence,
                        RecognizedText = piece.RecognizedText
                    };
                }
            }

            var (red, black) = position.CountPieces();
            board.TotalPieces = red + black;

            return board;
        }

        /// <summary>
        /// 转换棋子类型
        /// </summary>
        private ChessPieceType ConvertPieceType(PieceType type)
        {
            return type switch
            {
                PieceType.King => ChessPieceType.King,
                PieceType.Advisor => ChessPieceType.Advisor,
                PieceType.Elephant => ChessPieceType.Elephant,
                PieceType.Knight => ChessPieceType.Knight,
                PieceType.Rook => ChessPieceType.Rook,
                PieceType.Cannon => ChessPieceType.Cannon,
                PieceType.Pawn => ChessPieceType.Pawn,
                _ => ChessPieceType.Empty
            };
        }

        /// <summary>
        /// 报告进度
        /// </summary>
        private void ReportProgress(int step, int total, string name, double progress)
        {
            ProgressChanged?.Invoke(this, new RecognitionProgressEventArgs
            {
                CurrentStep = step,
                TotalSteps = total,
                StepName = name,
                Progress = progress,
                Message = $"步骤 {step}/{total}: {name}"
            });
        }

        /// <summary>
        /// 获取OCR引擎状态
        /// </summary>
        public Dictionary<string, OcrEngineStatus> GetEngineStatuses()
        {
            var statuses = new Dictionary<string, OcrEngineStatus>();
            foreach (var kvp in _ocrEngines)
            {
                statuses[kvp.Key] = kvp.Value.GetStatus();
            }
            return statuses;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _cts?.Cancel();
                _cts?.Dispose();

                foreach (var ocr in _ocrEngines.Values)
                {
                    ocr?.Dispose();
                }
                _ocrEngines.Clear();

                _disposed = true;
            }
        }
    }
}
