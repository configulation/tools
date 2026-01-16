using System.Drawing;
using Sunny.UI;
using WinFormsApp1.second_menu.ChessRecognizer.Config;

namespace WinFormsApp1.second_menu.ChessRecognizer.Forms
{
    /// <summary>
    /// 设置窗体
    /// </summary>
    public partial class FrmSettings : UIForm
    {
        public AppSettings Settings { get; private set; }

        public FrmSettings(AppSettings settings)
        {
            Settings = settings ?? new AppSettings();
            InitializeComponent();
            InitializeComboBoxItems();
            LoadSettings();
            SetupEventHandlers();
        }

        private void InitializeComboBoxItems()
        {
            cboDefaultEngine.Items.AddRange(new object[] { "Tesseract", "SimpleOCR" });
            cboAutoCorrection.Items.AddRange(new object[] { "关闭", "低", "中", "高" });
        }

        private void SetupEventHandlers()
        {
            trkConfidence.ValueChanged += (s, e) => lblConfidenceValue.Text = $"{trkConfidence.Value}%";
        }

        private void LoadSettings()
        {
            // OCR设置
            cboDefaultEngine.SelectedItem = Settings.Ocr.DefaultEngine;
            if (cboDefaultEngine.SelectedIndex < 0 && cboDefaultEngine.Items.Count > 0)
                cboDefaultEngine.SelectedIndex = 0;
            
            trkConfidence.Value = Math.Max(trkConfidence.Minimum, 
                Math.Min(trkConfidence.Maximum, (int)(Settings.Ocr.ConfidenceThreshold * 100)));
            lblConfidenceValue.Text = $"{trkConfidence.Value}%";
            
            chkPreprocessing.Checked = Settings.Ocr.EnablePreprocessing;
            chkBinarization.Checked = Settings.Ocr.EnableBinarization;
            chkDenoising.Checked = Settings.Ocr.EnableDenoising;
            chkSharpening.Checked = Settings.Ocr.EnableSharpening;
            chkMultiEngine.Checked = Settings.Ocr.EnableMultiEngineVoting;
            chkPositionGuessing.Checked = Settings.Ocr.EnablePositionGuessing;

            // 棋盘检测设置
            trkCanny1.Value = Math.Max(trkCanny1.Minimum, 
                Math.Min(trkCanny1.Maximum, (int)Settings.BoardDetection.CannyThreshold1));
            trkCanny2.Value = Math.Max(trkCanny2.Minimum, 
                Math.Min(trkCanny2.Maximum, (int)Settings.BoardDetection.CannyThreshold2));
            trkHough.Value = Math.Max(trkHough.Minimum, 
                Math.Min(trkHough.Maximum, Settings.BoardDetection.HoughThreshold));
            trkPieceScale.Value = Math.Max(trkPieceScale.Minimum, 
                Math.Min(trkPieceScale.Maximum, (int)(Settings.BoardDetection.PieceRegionScale * 100)));

            // FEN设置
            chkRuleValidation.Checked = Settings.Fen.EnableRuleValidation;
            cboAutoCorrection.SelectedIndex = Math.Max(0, 
                Math.Min(cboAutoCorrection.Items.Count - 1, Settings.Fen.AutoCorrectionLevel));
            chkDefaultRedTurn.Checked = Settings.Fen.DefaultRedTurn;

            // 性能设置
            trkThreads.Value = Math.Max(trkThreads.Minimum, 
                Math.Min(trkThreads.Maximum, Settings.Performance.ParallelThreads));
            trkMaxWidth.Value = Math.Max(trkMaxWidth.Minimum, 
                Math.Min(trkMaxWidth.Maximum, Settings.Performance.MaxImageWidth));
            trkTimeout.Value = Math.Max(trkTimeout.Minimum, 
                Math.Min(trkTimeout.Maximum, Settings.Performance.TimeoutSeconds));

            // 界面设置
            chkAutoCopyFEN.Checked = Settings.UI.AutoCopyFEN;
            chkAutoSave.Checked = Settings.UI.AutoSaveResult;
            trkRecentFiles.Value = Math.Max(trkRecentFiles.Minimum, 
                Math.Min(trkRecentFiles.Maximum, Settings.UI.RecentFilesCount));
        }

        private void SaveSettings()
        {
            // OCR设置
            Settings.Ocr.DefaultEngine = cboDefaultEngine.SelectedItem?.ToString() ?? "Tesseract";
            Settings.Ocr.ConfidenceThreshold = trkConfidence.Value / 100.0;
            Settings.Ocr.EnablePreprocessing = chkPreprocessing.Checked;
            Settings.Ocr.EnableBinarization = chkBinarization.Checked;
            Settings.Ocr.EnableDenoising = chkDenoising.Checked;
            Settings.Ocr.EnableSharpening = chkSharpening.Checked;
            Settings.Ocr.EnableMultiEngineVoting = chkMultiEngine.Checked;
            Settings.Ocr.EnablePositionGuessing = chkPositionGuessing.Checked;

            // 棋盘检测设置
            Settings.BoardDetection.CannyThreshold1 = trkCanny1.Value;
            Settings.BoardDetection.CannyThreshold2 = trkCanny2.Value;
            Settings.BoardDetection.HoughThreshold = trkHough.Value;
            Settings.BoardDetection.PieceRegionScale = trkPieceScale.Value / 100.0;

            // FEN设置
            Settings.Fen.EnableRuleValidation = chkRuleValidation.Checked;
            Settings.Fen.AutoCorrectionLevel = cboAutoCorrection.SelectedIndex;
            Settings.Fen.DefaultRedTurn = chkDefaultRedTurn.Checked;

            // 性能设置
            Settings.Performance.ParallelThreads = trkThreads.Value;
            Settings.Performance.MaxImageWidth = trkMaxWidth.Value;
            Settings.Performance.MaxImageHeight = trkMaxWidth.Value;
            Settings.Performance.TimeoutSeconds = trkTimeout.Value;

            // 界面设置
            Settings.UI.AutoCopyFEN = chkAutoCopyFEN.Checked;
            Settings.UI.AutoSaveResult = chkAutoSave.Checked;
            Settings.UI.RecentFilesCount = trkRecentFiles.Value;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要重置所有设置为默认值吗？", "确认", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Settings.Reset();
                LoadSettings();
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog
            {
                Filter = "JSON文件|*.json",
                FileName = "chess_recognizer_settings.json"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SaveSettings();
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(Settings, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(dialog.FileName, json);
                    MessageBox.Show("设置已导出", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"导出失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "JSON文件|*.json"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string json = File.ReadAllText(dialog.FileName);
                    Settings = Newtonsoft.Json.JsonConvert.DeserializeObject<AppSettings>(json);
                    LoadSettings();
                    MessageBox.Show("设置已导入", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"导入失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
