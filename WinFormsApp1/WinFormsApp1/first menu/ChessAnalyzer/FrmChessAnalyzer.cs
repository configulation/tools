using Sunny.UI;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.first_menu.ChessAnalyzer.Core;
using WinFormsApp1.first_menu.ChessAnalyzer.UI;
using WinFormsApp1.first_menu.ChessAnalyzer.Utils;
using WinFormsApp1.first_menu.ChessAnalyzer.Recognition;

namespace WinFormsApp1.first_menu.ChessAnalyzer
{
    /// <summary>
    /// 象棋实时分析主页面
    /// </summary>
    public partial class FrmChessAnalyzer : UIForm
    {
        private PikafishEngine _engine;
        internal OverlayForm _overlayWindow;
        internal ChessBoard _currentBoard;
        internal Rectangle _selectedArea = Rectangle.Empty;
        private System.Windows.Forms.Timer _scanTimer;
        private bool _isAnalyzing = false;
        private FeatureBasedRecognizer _recognizer;  // 使用基于特征的识别器

        public FrmChessAnalyzer()
        {
            InitializeComponent();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            // 初始化棋盘
            _currentBoard = new ChessBoard();

            // 初始化识别器（特征分析版 - 更准确）
            _recognizer = new FeatureBasedRecognizer();

            // 初始化扫描定时器
            _scanTimer = new System.Windows.Forms.Timer
            {
                Interval = 2000, // 2秒扫描一次
                Enabled = false
            };
            _scanTimer.Tick += ScanTimer_Tick;

            // 启动引擎
            await StartEngineAsync();
        }

        /// <summary>
        /// 启动引擎
        /// </summary>
        private async Task StartEngineAsync()
        {
            try
            {
                lblStatus.Text = "正在启动引擎...";
                lblStatus.ForeColor = Color.Orange;

                // 获取引擎路径
                string engineDir = Path.Combine(Application.StartupPath, "engine", "Pikafish.2025-10-27", "Windows");
                
                // 自动选择最佳引擎版本
                string[] enginePriority = new[]
                {
                    "pikafish-avx2.exe",
                    "pikafish-bmi2.exe",
                    "pikafish-sse41-popcnt.exe"
                };

                string enginePath = null;
                foreach (var engine in enginePriority)
                {
                    string path = Path.Combine(engineDir, engine);
                    if (File.Exists(path))
                    {
                        enginePath = path;
                        break;
                    }
                }

                if (enginePath == null)
                {
                    throw new FileNotFoundException("未找到引擎文件");
                }

                // 获取 NNUE 文件
                string nnuePath = Path.Combine(Application.StartupPath, "engine", "Pikafish.2025-10-27", "pikafish.nnue");

                // 创建引擎实例
                _engine = new PikafishEngine();
                _engine.BestMoveReceived += Engine_BestMoveReceived;
                _engine.InfoReceived += Engine_InfoReceived;
                _engine.LogReceived += Engine_LogReceived;

                // 启动引擎
                bool success = await _engine.StartAsync(enginePath, nnuePath);

                if (success)
                {
                    lblStatus.Text = "✓ 引擎已就绪";
                    lblStatus.ForeColor = Color.Green;
                    btnStartAnalysis.Enabled = true;
                    
                    // 记录日志
                    AppendLog($"引擎已启动: {Path.GetFileName(enginePath)}");
                    AppendLog($"NNUE文件: {Path.GetFileName(nnuePath)}");
                }
                else
                {
                    lblStatus.Text = "✗ 引擎启动失败";
                    lblStatus.ForeColor = Color.Red;
                    UIMessageBox.ShowError("引擎启动失败，请检查配置");
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "✗ 启动错误";
                lblStatus.ForeColor = Color.Red;
                UIMessageBox.ShowError($"启动引擎失败:\n{ex.Message}");
                AppendLog($"错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 选择棋盘区域
        /// </summary>
        private void btnSelectArea_Click(object sender, EventArgs e)
        {
            try
            {
                using (var selector = new AreaSelectorForm())
                {
                    if (selector.ShowDialog() == DialogResult.OK)
                    {
                        _selectedArea = selector.SelectedArea;
                        lblAreaInfo.Text = $"已选择区域: {_selectedArea.Width} x {_selectedArea.Height}";
                        btnStartAnalysis.Enabled = _engine != null && _engine.IsReady;
                        AppendLog($"选择区域: {_selectedArea}");
                    }
                }
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError($"选择区域失败:\n{ex.Message}");
            }
        }

        /// <summary>
        /// 开始实时分析
        /// </summary>
        private void btnStartAnalysis_Click(object sender, EventArgs e)
        {
            if (_selectedArea == Rectangle.Empty)
            {
                UIMessageBox.ShowWarning("请先选择棋盘区域");
                return;
            }

            if (_isAnalyzing)
            {
                // 停止分析
                StopAnalysis();
            }
            else
            {
                // 开始分析
                StartAnalysis();
            }
        }

        /// <summary>
        /// 开始分析
        /// </summary>
        private void StartAnalysis()
        {
            _isAnalyzing = true;
            btnStartAnalysis.Text = "停止分析";
            btnStartAnalysis.Symbol = 61516; // 停止图标

            // 创建悬浮窗口
            if (_overlayWindow == null || _overlayWindow.IsDisposed)
            {
                _overlayWindow = new OverlayForm();
            }
            _overlayWindow.Show();

            // 启动定时扫描
            _scanTimer.Start();

            AppendLog("开始实时分析...");
        }

        /// <summary>
        /// 停止分析
        /// </summary>
        private void StopAnalysis()
        {
            _isAnalyzing = false;
            btnStartAnalysis.Text = "开始分析";
            btnStartAnalysis.Symbol = 61515; // 播放图标

            _scanTimer.Stop();
            _engine?.Stop();

            AppendLog("停止分析");
        }

        /// <summary>
        /// 定时扫描棋盘
        /// </summary>
        private async void ScanTimer_Tick(object sender, EventArgs e)
        {
            if (!_isAnalyzing) return;

            try
            {
                // 1. 截图
                var screenshot = ScreenCapture.CaptureScreen(_selectedArea);

                // 2. 图像识别转棋盘
                var result = _recognizer.RecognizeToBoard(screenshot);

                if (result.Success && !string.IsNullOrEmpty(result.FEN))
                {
                    // 3. 加载到棋盘
                    _currentBoard.LoadFromFEN(result.FEN);

                    // 4. 发送给引擎分析
                    int depth = (int)numDepth.Value;
                    _engine.AnalyzePosition(result.FEN, depth);

                    lblLastScan.Text = $"上次扫描: {DateTime.Now:HH:mm:ss} ✓";
                    AppendLog($"识别成功");
                }
                else
                {
                    lblLastScan.Text = $"上次扫描: {DateTime.Now:HH:mm:ss} ✗";
                    AppendLog($"识别失败: {result.Message}");
                }

                screenshot?.Dispose();
            }
            catch (Exception ex)
            {
                AppendLog($"扫描错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 引擎返回最佳走法
        /// </summary>
        private void Engine_BestMoveReceived(object sender, BestMoveEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => Engine_BestMoveReceived(sender, e)));
                return;
            }

            // 转换为中文记谱
            string bestMoveChinese = MoveConverter.ToChineseNotation(e.BestMove, _currentBoard);
            string ponderMoveChinese = !string.IsNullOrEmpty(e.PonderMove) 
                ? MoveConverter.ToChineseNotation(e.PonderMove, _currentBoard) 
                : "无";

            // 显示：中文（ICCS）
            lblBestMove.Text = $"推荐: {bestMoveChinese} ({e.BestMove})";
            lblPonderMove.Text = $"应对: {ponderMoveChinese}" + 
                (string.IsNullOrEmpty(e.PonderMove) ? "" : $" ({e.PonderMove})");

            // 更新悬浮窗口（也传中文走法）
            if (_overlayWindow != null && !_overlayWindow.IsDisposed)
            {
                _overlayWindow.UpdateBoard(_currentBoard, e.BestMove, e.PonderMove, 0, 0, 
                    bestMoveChinese, ponderMoveChinese);
            }

            AppendLog($"最佳走法: {bestMoveChinese} ({e.BestMove})");
        }

        /// <summary>
        /// 引擎分析信息
        /// </summary>
        private void Engine_InfoReceived(object sender, EngineInfoEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => Engine_InfoReceived(sender, e)));
                return;
            }

            var info = e.Info;
            lblDepth.Text = $"深度: {info.Depth}";
            lblScore.Text = $"评分: {info.GetScoreText()}";
            lblNodes.Text = $"节点: {info.Nodes:N0}";

            // 更新悬浮窗口
            if (_overlayWindow != null && !_overlayWindow.IsDisposed && !string.IsNullOrEmpty(info.PrincipalVariation))
            {
                var firstMove = info.PrincipalVariation.Split(' ').FirstOrDefault();
                if (!string.IsNullOrEmpty(firstMove))
                {
                    _overlayWindow.UpdateBoard(_currentBoard, firstMove, null, info.Score, info.Depth);
                }
            }
        }

        /// <summary>
        /// 引擎日志
        /// </summary>
        private void Engine_LogReceived(object sender, string e)
        {
            // 可以选择是否显示详细日志
            // AppendLog(e);
        }

        /// <summary>
        /// 手动输入 FEN（使用新的 FenParser）
        /// </summary>
        private void btnManualFEN_Click(object sender, EventArgs e)
        {
            string fen = txtFEN.Text.Trim();
            if (string.IsNullOrEmpty(fen))
            {
                UIMessageBox.ShowWarning("请输入 FEN 字符串");
                return;
            }

            try
            {
                // 使用新的 FenParser 进行解析
                var result = Core.FenParser.Parse(fen);
                
                if (!result.Success)
                {
                    UIMessageBox.ShowError($"FEN 解析失败:\n{result.ErrorMessage}");
                    AppendLog($"✗ FEN 解析失败: {result.ErrorMessage}");
                    return;
                }

                // 解析成功，加载到棋盘
                _currentBoard.LoadFromFEN(fen);
                AppendLog("✓ FEN 解析成功！");
                AppendLog($"当前走棋方：{result.CurrentPlayer}");
                
                // 显示文本棋盘（调试用）
                AppendLog("--- 棋盘布局 ---");
                string textBoard = Core.FenParser.ToTextBoard(result.Board, false);
                var lines = textBoard.Split('\n');
                foreach (var line in lines.Take(13)) // 只显示前13行
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        AppendLog(line);
                }

                // 显示悬浮窗
                if (_overlayWindow == null || _overlayWindow.IsDisposed)
                {
                    _overlayWindow = new OverlayForm();
                }
                _overlayWindow.Show();
                _overlayWindow.UpdateBoard(_currentBoard, "", "", 0, 0);
                AppendLog("✓ 已在悬浮窗显示棋盘");

                // 调用引擎分析
                _engine.AnalyzePosition(fen, (int)numDepth.Value);
                AppendLog($"开始分析局面（深度{numDepth.Value}）...");
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError($"处理 FEN 时出错:\n{ex.Message}");
                AppendLog($"✗ 错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 测试开局局面
        /// </summary>
        private void btnTestPosition_Click(object sender, EventArgs e)
        {
            string fen = "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1";
            txtFEN.Text = fen;
            _currentBoard.LoadFromFEN(fen);
            _engine.AnalyzePosition(fen, (int)numDepth.Value);
            
            // 显示悬浮窗口
            if (_overlayWindow == null || _overlayWindow.IsDisposed)
            {
                _overlayWindow = new OverlayForm();
            }
            _overlayWindow.Show();
            _overlayWindow.UpdateBoard(_currentBoard, "", "", 0, 0);
            
            AppendLog("测试开局局面");
        }

        /// <summary>
        /// 显示/隐藏悬浮窗
        /// </summary>
        private void btnToggleOverlay_Click(object sender, EventArgs e)
        {
            if (_overlayWindow == null || _overlayWindow.IsDisposed)
            {
                _overlayWindow = new OverlayForm();
                _overlayWindow.UpdateBoard(_currentBoard, "", "", 0, 0);
            }

            if (_overlayWindow.Visible)
            {
                _overlayWindow.Hide();
                btnToggleOverlay.Text = "显示悬浮窗";
            }
            else
            {
                _overlayWindow.Show();
                btnToggleOverlay.Text = "隐藏悬浮窗";
            }
        }

        /// <summary>
        /// 清空日志
        /// </summary>
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        internal void AppendLog(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => AppendLog(message)));
                return;
            }

            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
            txtLog.ScrollToCaret();
        }

        /// <summary>
        /// 获取测试 FEN（简化版，实际应该通过图像识别）
        /// </summary>
        private string GetTestFEN()
        {
            // 这里返回一个测试局面
            // 实际应用中应该通过图像识别获取
            return txtFEN.Text.Trim();
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopAnalysis();
            _engine?.Dispose();
            _recognizer?.Dispose();
            _overlayWindow?.Close();
            _overlayWindow?.Dispose();

            base.OnFormClosing(e);
        }

        /// <summary>
        /// 窗口大小改变时调整控件
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            AdjustLayout();
        }

        /// <summary>
        /// 调整布局以适应窗口大小
        /// </summary>
        private void AdjustLayout()
        {
            if (this.WindowState == FormWindowState.Minimized) return;

            // 这里可以添加响应式布局代码
            // 根据窗口大小调整控件位置和大小
        }
    }
}
