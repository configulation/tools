using Sunny.UI;
using System;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp1.EquipmentMaintenanceSystem.Services;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    public partial class FrmMaintenanceRecord : UIForm
    {
        private readonly DataService _dataService;

        public FrmMaintenanceRecord()
        {
            InitializeComponent();
            _dataService = new DataService();
        }

        private void FrmMaintenanceRecord_Load(object sender, EventArgs e)
        {
            InitializeFilters();
            LoadRecords();
        }

        private void InitializeFilters()
        {
            cmbType.Items.Clear();
            cmbType.Items.Add("全部");
            cmbType.Items.Add("设备");
            cmbType.Items.Add("工装");
            cmbType.SelectedIndex = 0;
            
            dtpStartDate.Value = DateTime.Now.AddMonths(-1);
            dtpEndDate.Value = DateTime.Now;
        }

        private void LoadRecords()
        {
            var records = _dataService.LoadRecords();
            
            var startDate = dtpStartDate.Value.Date;
            var endDate = dtpEndDate.Value.Date.AddDays(1).AddSeconds(-1);
            var typeFilter = cmbType.SelectedItem?.ToString();
            var keyword = txtKeyword.Text.Trim();
            
            var filteredRecords = records.Where(r => 
                r.MaintenanceTime >= startDate && 
                r.MaintenanceTime <= endDate
            );
            
            if (typeFilter != "全部")
            {
                filteredRecords = filteredRecords.Where(r => r.TargetType == typeFilter);
            }
            
            if (!string.IsNullOrEmpty(keyword))
            {
                filteredRecords = filteredRecords.Where(r => 
                    r.TargetId.Contains(keyword) ||
                    r.Operator.Contains(keyword) ||
                    r.MaintenanceItems.Contains(keyword)
                );
            }
            
            dgvRecords.Rows.Clear();
            
            foreach (var record in filteredRecords.OrderByDescending(r => r.MaintenanceTime))
            {
                var index = dgvRecords.Rows.Add(
                    record.RecordId,
                    record.TargetType,
                    record.TargetId,
                    record.MaintenanceTime.ToString("yyyy-MM-dd HH:mm"),
                    record.MaintenanceItems,
                    record.Operator,
                    record.Result ?? "正常",
                    record.Notes
                );
                
                // 根据保养结果设置行颜色
                if (record.Result == "异常")
                {
                    dgvRecords.Rows[index].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 205, 210);
                    dgvRecords.Rows[index].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
                }
                else
                {
                    dgvRecords.Rows[index].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    dgvRecords.Rows[index].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                }
            }
            
            lblTotal.Text = "共 " + dgvRecords.Rows.Count + " 条记录";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            InitializeFilters();
            txtKeyword.Text = "";
            LoadRecords();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvRecords.Rows.Count == 0)
            {
                UIMessageBox.ShowWarning("没有数据可导出");
                return;
            }
            
            UIMessageBox.ShowInfo("导出功能开发中...");
        }

        private void btnViewDetail_Click(object sender, EventArgs e)
        {
            if (dgvRecords.SelectedRows.Count == 0)
            {
                UIMessageBox.ShowWarning("请选择一条记录");
                return;
            }
            
            var recordId = dgvRecords.SelectedRows[0].Cells["colRecordId"].Value?.ToString();
            if (string.IsNullOrEmpty(recordId)) return;
            
            var record = _dataService.LoadRecords().FirstOrDefault(r => r.RecordId == recordId);
            if (record == null)
            {
                UIMessageBox.ShowError("记录不存在");
                return;
            }
            
            var message = "保养记录详情\n\n";
            message += "记录ID：" + record.RecordId + "\n";
            message += "类型：" + record.TargetType + "\n";
            message += "编号：" + record.TargetId + "\n";
            message += "保养时间：" + record.MaintenanceTime.ToString("yyyy-MM-dd HH:mm:ss") + "\n";
            message += "保养项目：\n" + record.MaintenanceItems + "\n";
            message += "操作人员：" + record.Operator + "\n";
            message += "备注：" + (string.IsNullOrEmpty(record.Notes) ? "无" : record.Notes);
            
            UIMessageBox.Show(message, "保养记录详情", UIStyle.Blue, UIMessageBoxButtons.OK);
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            var records = _dataService.LoadRecords();
            var startDate = dtpStartDate.Value.Date;
            var endDate = dtpEndDate.Value.Date.AddDays(1).AddSeconds(-1);
            
            var filteredRecords = records.Where(r => 
                r.MaintenanceTime >= startDate && 
                r.MaintenanceTime <= endDate
            ).ToList();
            
            var equipmentCount = filteredRecords.Count(r => r.TargetType == "设备");
            var toolCount = filteredRecords.Count(r => r.TargetType == "工装");
            var totalCount = filteredRecords.Count;
            
            var operators = filteredRecords.GroupBy(r => r.Operator)
                .Select(g => g.Key + "：" + g.Count() + "次")
                .ToList();
            
            var message = "保养统计分析\n\n";
            message += "时间范围：" + startDate.ToString("yyyy-MM-dd") + " 至 " + endDate.ToString("yyyy-MM-dd") + "\n\n";
            message += "总保养次数：" + totalCount + " 次\n";
            message += "设备保养：" + equipmentCount + " 次\n";
            message += "工装保养：" + toolCount + " 次\n\n";
            message += "操作人员统计：\n";
            message += string.Join("\n", operators);
            
            UIMessageBox.Show(message, "统计分析", UIStyle.Blue, UIMessageBoxButtons.OK);
        }

        private void dgvRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            btnViewDetail_Click(sender, e);
        }
    }
}
