using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp1.EquipmentMaintenanceSystem.Models;
using WinFormsApp1.EquipmentMaintenanceSystem.Services;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    public partial class FrmMaintenanceExecute : UIForm
    {
        private readonly string _targetId;
        private readonly string _targetType;
        private readonly MaintenanceService _maintenanceService;
        private readonly BaseDataService _baseDataService;
        private List<string> _selectedItems = new List<string>();
        private bool _isTaskListMode = false;

        // 构造函数1：执行具体保养（从设备管理或保养计划进入）
        public FrmMaintenanceExecute(string targetId, string targetType, MaintenanceService maintenanceService)
        {
            InitializeComponent();
            _targetId = targetId;
            _targetType = targetType;
            _maintenanceService = maintenanceService;
            _baseDataService = new BaseDataService();
            _isTaskListMode = false;

            InitializeComboBoxes();
            ShowExecuteMode();
            LoadData();
            LoadMaintenanceItems();
        }

        // 构造函数2：显示待执行任务列表（从主菜单进入）
        public FrmMaintenanceExecute()
        {
            InitializeComponent();
            _maintenanceService = new MaintenanceService(new DataService());
            _baseDataService = new BaseDataService();
            _isTaskListMode = true;
            
            ShowTaskListMode();
            InitializeTaskListView();
            LoadPendingTasks();
        }

        // 扫描二维码按钮点击事件
        private void btnScanCode_Click(object sender, EventArgs e)
        {
            using (var inputForm = new UIForm())
            {
                inputForm.Text = "扫描设备/工装编码";
                inputForm.Width = 400;
                inputForm.Height = 200;
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.MaximizeBox = false;
                inputForm.MinimizeBox = false;

                var lblTip = new UILabel
                {
                    Text = "请输入或扫描设备ID/工装编码：",
                    Location = new System.Drawing.Point(20, 50),
                    Size = new System.Drawing.Size(350, 30),
                    Font = new System.Drawing.Font("微软雅黑", 11F)
                };

                var txtCode = new UITextBox
                {
                    Location = new System.Drawing.Point(20, 90),
                    Size = new System.Drawing.Size(350, 35),
                    Font = new System.Drawing.Font("微软雅黑", 11F)
                };

                var btnOk = new UIButton
                {
                    Text = "确定",
                    Location = new System.Drawing.Point(170, 140),
                    Size = new System.Drawing.Size(90, 35),
                    Font = new System.Drawing.Font("微软雅黑", 11F)
                };

                var btnCancelScan = new UIButton
                {
                    Text = "取消",
                    Location = new System.Drawing.Point(280, 140),
                    Size = new System.Drawing.Size(90, 35),
                    Font = new System.Drawing.Font("微软雅黑", 11F)
                };

                btnOk.Click += (s, args) =>
                {
                    var code = txtCode.Text.Trim();
                    if (string.IsNullOrEmpty(code))
                    {
                        UIMessageBox.ShowWarning("请输入设备ID或工装编码");
                        return;
                    }

                    // 查找设备或工装
                    var equipment = _maintenanceService.GetDataService().LoadEquipments()
                        .FirstOrDefault(eq => eq.EquipmentId == code);
                    
                    if (equipment != null)
                    {
                        inputForm.DialogResult = DialogResult.OK;
                        inputForm.Close();
                        
                        var dialog = new FrmMaintenanceExecute(code, "Equipment", _maintenanceService);
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            LoadPendingTasks();
                        }
                        return;
                    }

                    var tool = _maintenanceService.GetDataService().LoadTools()
                        .FirstOrDefault(t => t.ToolCode == code);
                    
                    if (tool != null)
                    {
                        inputForm.DialogResult = DialogResult.OK;
                        inputForm.Close();
                        
                        var dialog = new FrmMaintenanceExecute(code, "Tool", _maintenanceService);
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            LoadPendingTasks();
                        }
                        return;
                    }

                    UIMessageBox.ShowWarning("未找到该设备或工装，请检查编码是否正确");
                };

                btnCancelScan.Click += (s, args) =>
                {
                    inputForm.DialogResult = DialogResult.Cancel;
                    inputForm.Close();
                };

                txtCode.KeyDown += (s, args) =>
                {
                    if (args.KeyCode == Keys.Enter)
                    {
                        btnOk.PerformClick();
                    }
                };

                inputForm.Controls.Add(lblTip);
                inputForm.Controls.Add(txtCode);
                inputForm.Controls.Add(btnOk);
                inputForm.Controls.Add(btnCancelScan);

                inputForm.Shown += (s, args) => txtCode.Focus();
                inputForm.ShowDialog();
            }
        }

        // 显示执行保养模式
        private void ShowExecuteMode()
        {
            // 隐藏任务列表控件
            dgvTasks.Visible = false;
            lblTaskCount.Visible = false;

            // 显示保养表单控件
            lblTargetId.Visible = true;
            txtTargetId.Visible = true;
            lblMaintenanceTime.Visible = true;
            dtpMaintenanceTime.Visible = true;
            lblOperator.Visible = true;
            txtOperator.Visible = true;
            lblMaintenanceItems.Visible = true;
            txtMaintenanceItems.Visible = true;
            btnSelectItems.Visible = true;
            lblResult.Visible = true;
            cmbResult.Visible = true;
            lblNotes.Visible = true;
            txtNotes.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;

            // 根据类型显示/隐藏间隔天数
            if (_targetType == "Tool")
            {
                lblNextIntervalDays.Visible = true;
                txtNextIntervalDays.Visible = true;
            }
            else
            {
                lblNextIntervalDays.Visible = false;
                txtNextIntervalDays.Visible = false;
            }

            // 设置窗体大小
            this.ClientSize = new System.Drawing.Size(450, 500);
        }

        // 显示任务列表模式
        private void ShowTaskListMode()
        {
            // 显示任务列表控件
            dgvTasks.Visible = true;
            lblTaskCount.Visible = true;
            btnScanCode.Visible = true;

            // 隐藏保养表单控件
            lblTargetId.Visible = false;
            txtTargetId.Visible = false;
            lblMaintenanceTime.Visible = false;
            dtpMaintenanceTime.Visible = false;
            lblOperator.Visible = false;
            txtOperator.Visible = false;
            lblMaintenanceItems.Visible = false;
            txtMaintenanceItems.Visible = false;
            btnSelectItems.Visible = false;
            lblResult.Visible = false;
            cmbResult.Visible = false;
            lblNextIntervalDays.Visible = false;
            txtNextIntervalDays.Visible = false;
            lblNotes.Visible = false;
            txtNotes.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;

            // 设置窗体大小
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Text = "保养执行";
        }

        private void InitializeTaskListView()
        {
            dgvTasks.AllowUserToAddRows = false;
            dgvTasks.AutoGenerateColumns = false;
            dgvTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTasks.MultiSelect = false;

            dgvTasks.Columns.Clear();

            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Type",
                HeaderText = "类型",
                Width = 80
            });

            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TargetId",
                HeaderText = "编号",
                Width = 120
            });

            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LineLocation",
                HeaderText = "线别储位",
                Width = 100
            });

            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Category",
                HeaderText = "类别",
                Width = 150
            });

            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NextMaintenanceDate",
                HeaderText = "计划日期",
                Width = 120
            });

            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "状态",
                Width = 120
            });

            var btnExecute = new DataGridViewButtonColumn
            {
                Name = "Execute",
                HeaderText = "操作",
                Text = "执行保养",
                UseColumnTextForButtonValue = true,
                Width = 100
            };
            dgvTasks.Columns.Add(btnExecute);

            dgvTasks.CellContentClick += dgvTasks_CellContentClick;
        }

        private void LoadPendingTasks()
        {
            dgvTasks.Rows.Clear();

            var overdueEquipments = _maintenanceService.GetOverdueEquipments();
            var upcomingEquipments = _maintenanceService.GetUpcomingEquipments(3);
            var overdueTools = _maintenanceService.GetOverdueTools();
            var upcomingTools = _maintenanceService.GetUpcomingTools(3);

            foreach (var equipment in overdueEquipments)
            {
                var daysLeft = (equipment.NextMaintenanceDate.Date - DateTime.Now.Date).Days;
                var status = "已过期 " + Math.Abs(daysLeft) + "天";
                
                var index = dgvTasks.Rows.Add(
                    "设备",
                    equipment.EquipmentId,
                    equipment.LineLocation,
                    equipment.Category + " " + equipment.SubCategory,
                    equipment.NextMaintenanceDate.ToString("yyyy-MM-dd"),
                    status
                );
                
                dgvTasks.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 210);
                dgvTasks.Rows[index].Tag = new { Type = "Equipment", Id = equipment.EquipmentId };
            }

            foreach (var equipment in upcomingEquipments)
            {
                var daysLeft = (equipment.NextMaintenanceDate.Date - DateTime.Now.Date).Days;
                var status = daysLeft == 0 ? "今天" : daysLeft + "天后";
                
                var index = dgvTasks.Rows.Add(
                    "设备",
                    equipment.EquipmentId,
                    equipment.LineLocation,
                    equipment.Category + " " + equipment.SubCategory,
                    equipment.NextMaintenanceDate.ToString("yyyy-MM-dd"),
                    status
                );
                
                dgvTasks.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 196);
                dgvTasks.Rows[index].Tag = new { Type = "Equipment", Id = equipment.EquipmentId };
            }

            foreach (var tool in overdueTools)
            {
                var daysLeft = (tool.NextMaintenanceDate.Date - DateTime.Now.Date).Days;
                var status = "已过期 " + Math.Abs(daysLeft) + "天";
                
                var index = dgvTasks.Rows.Add(
                    "工装",
                    tool.ToolCode,
                    tool.LineLocation,
                    tool.Category + " " + tool.SubCategory,
                    tool.NextMaintenanceDate.ToString("yyyy-MM-dd"),
                    status
                );
                
                dgvTasks.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 210);
                dgvTasks.Rows[index].Tag = new { Type = "Tool", Id = tool.ToolCode };
            }

            foreach (var tool in upcomingTools)
            {
                var daysLeft = (tool.NextMaintenanceDate.Date - DateTime.Now.Date).Days;
                var status = daysLeft == 0 ? "今天" : daysLeft + "天后";
                
                var index = dgvTasks.Rows.Add(
                    "工装",
                    tool.ToolCode,
                    tool.LineLocation,
                    tool.Category + " " + tool.SubCategory,
                    tool.NextMaintenanceDate.ToString("yyyy-MM-dd"),
                    status
                );
                
                dgvTasks.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 196);
                dgvTasks.Rows[index].Tag = new { Type = "Tool", Id = tool.ToolCode };
            }

            lblTaskCount.Text = $"共 {dgvTasks.Rows.Count} 项待执行任务";
        }

        private void dgvTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvTasks.Columns["Execute"].Index)
                return;

            var row = dgvTasks.Rows[e.RowIndex];
            var tag = row.Tag as dynamic;
            if (tag == null) return;

            var dialog = new FrmMaintenanceExecute(tag.Id, tag.Type, _maintenanceService);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadPendingTasks();
            }
        }

        private void InitializeComboBoxes()
        {
            cmbResult.Items.AddRange(new string[] { "正常", "异常" });
            cmbResult.SelectedIndex = 0;
        }

        private void LoadData()
        {
            Text = _targetType == "Equipment" ? "设备保养" : "工装保养";
            lblTargetId.Text = $"{(_targetType == "Equipment" ? "设备ID" : "工装编码")}:";
            txtTargetId.Text = _targetId;
            txtTargetId.ReadOnly = true;
            dtpMaintenanceTime.Value = DateTime.Now;
        }

        private void LoadMaintenanceItems()
        {
            try
            {
                if (_targetType == "Equipment")
                {
                    var equipment = _maintenanceService.GetDataService().LoadEquipments()
                        .FirstOrDefault(e => e.EquipmentId == _targetId);
                    if (equipment != null && equipment.MaintenanceItems != null && equipment.MaintenanceItems.Count > 0)
                    {
                        _selectedItems = new List<string>(equipment.MaintenanceItems);
                        txtMaintenanceItems.Text = string.Join(", ", _selectedItems);
                    }
                }
                else
                {
                    var tool = _maintenanceService.GetDataService().LoadTools()
                        .FirstOrDefault(t => t.ToolCode == _targetId);
                    if (tool != null && tool.MaintenanceItems != null && tool.MaintenanceItems.Count > 0)
                    {
                        _selectedItems = new List<string>(tool.MaintenanceItems);
                        txtMaintenanceItems.Text = string.Join(", ", _selectedItems);
                    }
                }
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowWarning($"加载保养项目失败: {ex.Message}");
            }
        }

        private void btnSelectItems_Click(object sender, EventArgs e)
        {
            var items = _baseDataService.GetMaintenanceItems();
            if (items == null || items.Count == 0)
            {
                UIMessageBox.ShowWarning("没有可用的保养项目，请先在基础数据中添加");
                return;
            }

            using (var selectForm = new UIForm())
            {
                selectForm.Text = "选择保养项目";
                selectForm.Width = 500;
                selectForm.Height = 600;
                selectForm.StartPosition = FormStartPosition.CenterParent;
                selectForm.MaximizeBox = false;
                selectForm.MinimizeBox = false;

                var checkedListBox = new CheckedListBox
                {
                    Location = new System.Drawing.Point(20, 50),
                    Size = new System.Drawing.Size(440, 450),
                    Font = new System.Drawing.Font("微软雅黑", 11F),
                    CheckOnClick = true
                };

                foreach (var item in items)
                {
                    var isChecked = _selectedItems.Contains(item);
                    checkedListBox.Items.Add(item, isChecked);
                }

                var btnOk = new UIButton
                {
                    Text = "确定",
                    Location = new System.Drawing.Point(260, 520),
                    Size = new System.Drawing.Size(90, 35),
                    Font = new System.Drawing.Font("微软雅黑", 11F)
                };

                var btnCancelSelect = new UIButton
                {
                    Text = "取消",
                    Location = new System.Drawing.Point(370, 520),
                    Size = new System.Drawing.Size(90, 35),
                    Font = new System.Drawing.Font("微软雅黑", 11F)
                };

                btnOk.Click += (s, args) =>
                {
                    _selectedItems.Clear();
                    foreach (var checkedItem in checkedListBox.CheckedItems)
                    {
                        _selectedItems.Add(checkedItem.ToString());
                    }
                    txtMaintenanceItems.Text = string.Join(", ", _selectedItems);
                    selectForm.DialogResult = DialogResult.OK;
                    selectForm.Close();
                };

                btnCancelSelect.Click += (s, args) =>
                {
                    selectForm.DialogResult = DialogResult.Cancel;
                    selectForm.Close();
                };

                selectForm.Controls.Add(checkedListBox);
                selectForm.Controls.Add(btnOk);
                selectForm.Controls.Add(btnCancelSelect);

                selectForm.ShowDialog();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var operatorName = txtOperator.Text.Trim();
            var items = txtMaintenanceItems.Text.Trim();
            var result = cmbResult.Text;
            var notes = txtNotes.Text.Trim();

            if (string.IsNullOrEmpty(operatorName))
            {
                UIMessageBox.ShowWarning("请输入操作员");
                return;
            }

            if (string.IsNullOrEmpty(items))
            {
                UIMessageBox.ShowWarning("请选择保养项目");
                return;
            }

            bool success;
            if (_targetType == "Equipment")
            {
                success = _maintenanceService.PerformEquipmentMaintenance(_targetId, operatorName, items, result, notes);
            }
            else
            {
                if (!int.TryParse(txtNextIntervalDays.Text, out int intervalDays) || intervalDays <= 0)
                {
                    UIMessageBox.ShowWarning("请输入有效的下次保养间隔天数");
                    return;
                }
                success = _maintenanceService.PerformToolMaintenance(_targetId, operatorName, items, result, notes, intervalDays);
            }

            if (success)
            {
                UIMessageBox.ShowSuccess("保养记录已保存");
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                UIMessageBox.ShowError("保养记录保存失败");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
