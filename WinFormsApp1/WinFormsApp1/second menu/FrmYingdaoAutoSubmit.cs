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
		private System.Windows.Forms.Timer dailyTimer;

		public FrmYingdaoAutoSubmit()
		{
			InitializeComponent();

			string cfgDir = Path.Combine(Application.StartupPath, "Config");
			stateFilePath = Path.Combine(cfgDir, "yingdao_auto_submit_state.json");
			LoadSettingsIntoUI();
			
			// 初始化定时器，每分钟检查一次
			dailyTimer = new System.Windows.Forms.Timer();
			dailyTimer.Interval = 60000; // 1分钟
			dailyTimer.Tick += DailyTimer_Tick;
		}

		private async void FrmYingdaoAutoSubmit_Load(object sender, EventArgs e)
		{
			dailyTimer.Start();
			AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 定时器已启动，每分钟检查执行条件");
			
			// 启动时立即检查一次
			if (autoStartEnabled && ShouldRunNow())
			{
				AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 启动时检测到需要执行");
				await RunAsync(false);
			}
		}

		private async void DailyTimer_Tick(object sender, EventArgs e)
		{
			if (!autoStartEnabled) return;
			if (!ShouldRunNow()) return;
			
			var cfg = LoadConfig();
			string today = DateTime.Today.ToString("yyyy-MM-dd");
			if (cfg != null && string.Equals(cfg.LastSubmittedDate, today, StringComparison.OrdinalIgnoreCase))
			{
				return; // 今天已执行过
			}
			
			AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 定时触发自动执行");
			await RunAsync(false);
		}

		/// <summary>
		/// 判断当前是否在可执行时间范围内（0:00 - 13:30）
		/// 抽签截止14:00，提前30分钟确保有足够时间
		/// </summary>
		private bool ShouldRunNow()
		{
			var now = DateTime.Now;
			var startTime = now.Date; // 0:00
			var endTime = now.Date.AddHours(13).AddMinutes(30); // 13:30
			
			return now >= startTime && now <= endTime;
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
			if (cfg == null) return;

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

			// 先选择厂区（如果有）
			if (!string.IsNullOrWhiteSpace(factory))
			{
				await SelectFactoryAsync(factory);
				await Task.Delay(500);
			}

			// 填写员工号并等待姓名加载
			if (!string.IsNullOrWhiteSpace(employeeId))
			{
				await SetInputValueAsync("txtEmployeeID", employeeId);
				AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 已填写员工号，等待姓名加载...");
				await Task.Delay(3500); // 等待员工姓名加载
			}

			// 填写手机号
			if (!string.IsNullOrWhiteSpace(phone))
			{
				await SetInputValueAsync("txtPhoneNo", phone);
				await Task.Delay(300);
			}

			// 填写车牌号
			if (!string.IsNullOrWhiteSpace(carNo))
			{
				await SetInputValueAsync("txtCarNo", carNo);
				await Task.Delay(300);
			}
		}

		/// <summary>
		/// 使用 React 兼容方式设置输入框值
		/// </summary>
		private async Task SetInputValueAsync(string widgetId, string value)
		{
			string script = $@"
				(function() {{
					var selector = '[data-widget-id=""{widgetId}""]';
					var root = document.querySelector(selector);
					if (!root) return false;
					var input = root.querySelector('input');
					if (!input) return false;

					// React 兼容：使用原生 setter 设置值
					var nativeInputValueSetter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set;
					nativeInputValueSetter.call(input, '{value}');

					// 触发 React 能识别的事件
					var inputEvent = new Event('input', {{ bubbles: true, cancelable: true }});
					input.dispatchEvent(inputEvent);

					var changeEvent = new Event('change', {{ bubbles: true, cancelable: true }});
					input.dispatchEvent(changeEvent);

					// 模拟失焦以触发验证
					input.blur();
					return true;
				}})();";

			await web.ExecuteScriptAsync(script);
		}

		/// <summary>
		/// 选择厂区（Ant Design Mobile Checkbox 弹窗）
		/// </summary>
		private async Task SelectFactoryAsync(string factoryName)
		{
			AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 开始选择厂区: {factoryName}");

			// 1. 点击厂区选择框打开弹窗 - 需要点击内部的选择框区域
			string openScript = @"
				(function() {
					var root = document.querySelector('[data-widget-id=""cmbFty""]');
					if (!root) return 'no_widget';
					
					// 查找内部可点击的选择框
					var selectBox = root.querySelector('.min-h-12.border');
					if (selectBox) {
						selectBox.click();
						return 'clicked_selectbox';
					}
					
					// 备用：查找包含下拉箭头的区域
					var arrowArea = root.querySelector('.anticon-down');
					if (arrowArea) {
						var clickTarget = arrowArea.closest('.min-h-12') || arrowArea.parentElement;
						if (clickTarget) {
							clickTarget.click();
							return 'clicked_arrow_area';
						}
					}
					
					// 备用：点击整个widget
					root.click();
					return 'clicked_root';
				})();";

			string openResult = await web.ExecuteScriptAsync(openScript);
			AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 打开弹窗: {openResult}");

			// 2. 等待弹窗出现（最多等待 5 秒）
			string waitResult = "timeout";
			for (int i = 0; i < 25; i++)
			{
				await Task.Delay(200);
				string checkScript = @"
					(function() {
						var popup = document.querySelector('.adm-popup-body');
						if (popup && popup.offsetHeight > 0) {
							// 检查弹窗内是否有 checkbox 选项
							var checkboxes = popup.querySelectorAll('label.adm-checkbox');
							return 'visible:' + checkboxes.length + '_options';
						}
						return 'hidden';
					})();";
				waitResult = await web.ExecuteScriptAsync(checkScript);
				if (waitResult.Contains("visible"))
				{
					AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 弹窗已出现 (等待 {(i + 1) * 200}ms): {waitResult}");
					break;
				}
			}
			if (!waitResult.Contains("visible"))
			{
				AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 弹窗等待超时，尝试直接查找选项");
			}

			await Task.Delay(500); // 额外等待动画完成

			// 3. 选择厂区 - 根据实际HTML结构：
			// <div class="flex items-center justify-between px-4 py-4 border-b border-borderColor gap-2">
			//   <label class="adm-checkbox"><input type="checkbox">...</label>
			//   <div class="flex-auto">总厂</div>
			// </div>
			string selectScript = $@"
				(function() {{
					var target = '{factoryName}'.trim();
					var popup = document.querySelector('.adm-popup-body');
					if (!popup) return 'no_popup';
					
					// 调试：输出弹窗内所有文本
					var allText = popup.innerText;
					console.log('弹窗内容:', allText);
					
					// 方法1：查找弹窗内所有行（包含 checkbox 的行）
					var rows = popup.querySelectorAll('.flex.items-center.justify-between');
					console.log('找到行数:', rows.length);
					
					for (var i = 0; i < rows.length; i++) {{
						var row = rows[i];
						// 查找行内的 flex-auto div（包含厂区名称）
						var nameDiv = row.querySelector('.flex-auto');
						if (nameDiv) {{
							var text = (nameDiv.innerText || nameDiv.textContent || '').trim();
							console.log('行' + i + '文本:', text);
							if (text === target) {{
								// 找到匹配的厂区，点击 checkbox
								var checkbox = row.querySelector('input[type=""checkbox""]');
								if (checkbox) {{
									checkbox.click();
									return 'clicked_checkbox:' + text;
								}}
								// 备用：点击 label
								var label = row.querySelector('label.adm-checkbox');
								if (label) {{
									label.click();
									return 'clicked_label:' + text;
								}}
								// 备用：点击整行
								row.click();
								return 'clicked_row:' + text;
							}}
						}}
					}}
					
					// 方法2：直接查找所有 flex-auto div
					var flexAutoDivs = popup.querySelectorAll('.flex-auto');
					console.log('找到flex-auto数:', flexAutoDivs.length);
					
					for (var j = 0; j < flexAutoDivs.length; j++) {{
						var div = flexAutoDivs[j];
						var text = (div.innerText || div.textContent || '').trim();
						if (text === target) {{
							var parent = div.parentElement;
							if (parent) {{
								var checkbox = parent.querySelector('input[type=""checkbox""]');
								if (checkbox) {{
									checkbox.click();
									return 'method2_clicked_checkbox:' + text;
								}}
							}}
						}}
					}}
					
					// 方法3：遍历所有 adm-checkbox
					var checkboxLabels = popup.querySelectorAll('label.adm-checkbox');
					console.log('找到checkbox数:', checkboxLabels.length);
					
					for (var k = 0; k < checkboxLabels.length; k++) {{
						var label = checkboxLabels[k];
						var parent = label.parentElement;
						if (parent) {{
							var nameDiv = parent.querySelector('.flex-auto');
							if (nameDiv) {{
								var text = (nameDiv.innerText || nameDiv.textContent || '').trim();
								if (text === target) {{
									var checkbox = label.querySelector('input[type=""checkbox""]');
									if (checkbox) {{
										checkbox.click();
										return 'method3_clicked:' + text;
									}}
								}}
							}}
						}}
					}}
					
					return 'not_found:target=' + target + ',rows=' + rows.length;
				}})();";

			string selectResult = await web.ExecuteScriptAsync(selectScript);
			AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 厂区选择结果: {selectResult}");
			await Task.Delay(500);

			// 4. 点击确定按钮 - 根据实际HTML：<div class="text-lg">确定</div>
			string confirmScript = @"
				(function() {
					var popup = document.querySelector('.adm-popup-body');
					if (!popup) return 'no_popup_for_confirm';
					
					// 查找弹窗顶部的确定按钮
					var textLgDivs = popup.querySelectorAll('.text-lg');
					for (var i = 0; i < textLgDivs.length; i++) {
						var div = textLgDivs[i];
						var txt = (div.innerText || div.textContent || '').trim();
						if (txt === '确定' || txt === '确认') {
							div.click();
							return 'confirmed:' + txt;
						}
					}
					
					return 'no_confirm_btn:found_' + textLgDivs.length + '_text-lg';
				})();";

			string confirmResult = await web.ExecuteScriptAsync(confirmScript);
			AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 确定按钮: {confirmResult}");
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
