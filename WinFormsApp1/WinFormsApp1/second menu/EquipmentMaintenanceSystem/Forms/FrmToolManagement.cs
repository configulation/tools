using Sunny.UI;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp1.EquipmentMaintenanceSystem.Models;
using WinFormsApp1.EquipmentMaintenanceSystem.Services;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    public partial class FrmToolManagement : UIForm
    {
        private readonly DataService _dataService;
        private readonly MaintenanceService _maintenanceService;
        private readonly ValidationService _validationService;

        public FrmToolManagement()
        {
            InitializeComponent();
            _dataService = new DataService();
            _maintenanceService = new MaintenanceService(_dataService);
            _validationService = new ValidationService();
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            dgvTool.AllowUserToAddRows = false;
            dgvTool.AutoGenerateColumns = false;
            dgvTool.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTool.MultiSelect = false;

            dgvTool.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ToolCode",
                HeaderText = "工装编码",
                DataPropertyName = "ToolCode",
                Width = 120
            });

            dgvTool.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LineLocation",
                HeaderText = "线别储位",
                DataPropertyName = "LineLocation",
                Width = 120
            });

            dgvTool.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Category",
                HeaderText = "类别",
                DataPropertyName = "Category",
                Width = 100
            });

            dgvTool.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SubCategory",
                HeaderText = "子类别",
                DataPropertyName = "SubCategory",
                Width = 100
            });

            dgvTool.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UsageInfo",
                HeaderText = "使用次数",
                Width = 120
            });

            dgvTool.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NextMaintenanceDate",
                HeaderText = "计划日期",
                DataPropertyName = "NextMaintenanceDate",
                Width = 120
            });

            dgvTool.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "状态",
                DataPropertyName = "Status",
                Width = 100
            });
        }

        private void FrmToolManagement_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            var tools = _dataService.LoadTools();
            dgvTool.Rows.Clear();

            foreach (var tool in tools)
            {
                var row = new DataGridViewRow();
                row.CreateCells(dgvTool);
                row.Cells[0].Value = tool.ToolCode;
                row.Cells[1].Value = tool.LineLocation;
                row.Cells[2].Value = tool.Category;
                row.Cells[3].Value = tool.SubCategory;
                row.Cells[4].Value = $"{tool.UsageCount}/{tool.TotalUsage}";
                row.Cells[5].Value = tool.NextMaintenanceDate.ToString("yyyy-MM-dd");
                row.Cells[6].Value = tool.Status;
                row.Tag = tool;

                if (tool.NextMaintenanceDate < DateTime.Now || tool.UsageCount >= tool.TotalUsage)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 210);
                }
                else if (tool.NextMaintenanceDate <= DateTime.Now.AddDays(7))
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 196);
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
                }

                dgvTool.Rows.Add(row);
            }

            lblTotal.Text = $"共 {tools.Count} 条记录";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var dialog = new FrmToolEdit(null, _dataService, _validationService);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvTool.SelectedRows.Count == 0)
            {
                UIMessageBox.ShowWarning("请选择要编辑的工装");
                return;
            }

            var tool = dgvTool.SelectedRows[0].Tag as Tool;
            var dialog = new FrmToolEdit(tool, _dataService, _validationService);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTool.SelectedRows.Count == 0)
            {
                UIMessageBox.ShowWarning("请选择要删除的工装");
                return;
            }

            if (UIMessageBox.ShowAsk("确定要删除选中的工装吗？"))
            {
                var tool = dgvTool.SelectedRows[0].Tag as Tool;
                if (_dataService.DeleteTool(tool.ToolCode))
                {
                    UIMessageBox.ShowSuccess("删除成功");
                    LoadData();
                }
                else
                {
                    UIMessageBox.ShowError("删除失败");
                }
            }
        }

        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            if (dgvTool.SelectedRows.Count == 0)
            {
                UIMessageBox.ShowWarning("请选择要保养的工装");
                return;
            }

            var tool = dgvTool.SelectedRows[0].Tag as Tool;
            var dialog = new FrmMaintenanceExecute(tool.ToolCode, "Tool", _maintenanceService);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadData();
                return;
            }

            var tools = _dataService.LoadTools()
                .Where(t => t.ToolCode.Contains(keyword) ||
                           t.LineLocation.Contains(keyword) ||
                           t.Category.Contains(keyword) ||
                           t.SubCategory.Contains(keyword))
                .ToList();

            dgvTool.Rows.Clear();
            foreach (var tool in tools)
            {
                var row = new DataGridViewRow();
                row.CreateCells(dgvTool);
                row.Cells[0].Value = tool.ToolCode;
                row.Cells[1].Value = tool.LineLocation;
                row.Cells[2].Value = tool.Category;
                row.Cells[3].Value = tool.SubCategory;
                row.Cells[4].Value = $"{tool.UsageCount}/{tool.TotalUsage}";
                row.Cells[5].Value = tool.NextMaintenanceDate.ToString("yyyy-MM-dd");
                row.Cells[6].Value = tool.Status;
                row.Tag = tool;
                dgvTool.Rows.Add(row);
            }

            lblTotal.Text = $"共 {tools.Count} 条记录";
        }
    }
}
