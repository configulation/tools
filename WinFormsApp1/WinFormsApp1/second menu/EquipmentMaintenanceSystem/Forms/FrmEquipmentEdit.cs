using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp1.EquipmentMaintenanceSystem.Models;
using WinFormsApp1.EquipmentMaintenanceSystem.Services;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    public partial class FrmEquipmentEdit : UIForm
    {
        private readonly Equipment _equipment;
        private readonly DataService _dataService;
        private readonly ValidationService _validationService;
        private readonly BaseDataService _baseDataService;
        private readonly bool _isEdit;

        public FrmEquipmentEdit(Equipment equipment, DataService dataService, ValidationService validationService)
        {
            InitializeComponent();
            _equipment = equipment;
            _dataService = dataService;
            _validationService = validationService;
            _baseDataService = new BaseDataService();
            _isEdit = equipment != null;

            InitializeComboBoxes();
            LoadData();
        }

        private void InitializeComboBoxes()
        {
            cmbStatus.Items.AddRange(new string[] { "新购", "正常使用", "保养中", "维修中" });
            cmbStatus.SelectedIndex = 1;

            var lineLocations = _baseDataService.GetActiveLineLocationNames();
            cmbLineLocation.Items.Clear();
            cmbLineLocation.Items.AddRange(lineLocations.ToArray());

            var categories = _baseDataService.GetActiveCategoryNames("设备");
            cmbCategory.Items.Clear();
            cmbCategory.Items.AddRange(categories.ToArray());
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedIndex >= 0)
            {
                var categoryName = cmbCategory.Text;
                var subCategories = _baseDataService.GetActiveSubCategoryNames(categoryName);
                cmbSubCategory.Items.Clear();
                cmbSubCategory.Items.AddRange(subCategories.ToArray());
            }
        }

        private void LoadData()
        {
            if (_isEdit)
            {
                Text = "编辑设备";
                txtEquipmentId.Text = _equipment.EquipmentId;
                txtEquipmentId.ReadOnly = true;
                cmbLineLocation.Text = _equipment.LineLocation;
                cmbCategory.Text = _equipment.Category;
                
                var subCategories = _baseDataService.GetActiveSubCategoryNames(_equipment.Category);
                cmbSubCategory.Items.Clear();
                cmbSubCategory.Items.AddRange(subCategories.ToArray());
                cmbSubCategory.Text = _equipment.SubCategory;
                
                txtIntervalDays.Text = _equipment.MaintenanceIntervalDays.ToString();
                dtpNextMaintenance.Value = _equipment.NextMaintenanceDate;
                cmbStatus.Text = _equipment.Status;
                txtOperatorId.Text = _equipment.OperatorId;
                txtMaintenanceItems.Text = string.Join(", ", _equipment.MaintenanceItems);
                txtNotes.Text = _equipment.Notes;
            }
            else
            {
                Text = "新增设备";
                dtpNextMaintenance.Value = DateTime.Now.AddDays(15);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var equipment = _isEdit ? _equipment : new Equipment();

            equipment.EquipmentId = txtEquipmentId.Text.Trim();
            equipment.LineLocation = cmbLineLocation.Text.Trim();
            equipment.Category = cmbCategory.Text.Trim();
            equipment.SubCategory = cmbSubCategory.Text.Trim();
            
            if (!int.TryParse(txtIntervalDays.Text, out int intervalDays))
            {
                UIMessageBox.ShowWarning("保养周期必须是数字");
                return;
            }
            equipment.MaintenanceIntervalDays = intervalDays;
            
            equipment.NextMaintenanceDate = dtpNextMaintenance.Value;
            equipment.Status = cmbStatus.Text;
            equipment.OperatorId = txtOperatorId.Text.Trim();
            equipment.Notes = txtNotes.Text.Trim();

            var items = txtMaintenanceItems.Text.Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            equipment.MaintenanceItems.Clear();
            foreach (var item in items)
            {
                equipment.MaintenanceItems.Add(item.Trim());
            }

            var validation = _validationService.ValidateEquipment(equipment);
            if (!validation.isValid)
            {
                UIMessageBox.ShowWarning(validation.message);
                return;
            }

            bool success;
            if (_isEdit)
            {
                success = _dataService.UpdateEquipment(equipment);
            }
            else
            {
                success = _dataService.AddEquipment(equipment);
            }

            if (success)
            {
                UIMessageBox.ShowSuccess(_isEdit ? "更新成功" : "添加成功");
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                UIMessageBox.ShowError(_isEdit ? "更新失败" : "添加失败，设备ID可能已存在");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSelectItems_Click(object sender, EventArgs e)
        {
            var currentItems = new List<string>();
            if (!string.IsNullOrWhiteSpace(txtMaintenanceItems.Text))
            {
                var items = txtMaintenanceItems.Text.Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                currentItems = items.Select(x => x.Trim()).ToList();
            }

            using (var frm = new FrmMaintenanceItemSelector("设备", currentItems))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtMaintenanceItems.Text = string.Join(", ", frm.SelectedItems);
                }
            }
        }
    }
}
