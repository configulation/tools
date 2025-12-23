using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace WinFormsApp1.first_menu
{
    public partial class FrmFenRecognition : UIForm
    {
        private const string DefaultServerUrl = "http://127.0.0.1:5000/api/upload";
        private const string DefaultParamJson = "{\"platform\": \"JJ\", \"autoModel\": \"Off\"}";

        private static readonly HttpClient httpClient = CreateHttpClient();
        private readonly DataTable boardTable = new DataTable("Board");

        public FrmFenRecognition()
        {
            InitializeComponent();
        }

        private static HttpClient CreateHttpClient()
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(120)
            };
            return client;
        }

        private void FrmFenRecognition_Load(object sender, EventArgs e)
        {
            InitBoardGrid();

            if (string.IsNullOrWhiteSpace(txtServerUrl.Text))
            {
                txtServerUrl.Text = DefaultServerUrl;
            }

            if (string.IsNullOrWhiteSpace(txtParam.Text))
            {
                txtParam.Text = DefaultParamJson;
            }

            AppendLog("请确保 Python 识别服务已启动，并能够访问上传接口。");
        }

        private void InitBoardGrid()
        {
            if (boardTable.Columns.Count == 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    string header = ((char)('A' + i)).ToString();
                    boardTable.Columns.Add(header, typeof(string));
                }
            }

            gridBoard.AutoGenerateColumns = false;
            gridBoard.Columns.Clear();

            foreach (DataColumn column in boardTable.Columns)
            {
                gridBoard.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = column.ColumnName,
                    HeaderText = column.ColumnName,
                    Width = 60,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                });
            }

            gridBoard.DataSource = boardTable;
            ResetBoardGrid();
        }

        private void ResetBoardGrid()
        {
            boardTable.Rows.Clear();
            for (int i = 0; i < 10; i++)
            {
                boardTable.Rows.Add(ParseFenRow(string.Empty));
            }
        }

        private object[] ParseFenRow(string fenRow)
        {
            var cells = new List<object>(9);
            foreach (char ch in fenRow ?? string.Empty)
            {
                if (char.IsDigit(ch))
                {
                    int count = ch - '0';
                    for (int i = 0; i < count && cells.Count < 9; i++)
                    {
                        cells.Add("-");
                    }
                }
                else if (!char.IsWhiteSpace(ch))
                {
                    if (cells.Count < 9)
                    {
                        cells.Add(ch.ToString());
                    }
                }

                if (cells.Count >= 9)
                {
                    break;
                }
            }

            while (cells.Count < 9)
            {
                cells.Add("-");
            }

            return cells.ToArray();
        }

        private void UpdateBoardGridFromFen(string fen)
        {
            boardTable.Rows.Clear();

            if (string.IsNullOrWhiteSpace(fen))
            {
                ResetBoardGrid();
                return;
            }

            string boardPart = fen.Split(' ')[0];
            string[] rows = boardPart.Split('/');
            if (rows.Length != 10)
            {
                AppendLog($"FEN 格式不正确，期望 10 行，实际 {rows.Length}: {boardPart}");
                ResetBoardGrid();
                return;
            }

            foreach (var row in rows)
            {
                boardTable.Rows.Add(ParseFenRow(row));
            }
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "图像文件|*.png;*.jpg;*.jpeg;*.bmp;*.webp|所有文件|*.*";
                dialog.Title = "选择待识别的棋盘图片";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtImagePath.Text = dialog.FileName;
                    SetPreviewImage(dialog.FileName);
                    AppendLog("已选择图片: " + dialog.FileName);
                }
            }
        }

        private void SetPreviewImage(string path)
        {
            picturePreview.Image?.Dispose();
            picturePreview.Image = null;

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return;
            }

            try
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var source = Image.FromStream(fs))
                {
                    picturePreview.Image = new Bitmap(source);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"加载图片失败: {ex.Message}");
                ShowWarningTip("无法加载图片，请确认文件未被占用。");
            }
        }

        private void btnOpenImageLocation_Click(object sender, EventArgs e)
        {
            string path = txtImagePath.Text.Trim();
            if (!File.Exists(path))
            {
                ShowWarningTip("请选择已存在的图片文件。");
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = $"/select,\"{path}\"",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                AppendLog($"打开文件所在位置失败: {ex.Message}");
                ShowWarningTip("无法打开资源管理器。");
            }
        }

        private async void btnRecognize_Click(object sender, EventArgs e)
        {
            string imagePath = txtImagePath.Text.Trim();
            if (!File.Exists(imagePath))
            {
                ShowWarningTip("请先选择需要识别的棋盘图片。");
                return;
            }

            string urlText = txtServerUrl.Text.Trim();
            if (!Uri.TryCreate(urlText, UriKind.Absolute, out var requestUri))
            {
                ShowWarningTip("请输入有效的识别服务地址。");
                return;
            }

            string paramText = txtParam.Text.Trim();
            if (string.IsNullOrWhiteSpace(paramText))
            {
                paramText = "{}";
            }
            else
            {
                try
                {
                    JToken.Parse(paramText);
                }
                catch (Exception ex)
                {
                    ShowWarningTip($"识别参数不是有效的 JSON: {ex.Message}");
                    return;
                }
            }

            btnRecognize.Enabled = false;
            AppendLog($"开始识别：{Path.GetFileName(imagePath)}");

            try
            {
                var responseJson = await RecognizeImageAsync(requestUri, imagePath, paramText);
                HandleRecognitionResponse(responseJson);
            }
            catch (Exception ex)
            {
                AppendLog($"识别失败: {ex.Message}");
                string message = ex.Message.Length > 180 ? ex.Message.Substring(0, 180) + "..." : ex.Message;
                ShowErrorTip(message);
            }
            finally
            {
                btnRecognize.Enabled = true;
            }
        }

        private async Task<JObject> RecognizeImageAsync(Uri requestUri, string imagePath, string paramJson)
        {
            using var content = new MultipartFormDataContent();
            using var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            content.Add(fileContent, "image", Path.GetFileName(imagePath));

            var paramContent = new StringContent(paramJson, Encoding.UTF8);
            paramContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            content.Add(paramContent, "param");

            using var response = await httpClient.PostAsync(requestUri, content);
            string payload = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"识别接口返回 {(int)response.StatusCode}: {payload}");
            }

            try
            {
                return JObject.Parse(payload);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("解析识别结果失败: " + ex.Message + Environment.NewLine + payload, ex);
            }
        }

        private void HandleRecognitionResponse(JObject responseJson)
        {
            if (responseJson == null)
            {
                AppendLog("识别返回空结果。");
                ShowWarningTip("识别返回结果为空。");
                return;
            }

            bool success = responseJson.Value<bool?>("success") ?? false;
            if (!success)
            {
                string message = responseJson.Value<string>("message") ?? "识别失败";
                AppendLog("识别失败: " + message);
                ShowWarningTip(message);
                return;
            }

            var resultToken = responseJson["result"] ?? responseJson["recognition_result"];
            if (resultToken == null)
            {
                AppendLog("识别结果缺少 result 字段。");
                ShowWarningTip("识别结果格式不符合预期。");
                return;
            }

            string fen = resultToken.Value<string>("fen") ?? string.Empty;
            txtFen.Text = fen;

            txtMove.Text = resultToken.Value<string>("move") ?? string.Empty;
            txtChineseMove.Text = resultToken.Value<string>("chinese_move") ?? string.Empty;
            txtSide.Text = resultToken.Value<string>("is_move") ?? string.Empty;
            txtScore.Text = resultToken["score"]?.ToString();
            txtWinRate.Text = resultToken["win_rate"]?.ToString();

            UpdateBoardGridFromFen(fen);

            AppendLog("识别成功。");
            if (!string.IsNullOrWhiteSpace(fen))
            {
                AppendLog("FEN: " + fen);
            }

            ShowSuccessTip("识别完成");
        }

        private void btnCopyFen_Click(object sender, EventArgs e)
        {
            string fen = txtFen.Text.Trim();
            if (string.IsNullOrEmpty(fen))
            {
                ShowWarningTip("当前没有可复制的 FEN 结果。");
                return;
            }

            try
            {
                Clipboard.SetText(fen);
                ShowSuccessTip("FEN 已复制到剪贴板");
            }
            catch (Exception ex)
            {
                AppendLog($"复制 FEN 失败: {ex.Message}");
                ShowWarningTip("复制失败，请手动选择文本复制。");
            }
        }

        private void AppendLog(string message)
        {
            if (IsDisposed || rtbLog.IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(AppendLog), message);
                return;
            }

            string line = $"[{DateTime.Now:HH:mm:ss}] {message}";
            rtbLog.AppendText(line + Environment.NewLine);
            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.ScrollToCaret();
        }
    }
}



