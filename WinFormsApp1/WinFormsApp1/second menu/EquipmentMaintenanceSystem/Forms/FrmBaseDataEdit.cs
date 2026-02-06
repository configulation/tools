using Sunny.UI;
using System;
using System.Windows.Forms;
using WinFormsApp1.EquipmentMaintenanceSystem.Models;
using WinFormsApp1.EquipmentMaintenanceSystem.Services;
using System.Linq;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    public partial class FrmBaseDataEdit : UIForm
    {
        private readonly string dataType;
        private readonly object editItem;
        private readonly BaseDataService baseDataService;

        public FrmBaseDataEdit(string type, object item)
        {
            InitializeComponent();
            dataType = type;
            editItem = item;
            baseDataService = new BaseDataService();
            InitializeUI();
            LoadData();
        }
        
        private bool IsNewItem()
        {
            if (editItem == null) return true;
            
            return editItem switch
            {
                LineLocation ll => string.IsNullOrEmpty(ll.Id),
                Category c => string.IsNullOrEmpty(c.Id),
                SubCategory sc => string.IsNullOrEmpty(sc.Id),
                MaintenanceItem mi => string.IsNullOrEmpty(mi.Id),
                Operator op => string.IsNullOrEmpty(op.Id),
                _ => true
            };
        }

        private void InitializeUI()
        {
            Text = editItem == null ? $"新增{dataType}" : $"编辑{dataType}";
            ShowRadius = false;
        }

        private void LoadData()
        {
            switch (dataType)
            {
                case "线位储位":
                    LoadLineLocationData();
                    break;
                case "类别":
                    LoadCategoryData();
                    break;
                case "子类别":
                    LoadSubCategoryData();
                    break;
                case "保养项目":
                    LoadMaintenanceItemData();
                    break;
                case "操作员":
                    LoadOperatorData();
                    break;
            }
        }

        private void LoadLineLocationData()
        {
            lblField1.Text = "*编码";
            lblField2.Text = "*名称";
            lblField3.Text = "描述";
            lblField4.Visible = false;
            txtField4.Visible = false;
            lblField5.Visible = false;
            txtField5.Visible = false;
            cmbField1.Visible = false;

            if (editItem is LineLocation item)
            {
                txtField1.Text = item.Code;
                txtField1.ReadOnly = !IsNewItem();
                txtField2.Text = item.Name;
                txtField3.Text = item.Description;
                chkIsActive.Checked = item.IsActive;
            }
        }

        private void LoadCategoryData()
        {
            lblField1.Text = "*编码";
            lblField2.Text = "*名称";
            lblField3.Text = "*类型";
            lblField4.Visible = false;
            txtField4.Visible = false;
            lblField5.Visible = false;
            txtField5.Visible = false;
            
            txtField3.Visible = false;
            cmbField1.Visible = true;
            cmbField1.Location = txtField3.Location;
            cmbField1.Items.Clear();
            cmbField1.Items.AddRange(new string[] { "设备", "工装" });

            if (editItem is Category item)
            {
                txtField1.Text = item.Code;
                txtField1.ReadOnly = !IsNewItem();
                txtField2.Text = item.Name;
                cmbField1.Text = item.Type;
                chkIsActive.Checked = item.IsActive;
            }
            else
            {
                cmbField1.SelectedIndex = 0;
            }
        }

        private void LoadSubCategoryData()
        {
            lblField1.Text = "*编码";
            lblField2.Text = "*名称";
            lblField3.Text = "*所属类别";
            lblField4.Visible = false;
            txtField4.Visible = false;
            lblField5.Visible = false;
            txtField5.Visible = false;

            txtField3.Visible = false;
            cmbField1.Visible = true;
            cmbField1.Location = txtField3.Location;
            cmbField1.Items.Clear();
            var categories = baseDataService.LoadCategories().Where(x => x.IsActive).Select(x => x.Name).ToArray();
            cmbField1.Items.AddRange(categories);

            if (editItem is SubCategory item)
            {
                txtField1.Text = item.Code;
                txtField1.ReadOnly = !IsNewItem();
                txtField2.Text = item.Name;
                var category = baseDataService.LoadCategories().FirstOrDefault(x => x.Id == item.CategoryId);
                if (category != null)
                    cmbField1.Text = category.Name;
                chkIsActive.Checked = item.IsActive;
            }
        }

        private void LoadMaintenanceItemData()
        {
            lblField1.Text = "*编码";
            lblField2.Text = "*名称";
            lblField3.Text = "*类型";
            lblField4.Text = "描述";
            lblField4.Visible = true;
            txtField4.Visible = true;
            lblField5.Text = "标准耗时(分钟)";
            lblField5.Visible = true;
            txtField5.Visible = true;

            txtField3.Visible = false;
            cmbField1.Visible = true;
            cmbField1.Location = txtField3.Location;
            cmbField1.Items.Clear();
            cmbField1.Items.AddRange(new string[] { "设备", "工装" });

            if (editItem is MaintenanceItem item)
            {
                txtField1.Text = item.Code;
                txtField1.ReadOnly = !IsNewItem();
                txtField2.Text = item.Name;
                cmbField1.Text = item.Type;
                txtField4.Text = item.Description;
                txtField5.Text = item.StandardDuration.ToString();
                chkIsActive.Checked = item.IsActive;
            }
            else
            {
                cmbField1.SelectedIndex = 0;
                txtField5.Text = "30";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                bool success = false;
                switch (dataType)
                {
                    case "线位储位":
                        success = SaveLineLocation();
                        break;
                    case "类别":
                        success = SaveCategory();
                        break;
                    case "子类别":
                        success = SaveSubCategory();
                        break;
                    case "保养项目":
                        success = SaveMaintenanceItem();
                        break;
                    case "操作员":
                        success = SaveOperator();
                        break;
                }

                if (success)
                {
                    UIMessageBox.ShowSuccess("保存成功！");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    UIMessageBox.ShowError("保存失败，编码可能已存在！");
                }
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError($"保存失败：{ex.Message}");
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtField1.Text))
            {
                UIMessageBox.ShowWarning("请输入编码！");
                txtField1.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtField2.Text))
            {
                UIMessageBox.ShowWarning("请输入名称！");
                txtField2.Focus();
                return false;
            }

            return true;
        }

        private bool SaveLineLocation()
        {
            var item = editItem as LineLocation ?? new LineLocation();
            item.Code = txtField1.Text.Trim();
            item.Name = txtField2.Text.Trim();
            item.Description = txtField3.Text.Trim();
            item.IsActive = chkIsActive.Checked;

            return editItem == null ? baseDataService.AddLineLocation(item) : baseDataService.UpdateLineLocation(item);
        }

        private bool SaveCategory()
        {
            var item = editItem as Category ?? new Category();
            item.Code = txtField1.Text.Trim();
            item.Name = txtField2.Text.Trim();
            item.Type = cmbField1.Text;
            item.IsActive = chkIsActive.Checked;

            return editItem == null ? baseDataService.AddCategory(item) : baseDataService.UpdateCategory(item);
        }

        private bool SaveSubCategory()
        {
            var category = baseDataService.LoadCategories().FirstOrDefault(x => x.Name == cmbField1.Text);
            if (category == null)
            {
                UIMessageBox.ShowWarning("请选择所属类别！");
                return false;
            }

            var item = editItem as SubCategory ?? new SubCategory();
            item.Code = txtField1.Text.Trim();
            item.Name = txtField2.Text.Trim();
            item.CategoryId = category.Id;
            item.IsActive = chkIsActive.Checked;

            return editItem == null ? baseDataService.AddSubCategory(item) : baseDataService.UpdateSubCategory(item);
        }

        private bool SaveMaintenanceItem()
        {
            if (!int.TryParse(txtField5.Text, out int duration))
            {
                UIMessageBox.ShowWarning("标准耗时必须是数字！");
                return false;
            }

            var item = editItem as MaintenanceItem ?? new MaintenanceItem();
            item.Code = txtField1.Text.Trim();
            item.Name = txtField2.Text.Trim();
            item.Type = cmbField1.Text;
            item.Description = txtField4.Text.Trim();
            item.StandardDuration = duration;
            item.IsActive = chkIsActive.Checked;

            return editItem == null ? baseDataService.AddMaintenanceItem(item) : baseDataService.UpdateMaintenanceItem(item);
        }

        private bool SaveOperator()
        {
            var item = editItem as Operator ?? new Operator();
            item.Code = txtField1.Text.Trim();
            item.Name = txtField2.Text.Trim();
            item.Department = txtField3.Text.Trim();
            item.Phone = txtField4.Text.Trim();
            item.IsActive = chkIsActive.Checked;

            return editItem == null ? baseDataService.AddOperator(item) : baseDataService.UpdateOperator(item);
        }

        private void LoadOperatorData()
        {
            lblField1.Text = "*工号";
            lblField2.Text = "*姓名";
            lblField3.Text = "部门";
            lblField4.Text = "电话";
            lblField4.Visible = true;
            txtField4.Visible = true;
            lblField5.Visible = false;
            txtField5.Visible = false;
            cmbField1.Visible = false;

            if (editItem is Operator item)
            {
                txtField1.Text = item.Code;
                txtField1.ReadOnly = !IsNewItem();
                txtField2.Text = item.Name;
                txtField3.Text = item.Department;
                txtField4.Text = item.Phone;
                chkIsActive.Checked = item.IsActive;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
