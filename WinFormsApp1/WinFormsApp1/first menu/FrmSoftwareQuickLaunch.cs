using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WinFormsApp1.first_menu
{
    public partial class FrmSoftwareQuickLaunch : UIForm
    {
        private const string ConfigDirectoryName = "Config";
        private const string ConfigFileName = "software_launcher.json";

        private readonly BindingList<SoftwareItem> softwareItems = new BindingList<SoftwareItem>();
        private readonly BindingSource bindingSource = new BindingSource();
        private bool loadingConfig = false;

        private string ConfigFullPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigDirectoryName, ConfigFileName);

        public FrmSoftwareQuickLaunch()
        {
            InitializeComponent();
            InitGrid();
        }

        private void FrmSoftwareQuickLaunch_Load(object sender, EventArgs e)
        {
            _ = LoadConfigAsync();
        }

        private void InitGrid()
        {
            gridSoftware.AutoGenerateColumns = false;
            gridSoftware.AllowUserToAddRows = true;
            gridSoftware.AllowUserToDeleteRows = true;
            gridSoftware.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridSoftware.MultiSelect = true;
            gridSoftware.RowHeadersVisible = false;
            gridSoftware.EnableHeadersVisualStyles = false;
            gridSoftware.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 249, 255);
            gridSoftware.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            gridSoftware.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 253, 255);
            gridSoftware.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            bindingSource.DataSource = softwareItems;
            gridSoftware.DataSource = bindingSource;

            if (gridSoftware.Columns.Count == 0)
            {
                gridSoftware.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    DataPropertyName = nameof(SoftwareItem.AutoLaunch),
                    HeaderText = "自动",
                    Width = 60,
                    ToolTipText = "是否默认参与一键启动"
                });

                gridSoftware.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(SoftwareItem.Name),
                    HeaderText = "名称",
                    Width = 160
                });

                gridSoftware.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(SoftwareItem.Path),
                    HeaderText = "可执行路径",
                    Width = 260
                });

                gridSoftware.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(SoftwareItem.Arguments),
                    HeaderText = "启动参数",
                    Width = 180
                });

                gridSoftware.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(SoftwareItem.WorkingDirectory),
                    HeaderText = "工作目录",
                    Width = 200
                });

                gridSoftware.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(SoftwareItem.DelaySeconds),
                    HeaderText = "延迟(秒)",
                    Width = 90,
                    ValueType = typeof(int),
                    ToolTipText = "启动下一个软件前的等待秒数"
                });
            }
        }

        private async Task LoadConfigAsync()
        {
            if (loadingConfig) return;
            loadingConfig = true;

            try
            {
                EnsureConfigExists();
                string json = await Task.Run(() => File.ReadAllText(ConfigFullPath, Encoding.UTF8));
                var items = JsonConvert.DeserializeObject<List<SoftwareItem>>(json) ?? new List<SoftwareItem>();

                softwareItems.Clear();
                foreach (var item in items)
                {
                    softwareItems.Add(item);
                }

                txtConfigPath.Text = ConfigFullPath;
                AppendLog("配置加载完成，共 " + softwareItems.Count + " 条记录。");
            }
            catch (Exception ex)
            {
                ShowErrorTip("加载配置失败：" + ex.Message);
                AppendLog("加载配置失败：" + ex.Message);
            }
            finally
            {
                loadingConfig = false;
            }
        }

        private void EnsureConfigExists()
        {
            string dir = Path.GetDirectoryName(ConfigFullPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (!File.Exists(ConfigFullPath))
            {
                var defaults = new List<SoftwareItem>
                {
                    new SoftwareItem
                    {
                        Name = "示例-记事本",
                        Path = "C:/Windows/System32/notepad.exe",
                        Arguments = string.Empty,
                        WorkingDirectory = string.Empty,
                        AutoLaunch = true,
                        DelaySeconds = 0
                    }
                };
                string json = JsonConvert.SerializeObject(defaults, Formatting.Indented);
                File.WriteAllText(ConfigFullPath, json, Encoding.UTF8);
            }
        }

        private async Task SaveConfigAsync()
        {
            try
            {
                gridSoftware.EndEdit();
                bindingSource.EndEdit();

                var list = softwareItems.Where(x => !string.IsNullOrWhiteSpace(x.Path)).ToList();
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                await Task.Run(() => File.WriteAllText(ConfigFullPath, json, Encoding.UTF8));
                AppendLog("配置已保存。");
                ShowSuccessTip("配置保存成功");
            }
            catch (Exception ex)
            {
                ShowErrorTip("保存配置失败：" + ex.Message);
                AppendLog("保存配置失败：" + ex.Message);
            }
        }

        private void AppendLog(string message)
        {
            if (rtbLog.IsDisposed) return;
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

        private void btnReload_Click(object sender, EventArgs e)
        {
            _ = LoadConfigAsync();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            await SaveConfigAsync();
        }

        private void btnOpenConfig_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureConfigExists();
                var psi = new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = $"/select,\"{ConfigFullPath}\"",
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                ShowErrorTip("打开配置位置失败：" + ex.Message);
            }
        }

        private async void btnLaunchAuto_Click(object sender, EventArgs e)
        {
            var targets = softwareItems.Where(x => x.AutoLaunch).ToList();
            if (targets.Count == 0)
            {
                ShowWarningTip("没有勾选自动启动的软件。");
                return;
            }
            await LaunchSoftwareListAsync(targets);
        }

        private async void btnLaunchAll_Click(object sender, EventArgs e)
        {
            var targets = softwareItems.Where(x => !string.IsNullOrWhiteSpace(x.Path)).ToList();
            if (targets.Count == 0)
            {
                ShowWarningTip("列表为空，没有可启动的软件。");
                return;
            }
            await LaunchSoftwareListAsync(targets);
        }

        private async void btnLaunchSelected_Click(object sender, EventArgs e)
        {
            var selected = new List<SoftwareItem>();
            foreach (DataGridViewRow row in gridSoftware.SelectedRows)
            {
                if (row.DataBoundItem is SoftwareItem item && !selected.Contains(item))
                {
                    selected.Add(item);
                }
            }

            if (selected.Count == 0)
            {
                ShowWarningTip("请先选择需要启动的行。");
                return;
            }

            selected = selected.Where(x => !string.IsNullOrWhiteSpace(x.Path)).ToList();
            if (selected.Count == 0)
            {
                ShowWarningTip("所选行没有有效的路径。");
                return;
            }

            await LaunchSoftwareListAsync(selected);
        }

        private async Task LaunchSoftwareListAsync(List<SoftwareItem> items)
        {
            await Task.Run(async () =>
            {
                foreach (var item in items)
                {
                    string exePath = ResolvePath(item.Path);
                    if (string.IsNullOrWhiteSpace(exePath))
                    {
                        AppendLog($"跳过[{item.Name}]，未提供路径。");
                        continue;
                    }

                    if (!File.Exists(exePath))
                    {
                        AppendLog($"未找到文件：{exePath}");
                        continue;
                    }

                    string workingDir = ResolveWorkingDirectory(item, exePath);

                    try
                    {
                        var psi = new ProcessStartInfo
                        {
                            FileName = exePath,
                            Arguments = item.Arguments ?? string.Empty,
                            WorkingDirectory = workingDir,
                            UseShellExecute = true
                        };
                        Process.Start(psi);
                        AppendLog($"已启动[{item.Name}]。");
                    }
                    catch (Exception ex)
                    {
                        AppendLog($"启动[{item.Name}]失败：{ex.Message}");
                    }

                    int delay = Math.Max(0, item.DelaySeconds);
                    if (delay > 0)
                    {
                        AppendLog($"等待 {delay} 秒后继续...");
                        await Task.Delay(delay * 1000);
                    }
                }
            });
        }

        private static string ResolvePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;
            string expanded = Environment.ExpandEnvironmentVariables(path.Trim());
            if (Path.IsPathRooted(expanded)) return expanded;
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, expanded);
        }

        private static string ResolveWorkingDirectory(SoftwareItem item, string exePath)
        {
            string workingDir = item.WorkingDirectory;
            if (string.IsNullOrWhiteSpace(workingDir))
            {
                return Path.GetDirectoryName(exePath) ?? AppDomain.CurrentDomain.BaseDirectory;
            }

            string expanded = Environment.ExpandEnvironmentVariables(workingDir.Trim());
            if (!Path.IsPathRooted(expanded))
            {
                expanded = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, expanded);
            }
            return Directory.Exists(expanded) ? expanded : Path.GetDirectoryName(exePath) ?? AppDomain.CurrentDomain.BaseDirectory;
        }

        private void gridSoftware_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (gridSoftware.IsCurrentCellDirty)
            {
                gridSoftware.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void gridSoftware_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void gridSoftware_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (gridSoftware.Rows[e.RowIndex].DataBoundItem is SoftwareItem item)
            {
                string exePath = ResolvePath(item.Path);
                if (File.Exists(exePath))
                {
                    try
                    {
                        var psi = new ProcessStartInfo
                        {
                            FileName = "explorer.exe",
                            Arguments = $"/select,\"{exePath}\"",
                            UseShellExecute = true
                        };
                        Process.Start(psi);
                    }
                    catch (Exception ex)
                    {
                        AppendLog($"打开文件位置失败：{ex.Message}");
                    }
                }
                else
                {
                    AppendLog($"无法定位路径：{exePath}");
                }
            }
        }

        private class SoftwareItem
        {
            public string Name { get; set; } = string.Empty;
            public string Path { get; set; } = string.Empty;
            public string Arguments { get; set; } = string.Empty;
            public string WorkingDirectory { get; set; } = string.Empty;
            public bool AutoLaunch { get; set; } = true;
            public int DelaySeconds { get; set; } = 0;
        }
    }
}

