using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using Sunny.UI;
using WinFormsApp1.Common.Database.Services;

namespace WinFormsApp1.second_menu
{
	public partial class FrmYingdaoAutoSubmit : UIForm
	{
		private const string TargetUrl = "https://esquel.yingdaoapps.com/public/app/821296610254688256/%E8%BD%A6%E4%BD%8D%E6%8A%BD%E7%AD%BE/defaultPage";

		private readonly YingdaoConfigService configService;
		private bool isRunning;
		private bool isLoadingSettings;
		private System.Windows.Forms.Timer dailyTimer;
		private int currentUserIndex = -1;
		
		// 标记待删除的行（员工号列表）
		private readonly HashSet<string> pendingDeleteEmployeeIds = new HashSet<string>();

		public FrmYingdaoAutoSubmit()
		{
			InitializeComponent();

			try
			{
				configService = new YingdaoConfigService();
				InitializeDataGridView();
				LoadConfigToUI();
			}
			catch (Exception ex)
			{
				UIMessageBox.ShowError($"初始化配置服务失败：{ex.Message}\n\n请确认：\n1. appsettings.json 文件存在\n2. 数据库连接正常\n3. sys_config 表已创建");
			}
			
			dailyTimer = new System.Windows.Forms.Timer();
			dailyTimer.Interval = 60000;
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
			if (configService == null)
			{
				AppendLog("配置服务未初始化，请检查数据库连接");
				return;
			}
			
			dailyTimer.Start();
			string daysDesc = GetEnabledDaysDescription();
			AppendLog($"定时器已启动，每分钟检查执行条件（{daysDesc} 0:00-13:30）");
			
			if (configService.GetAutoStartEnabled() && ShouldRunToday() && IsInTimeWindow())
			{
				AppendLog("启动时检测到需要执行");
				await RunAllUsersAsync();
			}
		}

		private async void DailyTimer_Tick(object sender, EventArgs e)
		{
			if (configService == null) return;
			if (!configService.GetAutoStartEnabled()) return;
			if (!ShouldRunToday()) return;
			if (!IsInTimeWindow()) return;
			
			string today = DateTime.Today.ToString("yyyy-MM-dd");
			var users = configService.GetAllUsers();
			bool hasUnfinished = users.Any(u => u.Enabled && u.LastSubmittedDate != today);
			
			if (hasUnfinished)
			{
				AppendLog("定时触发自动执行");
				await RunAllUsersAsync();
			}
		}

		private bool ShouldRunToday()
		{
			if (configService == null) return false;
			int dayOfWeek = (int)DateTime.Today.DayOfWeek;
			return configService.GetEnabledDays().Contains(dayOfWeek);
		}

		private string GetEnabledDaysDescription()
		{
			if (configService == null) return "未配置";
			string[] dayNames = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
			var enabledNames = configService.GetEnabledDays().OrderBy(d => d).Select(d => dayNames[d]);
			return string.Join("、", enabledNames);
		}

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
			if (dgvUsers.CurrentRow == null || dgvUsers.CurrentRow.IsNewRow)
			{
				UIMessageTip.ShowWarning("请先选择要删除的用户");
				return;
			}
			
			int rowIndex = dgvUsers.CurrentRow.Index;
			string employeeId = dgvUsers.Rows[rowIndex].Cells["colEmployeeId"].Value?.ToString();
			
			if (string.IsNullOrWhiteSpace(employeeId))
			{
				// 如果是新添加还未保存的行，直接删除
				dgvUsers.Rows.RemoveAt(rowIndex);
				AppendLog("已删除未保存的新行");
				return;
			}
			
			// 标记为待删除（变红色）
			if (!pendingDeleteEmployeeIds.Contains(employeeId))
			{
				pendingDeleteEmployeeIds.Add(employeeId);
				dgvUsers.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
				dgvUsers.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.White;
				AppendLog($"已标记删除用户 {employeeId}，点击保存后生效");
			}
			else
			{
				// 取消删除标记
				pendingDeleteEmployeeIds.Remove(employeeId);
				dgvUsers.Rows[rowIndex].DefaultCellStyle.BackColor = Color.White;
				dgvUsers.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
				AppendLog($"已取消删除用户 {employeeId}");
			}
		}

		private void btnSaveConfig_Click(object sender, EventArgs e)
		{
			try
			{
				// 先处理删除操作
				foreach (var employeeId in pendingDeleteEmployeeIds)
				{
					configService.DeleteUser(employeeId);
					AppendLog($"已删除用户: {employeeId}");
				}
				pendingDeleteEmployeeIds.Clear();
				
				// 保存配置
				SaveUIToConfig();
				
				// 重新加载数据
				LoadConfigToUI();
				
				UIMessageTip.ShowOk("配置已保存");
				AppendLog("配置已保存到数据库");
			}
			catch (Exception ex)
			{
				UIMessageBox.ShowError($"保存失败：{ex.Message}");
				AppendLog($"保存失败：{ex.Message}");
			}
		}

		private void ChkAutoStart_CheckedChanged(object sender, EventArgs e)
		{
			if (isLoadingSettings) return;
			configService.SetAutoStartEnabled(chkAutoStart.Checked);
			AppendLog($"自动执行已{(chkAutoStart.Checked ? "开启" : "关闭")}");
		}

		private void ChkDay_CheckedChanged(object sender, EventArgs e)
		{
			if (isLoadingSettings) return;
			SaveDaysFromUI();
			string daysDesc = GetEnabledDaysDescription();
			AppendLog($"执行日期已更新：{daysDesc}");
		}

		private void SaveDaysFromUI()
		{
			var days = new List<int>();
			if (chkSunday.Checked) days.Add(0);
			if (chkMonday.Checked) days.Add(1);
			if (chkTuesday.Checked) days.Add(2);
			if (chkWednesday.Checked) days.Add(3);
			if (chkThursday.Checked) days.Add(4);
			if (chkFriday.Checked) days.Add(5);
			if (chkSaturday.Checked) days.Add(6);
			configService.SetEnabledDays(days);
		}

		private void LoadDaysToUI()
		{
			var days = configService.GetEnabledDays();
			chkSunday.Checked = days.Contains(0);
			chkMonday.Checked = days.Contains(1);
			chkTuesday.Checked = days.Contains(2);
			chkWednesday.Checked = days.Contains(3);
			chkThursday.Checked = days.Contains(4);
			chkFriday.Checked = days.Contains(5);
			chkSaturday.Checked = days.Contains(6);
		}

		private async Task RunAllUsersAsync()
		{
			if (configService == null)
			{
				AppendLog("配置服务未初始化");
				return;
			}
			
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

			isRunning = true;
			btnRun.Enabled = false;

			try
			{
				string today = DateTime.Today.ToString("yyyy-MM-dd");
				var enabledUsers = configService.GetAllUsers().Where(u => u.Enabled).ToList();
				
				if (enabledUsers.Count == 0)
				{
					AppendLog("没有启用的用户");
					return;
				}

				AppendLog($"开始执行，共 {enabledUsers.Count} 个用户");

				for (int i = 0; i < enabledUsers.Count; i++)
				{
					var user = enabledUsers[i];
					
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
						user.LastResult = "失败或已提交";
					}
					
					configService.SaveUser(user);
					LoadConfigToUI();

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

		private async Task<bool> RunSingleUserAsync(YingdaoUserConfig user)
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

		#region 配置管理

		private void LoadConfigToUI()
		{
			if (configService == null) return;
			
			isLoadingSettings = true;
			try
			{
				chkAutoStart.Checked = configService.GetAutoStartEnabled();
				LoadDaysToUI();
				
				dgvUsers.Rows.Clear();
				pendingDeleteEmployeeIds.Clear();
				
				foreach (var user in configService.GetAllUsers())
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
			catch (Exception ex)
			{
				AppendLog($"加载配置失败：{ex.Message}");
			}
			finally
			{
				isLoadingSettings = false;
			}
		}

		private void SaveUIToConfig()
		{
			configService.SetAutoStartEnabled(chkAutoStart.Checked);
			SaveDaysFromUI();
			
			foreach (DataGridViewRow row in dgvUsers.Rows)
			{
				if (row.IsNewRow) continue;
				
				string employeeId = row.Cells["colEmployeeId"].Value?.ToString() ?? "";
				
				// 跳过标记为删除的行
				if (pendingDeleteEmployeeIds.Contains(employeeId))
				{
					continue;
				}
				
				var user = new YingdaoUserConfig
				{
					Enabled = Convert.ToBoolean(row.Cells["colEnabled"].Value ?? false),
					Factory = row.Cells["colFactory"].Value?.ToString() ?? "",
					EmployeeId = employeeId,
					Phone = row.Cells["colPhone"].Value?.ToString() ?? "",
					CarNo = row.Cells["colCarNo"].Value?.ToString() ?? "",
					LastSubmittedDate = row.Cells["colLastDate"].Value?.ToString() ?? "",
					LastResult = row.Cells["colLastResult"].Value?.ToString() ?? ""
				};
				
				if (!string.IsNullOrWhiteSpace(user.EmployeeId))
				{
					configService.SaveUser(user);
				}
			}
		}

		#endregion

		#region 表单操作

		private async Task FillFormAsync(YingdaoUserConfig user)
		{
			if (!string.IsNullOrWhiteSpace(user.Factory))
			{
				await SelectFactoryAsync(user.Factory);
				await Task.Delay(500);
			}

			if (!string.IsNullOrWhiteSpace(user.EmployeeId))
			{
				await SetInputValueAsync("txtEmployeeID", user.EmployeeId);
				AppendLog("已填写员工号，等待姓名加载...");
				await Task.Delay(3500);
			}

			if (!string.IsNullOrWhiteSpace(user.Phone))
			{
				await SetInputValueAsync("txtPhoneNo", user.Phone);
				await Task.Delay(300);
			}

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
