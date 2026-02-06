using Sunny.UI;
using System;
using System.Windows.Forms;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    public partial class FrmMaintenanceMain : UIForm
    {
        public FrmMaintenanceMain()
        {
            InitializeComponent();
            InitializeUI();
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
