using Sunny.UI;
using System;
using System.Windows.Forms;
using WinFormsApp1.EquipmentMaintenanceSystem.Models;
using WinFormsApp1.EquipmentMaintenanceSystem.Services;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    public partial class FrmToolEdit : UIForm
    {
        private readonly Tool _tool;
        private readonly DataService _dataService;
        private readonly ValidationService _validationService;
        private readonly BaseDataService _baseDataService;
        private readonly bool _isEdit;

        public FrmToolEdit(Tool tool, DataService dataService, ValidationService validationService)
        {
            InitializeComponent();
            _tool = tool;
            _dataService = dataService;
            _validationService = validationService;
            _baseDataService = new BaseDataService();
            _isEdit = tool != null;

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

            var categories = _baseDataService.GetActiveCategoryNames("工装");
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
                Text = "编辑工装";
                txtToolCode.Text = _tool.ToolCode;
                txtToolCode.ReadOnly = true;
                cmbLineLocation.Text = _tool.LineLocation;
                cmbCategory.Text = _tool.Category;
                
                var subCategories = _baseDataService.GetActiveSubCategoryNames(_tool.Category);
                cmbSubCategory.Items.Clear();
                cmbSubCategory.Items.AddRange(subCategories.ToArray());
                cmbSubCategory.Text = _tool.SubCategory;
                
                txtWorkOrder.Text = _tool.WorkOrder;
                txtOrderQuantity.Text = _tool.OrderQuantity.ToString();
                txtPanelQuantity.Text = _tool.PanelQuantity.ToString();
                txtScraperCount.Text = _tool.ScraperCount.ToString();
                txtUsageCount.Text = _tool.UsageCount.ToString();
                txtTotalUsage.Text = _tool.TotalUsage.ToString();
                txtMaintenanceInterval.Text = _tool.MaintenanceInterval;
                dtpNextMaintenance.Value = _tool.NextMaintenanceDate;
                cmbStatus.Text = _tool.Status;
                txtMaintenanceItems.Text = string.Join(", ", _tool.MaintenanceItems);
                txtNotes.Text = _tool.Notes;
            }
            else
            {
                Text = "新增工装";
                dtpNextMaintenance.Value = DateTime.Now.AddDays(30);
                txtMaintenanceInterval.Text = "200000次";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var tool = _isEdit ? _tool : new Tool();

            tool.ToolCode = txtToolCode.Text.Trim();
            tool.LineLocation = cmbLineLocation.Text.Trim();
            tool.Category = cmbCategory.Text.Trim();
            tool.SubCategory = cmbSubCategory.Text.Trim();
            tool.WorkOrder = txtWorkOrder.Text.Trim();

            if (!int.TryParse(txtOrderQuantity.Text, out int orderQty))
            {
                UIMessageBox.ShowWarning("工单数量必须是数字");
                return;
            }
            tool.OrderQuantity = orderQty;

            if (!int.TryParse(txtPanelQuantity.Text, out int panelQty))
            {
                UIMessageBox.ShowWarning("拼料数量必须是数字");
                return;
            }
            tool.PanelQuantity = panelQty;

            if (!int.TryParse(txtScraperCount.Text, out int scraperCount))
            {
                UIMessageBox.ShowWarning("刮刀数量必须是数字");
                return;
            }
            tool.ScraperCount = scraperCount;

            if (!int.TryParse(txtUsageCount.Text, out int usageCount))
            {
                UIMessageBox.ShowWarning("使用次数必须是数字");
                return;
            }
            tool.UsageCount = usageCount;

            if (!int.TryParse(txtTotalUsage.Text, out int totalUsage))
            {
                UIMessageBox.ShowWarning("总使用次数必须是数字");
                return;
            }
            tool.TotalUsage = totalUsage;

            tool.MaintenanceInterval = txtMaintenanceInterval.Text.Trim();
            tool.NextMaintenanceDate = dtpNextMaintenance.Value;
            tool.Status = cmbStatus.Text;
            tool.Notes = txtNotes.Text.Trim();

            var items = txtMaintenanceItems.Text.Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            tool.MaintenanceItems.Clear();
            foreach (var item in items)
            {
                tool.MaintenanceItems.Add(item.Trim());
            }

            var validation = _validationService.ValidateTool(tool);
            if (!validation.isValid)
            {
                UIMessageBox.ShowWarning(validation.message);
                return;
            }

            bool success;
            if (_isEdit)
            {
                success = _dataService.UpdateTool(tool);
            }
            else
            {
                success = _dataService.AddTool(tool);
            }

            if (success)
            {
                UIMessageBox.ShowSuccess(_isEdit ? "更新成功" : "添加成功");
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                UIMessageBox.ShowError(_isEdit ? "更新失败" : "添加失败，工装编码可能已存在");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnCalculateUsage_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtOrderQuantity.Text, out int orderQty) &&
                int.TryParse(txtPanelQuantity.Text, out int panelQty) &&
                int.TryParse(txtScraperCount.Text, out int scraperCount) &&
                panelQty > 0 && scraperCount > 0)
            {
                int calculatedUsage = orderQty / panelQty / scraperCount;
                txtUsageCount.Text = calculatedUsage.ToString();
                UIMessageBox.ShowSuccess($"计算结果：{calculatedUsage} 次");
            }
            else
            {
                UIMessageBox.ShowWarning("请输入有效的数字，且拼料数量和刮刀数量必须大于0");
            }
        }
    }
}
