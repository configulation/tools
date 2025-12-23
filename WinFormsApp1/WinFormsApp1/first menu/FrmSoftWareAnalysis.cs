using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace WinFormsApp1.first_menu
{
	public partial class FrmSoftWareAnalysis : UIForm
	{
		private readonly HttpClient http = new HttpClient();
		private DataTable sourceTable = null; // 原始数据
		private readonly BindingList<ModelConfig> models = new BindingList<ModelConfig>();
		private const string ConfigDir = "Config";
		private const string ConfigFile = "software_models.json";

		public FrmSoftWareAnalysis()
		{
			InitializeComponent();
			InitGrid();
			InitBatchSize();
			_ = LoadConfigAsync();
		}

		private void InitGrid()
		{
			gridModels.AutoGenerateColumns = false;
			if (gridModels.Columns.Count == 0)
			{
				gridModels.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModelConfig.Name), HeaderText = "模型名称", Width = 140 });
				gridModels.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModelConfig.ApiUrl), HeaderText = "API地址", Width = 260 });
				gridModels.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModelConfig.ApiKey), HeaderText = "API Key", Width = 220 });
				gridModels.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModelConfig.Model), HeaderText = "模型标识", Width = 150 });
				gridModels.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = nameof(ModelConfig.Enabled), HeaderText = "启用", Width = 60 });
			}
			gridModels.DataSource = models;
		}

		private void InitBatchSize()
		{
			cboBatchSize.Items.Clear();
			cboBatchSize.Items.AddRange(new object[] { 1, 2, 3, 10, 20,30, 60, 100, 200, 500, 1000, 2000, 5000 });
			cboBatchSize.SelectedIndex = 3;
		}

		private async Task LoadConfigAsync()
		{
			try
			{
				string path = Path.Combine(Application.StartupPath, ConfigDir, ConfigFile);
				if (File.Exists(path))
				{
					var json = File.ReadAllText(path, Encoding.UTF8);
					var list = JsonSerializer.Deserialize<List<ModelConfig>>(json) ?? new List<ModelConfig>();
					models.Clear();
					foreach (var m in list) models.Add(m);
				}
				else
				{
					// 提供两个示例模型位
					models.Add(new ModelConfig { Name = "Qwen", ApiUrl = "https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions", Model = "qwen-plus", Enabled = true });
					models.Add(new ModelConfig { Name = "DeepSeek", ApiUrl = "https://api.deepseek.com/v1/chat/completions", Model = "deepseek-chat", Enabled = false });
				}
			}
			catch (Exception ex)
			{
				ShowErrorTip($"读取配置失败: {ex.Message}");
			}
		}

		private async Task SaveConfigAsync()
		{
			try
			{
				string dir = Path.Combine(Application.StartupPath, ConfigDir);
				Directory.CreateDirectory(dir);
				string path = Path.Combine(dir, ConfigFile);
				var json = JsonSerializer.Serialize(models.ToList(), new JsonSerializerOptions { WriteIndented = true });
				File.WriteAllText(path, json, Encoding.UTF8);
				ShowSuccessTip("配置已保存");
			}
			catch (Exception ex)
			{
				ShowErrorTip($"保存配置失败: {ex.Message}");
			}
		}

		private void btnLoadConfig_Click(object sender, EventArgs e)
		{
			_ = LoadConfigAsync();
		}

		private void btnSaveConfig_Click(object sender, EventArgs e)
		{
			_ = SaveConfigAsync();
		}

		private void btnImportExcel_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog ofd = new OpenFileDialog())
			{
				ofd.Filter = "Excel|*.xlsx;*.xls";
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					txtExcelPath.Text = ofd.FileName;
					try
					{
						sourceTable = LoadExcel(ofd.FileName);
						ShowSuccessTip($"已加载 {sourceTable.Rows.Count} 行");
					}
					catch (Exception ex)
					{
						ShowErrorTip($"读取Excel失败: {ex.Message}");
					}
				}
			}
		}

		private DataTable LoadExcel(string file)
		{
			using var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			IWorkbook wb = Path.GetExtension(file).ToLower() == ".xls" ? (IWorkbook)new HSSFWorkbook(fs) : new XSSFWorkbook(fs);
			ISheet sheet = wb.NumberOfSheets > 0 ? wb.GetSheetAt(0) : null;
			if (sheet == null) throw new Exception("Excel无数据");
			DataTable dt = new DataTable();
			IRow header = sheet.GetRow(sheet.FirstRowNum);
			int cols = header.LastCellNum;
			for (int c = 0; c < cols; c++)
			{
				string name = header.GetCell(c)?.ToString();
				if (string.IsNullOrWhiteSpace(name)) name = $"列{c + 1}";
				dt.Columns.Add(name);
			}
			for (int r = sheet.FirstRowNum + 1; r <= sheet.LastRowNum; r++)
			{
				IRow row = sheet.GetRow(r);
				if (row == null) continue;
				var dr = dt.NewRow();
				for (int c = 0; c < cols; c++) dr[c] = row.GetCell(c)?.ToString();
				dt.Rows.Add(dr);
			}
			return dt;
		}

		private async void btnRun_Click(object sender, EventArgs e)
		{
			if (sourceTable == null || sourceTable.Rows.Count == 0)
			{
				ShowWarningTip("请先导入Excel");
				return;
			}
			var actives = models.Where(m => m.Enabled && !string.IsNullOrWhiteSpace(m.ApiUrl) && !string.IsNullOrWhiteSpace(m.Model)).ToList();
			if (actives.Count == 0)
			{
				ShowWarningTip("请在表格中至少启用一个模型并填写API信息");
				return;
			}

			int batch = Convert.ToInt32(cboBatchSize.SelectedItem ?? 5);
			// 每个模型生成一个Sheet(Tab)
			tabResults.TabPages.Clear();
			foreach (var m in actives)
			{
				var grid = BuildResultGridForModel(m);
				var page = new TabPage(m.Name) { BackColor = Color.White };
				page.Controls.Add(grid);
				tabResults.TabPages.Add(page);
				await AnalyzeAsync(m, sourceTable, grid, batch);
			}
		}

		private UIDataGridView BuildResultGridForModel(ModelConfig m)
		{
			var grid = new UIDataGridView
			{
				Dock = DockStyle.Fill,
				BackgroundColor = Color.White,
				Font = new Font("微软雅黑", 10.5f)
			};
            // 列头与示例保持一致：A:编号 B:软件名称 C:授权类型 D:是否主程序 E:是否需要监控 F:软件许可协议网页 G:Remark H:软件厂商
            grid.AutoGenerateColumns = false;
			grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "编号", Width = 90 });            // A
			grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "软件名称", Width = 260 });      // B
			grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "授权类型", Width = 150 });      // C
			grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "是否主程序", Width = 150 });    // D
			grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "是否需要监控", Width = 120 });    // E
			grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "软件许可协议网页", Width = 350 });// F
			grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Remark", Width = 220 });       // G（用于放收费URL或备注）
			grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "软件厂商", Width = 220 });      // H（原表H列，如存在）
			return grid;
		}

		private async Task AnalyzeAsync(ModelConfig model, DataTable table, UIDataGridView grid, int batch)
		{
			// 清空当前 grid 数据
			grid.Rows.Clear();

			// 准备请求：按批量把 B列(软件名) 聚合发送
			List<(int RowIndex, string Name)> items = new List<(int, string)>();
			for (int i = 0; i < table.Rows.Count; i++)
			{
				string name = Convert.ToString(table.Rows[i][1]); // B列 软件名称
				if (!string.IsNullOrWhiteSpace(name)) items.Add((i, name.Trim()));
			}

			// 若小于等于批量，直接一次处理，避免额外遍历
			if (items.Count <= batch)
			{
				var slice = items;
				string prompt = BuildPrompt(table, slice);
				string resp = await CallChatApiAsync(model, prompt);
				var parsed = ParseResult(resp, slice.Count);
				for (int k = 0; k < slice.Count; k++)
				{
					var row = grid.Rows.Add();
					string idVal = Convert.ToString(table.Rows[slice[k].RowIndex][0]); // A 编号
					string vendor = table.Columns.Count > 7 ? Convert.ToString(table.Rows[slice[k].RowIndex][7]) : string.Empty; // H 软件厂商（如有）
					grid.Rows[row].Cells[0].Value = idVal;                       // 编号
					grid.Rows[row].Cells[1].Value = slice[k].Name;               // 软件名称
					grid.Rows[row].Cells[2].Value = parsed[k].Category;          // 授权类型
					grid.Rows[row].Cells[3].Value = parsed[k].MasterOrBelong;    // 是否主程序（主程序=Y；否则填隶属名）
					grid.Rows[row].Cells[4].Value = parsed[k].NeedHuman;          // 是否需要监控（Y/空）
                    grid.Rows[row].Cells[5].Value = parsed[k].LicenseUrl;         // 软件许可协议网页
					grid.Rows[row].Cells[6].Value = parsed[k].PricingUrl;         // Remark（放收费URL或备注）
					grid.Rows[row].Cells[7].Value = vendor;                      // 软件厂商
				}
				return;
			}

			// 否则分批处理
			for (int i = 0; i < items.Count; i += batch)
			{
				int len = Math.Min(batch, items.Count - i);
				var slice = items.GetRange(i, len);
				string prompt = BuildPrompt(table, slice);
				string resp = await CallChatApiAsync(model, prompt);
				var parsed = ParseResult(resp, slice.Count);
				// 写入Grid
				for (int k = 0; k < slice.Count; k++)
				{
					var row = grid.Rows.Add();
					string idVal = Convert.ToString(table.Rows[slice[k].RowIndex][0]); // A 编号
					string vendor = table.Columns.Count > 7 ? Convert.ToString(table.Rows[slice[k].RowIndex][7]) : string.Empty; // H 软件厂商（如有）
					grid.Rows[row].Cells[0].Value = idVal;                       // 编号
					grid.Rows[row].Cells[1].Value = slice[k].Name;               // 软件名称
					grid.Rows[row].Cells[2].Value = parsed[k].Category;          // 授权类型
					grid.Rows[row].Cells[3].Value = parsed[k].MasterOrBelong;    // 是否主程序（主程序=Y；否则填隶属名）
					grid.Rows[row].Cells[4].Value = parsed[k].NeedHuman;          // 是否需要监控（Y/空）
                    grid.Rows[row].Cells[5].Value = parsed[k].LicenseUrl;         // 软件许可协议网页
					grid.Rows[row].Cells[6].Value = parsed[k].PricingUrl;         // Remark（放收费URL或备注）
					grid.Rows[row].Cells[7].Value = vendor;                      // 软件厂商
				}
				await Task.Delay(50); // 轻微间隔，避免限流
			}
		}

		private string BuildPrompt(DataTable table, List<(int RowIndex, string Name)> slice)
		{
			// 规则：
			// - C列取值：Commercial/Free/UnIdentified（完全免费才Free，存在条件或规模限制均归Commercial）
			// - D列：主程序 => Y；否则填隶属的软件名
			// - E列：模型不确定 => Y
			// - F列：软件许可协议URL
			// - G列：收费页面URL
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("你是软件合规分析助手。请严格按要求返回每一项的判断结果，格式为 JSON 数组，数组每个元素包含字段：Category, MasterOrBelong, NeedHuman, LicenseUrl, PricingUrl。不允许输出多余文字。");
			sb.AppendLine("判定规则：完全免费才标 Free；存在任何前提或规模限制等均标 Commercial；无法确定标 UnIdentified。");
			sb.AppendLine("MasterOrBelong：若该软件是主程序，则填 'Y'；否则填其隶属的软件名（来自原表D列）。");
			sb.AppendLine("NeedHuman：若你不确定 C列分类，请填 'Y'；否则为空字符串。");
			sb.AppendLine("LicenseUrl：给出该软件的许可协议网页URL；PricingUrl：给出该软件的收费页面URL。");
			sb.AppendLine("请联网检索后再判断，确保可靠性。");
			sb.AppendLine("待判断的软件列表（含原始D列信息）如下：");
			foreach (var it in slice)
			{
				string belong = Convert.ToString(table.Rows[it.RowIndex][3]); // D列
				sb.AppendLine($"- Name: {it.Name}; RawD: {belong}");
			}
			return sb.ToString();
		}

		private async Task<string> CallChatApiAsync(ModelConfig cfg, string prompt)
		{
			// 兼容阿里云DashScope/通用OpenAI兼容接口
			var req = new
			{
				model = cfg.Model,
				messages = new object[]
				{
					new { role = "user", content = prompt }
				},
				temperature = 0.2,
				max_tokens = 2000
			};
			string body = JsonSerializer.Serialize(req);
			using var content = new StringContent(body, Encoding.UTF8, "application/json");
			http.DefaultRequestHeaders.Clear();
			if (!string.IsNullOrWhiteSpace(cfg.ApiKey))
				http.DefaultRequestHeaders.Add("Authorization", $"Bearer {cfg.ApiKey}");
			try
			{
				var resp = await http.PostAsync(cfg.ApiUrl, content);
				resp.EnsureSuccessStatusCode();
				return await resp.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				// 若抛异常继续执行
				ShowErrorTip($"API调用失败: {ex.Message}");
				return $"{{\"choices\":[{{\"message\":{{\"content\":\"API调用错误: {ex.Message}\"}}}}]}}";
			}
		}

		private List<ItemResult> ParseResult(string response, int expect)
		{
			try
			{
				using var doc = JsonDocument.Parse(response);
				if (doc.RootElement.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
				{
					var msg = choices[0].GetProperty("message");
					string content = msg.GetProperty("content").GetString();
					var inner = content == null ? string.Empty : content.Trim();
					if (string.IsNullOrEmpty(inner)) return MakeFallback(expect);

					// 1) 去除markdown代码块如```json ... ```
					if (inner.StartsWith("```"))
					{
						int firstFence = inner.IndexOf("```", StringComparison.Ordinal);
						int secondFence = inner.IndexOf("```", firstFence + 3, StringComparison.Ordinal);
						if (firstFence >= 0)
						{
							int start = firstFence + 3;
							string fenced = secondFence > start ? inner.Substring(start, secondFence - start) : inner.Substring(start);
							inner = fenced.Trim();
						}
					}

					// 2) 去掉前导的语言标记（json/JSON）
					if (inner.StartsWith("json", StringComparison.OrdinalIgnoreCase))
					{
						int nl = inner.IndexOf('\n');
						if (nl >= 0) inner = inner.Substring(nl + 1).Trim();
					}

					// 3) 提取第一个JSON结构（数组优先，其次对象）
					string extracted = inner;
					int lb = inner.IndexOf('[');
					int rb = inner.LastIndexOf(']');
					if (lb >= 0 && rb > lb)
					{
						extracted = inner.Substring(lb, rb - lb + 1);
					}
					else
					{
						int lo = inner.IndexOf('{');
						int ro = inner.LastIndexOf('}');
						if (lo >= 0 && ro > lo)
						{
							string obj = inner.Substring(lo, ro - lo + 1);
							extracted = "[" + obj + "]"; // 单对象容错
						}
						else
						{
							return MakeFallback(expect);
						}
					}

					var arr = JsonSerializer.Deserialize<List<ItemResult>>(extracted);
					if (arr == null || arr.Count == 0) return MakeFallback(expect);
					// 长度对齐
					if (arr.Count < expect)
					{
						while (arr.Count < expect) arr.Add(new ItemResult());
					}
					else if (arr.Count > expect)
					{
						arr = arr.Take(expect).ToList();
					}
					return arr;
				}
			}
			catch
			{
				// ignore
			}
			return MakeFallback(expect);
		}

		private List<ItemResult> MakeFallback(int n)
		{
			var list = new List<ItemResult>();
			for (int i = 0; i < n; i++) list.Add(new ItemResult());
			return list;
		}

		private void btnExportCurrent_Click(object sender, EventArgs e)
		{
			if (tabResults.TabPages.Count == 0) { ShowWarningTip("无可导出的结果"); return; }
			var page = tabResults.SelectedTab;
			var grid = page?.Controls.OfType<UIDataGridView>().FirstOrDefault();
			if (grid == null || grid.Rows.Count == 0) { ShowWarningTip("当前页无数据"); return; }

			using (SaveFileDialog sfd = new SaveFileDialog())
			{
				sfd.Filter = "Excel|*.xlsx";
				sfd.FileName = $"{page.Text}_结果_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					ExportGridToExcel(grid, sfd.FileName);
					ShowSuccessTip("导出成功");
				}
			}
		}

		private void ExportGridToExcel(UIDataGridView grid, string file)
		{
			IWorkbook wb = new XSSFWorkbook();
			ISheet sheet = wb.CreateSheet("Result");
			IRow header = sheet.CreateRow(0);
			for (int c = 0; c < grid.Columns.Count; c++) header.CreateCell(c).SetCellValue(grid.Columns[c].HeaderText);
			for (int r = 0; r < grid.Rows.Count; r++)
			{
				var row = sheet.CreateRow(r + 1);
				for (int c = 0; c < grid.Columns.Count; c++)
				{
					var val = grid.Rows[r].Cells[c].Value?.ToString() ?? string.Empty;
					row.CreateCell(c).SetCellValue(val);
				}
			}
			using var fs = new FileStream(file, FileMode.Create, FileAccess.Write);
			wb.Write(fs);
		}

		private class ModelConfig
		{
			public string Name { get; set; }
			public string ApiUrl { get; set; }
			public string ApiKey { get; set; }
			public string Model { get; set; }
			public bool Enabled { get; set; }
		}

		private class ItemResult
		{
			public string Category { get; set; } = "";        // Commercial/Free/UnIdentified
			public string MasterOrBelong { get; set; } = "";  // Y 或 隶属名
			public string NeedHuman { get; set; } = "";        // Y 或 空
			public string LicenseUrl { get; set; } = "";
			public string PricingUrl { get; set; } = "";       // 收费页
		}
	}
}
