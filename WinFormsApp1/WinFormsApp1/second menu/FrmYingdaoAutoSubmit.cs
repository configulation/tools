using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using Sunny.UI;

namespace WinFormsApp1.second_menu
{
	public partial class FrmYingdaoAutoSubmit : UIForm
	{
		private const string TargetUrl = "https://esquel.yingdaoapps.com/public/app/821296610254688256/%E8%BD%A6%E4%BD%8D%E6%8A%BD%E7%AD%BE/defaultPage";

		private readonly string configFilePath;
		private AppConfig config;
		private bool isRunning;
		private bool isLoadingSettings;
		private System.Windows.Forms.Timer dailyTimer;
		private int currentUserIndex = -1;

		public FrmYingdaoAutoSubmit()
		{
			InitializeComponent();

			string cfgDir = Path.Combine(Application.StartupPath, "Config");
			configFilePath = Path.Combine(cfgDir, "yingdao_config.json");
			
			InitializeDataGridView();
			LoadConfig();
			LoadConfigToUI();
			
			// 初始化定时器
			dailyTimer = new System.Windows.Forms.Timer();
			dailyTimer.Interval = 60000; // 1分钟
			dailyTimer.Tick += DailyTimer_Tick;
		}

		private void InitializeDataGridView()
		{
			dgvUsers.Columns.Clear();
			dgvUsers.Columns.Add(new DataGridViewCheckBoxColumn
			{
				Name = "colEnabled",
				HeaderText = "启用",
				Width = 50
			});
			dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
			{
				Name = "colFactory",
				HeaderText = "厂区",
				Width = 80
			});
			dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
			{
				Name = "colEmployeeId",
				HeaderText = "员工号",
				Width = 100
			});
			dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
			{
				Name = "colPhone",
				HeaderText = "手机号",
				Width = 120
			});
			dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
			{
				Name = "colCarNo",
				HeaderText = "车牌号",
				Width = 100
			});
			dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
			{
				Name = "colLastDate",
				HeaderText = "上次执行",
				Width = 100,
				ReadOnly = true
			});
			dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
			{
				Name = "colLastResult",
				HeaderText = "执行结果",
				Width = 80,
				ReadOnly = true
			});
		}

		private async void FrmYingdaoAutoSubmit_Load(object sender, EventArgs e)
		{
			dailyTimer.Start();
			string daysDesc = GetEnabledDaysDescription();
			AppendLog($"定时器已启动，每分钟检查执行条件（{daysDesc} 0:00-13:30）");
			
			// 启动时检查
			if (config.AutoStartEnabled && ShouldRunToday() && IsInTimeWindow())
			{
				AppendLog("启动时检测到需要执行");
				await RunAllUsersAsync();
			}
		}

		private async void DailyTimer_Tick(object sender, EventArgs e)
		{
			if (!config.AutoStartEnabled) return;
			if (!ShouldRunToday()) return;
			if (!IsInTimeWindow()) return;
			
			// 检查是否有用户今天还没执行
			string today = DateTime.Today.ToString("yyyy-MM-dd");
			bool hasUnfinished = config.Users.Any(u => u.Enabled && u.LastSubmittedDate != today);
			
			if (hasUnfinished)
			{
				AppendLog("定时触发自动执行");
				await RunAllUsersAsync();
			}
		}

		/// <summary>
		/// 判断今天是否需要抽签（根据配置的星期）
		/// </summary>
		private bool ShouldRunToday()
		{
			int dayOfWeek = (int)DateTime.Today.DayOfWeek;
			return config.EnabledDays.Contains(dayOfWeek);
		}

		/// <summary>
		/// 获取启用的星期描述
		/// </summary>
		private string GetEnabledDaysDescription()
		{
			string[] dayNames = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
			var enabledNames = config.EnabledDays.OrderBy(d => d).Select(d => dayNames[d]);
			return string.Join("、", enabledNames);
		}

		/// <summary>
		/// 判断当前是否在可执行时间范围内（0:00 - 13:30）
		/// </summary>
		private bool IsInTimeWindow()
		{
			var now = DateTime.Now;
			var endTime = now.Date.AddHours(13).AddMinutes(30);
			return now <= endTime;
		}

		private async void btnRun_Click(object sender, EventArgs e)
		{
			await RunAllUsersAsync();
		}

		private void btnAddUser_Click(object sender, EventArgs e)
		{
			dgvUsers.Rows.Add(true, "总厂", "", "", "", "", "");
			AppendLog("已添加新用户行，请填写信息后点击保存");
		}

		private void btnDeleteUser_Click(object sender, EventArgs e)
		{
			if (dgvUsers.CurrentRow == null)
			{
				UIMessageTip.ShowWarning("请先选择要删除的用户");
				return;
			}
			
			int rowIndex = dgvUsers.CurrentRow.Index;
			if (rowIndex >= 0 && !dgvUsers.CurrentRow.IsNewRow)
			{
				dgvUsers.Rows.RemoveAt(rowIndex);
				AppendLog("已删除用户，请点击保存配置");
			}
		}

		private void btnSaveConfig_Click(object sender, EventArgs e)
		{
			SaveUIToConfig();
			SaveConfig();
			UIMessageTip.ShowOk("配置已保存");
			AppendLog("配置已保存到文件");
		}

		private void ChkAutoStart_CheckedChanged(object sender, EventArgs e)
		{
			if (isLoadingSettings) return;
			config.AutoStartEnabled = chkAutoStart.Checked;
			AppendLog($"自动执行已{(config.AutoStartEnabled ? "开启" : "关闭")}");
		}

		private void ChkDay_CheckedChanged(object sender, EventArgs e)
		{
			if (isLoadingSettings) return;
			// 星期选择变化时自动保存
			SaveDaysFromUI();
			string daysDesc = GetEnabledDaysDescription();
			AppendLog($"执行日期已更新：{daysDesc}");
		}

		private void SaveDaysFromUI()
		{
			config.EnabledDays.Clear();
			if (chkSunday.Checked) config.EnabledDays.Add(0);
			if (chkMonday.Checked) config.EnabledDays.Add(1);
			if (chkTuesday.Checked) config.EnabledDays.Add(2);
			if (chkWednesday.Checked) config.EnabledDays.Add(3);
			if (chkThursday.Checked) config.EnabledDays.Add(4);
			if (chkFriday.Checked) config.EnabledDays.Add(5);
			if (chkSaturday.Checked) config.EnabledDays.Add(6);
		}

		private void LoadDaysToUI()
		{
			chkSunday.Checked = config.EnabledDays.Contains(0);
			chkMonday.Checked = config.EnabledDays.Contains(1);
			chkTuesday.Checked = config.EnabledDays.Contains(2);
			chkWednesday.Checked = config.EnabledDays.Contains(3);
			chkThursday.Checked = config.EnabledDays.Contains(4);
			chkFriday.Checked = config.EnabledDays.Contains(5);
			chkSaturday.Checked = config.EnabledDays.Contains(6);
		}

		private async Task RunAllUsersAsync()
		{
			if (isRunning)
			{
				AppendLog("已有任务在执行中，请稍候");
				return;
			}

			if (!ShouldRunToday())
			{
				string daysDesc = GetEnabledDaysDescription();
				AppendLog($"今天是{DateTime.Today.DayOfWeek}，不需要抽签（仅{daysDesc}）");
				return;
			}

			if (!IsInTimeWindow())
			{
				AppendLog("当前时间超过13:30，已过抽签截止时间");
				return;
			}

			isRunning = true;
			btnRun.Enabled = false;

			try
			{
				string today = DateTime.Today.ToString("yyyy-MM-dd");
				var enabledUsers = config.Users.Where(u => u.Enabled).ToList();
				
				if (enabledUsers.Count == 0)
				{
					AppendLog("没有启用的用户");
					return;
				}

				AppendLog($"开始执行，共 {enabledUsers.Count} 个用户");

				for (int i = 0; i < enabledUsers.Count; i++)
				{
					var user = enabledUsers[i];
					currentUserIndex = config.Users.IndexOf(user);
					
					if (user.LastSubmittedDate == today)
					{
						AppendLog($"用户 {user.EmployeeId} 今天已执行过，跳过");
						continue;
					}

					AppendLog($"--- 开始处理用户 {i + 1}/{enabledUsers.Count}: {user.EmployeeId} ---");
					
					bool success = await RunSingleUserAsync(user);
					
					if (success)
					{
						user.LastSubmittedDate = today;
						user.LastResult = "成功";
					}
					else
					{
						user.LastResult = "失败";
					}
					
					// 更新UI
					UpdateUserRowStatus(currentUserIndex, user);
					SaveConfig();

					// 用户之间间隔
					if (i < enabledUsers.Count - 1)
					{
						AppendLog("等待5秒后处理下一个用户...");
						await Task.Delay(5000);
					}
				}

				AppendLog("所有用户处理完成");
			}
			catch (Exception ex)
			{
				AppendLog($"执行异常：{ex.Message}");
			}
			finally
			{
				isRunning = false;
				btnRun.Enabled = true;
				currentUserIndex = -1;
			}
		}

		private async Task<bool> RunSingleUserAsync(UserConfig user)
		{
			try
			{
				AppendLog("初始化浏览器...");
				if (!await EnsureWebViewReadyAsync()) return false;

				AppendLog($"打开页面...");
				bool navOk = await NavigateAsync(TargetUrl);
				if (!navOk)
				{
					AppendLog("页面加载失败");
					return false;
				}

				await Task.Delay(2000);

				// 检查表单是否已填写
				var alreadyFilled = await IsFormAlreadyFilledAsync();
				if (alreadyFilled)
				{
					AppendLog("表单已有数据，跳过填充");
				}
				else
				{
					await FillFormAsync(user);
					var nameOk = await WaitEmployeeNameOkAsync();
					if (!nameOk)
					{
						AppendLog("员工姓名未加载成功，本次不提交");
						return false;
					}
				}

				// 查找提交按钮
				var probe = await ProbeSubmitButtonAsync();
				if (probe == null || !probe.Found)
				{
					AppendLog("未找到提交按钮");
					return false;
				}

				AppendLog($"按钮状态：disabled={probe.Disabled} visible={probe.Visible}");

				if (probe.Disabled)
				{
					AppendLog("按钮已禁用，可能已提交或未填写完整");
					return false;
				}

				AppendLog("点击提交...");
				var click = await ClickSubmitButtonAsync();
				if (click?.Clicked == true)
				{
					AppendLog("提交成功");
					await Task.Delay(2000);
					return true;
				}
				else
				{
					AppendLog($"提交失败：{click?.Reason}");
					return false;
				}
			}
			catch (Exception ex)
			{
				AppendLog($"处理用户异常：{ex.Message}");
				return false;
			}
		}

		private void UpdateUserRowStatus(int index, UserConfig user)
		{
			if (index < 0 || index >= dgvUsers.Rows.Count) return;
			
			if (InvokeRequired)
			{
				BeginInvoke(new Action(() => UpdateUserRowStatus(index, user)));
				return;
			}

			dgvUsers.Rows[index].Cells["colLastDate"].Value = user.LastSubmittedDate;
			dgvUsers.Rows[index].Cells["colLastResult"].Value = user.LastResult;
		}


		#region 配置管理

		private void LoadConfig()
		{
			try
			{
				if (File.Exists(configFilePath))
				{
					string json = File.ReadAllText(configFilePath, Encoding.UTF8);
					config = JsonConvert.DeserializeObject<AppConfig>(json) ?? new AppConfig();
				}
				else
				{
					config = new AppConfig();
				}
			}
			catch
			{
				config = new AppConfig();
			}
		}

		private void SaveConfig()
		{
			try
			{
				string dir = Path.GetDirectoryName(configFilePath);
				if (!string.IsNullOrWhiteSpace(dir)) Directory.CreateDirectory(dir);
				string json = JsonConvert.SerializeObject(config, Formatting.Indented);
				File.WriteAllText(configFilePath, json, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				AppendLog($"保存配置失败：{ex.Message}");
			}
		}

		private void LoadConfigToUI()
		{
			isLoadingSettings = true;
			try
			{
				chkAutoStart.Checked = config.AutoStartEnabled;
				LoadDaysToUI();
				
				dgvUsers.Rows.Clear();
				foreach (var user in config.Users)
				{
					dgvUsers.Rows.Add(
						user.Enabled,
						user.Factory,
						user.EmployeeId,
						user.Phone,
						user.CarNo,
						user.LastSubmittedDate,
						user.LastResult
					);
				}
			}
			finally
			{
				isLoadingSettings = false;
			}
		}

		private void SaveUIToConfig()
		{
			config.AutoStartEnabled = chkAutoStart.Checked;
			SaveDaysFromUI();
			config.Users.Clear();
			
			foreach (DataGridViewRow row in dgvUsers.Rows)
			{
				if (row.IsNewRow) continue;
				
				config.Users.Add(new UserConfig
				{
					Enabled = Convert.ToBoolean(row.Cells["colEnabled"].Value ?? false),
					Factory = row.Cells["colFactory"].Value?.ToString() ?? "",
					EmployeeId = row.Cells["colEmployeeId"].Value?.ToString() ?? "",
					Phone = row.Cells["colPhone"].Value?.ToString() ?? "",
					CarNo = row.Cells["colCarNo"].Value?.ToString() ?? "",
					LastSubmittedDate = row.Cells["colLastDate"].Value?.ToString() ?? "",
					LastResult = row.Cells["colLastResult"].Value?.ToString() ?? ""
				});
			}
		}

		#endregion

		#region 表单操作

		private async Task FillFormAsync(UserConfig user)
		{
			// 先选择厂区
			if (!string.IsNullOrWhiteSpace(user.Factory))
			{
				await SelectFactoryAsync(user.Factory);
				await Task.Delay(500);
			}

			// 填写员工号
			if (!string.IsNullOrWhiteSpace(user.EmployeeId))
			{
				await SetInputValueAsync("txtEmployeeID", user.EmployeeId);
				AppendLog("已填写员工号，等待姓名加载...");
				await Task.Delay(3500);
			}

			// 填写手机号
			if (!string.IsNullOrWhiteSpace(user.Phone))
			{
				await SetInputValueAsync("txtPhoneNo", user.Phone);
				await Task.Delay(300);
			}

			// 填写车牌号
			if (!string.IsNullOrWhiteSpace(user.CarNo))
			{
				await SetInputValueAsync("txtCarNo", user.CarNo);
				await Task.Delay(300);
			}
		}

		private async Task SetInputValueAsync(string widgetId, string value)
		{
			string escapedValue = JsEscape(value);
			string script = $@"
				(function() {{
					var selector = '[data-widget-id=""{widgetId}""]';
					var root = document.querySelector(selector);
					if (!root) return false;
					var input = root.querySelector('input');
					if (!input) return false;

					var nativeInputValueSetter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set;
					nativeInputValueSetter.call(input, '{escapedValue}');

					var inputEvent = new Event('input', {{ bubbles: true, cancelable: true }});
					input.dispatchEvent(inputEvent);

					var changeEvent = new Event('change', {{ bubbles: true, cancelable: true }});
					input.dispatchEvent(changeEvent);

					input.blur();
					return true;
				}})();";

			await web.ExecuteScriptAsync(script);
		}

		private async Task SelectFactoryAsync(string factoryName)
		{
			AppendLog($"选择厂区: {factoryName}");

			// 点击打开弹窗
			string openScript = @"
				(function() {
					var root = document.querySelector('[data-widget-id=""cmbFty""]');
					if (!root) return 'no_widget';
					
					var selectBox = root.querySelector('.min-h-12.border');
					if (selectBox) {
						selectBox.click();
						return 'clicked_selectbox';
					}
					
					var arrowArea = root.querySelector('.anticon-down');
					if (arrowArea) {
						var clickTarget = arrowArea.closest('.min-h-12') || arrowArea.parentElement;
						if (clickTarget) {
							clickTarget.click();
							return 'clicked_arrow_area';
						}
					}
					
					root.click();
					return 'clicked_root';
				})();";

			string openResult = await web.ExecuteScriptAsync(openScript);
			AppendLog($"打开弹窗: {openResult}");

			// 等待弹窗
			for (int i = 0; i < 25; i++)
			{
				await Task.Delay(200);
				string checkScript = @"
					(function() {
						var popup = document.querySelector('.adm-popup-body');
						if (popup && popup.offsetHeight > 0) {
							var checkboxes = popup.querySelectorAll('label.adm-checkbox');
							return 'visible:' + checkboxes.length;
						}
						return 'hidden';
					})();";
				string waitResult = await web.ExecuteScriptAsync(checkScript);
				if (waitResult.Contains("visible"))
				{
					AppendLog($"弹窗已出现");
					break;
				}
			}

			await Task.Delay(500);

			// 选择厂区
			string escapedFactory = JsEscape(factoryName);
			string selectScript = $@"
				(function() {{
					var target = '{escapedFactory}'.trim();
					var popup = document.querySelector('.adm-popup-body');
					if (!popup) return 'no_popup';
					
					var rows = popup.querySelectorAll('.flex.items-center.justify-between');
					for (var i = 0; i < rows.length; i++) {{
						var row = rows[i];
						var nameDiv = row.querySelector('.flex-auto');
						if (nameDiv) {{
							var text = (nameDiv.innerText || nameDiv.textContent || '').trim();
							if (text === target) {{
								var checkbox = row.querySelector('input[type=""checkbox""]');
								if (checkbox) {{
									checkbox.click();
									return 'clicked:' + text;
								}}
							}}
						}}
					}}
					return 'not_found';
				}})();";

			string selectResult = await web.ExecuteScriptAsync(selectScript);
			AppendLog($"厂区选择: {selectResult}");
			await Task.Delay(500);

			// 点击确定
			string confirmScript = @"
				(function() {
					var popup = document.querySelector('.adm-popup-body');
					if (!popup) return 'no_popup';
					
					var textLgDivs = popup.querySelectorAll('.text-lg');
					for (var i = 0; i < textLgDivs.length; i++) {
						var div = textLgDivs[i];
						var txt = (div.innerText || div.textContent || '').trim();
						if (txt === '确定' || txt === '确认') {
							div.click();
							return 'confirmed';
						}
					}
					return 'no_confirm_btn';
				})();";

			string confirmResult = await web.ExecuteScriptAsync(confirmScript);
			AppendLog($"确定按钮: {confirmResult}");
		}

		#endregion


		#region WebView操作

		private async Task<bool> EnsureWebViewReadyAsync()
		{
			try
			{
				await web.EnsureCoreWebView2Async();
				web.CoreWebView2.Settings.AreDevToolsEnabled = true;
				return true;
			}
			catch (Exception ex)
			{
				AppendLog($"WebView2初始化失败：{ex.Message}");
				return false;
			}
		}

		private async Task<bool> NavigateAsync(string url)
		{
			if (string.IsNullOrWhiteSpace(url)) return false;

			var tcs = new TaskCompletionSource<bool>();
			void Handler(object sender, CoreWebView2NavigationCompletedEventArgs e)
			{
				web.NavigationCompleted -= Handler;
				tcs.TrySetResult(e.IsSuccess);
			}

			web.NavigationCompleted += Handler;
			try
			{
				web.Source = new Uri(url);
			}
			catch
			{
				web.NavigationCompleted -= Handler;
				return false;
			}

			var completed = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(60)));
			if (completed != tcs.Task)
			{
				web.NavigationCompleted -= Handler;
				return false;
			}
			return await tcs.Task;
		}

		private async Task<bool> IsFormAlreadyFilledAsync()
		{
			try
			{
				string script = @"
					(function () {
						function hasValueByWidgetId(widgetId) {
							var selector = '[data-widget-id=' + JSON.stringify(widgetId) + ']';
							var root = document.querySelector(selector);
							if (!root) return false;
							var input = root.querySelector('input');
							if (!input) return false;
							return input.value && input.value.trim().length > 0;
						}
						return hasValueByWidgetId('txtEmployeeID') || hasValueByWidgetId('txtPhoneNo') || hasValueByWidgetId('txtCarNo');
					})();";

				string result = await web.ExecuteScriptAsync(script);
				return JsonConvert.DeserializeObject<bool>(result);
			}
			catch
			{
				return false;
			}
		}

		private async Task<bool> HasEmployeeNameErrorAsync()
		{
			try
			{
				string script = @"
					(function () {
						var nodes = document.querySelectorAll('*');
						for (var i = 0; i < nodes.length; i++) {
							var t = (nodes[i].innerText || nodes[i].textContent || '').replace(/\s+/g, '').trim();
							if (t === '员工号不正确' || t === '本字段必填') return true;
						}
						return false;
					})();";

				string result = await web.ExecuteScriptAsync(script);
				return JsonConvert.DeserializeObject<bool>(result);
			}
			catch
			{
				return false;
			}
		}

		private async Task<bool> WaitEmployeeNameOkAsync()
		{
			for (int i = 0; i < 8; i++)
			{
				bool hasError = await HasEmployeeNameErrorAsync();
				if (!hasError) return true;
				await Task.Delay(1000);
			}
			return false;
		}

		private async Task<SubmitButtonProbeResult> ProbeSubmitButtonAsync()
		{
			try
			{
				string script = @"
					(function () {
						function isVisible(el) {
							if (!el) return false;
							var rect = el.getBoundingClientRect();
							if (rect.width <= 0 || rect.height <= 0) return false;
							var style = window.getComputedStyle(el);
							return style.display !== 'none' && style.visibility !== 'hidden' && style.opacity !== '0';
						}

						var candidates = Array.prototype.slice.call(document.querySelectorAll('button'))
							.concat(Array.prototype.slice.call(document.querySelectorAll('[role=button]')));
						
						var match = candidates.find(function (el) {
							var t = (el.innerText || '').trim();
							return t.indexOf('提交') >= 0;
						});

						if (!match) return { found: false };

						var className = (match.className || '').toLowerCase();
						var disabled = !!match.disabled || match.getAttribute('aria-disabled') === 'true' || className.indexOf('disabled') >= 0;

						return {
							found: true,
							text: (match.innerText || '').trim(),
							disabled: disabled,
							visible: isVisible(match)
						};
					})();";

				string result = await web.ExecuteScriptAsync(script);
				return JsonConvert.DeserializeObject<SubmitButtonProbeResult>(result);
			}
			catch
			{
				return null;
			}
		}

		private async Task<SubmitButtonClickResult> ClickSubmitButtonAsync()
		{
			try
			{
				string script = @"
					(function () {
						var candidates = Array.prototype.slice.call(document.querySelectorAll('button'))
							.concat(Array.prototype.slice.call(document.querySelectorAll('[role=button]')));
						
						var match = candidates.find(function (el) {
							var t = (el.innerText || '').trim();
							return t.indexOf('提交') >= 0;
						});

						if (!match) return { clicked: false, reason: 'not_found' };

						var className = (match.className || '').toLowerCase();
						var disabled = !!match.disabled || match.getAttribute('aria-disabled') === 'true' || className.indexOf('disabled') >= 0;

						if (disabled) return { clicked: false, reason: 'disabled' };

						match.click();
						return { clicked: true, reason: 'clicked' };
					})();";

				string result = await web.ExecuteScriptAsync(script);
				return JsonConvert.DeserializeObject<SubmitButtonClickResult>(result);
			}
			catch
			{
				return null;
			}
		}

		#endregion

		#region 辅助方法

		private static string JsEscape(string value)
		{
			if (string.IsNullOrEmpty(value)) return string.Empty;
			return value.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "").Replace("\n", "");
		}

		private void AppendLog(string line)
		{
			try
			{
				if (InvokeRequired)
				{
					BeginInvoke(new Action<string>(AppendLog), line);
					return;
				}
				txtLog.AppendText($"{DateTime.Now:HH:mm:ss} {line}{Environment.NewLine}");
			}
			catch { }
		}

		#endregion

		#region 数据模型

		private class AppConfig
		{
			public bool AutoStartEnabled { get; set; } = false;
			public List<UserConfig> Users { get; set; } = new List<UserConfig>();
			// 执行日期配置：周日=0, 周一=1, ..., 周六=6
			public List<int> EnabledDays { get; set; } = new List<int> { 0, 1, 2, 3, 4 }; // 默认周日-周四
		}

		private class UserConfig
		{
			public bool Enabled { get; set; } = true;
			public string Factory { get; set; } = "";
			public string EmployeeId { get; set; } = "";
			public string Phone { get; set; } = "";
			public string CarNo { get; set; } = "";
			public string LastSubmittedDate { get; set; } = "";
			public string LastResult { get; set; } = "";
		}

		private class SubmitButtonProbeResult
		{
			[JsonProperty("found")]
			public bool Found { get; set; }
			[JsonProperty("text")]
			public string Text { get; set; }
			[JsonProperty("disabled")]
			public bool Disabled { get; set; }
			[JsonProperty("visible")]
			public bool Visible { get; set; }
		}

		private class SubmitButtonClickResult
		{
			[JsonProperty("clicked")]
			public bool Clicked { get; set; }
			[JsonProperty("reason")]
			public string Reason { get; set; }
		}

		#endregion
	}
}
