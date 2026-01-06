using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using Sunny.UI;

namespace WinFormsApp1.second_menu
{
	public partial class FrmYingdaoAutoSubmit : UIForm
	{
		private const string TargetUrl = "https://esquel.yingdaoapps.com/public/app/821296610254688256/%E8%BD%A6%E4%BD%8D%E6%8A%BD%E7%AD%BE/defaultPage";

		private readonly string stateFilePath;
		private bool hasAutoStarted;
		private bool isRunning;
		private bool isLoadingSettings;
		private bool autoStartEnabled = true;

		public FrmYingdaoAutoSubmit()
		{
			InitializeComponent();

			string cfgDir = Path.Combine(Application.StartupPath, "Config");
			stateFilePath = Path.Combine(cfgDir, "yingdao_auto_submit_state.json");
			LoadSettingsIntoUI();
		}

		private async void FrmYingdaoAutoSubmit_Load(object sender, EventArgs e)
		{
			// if (!autoStartEnabled)
			// {
			// 	AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 已关闭自动执行");
			// 	return;
			// }
			// await RunAsync(false);
		}

		private async void btnRun_Click(object sender, EventArgs e)
		{
			await RunAsync(true);
		}

		private void ChkAutoStart_CheckedChanged(object sender, EventArgs e)
		{
			if (isLoadingSettings) return;
			autoStartEnabled = chkAutoStart.Checked;
			var cfg = LoadConfig();
			cfg.AutoStartEnabled = autoStartEnabled;
			SaveConfig(cfg);
			AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 自动执行已{(autoStartEnabled ? "开启" : "关闭")}");
		}

		private void LoadSettingsIntoUI()
		{
			isLoadingSettings = true;
			try
			{
				var cfg = LoadConfig();
				autoStartEnabled = cfg?.AutoStartEnabled ?? false;
				chkAutoStart.Checked = autoStartEnabled;
			}
			finally
			{
				isLoadingSettings = false;
			}
		}

		private async Task<bool> HasEmployeeNameErrorAsync()
		{
			try
			{
				string script = @"
					(function () {
						function normalize(text) {
							if (!text) return '';
							return text.replace(/\s+/g, '').trim();
						}

						var nodes = document.querySelectorAll('*');
						for (var i = 0; i < nodes.length; i++) {
							var t = normalize(nodes[i].innerText || nodes[i].textContent);
							if (t === '员工号不正确' || t === '本字段必填') {
								return true;
							}
						}
						return false;
					})();";

				string result = await web.ExecuteScriptAsync(script);
				if (string.IsNullOrWhiteSpace(result)) return false;
				return JsonConvert.DeserializeObject<bool>(result);
			}
			catch
			{
				return false;
			}
		}

		private async Task<bool> WaitEmployeeNameOkAsync()
		{
			// 最多等待约 8 秒，直到错误提示消失
			for (int i = 0; i < 8; i++)
			{
				bool hasError = await HasEmployeeNameErrorAsync();
				if (!hasError)
				{
					return true;
				}
				await Task.Delay(1000);
			}
			return false;
		}

		private AutoSubmitConfig LoadConfig()
		{
			try
			{
				if (!File.Exists(stateFilePath)) return new AutoSubmitConfig();
				string json = File.ReadAllText(stateFilePath, Encoding.UTF8);
				if (string.IsNullOrWhiteSpace(json)) return new AutoSubmitConfig();
				return JsonConvert.DeserializeObject<AutoSubmitConfig>(json) ?? new AutoSubmitConfig();
			}
			catch
			{
				return new AutoSubmitConfig();
			}
		}

		private void SaveConfig(AutoSubmitConfig cfg)
		{
			try
			{
				if (cfg == null) return;
				string dir = Path.GetDirectoryName(stateFilePath);
				if (!string.IsNullOrWhiteSpace(dir)) Directory.CreateDirectory(dir);
				string json = JsonConvert.SerializeObject(cfg, Formatting.Indented);
				File.WriteAllText(stateFilePath, json, Encoding.UTF8);
			}
			catch
			{
			}
		}

		private async Task RunAsync(bool manual)
		{
			if (!manual)
			{
				if (hasAutoStarted) return;
				hasAutoStarted = true;
			}

			if (isRunning) return;
			isRunning = true;
			try
			{
				if (!IsWorkday(DateTime.Today))
				{
					AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 今日非工作日，跳过");
					return;
				}

				var cfg = LoadConfig();
				string today = DateTime.Today.ToString("yyyy-MM-dd");
				if (!manual && cfg != null && !string.IsNullOrWhiteSpace(cfg.LastSubmittedDate) && string.Equals(cfg.LastSubmittedDate, today, StringComparison.OrdinalIgnoreCase))
				{
					AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 今日已执行过（本机记录），跳过");
					return;
				}

				AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 初始化浏览器...");
				if (!await EnsureWebViewReadyAsync()) return;

				AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 打开页面: {TargetUrl}");
				bool navOk = await NavigateAsync(TargetUrl);
				if (!navOk)
				{
					AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 页面加载失败");
					return;
				}

				await Task.Delay(2000);
				var alreadyFilled = await IsFormAlreadyFilledAsync();
				if (alreadyFilled)
				{
					AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 表单已有数据，跳过自动填充");
				}
				else
				{
					await FillFormAsync();
					var nameOk = await WaitEmployeeNameOkAsync();
					if (!nameOk)
					{
						AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 员工姓名未加载成功（仍有错误提示），本次不自动提交");
						return;
					}
				}

				var probe = await ProbeSubmitButtonAsync();
				if (probe == null || !probe.Found)
				{
					AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 未找到“提交”按钮，无法自动提交");
					return;
				}

				AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 找到按钮：text={probe.Text} disabled={probe.Disabled} visible={probe.Visible}");

				if (probe.Disabled)
				{
					AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 按钮已禁用，表单当前不可提交（可能未填写完整或已提交），本次不写入本机记录");
					return;
				}

				if (!probe.Visible)
				{
					AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 按钮不可见，跳过");
					return;
				}

				AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 开始点击“提交”...");
				var click = await ClickSubmitButtonAsync();
				if (click == null)
				{
					AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 点击脚本执行失败");
					return;
				}

				AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 点击结果：clicked={click.Clicked} reason={click.Reason}");
				if (click.Clicked)
				{
					cfg.LastSubmittedDate = today;
					cfg.LastResult = "clicked";
					SaveConfig(cfg);
					await Task.Delay(2000);
					var after = await ProbeSubmitButtonAsync();
					if (after != null)
					{
						AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 点击后按钮状态：disabled={after.Disabled} visible={after.Visible}");
					}
				}
			}
			catch (Exception ex)
			{
				AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 执行异常：{ex.Message}");
			}
			finally
			{
				isRunning = false;
			}
		}

		private static bool IsWorkday(DateTime date)
		{
			return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
		}

		private async Task<bool> EnsureWebViewReadyAsync()
		{
			try
			{
				await web.EnsureCoreWebView2Async();
				web.CoreWebView2.Settings.AreDevToolsEnabled = true;
				web.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
				return true;
			}
			catch (Exception ex)
			{
				AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} WebView2 初始化失败：{ex.Message}");
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
							if (!input.value) return false;
							return input.value.trim().length > 0;
						}

						return hasValueByWidgetId('txtEmployeeID')
							|| hasValueByWidgetId('txtPhoneNo')
							|| hasValueByWidgetId('txtCarNo');
					})();";

				string result = await web.ExecuteScriptAsync(script);
				if (string.IsNullOrWhiteSpace(result)) return false;
				return JsonConvert.DeserializeObject<bool>(result);
			}
			catch
			{
				return false;
			}
		}

		private async Task FillFormAsync()
		{
			var cfg = LoadConfig();
			if (cfg == null)
			{
				return;
			}

			var employeeId = JsEscape(cfg.EmployeeId);
			var phone = JsEscape(cfg.Phone);
			var carNo = JsEscape(cfg.CarNo);
			var factory = JsEscape(cfg.Factory);

			if (string.IsNullOrWhiteSpace(employeeId) &&
			    string.IsNullOrWhiteSpace(phone) &&
			    string.IsNullOrWhiteSpace(carNo) &&
			    string.IsNullOrWhiteSpace(factory))
			{
				return;
			}

			string script = $@"
				(function () {{
					function setInputByWidgetId(widgetId, value) {{
						if (!value) return;
						var selector = '[data-widget-id=' + JSON.stringify(widgetId) + ']';
						var root = document.querySelector(selector);
						if (!root) return;
						var input = root.querySelector('input');
						if (!input) return;
						input.focus();
						input.value = value;
						var ev = new Event('input', {{ bubbles: true }});
						input.dispatchEvent(ev);
						var blurEv = new Event('blur', {{ bubbles: true }});
						input.dispatchEvent(blurEv);
					}}

					function openFactoryField() {{
						var nodes = document.querySelectorAll('*');
						for (var i = 0; i < nodes.length; i++) {{
							var el = nodes[i];
							var text = (el.innerText || el.textContent || '').replace(/\s+/g, '');
							if (!text) continue;
							if (text.indexOf('厂区*') >= 0 || text === '厂区' || text.indexOf('厂区请选择') >= 0) {{
								var clickable = el.closest ? el.closest('.adm-list-item') : null;
								if (!clickable) clickable = el;
								clickable.click();
								break;
							}}
						}}
					}}

					function selectFactoryByName(name) {{
						if (!name) return;
						var popup = document.querySelector('.adm-popup-body');
						if (!popup) return;

						function normalize(text) {{
							if (!text) return '';
							return text.replace(/\s+/g, '').trim();
						}}

						var target = normalize(name);
						var rows = popup.querySelectorAll('.flex.items-center.justify-between');
						for (var i = 0; i < rows.length; i++) {{
							var row = rows[i];
							var textEl = row.querySelector('.flex-auto');
							if (!textEl) continue;
							var text = normalize(textEl.innerText || textEl.textContent);
							if (text === target) {{
								var checkboxLabel = row.querySelector('.adm-checkbox');
								if (checkboxLabel) {{
									checkboxLabel.click();
								}} else {{
									row.click();
								}}
								break;
							}}
						}}

						var header = popup.querySelector('.flex.p-2.justify-between.items-center');
						if (!header) header = popup;
						var buttons = header.querySelectorAll('.text-lg');
						for (var j = 0; j < buttons.length; j++) {{
							var b = buttons[j];
							var txt = (b.innerText || b.textContent || '').trim();
							if (txt === '确定') {{
								b.click();
								break;
							}}
						}}
					}}

					function openFactoryField() {{
						var nodes = document.querySelectorAll('*');
						for (var i = 0; i < nodes.length; i++) {{
							var el = nodes[i];
							var text = (el.innerText || el.textContent || '').replace(/\s+/g, '');
							if (!text) continue;
							if (text.indexOf('厂区*') >= 0 || text === '厂区' || text.indexOf('厂区请选择') >= 0) {{
								var clickable = el.closest ? el.closest('.adm-list-item') : null;
								if (!clickable) clickable = el;
								clickable.click();
								break;
							}}
						}}
					}}

					setInputByWidgetId('txtEmployeeID', '{employeeId}');
					setInputByWidgetId('txtPhoneNo', '{phone}');
					setInputByWidgetId('txtCarNo', '{carNo}');

					if ('{factory}') {{
						setTimeout(function () {{
							openFactoryField();
							setTimeout(function () {{
								selectFactoryByName('{factory}');
							}}, 500);
						}}, 0);
					}}
				}})();";

			await web.ExecuteScriptAsync(script);
		}

		private static string GetProbeSubmitButtonScript()
		{
			return @"
				(function () {
					function isVisible(el) {
						if (!el) return false;
						var rect = el.getBoundingClientRect();
						if (rect.width <= 0 || rect.height <= 0) return false;
						var style = window.getComputedStyle(el);
						if (!style) return true;
						return style.display !== 'none'
							&& style.visibility !== 'hidden'
							&& style.opacity !== '0';
					}

					function getCandidates() {
						var list1 = Array.prototype.slice.call(document.querySelectorAll('button'));
						var list2 = Array.prototype.slice.call(document.querySelectorAll('[role=button]'));
						return list1.concat(list2);
					}

					var candidates = getCandidates();
					var match = candidates.find(function (el) {
						var t = (el.innerText || '').trim();
						return t === '提交' || t === '提交 ' || t.indexOf('提交') >= 0;
					});

					if (!match) {
						return { found: false };
					}

					var text = (match.innerText || '').trim();
					var className = (match.className || '').toLowerCase();
					var disabled = !!match.disabled
						|| match.getAttribute('aria-disabled') === 'true'
						|| className.indexOf('disabled') >= 0;

					return {
						found: true,
						text: text,
						disabled: disabled,
						visible: isVisible(match)
					};
				})();";
		}

		private static string GetClickSubmitButtonScript()
		{
			return @"
				(function () {
					function getCandidates() {
						var list1 = Array.prototype.slice.call(document.querySelectorAll('button'));
						var list2 = Array.prototype.slice.call(document.querySelectorAll('[role=button]'));
						return list1.concat(list2);
					}

					var candidates = getCandidates();
					var match = candidates.find(function (el) {
						var t = (el.innerText || '').trim();
						return t === '提交' || t === '提交 ' || t.indexOf('提交') >= 0;
					});

					if (!match) {
						return { clicked: false, reason: 'not_found' };
					}

					var className = (match.className || '').toLowerCase();
					var disabled = !!match.disabled
						|| match.getAttribute('aria-disabled') === 'true'
						|| className.indexOf('disabled') >= 0;

					if (disabled) {
						return { clicked: false, reason: 'disabled' };
					}

					match.click();
					return { clicked: true, reason: 'clicked' };
				})();";
		}

		private static string JsEscape(string value)
		{
			if (string.IsNullOrEmpty(value)) return string.Empty;
			return value
				.Replace("\\", "\\\\")
				.Replace("'", "\\'")
				.Replace("\r", string.Empty)
				.Replace("\n", string.Empty);
		}

		private async Task<SubmitButtonProbeResult> ProbeSubmitButtonAsync()
		{
			try
			{
				string script = GetProbeSubmitButtonScript();
				string result = await web.ExecuteScriptAsync(script);
				if (string.IsNullOrWhiteSpace(result)) return null;

				var obj = JsonConvert.DeserializeObject<SubmitButtonProbeResult>(result);
				return obj;
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
				string script = GetClickSubmitButtonScript();
				string result = await web.ExecuteScriptAsync(script);
				if (string.IsNullOrWhiteSpace(result)) return null;
				return JsonConvert.DeserializeObject<SubmitButtonClickResult>(result);
			}
			catch
			{
				return null;
			}
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
				txtLog.AppendText(line + Environment.NewLine);
			}
			catch
			{
			}
		}

		private class AutoSubmitConfig
		{
			// 配置
			public bool AutoStartEnabled { get; set; } = false;
			public string Factory { get; set; } = string.Empty;
			public string EmployeeId { get; set; } = string.Empty;
			public string Phone { get; set; } = string.Empty;
			public string CarNo { get; set; } = string.Empty;

			// 状态
			public string LastSubmittedDate { get; set; }
			public string LastResult { get; set; }
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
	}
}
