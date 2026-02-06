using Sunny.UI;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp1.EquipmentMaintenanceSystem.Models;
using WinFormsApp1.EquipmentMaintenanceSystem.Services;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    public partial class FrmEquipmentManagement : UIForm
    {
        private readonly DataService _dataService;
        private readonly MaintenanceService _maintenanceService;
        private readonly ValidationService _validationService;

        public FrmEquipmentManagement()
        {
            InitializeComponent();
            _dataService = new DataService();
            _maintenanceService = new MaintenanceService(_dataService);
            _validationService = new ValidationService();
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            dgvEquipment.AllowUserToAddRows = false;
            dgvEquipment.AutoGenerateColumns = false;
            dgvEquipment.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEquipment.MultiSelect = false;

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EquipmentId",
                HeaderText = "设备ID",
                DataPropertyName = "EquipmentId",
                Width = 150
            });

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LineLocation",
                HeaderText = "线别储位",
                DataPropertyName = "LineLocation",
                Width = 120
            });

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Category",
                HeaderText = "类别",
                DataPropertyName = "Category",
                Width = 100
            });

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SubCategory",
                HeaderText = "子类别",
                DataPropertyName = "SubCategory",
                Width = 100
            });

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaintenanceIntervalDays",
                HeaderText = "周期(天)",
                DataPropertyName = "MaintenanceIntervalDays",
                Width = 80
            });

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NextMaintenanceDate",
                HeaderText = "计划日期",
                DataPropertyName = "NextMaintenanceDate",
                Width = 120
            });

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "状态",
                DataPropertyName = "Status",
                Width = 100
            });
        }

        private void FrmEquipmentManagement_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            var equipments = _dataService.LoadEquipments();
            dgvEquipment.DataSource = equipments;

            foreach (DataGridViewRow row in dgvEquipment.Rows)
            {
                var equipment = row.DataBoundItem as Equipment;
                if (equipment != null)
                {
                    if (equipment.NextMaintenanceDate < DateTime.Now)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 210);
                    }
                    else if (equipment.NextMaintenanceDate <= DateTime.Now.AddDays(7))
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 196);
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
                    }
                }
            }

            lblTotal.Text = $"共 {equipments.Count} 条记录";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var dialog = new FrmEquipmentEdit(null, _dataService, _validationService);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvEquipment.SelectedRows.Count == 0)
            {
                UIMessageBox.ShowWarning("请选择要编辑的设备");
                return;
            }

            var equipment = dgvEquipment.SelectedRows[0].DataBoundItem as Equipment;
            var dialog = new FrmEquipmentEdit(equipment, _dataService, _validationService);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEquipment.SelectedRows.Count == 0)
            {
                UIMessageBox.ShowWarning("请选择要删除的设备");
                return;
            }

            if (UIMessageBox.ShowAsk("确定要删除选中的设备吗？"))
            {
                var equipment = dgvEquipment.SelectedRows[0].DataBoundItem as Equipment;
                if (_dataService.DeleteEquipment(equipment.EquipmentId))
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
            if (dgvEquipment.SelectedRows.Count == 0)
            {
                UIMessageBox.ShowWarning("请选择要保养的设备");
                return;
            }

            var equipment = dgvEquipment.SelectedRows[0].DataBoundItem as Equipment;
            var dialog = new FrmMaintenanceExecute(equipment.EquipmentId, "Equipment", _maintenanceService);
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

            var equipments = _dataService.LoadEquipments()
                .Where(e => e.EquipmentId.Contains(keyword) ||
                           e.LineLocation.Contains(keyword) ||
                           e.Category.Contains(keyword) ||
                           e.SubCategory.Contains(keyword))
                .ToList();

            dgvEquipment.DataSource = equipments;
            lblTotal.Text = $"共 {equipments.Count} 条记录";
        }
    }
}
