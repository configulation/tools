using System.Drawing;
using System.Drawing.Imaging;
using Sunny.UI;
using WinFormsApp1.second_menu.ChessRecognizer.Config;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Models;
using WinFormsApp1.second_menu.ChessRecognizer.Core.Services;
using WinFormsApp1.second_menu.ChessRecognizer.Utils;

namespace WinFormsApp1.second_menu.ChessRecognizer.Forms
{
    /// <summary>
    /// 中国象棋识别系统主窗体
    /// </summary>
    public partial class FrmChessRecognizer : UIPage
    {
        private ChessRecognitionEngine _engine;
        private AppSettings _settings;
        private Bitmap _currentImage;
        private FullRecognitionResult _lastResult;
        private bool _isProcessing = false;

        public FrmChessRecognizer()
        {
            InitializeComponent();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await InitializeEngineAsync();
        }

        private async Task InitializeEngineAsync()
        {
            try
            {
                lblStatus.Text = "正在初始化OCR引擎...";
                lblStatus.ForeColor = Color.Orange;

                _settings = AppSettings.Load();
                _engine = new ChessRecognitionEngine(_settings);
                _engine.ProgressChanged += Engine_ProgressChanged;

                bool success = await _engine.InitializeAsync();

                if (success)
                {
                    // 填充OCR引擎列表
                    cboOcrEngine.Items.Clear();
                    foreach (var engine in _engine.AvailableEngines)
                    {
                        cboOcrEngine.Items.Add(engine);
                    }

                    if (cboOcrEngine.Items.Count > 0)
                    {
                        int idx = cboOcrEngine.Items.IndexOf(_engine.CurrentOcrEngine);
                        cboOcrEngine.SelectedIndex = idx >= 0 ? idx : 0;
                    }

                    lblStatus.Text = $"就绪 - 使用 {_engine.CurrentOcrEngine} 引擎";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblStatus.Text = "OCR引擎初始化失败";
                    lblStatus.ForeColor = Color.Red;
                }

                chkAutoCopy.Checked = _settings.UI.AutoCopyFEN;
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"初始化失败: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void Engine_ProgressChanged(object sender, RecognitionProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => Engine_ProgressChanged(sender, e));
                return;
            }

            progressBar.Value = (int)e.Progress;
            lblStatus.Text = e.Message;
        }

        #region 事件处理

        private void FrmChessRecognizer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                BtnPaste_Click(sender, e);
                e.Handled = true;
            }
        }

        private void BtnPaste_Click(object sender, EventArgs e)
        {
            try
            {
                var image = ImageProcessor.GetFromClipboard();
                if (image != null)
                {
                    SetCurrentImage(image);
                }
                else
                {
                    ShowWarning("剪贴板中没有图片");
                }
            }
            catch (Exception ex)
            {
                ShowError($"粘贴失败: {ex.Message}");
            }
        }

        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "图片文件|*.png;*.jpg;*.jpeg;*.bmp;*.gif|所有文件|*.*",
                Title = "选择棋盘图片"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var image = ImageProcessor.Load(dialog.FileName);
                    SetCurrentImage(image);
                    AddRecentFile(dialog.FileName);
                }
                catch (Exception ex)
                {
                    ShowError($"加载图片失败: {ex.Message}");
                }
            }
        }

        private void PicPreview_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) ||
                e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void PicPreview_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (files.Length > 0)
                    {
                        var image = ImageProcessor.Load(files[0]);
                        SetCurrentImage(image);
                        AddRecentFile(files[0]);
                    }
                }
                else if (e.Data.GetDataPresent(DataFormats.Bitmap))
                {
                    var image = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
                    SetCurrentImage(new Bitmap(image));
                }
            }
            catch (Exception ex)
            {
                ShowError($"拖放失败: {ex.Message}");
            }
        }

        private void PicPreview_Click(object sender, EventArgs e)
        {
            if (_currentImage == null)
            {
                BtnSelectFile_Click(sender, e);
            }
        }

        private async void BtnRecognize_Click(object sender, EventArgs e)
        {
            if (_currentImage == null)
            {
                ShowWarning("请先选择或粘贴一张棋盘图片");
                return;
            }

            if (_isProcessing)
            {
                return;
            }

            await StartRecognitionAsync();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            _engine?.Cancel();
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            using var settingsForm = new FrmSettings(_settings);
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                _settings = settingsForm.Settings;
                _settings.Save();
                chkAutoCopy.Checked = _settings.UI.AutoCopyFEN;
            }
        }

        private void CboOcrEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboOcrEngine.SelectedItem != null && _engine != null)
            {
                string engineName = cboOcrEngine.SelectedItem.ToString();
                _engine.SwitchOcrEngine(engineName);
                lblStatus.Text = $"已切换到 {engineName} 引擎";
            }
        }

        private async void BtnTestPerf_Click(object sender, EventArgs e)
        {
            if (_currentImage == null)
            {
                ShowWarning("请先选择一张图片进行测试");
                return;
            }

            var statuses = _engine.GetEngineStatuses();
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("OCR引擎状态：");
            foreach (var kvp in statuses)
            {
                sb.AppendLine($"\n{kvp.Key}:");
                sb.AppendLine($"  就绪: {kvp.Value.IsReady}");
                sb.AppendLine($"  已处理: {kvp.Value.ProcessedCount}");
                sb.AppendLine($"  平均耗时: {kvp.Value.AverageProcessTime:F2}ms");
            }

            MessageBox.Show(sb.ToString(), "引擎状态", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCopyFEN_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFEN.Text))
            {
                Clipboard.SetText(txtFEN.Text);
                lblStatus.Text = "FEN已复制到剪贴板";
            }
        }

        private void BtnCopyBoard_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtConsole.Text))
            {
                Clipboard.SetText(txtConsole.Text);
                lblStatus.Text = "棋盘图已复制到剪贴板";
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            _currentImage?.Dispose();
            _currentImage = null;
            picPreview.Image = null;
            txtFEN.Text = "";
            txtConsole.Text = "";
            lblStatus.Text = "已清空";
            lblTime.Text = "处理时间: -";
            lblConfidence.Text = "置信度: -";
            _lastResult = null;
        }

        #endregion

        #region 辅助方法

        private void SetCurrentImage(Bitmap image)
        {
            _currentImage?.Dispose();
            _currentImage = image;
            picPreview.Image = image;
            lblStatus.Text = $"已加载图片 ({image.Width}x{image.Height})";
            lblStatus.ForeColor = Color.Green;
        }

        private async Task StartRecognitionAsync()
        {
            _isProcessing = true;
            btnRecognize.Enabled = false;
            btnStop.Enabled = true;
            progressBar.Visible = true;
            progressBar.Value = 0;

            try
            {
                lblStatus.Text = "正在识别...";
                lblStatus.ForeColor = Color.Orange;

                var result = await _engine.RecognizeAsync(_currentImage);
                _lastResult = result;

                if (result.Success)
                {
                    txtFEN.Text = result.FEN;

                    // 生成棋盘图
                    if (result.BoardPosition != null)
                    {
                        txtConsole.Text = ConsoleRenderer.RenderSimpleBoard(result.BoardPosition);
                    }

                    lblTime.Text = $"处理时间: {result.ProcessTime.TotalMilliseconds:F0}ms";
                    lblConfidence.Text = $"置信度: {result.OverallConfidence:P1}";
                    lblStatus.Text = $"识别完成 - 共{result.PieceCount}个棋子 (红{result.RedPieceCount}/黑{result.BlackPieceCount})";
                    lblStatus.ForeColor = Color.Green;

                    // 自动复制
                    if (chkAutoCopy.Checked && !string.IsNullOrEmpty(result.FEN))
                    {
                        Clipboard.SetText(result.FEN);
                    }

                    // 显示警告
                    if (result.Warnings.Count > 0)
                    {
                        var warningText = string.Join("\n", result.Warnings.Take(5));
                        ShowWarning($"识别完成，但有以下警告：\n{warningText}");
                    }
                }
                else
                {
                    lblStatus.Text = $"识别失败: {result.ErrorMessage}";
                    lblStatus.ForeColor = Color.Red;
                    ShowError(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"识别出错: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                ShowError(ex.Message);
            }
            finally
            {
                _isProcessing = false;
                btnRecognize.Enabled = true;
                btnStop.Enabled = false;
                progressBar.Visible = false;
            }
        }

        private void AddRecentFile(string path)
        {
            if (!_settings.UI.RecentFiles.Contains(path))
            {
                _settings.UI.RecentFiles.Insert(0, path);
                if (_settings.UI.RecentFiles.Count > _settings.UI.RecentFilesCount)
                {
                    _settings.UI.RecentFiles.RemoveAt(_settings.UI.RecentFiles.Count - 1);
                }
                _settings.Save();
            }
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _engine?.Dispose();
            _currentImage?.Dispose();
            _lastResult?.OriginalImage?.Dispose();
            _lastResult?.ProcessedImage?.Dispose();
        }
    }
}
