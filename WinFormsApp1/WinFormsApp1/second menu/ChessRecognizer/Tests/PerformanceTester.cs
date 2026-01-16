using System.Diagnostics;
using System.Drawing;
using System.Text;
using WinFormsApp1.second_menu.ChessRecognizer.Config;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Models;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Services;

namespace WinFormsApp1.second_menu.ChessRecognizer.Tests
{
    /// <summary>
    /// 性能测试器
    /// </summary>
    public class PerformanceTester
    {
        private readonly ChessRecognitionEngine _engine;
        private readonly string _testImagesPath;

        /// <summary>
        /// 测试进度事件
        /// </summary>
        public event EventHandler<TestProgressEventArgs> ProgressChanged;

        public PerformanceTester(ChessRecognitionEngine engine, string testImagesPath = null)
        {
            _engine = engine;
            _testImagesPath = testImagesPath ?? Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TestImages"
            );
        }

        /// <summary>
        /// 运行完整测试
        /// </summary>
        public async Task<TestReport> RunFullTestAsync(CancellationToken cancellationToken = default)
        {
            var report = new TestReport
            {
                StartTime = DateTime.Now,
                EngineName = _engine.CurrentOcrEngine
            };

            try
            {
                // 获取测试图片
                var testImages = GetTestImages();
                if (testImages.Count == 0)
                {
                    report.ErrorMessage = "未找到测试图片";
                    return report;
                }

                report.TotalImages = testImages.Count;
                int processed = 0;

                foreach (var imagePath in testImages)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var result = await TestSingleImageAsync(imagePath);
                    report.Results.Add(result);

                    processed++;
                    ReportProgress(processed, testImages.Count, imagePath);
                }

                // 计算统计数据
                CalculateStatistics(report);
            }
            catch (OperationCanceledException)
            {
                report.ErrorMessage = "测试已取消";
            }
            catch (Exception ex)
            {
                report.ErrorMessage = ex.Message;
            }

            report.EndTime = DateTime.Now;
            return report;
        }

        /// <summary>
        /// 测试单张图片
        /// </summary>
        public async Task<SingleTestResult> TestSingleImageAsync(string imagePath)
        {
            var result = new SingleTestResult
            {
                ImagePath = imagePath,
                ImageName = Path.GetFileName(imagePath)
            };

            try
            {
                using var image = new Bitmap(imagePath);
                result.ImageSize = new Size(image.Width, image.Height);

                var sw = Stopwatch.StartNew();
                var recognitionResult = await _engine.RecognizeAsync(image);
                sw.Stop();

                result.ProcessTime = sw.Elapsed;
                result.Success = recognitionResult.Success;
                result.FEN = recognitionResult.FEN;
                result.PieceCount = recognitionResult.PieceCount;
                result.Confidence = recognitionResult.OverallConfidence;
                result.ErrorMessage = recognitionResult.ErrorMessage;

                // 如果有预期FEN，进行比较
                string expectedFen = GetExpectedFEN(imagePath);
                if (!string.IsNullOrEmpty(expectedFen))
                {
                    result.ExpectedFEN = expectedFen;
                    result.FENMatch = CompareFEN(result.FEN, expectedFen);
                }

                // 记录内存使用
                result.MemoryUsage = GC.GetTotalMemory(false);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 比较OCR引擎性能
        /// </summary>
        public async Task<EngineComparisonReport> CompareEnginesAsync(string imagePath)
        {
            var report = new EngineComparisonReport
            {
                ImagePath = imagePath
            };

            using var image = new Bitmap(imagePath);

            foreach (var engineName in _engine.AvailableEngines)
            {
                _engine.SwitchOcrEngine(engineName);

                var sw = Stopwatch.StartNew();
                var result = await _engine.RecognizeAsync(image);
                sw.Stop();

                report.EngineResults[engineName] = new EngineTestResult
                {
                    EngineName = engineName,
                    ProcessTime = sw.Elapsed,
                    Success = result.Success,
                    FEN = result.FEN,
                    Confidence = result.OverallConfidence,
                    PieceCount = result.PieceCount
                };
            }

            return report;
        }

        /// <summary>
        /// 获取测试图片列表
        /// </summary>
        private List<string> GetTestImages()
        {
            var images = new List<string>();

            if (Directory.Exists(_testImagesPath))
            {
                var extensions = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp" };
                foreach (var ext in extensions)
                {
                    images.AddRange(Directory.GetFiles(_testImagesPath, ext, SearchOption.AllDirectories));
                }
            }

            return images;
        }

        /// <summary>
        /// 获取预期FEN（从同名txt文件）
        /// </summary>
        private string GetExpectedFEN(string imagePath)
        {
            string fenPath = Path.ChangeExtension(imagePath, ".txt");
            if (File.Exists(fenPath))
            {
                return File.ReadAllText(fenPath).Trim();
            }
            return null;
        }

        /// <summary>
        /// 比较FEN字符串
        /// </summary>
        private bool CompareFEN(string actual, string expected)
        {
            if (string.IsNullOrEmpty(actual) || string.IsNullOrEmpty(expected))
                return false;

            // 只比较棋盘部分
            var actualBoard = actual.Split(' ')[0];
            var expectedBoard = expected.Split(' ')[0];

            return actualBoard == expectedBoard;
        }

        /// <summary>
        /// 计算统计数据
        /// </summary>
        private void CalculateStatistics(TestReport report)
        {
            var successResults = report.Results.Where(r => r.Success).ToList();

            if (successResults.Count > 0)
            {
                report.SuccessCount = successResults.Count;
                report.SuccessRate = (double)successResults.Count / report.TotalImages;
                report.AverageProcessTime = TimeSpan.FromMilliseconds(
                    successResults.Average(r => r.ProcessTime.TotalMilliseconds)
                );
                report.AverageConfidence = successResults.Average(r => r.Confidence);
                report.MinProcessTime = successResults.Min(r => r.ProcessTime);
                report.MaxProcessTime = successResults.Max(r => r.ProcessTime);
            }

            // FEN匹配统计
            var matchResults = report.Results.Where(r => r.ExpectedFEN != null).ToList();
            if (matchResults.Count > 0)
            {
                report.FENMatchCount = matchResults.Count(r => r.FENMatch);
                report.FENMatchRate = (double)report.FENMatchCount / matchResults.Count;
            }
        }

        /// <summary>
        /// 报告进度
        /// </summary>
        private void ReportProgress(int current, int total, string currentImage)
        {
            ProgressChanged?.Invoke(this, new TestProgressEventArgs
            {
                Current = current,
                Total = total,
                CurrentImage = currentImage,
                Progress = (double)current / total * 100
            });
        }

        /// <summary>
        /// 生成HTML报告
        /// </summary>
        public string GenerateHtmlReport(TestReport report)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html><head>");
            sb.AppendLine("<meta charset='utf-8'>");
            sb.AppendLine("<title>象棋识别测试报告</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            sb.AppendLine("table { border-collapse: collapse; width: 100%; }");
            sb.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
            sb.AppendLine("th { background-color: #4CAF50; color: white; }");
            sb.AppendLine("tr:nth-child(even) { background-color: #f2f2f2; }");
            sb.AppendLine(".success { color: green; }");
            sb.AppendLine(".fail { color: red; }");
            sb.AppendLine(".summary { background-color: #e7f3fe; padding: 15px; margin-bottom: 20px; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head><body>");

            sb.AppendLine("<h1>象棋识别测试报告</h1>");

            // 摘要
            sb.AppendLine("<div class='summary'>");
            sb.AppendLine($"<p><strong>测试时间:</strong> {report.StartTime:yyyy-MM-dd HH:mm:ss}</p>");
            sb.AppendLine($"<p><strong>OCR引擎:</strong> {report.EngineName}</p>");
            sb.AppendLine($"<p><strong>测试图片数:</strong> {report.TotalImages}</p>");
            sb.AppendLine($"<p><strong>成功率:</strong> {report.SuccessRate:P1}</p>");
            sb.AppendLine($"<p><strong>平均处理时间:</strong> {report.AverageProcessTime.TotalMilliseconds:F0}ms</p>");
            sb.AppendLine($"<p><strong>平均置信度:</strong> {report.AverageConfidence:P1}</p>");
            if (report.FENMatchRate > 0)
            {
                sb.AppendLine($"<p><strong>FEN匹配率:</strong> {report.FENMatchRate:P1}</p>");
            }
            sb.AppendLine("</div>");

            // 详细结果表格
            sb.AppendLine("<h2>详细结果</h2>");
            sb.AppendLine("<table>");
            sb.AppendLine("<tr><th>图片</th><th>尺寸</th><th>状态</th><th>耗时</th><th>置信度</th><th>棋子数</th><th>FEN匹配</th></tr>");

            foreach (var result in report.Results)
            {
                string statusClass = result.Success ? "success" : "fail";
                string status = result.Success ? "成功" : "失败";
                string fenMatch = result.ExpectedFEN != null ? (result.FENMatch ? "✓" : "✗") : "-";

                sb.AppendLine($"<tr>");
                sb.AppendLine($"<td>{result.ImageName}</td>");
                sb.AppendLine($"<td>{result.ImageSize.Width}x{result.ImageSize.Height}</td>");
                sb.AppendLine($"<td class='{statusClass}'>{status}</td>");
                sb.AppendLine($"<td>{result.ProcessTime.TotalMilliseconds:F0}ms</td>");
                sb.AppendLine($"<td>{result.Confidence:P1}</td>");
                sb.AppendLine($"<td>{result.PieceCount}</td>");
                sb.AppendLine($"<td>{fenMatch}</td>");
                sb.AppendLine($"</tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }
    }

    #region 测试结果类

    /// <summary>
    /// 测试报告
    /// </summary>
    public class TestReport
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string EngineName { get; set; }
        public int TotalImages { get; set; }
        public int SuccessCount { get; set; }
        public double SuccessRate { get; set; }
        public TimeSpan AverageProcessTime { get; set; }
        public TimeSpan MinProcessTime { get; set; }
        public TimeSpan MaxProcessTime { get; set; }
        public double AverageConfidence { get; set; }
        public int FENMatchCount { get; set; }
        public double FENMatchRate { get; set; }
        public string ErrorMessage { get; set; }
        public List<SingleTestResult> Results { get; set; } = new();
    }

    /// <summary>
    /// 单张图片测试结果
    /// </summary>
    public class SingleTestResult
    {
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public Size ImageSize { get; set; }
        public bool Success { get; set; }
        public string FEN { get; set; }
        public string ExpectedFEN { get; set; }
        public bool FENMatch { get; set; }
        public int PieceCount { get; set; }
        public double Confidence { get; set; }
        public TimeSpan ProcessTime { get; set; }
        public long MemoryUsage { get; set; }
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// 引擎比较报告
    /// </summary>
    public class EngineComparisonReport
    {
        public string ImagePath { get; set; }
        public Dictionary<string, EngineTestResult> EngineResults { get; set; } = new();
    }

    /// <summary>
    /// 引擎测试结果
    /// </summary>
    public class EngineTestResult
    {
        public string EngineName { get; set; }
        public TimeSpan ProcessTime { get; set; }
        public bool Success { get; set; }
        public string FEN { get; set; }
        public double Confidence { get; set; }
        public int PieceCount { get; set; }
    }

    /// <summary>
    /// 测试进度事件参数
    /// </summary>
    public class TestProgressEventArgs : EventArgs
    {
        public int Current { get; set; }
        public int Total { get; set; }
        public string CurrentImage { get; set; }
        public double Progress { get; set; }
    }

    #endregion
}
