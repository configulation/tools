using Sunny.UI;
using System;
using System.Windows.Forms;
using WinFormsApp1.EquipmentMaintenanceSystem.Services;
using WinFormsApp1.EquipmentMaintenanceSystem.Models;
using System.Linq;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    public partial class FrmBaseDataManagement : UIForm
    {
        private readonly BaseDataService baseDataService;
        private string currentDataType = "LineLocation";

        public FrmBaseDataManagement()
        {
            InitializeComponent();
            baseDataService = new BaseDataService();
            InitializeUI();
        }

        private void InitializeUI()
        {
            Text = "基础数据维护";
            ShowRadius = false;
            
            cmbDataType.Items.AddRange(new string[] { "线位储位", "类别", "子类别", "保养项目", "操作员" });
            cmbDataType.SelectedIndex = 0;
        }

        private void cmbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbDataType.SelectedIndex)
            {
                case 0:
                    currentDataType = "LineLocation";
                    LoadLineLocations();
                    break;
                case 1:
                    currentDataType = "Category";
                    LoadCategories();
                    break;
                case 2:
                    currentDataType = "SubCategory";
                    LoadSubCategories();
                    break;
                case 3:
                    currentDataType = "MaintenanceItem";
                    LoadMaintenanceItems();
                    break;
                case 4:
                    currentDataType = "Operator";
                    LoadOperators();
                    break;
            }
        }

        private void LoadLineLocations()
        {
            dgvData.DataSource = null;
            var data = baseDataService.LoadLineLocations();
            dgvData.DataSource = data;
            SetupLineLocationColumns();
        }

        private void LoadCategories()
        {
            dgvData.DataSource = null;
            var data = baseDataService.LoadCategories();
            dgvData.DataSource = data;
            SetupCategoryColumns();
        }

        private void LoadSubCategories()
        {
            dgvData.DataSource = null;
            var data = baseDataService.LoadSubCategories();
            dgvData.DataSource = data;
            SetupSubCategoryColumns();
        }

        private void LoadMaintenanceItems()
        {
            dgvData.DataSource = null;
            var data = baseDataService.LoadMaintenanceItems();
            dgvData.DataSource = data;
            SetupMaintenanceItemColumns();
        }

        private void LoadOperators()
        {
            dgvData.DataSource = null;
            var data = baseDataService.LoadOperators();
            dgvData.DataSource = data;
            SetupOperatorColumns();
        }

        private void SetupLineLocationColumns()
        {
            dgvData.Columns["Id"].Visible = false;
            dgvData.Columns["CreateTime"].Visible = false;
            dgvData.Columns["UpdateTime"].Visible = false;
            dgvData.Columns["Code"].HeaderText = "编码";
            dgvData.Columns["Name"].HeaderText = "名称";
            dgvData.Columns["Description"].HeaderText = "描述";
            dgvData.Columns["IsActive"].HeaderText = "启用";
        }

        private void SetupCategoryColumns()
        {
            dgvData.Columns["Id"].Visible = false;
            dgvData.Columns["CreateTime"].Visible = false;
            dgvData.Columns["UpdateTime"].Visible = false;
            dgvData.Columns["Code"].HeaderText = "编码";
            dgvData.Columns["Name"].HeaderText = "名称";
            dgvData.Columns["Type"].HeaderText = "类型";
            dgvData.Columns["IsActive"].HeaderText = "启用";
        }

        private void SetupSubCategoryColumns()
        {
            dgvData.Columns["Id"].Visible = false;
            dgvData.Columns["CategoryId"].Visible = false;
            dgvData.Columns["CreateTime"].Visible = false;
            dgvData.Columns["UpdateTime"].Visible = false;
            dgvData.Columns["Code"].HeaderText = "编码";
            dgvData.Columns["Name"].HeaderText = "名称";
            dgvData.Columns["IsActive"].HeaderText = "启用";
        }

        private void SetupMaintenanceItemColumns()
        {
            dgvData.Columns["Id"].Visible = false;
            dgvData.Columns["CategoryId"].Visible = false;
            dgvData.Columns["CreateTime"].Visible = false;
            dgvData.Columns["UpdateTime"].Visible = false;
            dgvData.Columns["Code"].HeaderText = "编码";
            dgvData.Columns["Name"].HeaderText = "名称";
            dgvData.Columns["Type"].HeaderText = "类型";
            dgvData.Columns["Description"].HeaderText = "描述";
            dgvData.Columns["StandardDuration"].HeaderText = "标准耗时(分钟)";
            dgvData.Columns["IsActive"].HeaderText = "启用";
        }

        private void SetupOperatorColumns()
        {
            dgvData.Columns["Id"].Visible = false;
            dgvData.Columns["CreateTime"].Visible = false;
            dgvData.Columns["UpdateTime"].Visible = false;
            dgvData.Columns["Code"].HeaderText = "工号";
            dgvData.Columns["Name"].HeaderText = "姓名";
            dgvData.Columns["Department"].HeaderText = "部门";
            dgvData.Columns["Phone"].HeaderText = "电话";
            dgvData.Columns["IsActive"].HeaderText = "启用";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            switch (currentDataType)
            {
                case "LineLocation":
                    ShowLineLocationEditDialog(null);
                    break;
                case "Category":
                    ShowCategoryEditDialog(null);
                    break;
                case "SubCategory":
                    ShowSubCategoryEditDialog(null);
                    break;
                case "MaintenanceItem":
                    ShowMaintenanceItemEditDialog(null);
                    break;
                case "Operator":
                    ShowOperatorEditDialog(null);
                    break;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0)
            {
                UIMessageBox.ShowWarning("请选择要编辑的数据！");
                return;
            }

            var selectedRow = dgvData.SelectedRows[0];
            switch (currentDataType)
            {
                case "LineLocation":
                    ShowLineLocationEditDialog(selectedRow.DataBoundItem as LineLocation);
                    break;
                case "Category":
                    ShowCategoryEditDialog(selectedRow.DataBoundItem as Category);
                    break;
                case "SubCategory":
                    ShowSubCategoryEditDialog(selectedRow.DataBoundItem as SubCategory);
                    break;
                case "MaintenanceItem":
                    ShowMaintenanceItemEditDialog(selectedRow.DataBoundItem as MaintenanceItem);
                    break;
                case "Operator":
                    ShowOperatorEditDialog(selectedRow.DataBoundItem as Operator);
                    break;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0)
            {
                UIMessageBox.ShowWarning("请选择要复制的数据！");
                return;
            }

            var selectedRow = dgvData.SelectedRows[0];
            switch (currentDataType)
            {
                case "LineLocation":
                    var lineLocation = selectedRow.DataBoundItem as LineLocation;
                    if (lineLocation != null)
                    {
                        var newItem = new LineLocation
                        {
                            Code = lineLocation.Code + "_Copy",
                            Name = lineLocation.Name + "_副本",
                            Description = lineLocation.Description,
                            IsActive = lineLocation.IsActive
                        };
                        ShowLineLocationEditDialog(newItem);
                    }
                    break;
                case "Category":
                    var category = selectedRow.DataBoundItem as Category;
                    if (category != null)
                    {
                        var newItem = new Category
                        {
                            Code = category.Code + "_Copy",
                            Name = category.Name + "_副本",
                            Type = category.Type,
                            IsActive = category.IsActive
                        };
                        ShowCategoryEditDialog(newItem);
                    }
                    break;
                case "SubCategory":
                    var subCategory = selectedRow.DataBoundItem as SubCategory;
                    if (subCategory != null)
                    {
                        var newItem = new SubCategory
                        {
                            Code = subCategory.Code + "_Copy",
                            Name = subCategory.Name + "_副本",
                            CategoryId = subCategory.CategoryId,
                            IsActive = subCategory.IsActive
                        };
                        ShowSubCategoryEditDialog(newItem);
                    }
                    break;
                case "MaintenanceItem":
                    var maintenanceItem = selectedRow.DataBoundItem as MaintenanceItem;
                    if (maintenanceItem != null)
                    {
                        var newItem = new MaintenanceItem
                        {
                            Code = maintenanceItem.Code + "_Copy",
                            Name = maintenanceItem.Name + "_副本",
                            Type = maintenanceItem.Type,
                            Description = maintenanceItem.Description,
                            StandardDuration = maintenanceItem.StandardDuration,
                            IsActive = maintenanceItem.IsActive
                        };
                        ShowMaintenanceItemEditDialog(newItem);
                    }
                    break;
                case "Operator":
                    var operatorItem = selectedRow.DataBoundItem as Operator;
                    if (operatorItem != null)
                    {
                        var newItem = new Operator
                        {
                            Code = operatorItem.Code + "_Copy",
                            Name = operatorItem.Name + "_副本",
                            Department = operatorItem.Department,
                            Phone = operatorItem.Phone,
                            IsActive = operatorItem.IsActive
                        };
                        ShowOperatorEditDialog(newItem);
                    }
                    break;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0)
            {
                UIMessageBox.ShowWarning("请选择要删除的数据！");
                return;
            }

            if (!UIMessageBox.ShowAsk("确定要删除选中的数据吗？"))
                return;

            var selectedRow = dgvData.SelectedRows[0];
            string id = string.Empty;

            switch (currentDataType)
            {
                case "LineLocation":
                    id = (selectedRow.DataBoundItem as LineLocation)?.Id;
                    if (!string.IsNullOrEmpty(id))
                        baseDataService.DeleteLineLocation(id);
                    break;
                case "Category":
                    id = (selectedRow.DataBoundItem as Category)?.Id;
                    if (!string.IsNullOrEmpty(id))
                        baseDataService.DeleteCategory(id);
                    break;
                case "SubCategory":
                    id = (selectedRow.DataBoundItem as SubCategory)?.Id;
                    if (!string.IsNullOrEmpty(id))
                        baseDataService.DeleteSubCategory(id);
                    break;
                case "MaintenanceItem":
                    id = (selectedRow.DataBoundItem as MaintenanceItem)?.Id;
                    if (!string.IsNullOrEmpty(id))
                        baseDataService.DeleteMaintenanceItem(id);
                    break;
                case "Operator":
                    id = (selectedRow.DataBoundItem as Operator)?.Id;
                    if (!string.IsNullOrEmpty(id))
                        baseDataService.DeleteOperator(id);
                    break;
            }

            UIMessageBox.ShowSuccess("删除成功！");
            cmbDataType_SelectedIndexChanged(null, null);
        }

        private void ShowLineLocationEditDialog(LineLocation item)
        {
            using (var dialog = new FrmBaseDataEdit("线位储位", item))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadLineLocations();
                }
            }
        }

        private void ShowCategoryEditDialog(Category item)
        {
            using (var dialog = new FrmBaseDataEdit("类别", item))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadCategories();
                }
            }
        }

        private void ShowSubCategoryEditDialog(SubCategory item)
        {
            using (var dialog = new FrmBaseDataEdit("子类别", item))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadSubCategories();
                }
            }
        }

        private void ShowMaintenanceItemEditDialog(MaintenanceItem item)
        {
            using (var dialog = new FrmBaseDataEdit("保养项目", item))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadMaintenanceItems();
                }
            }
        }

        private void ShowOperatorEditDialog(Operator item)
        {
            using (var dialog = new FrmBaseDataEdit("操作员", item))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadOperators();
                }
            }
        }

        private void ShowLineLocationCopyDialog(LineLocation item)
        {
            if (item == null) return;
            
            var copyItem = new LineLocation
            {
                Code = item.Code + "_Copy",
                Name = item.Name + " (复制)",
                Description = item.Description,
                IsActive = item.IsActive
            };
            
            using (var dialog = new FrmBaseDataEdit("线位储位", copyItem))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadLineLocations();
                }
            }
        }

        private void ShowCategoryCopyDialog(Category item)
        {
            if (item == null) return;
            
            var copyItem = new Category
            {
                Code = item.Code + "_Copy",
                Name = item.Name + " (复制)",
                Type = item.Type,
                IsActive = item.IsActive
            };
            
            using (var dialog = new FrmBaseDataEdit("类别", copyItem))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadCategories();
                }
            }
        }

        private void ShowSubCategoryCopyDialog(SubCategory item)
        {
            if (item == null) return;
            
            var copyItem = new SubCategory
            {
                Code = item.Code + "_Copy",
                Name = item.Name + " (复制)",
                CategoryId = item.CategoryId,
                IsActive = item.IsActive
            };
            
            using (var dialog = new FrmBaseDataEdit("子类别", copyItem))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadSubCategories();
                }
            }
        }

        private void ShowMaintenanceItemCopyDialog(MaintenanceItem item)
        {
            if (item == null) return;
            
            var copyItem = new MaintenanceItem
            {
                Code = item.Code + "_Copy",
                Name = item.Name + " (复制)",
                Type = item.Type,
                CategoryId = item.CategoryId,
                Description = item.Description,
                StandardDuration = item.StandardDuration,
                IsActive = item.IsActive
            };
            
            using (var dialog = new FrmBaseDataEdit("保养项目", copyItem))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadMaintenanceItems();
                }
            }
        }

        private void ShowOperatorCopyDialog(Operator item)
        {
            if (item == null) return;
            
            var copyItem = new Operator
            {
                Code = item.Code + "_Copy",
                Name = item.Name + " (复制)",
                Department = item.Department,
                Phone = item.Phone,
                IsActive = item.IsActive
            };
            
            using (var dialog = new FrmBaseDataEdit("操作员", copyItem))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadOperators();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cmbDataType_SelectedIndexChanged(null, null);
        }
    }
}
