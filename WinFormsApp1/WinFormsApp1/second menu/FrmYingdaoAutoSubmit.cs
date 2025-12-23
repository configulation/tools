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
	public class FrmYingdaoAutoSubmit : UIForm
	{
		private const string TargetUrl = "https://esquel.yingdaoapps.com/public/app/821296610254688256/%E8%BD%A6%E4%BD%8D%E6%8A%BD%E7%AD%BE/defaultPage";

		private readonly WebView2 web;
		private readonly UIRichTextBox txtLog;
		private readonly UIButton btnRun;
		private readonly UICheckBox chkAutoStart;
		private readonly string stateFilePath;
		private readonly string settingsFilePath;
		private bool hasAutoStarted;
		private bool isRunning;
		private bool isLoadingSettings;
		private bool autoStartEnabled = true;

		public FrmYingdaoAutoSubmit()
		{
			Text = "车位抽签-自动提交";
			AutoScaleMode = AutoScaleMode.None;

			chkAutoStart = new UICheckBox
			{
				Dock = DockStyle.Top,
				Height = 29,
				Text = "启动时自动执行",
				Checked = true
			};

			btnRun = new UIButton
			{
				Dock = DockStyle.Top,
				Height = 35,
				Text = "立即执行检查/提交"
			};

			txtLog = new UIRichTextBox
			{
				Dock = DockStyle.Bottom,
				Height = 180,
				ReadOnly = true,
				WordWrap = false
			};

			web = new WebView2
			{
				Dock = DockStyle.Fill
			};

			Controls.Add(web);
			Controls.Add(txtLog);
			Controls.Add(btnRun);
			Controls.Add(chkAutoStart);

			btnRun.Click += async (s, e) => await RunAsync(true);
			chkAutoStart.CheckedChanged += ChkAutoStart_CheckedChanged;
			Load += FrmYingdaoAutoSubmit_Load;

			string cfgDir = Path.Combine(Application.StartupPath, "Config");
			stateFilePath = Path.Combine(cfgDir, "yingdao_auto_submit_state.json");
			settingsFilePath = Path.Combine(cfgDir, "yingdao_auto_submit_settings.json");
			LoadSettingsIntoUI();
		}

		private async void FrmYingdaoAutoSubmit_Load(object sender, EventArgs e)
		{
			if (!autoStartEnabled)
			{
				AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 已关闭自动执行");
				return;
			}
			await RunAsync(false);
		}

		private void ChkAutoStart_CheckedChanged(object sender, EventArgs e)
		{
			if (isLoadingSettings) return;
			autoStartEnabled = chkAutoStart.Checked;
			SaveSettings(new AutoSubmitSettings { AutoStartEnabled = autoStartEnabled });
			AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 自动执行已{(autoStartEnabled ? "开启" : "关闭")}");
		}

		private void LoadSettingsIntoUI()
		{
			isLoadingSettings = true;
			try
			{
				var settings = LoadSettings();
				autoStartEnabled = settings?.AutoStartEnabled ?? true;
				chkAutoStart.Checked = autoStartEnabled;
			}
			finally
			{
				isLoadingSettings = false;
			}
		}

		private AutoSubmitSettings LoadSettings()
		{
			try
			{
				if (!File.Exists(settingsFilePath)) return new AutoSubmitSettings();
				string json = File.ReadAllText(settingsFilePath, Encoding.UTF8);
				if (string.IsNullOrWhiteSpace(json)) return new AutoSubmitSettings();
				return JsonConvert.DeserializeObject<AutoSubmitSettings>(json) ?? new AutoSubmitSettings();
			}
			catch
			{
				return new AutoSubmitSettings();
			}
		}

		private void SaveSettings(AutoSubmitSettings settings)
		{
			try
			{
				if (settings == null) return;
				string dir = Path.GetDirectoryName(settingsFilePath);
				if (!string.IsNullOrWhiteSpace(dir)) Directory.CreateDirectory(dir);
				string json = JsonConvert.SerializeObject(settings);
				File.WriteAllText(settingsFilePath, json, Encoding.UTF8);
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

				var state = LoadState();
				string today = DateTime.Today.ToString("yyyy-MM-dd");
				if (!manual && state != null && string.Equals(state.LastSubmittedDate, today, StringComparison.OrdinalIgnoreCase))
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

				var probe = await ProbeSubmitButtonAsync();
				if (probe == null || !probe.Found)
				{
					AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 未找到“提交”按钮，无法自动提交");
					return;
				}

				AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 找到按钮：text={probe.Text} disabled={probe.Disabled} visible={probe.Visible}");

				if (probe.Disabled)
				{
					AppendLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 按钮已禁用，判定为已提交（或不可提交），写入本机记录");
					SaveState(new AutoSubmitState { LastSubmittedDate = today, LastResult = "disabled" });
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
					SaveState(new AutoSubmitState { LastSubmittedDate = today, LastResult = "clicked" });
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

		private async Task<SubmitButtonProbeResult> ProbeSubmitButtonAsync()
		{
			try
			{
				string script = "(() => {" +
					"const isVisible = (el) => {" +
					"if (!el) return false;" +
					"const rect = el.getBoundingClientRect();" +
					"if (rect.width <= 0 || rect.height <= 0) return false;" +
					"const style = window.getComputedStyle(el);" +
					"if (!style) return true;" +
					"return style.display !== 'none' && style.visibility !== 'hidden' && style.opacity !== '0';" +
					"};" +
					"const candidates = Array.from(document.querySelectorAll('button')).concat(Array.from(document.querySelectorAll('[role=button]')));" +
					"const match = candidates.find(el => {" +
					"const t = (el.innerText || '').trim();" +
					"return t === '提交' || t === '提交 ' || t.indexOf('提交') >= 0;" +
					"});" +
					"if (!match) return { found:false };" +
					"const text = (match.innerText || '').trim();" +
					"const disabled = !!match.disabled || match.getAttribute('aria-disabled') === 'true' || (match.className || '').toLowerCase().indexOf('disabled') >= 0;" +
					"return { found:true, text, disabled, visible:isVisible(match) };" +
					"})()";

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
				string script = "(() => {" +
					"const candidates = Array.from(document.querySelectorAll('button')).concat(Array.from(document.querySelectorAll('[role=button]')));" +
					"const match = candidates.find(el => {" +
					"const t = (el.innerText || '').trim();" +
					"return t === '提交' || t === '提交 ' || t.indexOf('提交') >= 0;" +
					"});" +
					"if (!match) return { clicked:false, reason:'not_found' };" +
					"const disabled = !!match.disabled || match.getAttribute('aria-disabled') === 'true' || (match.className || '').toLowerCase().indexOf('disabled') >= 0;" +
					"if (disabled) return { clicked:false, reason:'disabled' };" +
					"match.click();" +
					"return { clicked:true, reason:'clicked' };" +
					"})()";

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

		private AutoSubmitState LoadState()
		{
			try
			{
				if (!File.Exists(stateFilePath)) return null;
				string json = File.ReadAllText(stateFilePath, Encoding.UTF8);
				if (string.IsNullOrWhiteSpace(json)) return null;
				return JsonConvert.DeserializeObject<AutoSubmitState>(json);
			}
			catch
			{
				return null;
			}
		}

		private void SaveState(AutoSubmitState state)
		{
			try
			{
				if (state == null) return;
				string dir = Path.GetDirectoryName(stateFilePath);
				if (!string.IsNullOrWhiteSpace(dir)) Directory.CreateDirectory(dir);
				string json = JsonConvert.SerializeObject(state);
				File.WriteAllText(stateFilePath, json, Encoding.UTF8);
			}
			catch
			{
			}
		}

		private class AutoSubmitSettings
		{
			public bool AutoStartEnabled { get; set; } = true;
		}

		private class AutoSubmitState
		{
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
