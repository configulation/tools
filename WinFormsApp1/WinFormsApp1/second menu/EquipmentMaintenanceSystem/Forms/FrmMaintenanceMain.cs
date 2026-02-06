using Sunny.UI;
using System;
using System.Windows.Forms;
using WinFormsApp1.Common.Database;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    public partial class FrmMaintenanceMain : UIForm
    {
        /// <summary>
        /// 当前程序版本号（每次发版时修改此值）
        /// </summary>
        private const string APP_VERSION = "1.0.0";

        /// <summary>
        /// 版本定时检查器（每1小时轮询一次数据库）
        /// </summary>
        private System.Windows.Forms.Timer _versionCheckTimer;

        public FrmMaintenanceMain()
        {
            InitializeComponent();
            if (CheckVersion())
            {
                InitializeUI();
                StartVersionCheckTimer();
            }
        }

        /// <summary>
        /// 启动版本定时检查（每1小时查一次数据库）
        /// 不重启程序也能拦截
        /// </summary>
        private void StartVersionCheckTimer()
        {
            _versionCheckTimer = new System.Windows.Forms.Timer();
            _versionCheckTimer.Interval = 60 * 60 * 1000; // 1小时 = 3600000毫秒
            _versionCheckTimer.Tick += (s, e) =>
            {
                if (!CheckVersion())
                {
                    _versionCheckTimer.Stop();
                    this.Close();
                }
            };
            _versionCheckTimer.Start();
        }

        /// <summary>
        /// 窗体关闭时停止定时器，释放资源
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _versionCheckTimer?.Stop();
            _versionCheckTimer?.Dispose();
            base.OnFormClosed(e);
        }

        /// <summary>
        /// 版本校验：直接查 sys_config 表，比对 config_value 与程序版本
        /// config_key = 'APP_VERSION'，config_value 存放数据库要求的最低版本号
        /// 当 程序版本 &lt; 数据库版本 时拦截，不让用户使用
        /// </summary>
        /// <returns>true=通过, false=拦截</returns>
        private bool CheckVersion()
        {
            try
            {
                using var db = DbHelper.GetInstance();
                // 直接查 sys_config 一行数据
                string requiredVersion = db.Ado.GetString(
                    "SELECT config_value FROM sys_config WHERE config_key = @key LIMIT 1",
                    new { key = "APP_VERSION" });

                // 数据库没配置版本信息，放行
                if (string.IsNullOrWhiteSpace(requiredVersion))
                    return true;

                // 比对版本号
                if (CompareVersion(APP_VERSION, requiredVersion.Trim()) < 0)
                {
                    UIMessageBox.Show(
                        $"当前程序版本过旧，已被禁止使用！\n\n" +
                        $"当前版本：{APP_VERSION}\n" +
                        $"要求版本：{requiredVersion}\n\n" +
                        $"请联系管理员获取最新版本后再使用。",
                        "版本过期 - 禁止访问",
                        UIStyle.Red,
                        UIMessageBoxButtons.OK);

                    // 如果是构造阶段调用，用 Load 延迟关闭
                    if (!this.IsHandleCreated)
                        this.Load += (s, e) => this.Close();

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                UIMessageBox.Show(
                    $"版本校验失败，无法连接数据库！\n\n错误：{ex.Message}",
                    "版本校验异常",
                    UIStyle.Red,
                    UIMessageBoxButtons.OK);

                if (!this.IsHandleCreated)
                    this.Load += (s, e) => this.Close();

                return false;
            }
        }

        /// <summary>
        /// 版本号比较 (如 "1.0.0" vs "1.1.0")
        /// </summary>
        /// <returns>-1: v1 小于 v2, 0: 相等, 1: v1 大于 v2</returns>
        private static int CompareVersion(string v1, string v2)
        {
            var p1 = v1.Split('.');
            var p2 = v2.Split('.');
            int len = Math.Max(p1.Length, p2.Length);
            for (int i = 0; i < len; i++)
            {
                int n1 = i < p1.Length ? int.Parse(p1[i]) : 0;
                int n2 = i < p2.Length ? int.Parse(p2[i]) : 0;
                if (n1 < n2) return -1;
                if (n1 > n2) return 1;
            }
            return 0;
        }

        private void InitializeUI()
        {
            Text = "设备工装保养管理系统";
            Width = 1200;
            Height = 800;
            ShowRadius = false;
            RectColor = System.Drawing.Color.FromArgb(80, 160, 255);

            navMenu.MenuStyle = UIMenuStyle.Custom;
            navMenu.BackColor = System.Drawing.Color.FromArgb(80, 160, 255);
            navMenu.SelectedForeColor = System.Drawing.Color.White;
            navMenu.SelectedHighColor = System.Drawing.Color.FromArgb(255, 184, 0);
            navMenu.HoverColor = System.Drawing.Color.FromArgb(100, 180, 255);
            
            var rootNode = navMenu.CreateNode("设备工装保养", 61461, 24, 0);
            navMenu.CreateChildNode(rootNode, "设备管理", 1001);
            navMenu.CreateChildNode(rootNode, "工装管理", 1002);
            navMenu.CreateChildNode(rootNode, "保养计划", 1003);
            navMenu.CreateChildNode(rootNode, "保养记录", 1004);
            navMenu.CreateChildNode(rootNode, "保养执行", 1005);
            navMenu.CreateChildNode(rootNode, "基础数据", 1006);
            navMenu.CreateChildNode(rootNode, "数据导入导出", 1007);

            navMenu.MenuItemClick += NavMenu_MenuItemClick;
            // 默认选中第一个子节点
            if (rootNode.Nodes.Count > 0)
            {
                navMenu.SelectPage(1001);
            }
        }

        private void NavMenu_MenuItemClick(TreeNode node, NavMenuItem item, int pageIndex)
        {
            switch (pageIndex)
            {
                case 1001:
                    LoadEquipmentForm();
                    break;
                case 1002:
                    LoadToolForm();
                    break;
                case 1003:
                    LoadMaintenancePlanForm();
                    break;
                case 1004:
                    LoadMaintenanceRecordForm();
                    break;
                case 1005:
                    LoadMaintenanceExecuteForm();
                    break;
                case 1006:
                    LoadBaseDataForm();
                    break;
                case 1007:
                    LoadDataImportExportForm();
                    break;
            }
        }

        private void LoadEquipmentForm()
        {
            pnlContent.Controls.Clear();
            var form = new FrmEquipmentManagement
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
            pnlContent.Controls.Add(form);
            form.Show();
        }

        private void LoadToolForm()
        {
            pnlContent.Controls.Clear();
            var form = new FrmToolManagement
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
            pnlContent.Controls.Add(form);
            form.Show();
        }

        private void LoadMaintenancePlanForm()
        {
            pnlContent.Controls.Clear();
            var form = new FrmMaintenancePlan
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
            pnlContent.Controls.Add(form);
            form.Show();
        }

        private void LoadMaintenanceRecordForm()
        {
            pnlContent.Controls.Clear();
            var form = new FrmMaintenanceRecord
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
            pnlContent.Controls.Add(form);
            form.Show();
        }

        private void LoadMaintenanceExecuteForm()
        {
            pnlContent.Controls.Clear();
            var form = new FrmMaintenanceExecute
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
            pnlContent.Controls.Add(form);
            form.Show();
        }

        private void LoadBaseDataForm()
        {
            pnlContent.Controls.Clear();
            var form = new FrmBaseDataManagement
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
            pnlContent.Controls.Add(form);
            form.Show();
        }

        private void LoadDataImportExportForm()
        {
            pnlContent.Controls.Clear();
            var form = new FrmDataImportExport
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
            pnlContent.Controls.Add(form);
            form.Show();
        }
    }
}
