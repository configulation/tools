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
    public partial class FrmMaintenancePlan : UIForm
    {
        private readonly DataService _dataService;
        private readonly MaintenanceService _maintenanceService;
        private DateTime _currentMonth;

        public FrmMaintenancePlan()
        {
            InitializeComponent();
            _dataService = new DataService();
            _maintenanceService = new MaintenanceService(_dataService);
            _currentMonth = DateTime.Now;
        }

        private void FrmMaintenancePlan_Load(object sender, EventArgs e)
        {
            LoadMonthView();
            LoadPlanList();
        }

        private void btnPrevMonth_Click(object sender, EventArgs e)
        {
            _currentMonth = _currentMonth.AddMonths(-1);
            LoadMonthView();
        }

        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            _currentMonth = _currentMonth.AddMonths(1);
            LoadMonthView();
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            _currentMonth = DateTime.Now;
            LoadMonthView();
        }

        private void LoadMonthView()
        {
            lblMonth.Text = _currentMonth.ToString("yyyy年MM月");
            
            var equipments = _dataService.LoadEquipments();
            var tools = _dataService.LoadTools();
            
            dgvCalendar.Rows.Clear();
            
            var firstDay = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);
            
            for (var date = firstDay; date <= lastDay; date = date.AddDays(1))
            {
                var dayOfWeek = GetChineseDayOfWeek(date.DayOfWeek);
                var equipmentCount = equipments.Count(e => e.NextMaintenanceDate.Date == date.Date);
                var toolCount = tools.Count(t => t.NextMaintenanceDate.Date == date.Date);
                var totalCount = equipmentCount + toolCount;
                
                var status = totalCount == 0 ? "无计划" : totalCount + "项";
                
                var index = dgvCalendar.Rows.Add(
                    date.ToString("yyyy-MM-dd"),
                    dayOfWeek,
                    equipmentCount,
                    toolCount,
                    totalCount,
                    status
                );
                
                // 优先级：今天 > 有计划 > 无计划
                if (date.Date == DateTime.Now.Date)
                {
                    // 今天显示绿色
                    dgvCalendar.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
                }
                else if (totalCount > 0)
                {
                    // 有计划显示黄色
                    dgvCalendar.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 196);
                }
                else
                {
                    // 无计划显示白色
                    dgvCalendar.Rows[index].DefaultCellStyle.BackColor = Color.White;
                }
                
                // 周末文字显示红色
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    dgvCalendar.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    dgvCalendar.Rows[index].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void LoadPlanList()
        {
            var equipments = _dataService.LoadEquipments();
            var tools = _dataService.LoadTools();
            
            dgvPlanList.Rows.Clear();
            
            foreach (var equipment in equipments.OrderBy(e => e.NextMaintenanceDate))
            {
                var daysLeft = (equipment.NextMaintenanceDate.Date - DateTime.Now.Date).Days;
                var status = daysLeft < 0 ? "已过期" : daysLeft == 0 ? "今天" : daysLeft + "天后";
                
                var index = dgvPlanList.Rows.Add(
                    "设备",
                    equipment.EquipmentId,
                    equipment.LineLocation,
                    equipment.Category,
                    equipment.SubCategory,
                    equipment.NextMaintenanceDate.ToString("yyyy-MM-dd"),
                    status
                );
                
                if (daysLeft < 0)
                {
                    dgvPlanList.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 210);
                    dgvPlanList.Rows[index].DefaultCellStyle.ForeColor = Color.DarkRed;
                }
                else if (daysLeft <= 3)
                {
                    dgvPlanList.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 196);
                    dgvPlanList.Rows[index].DefaultCellStyle.ForeColor = Color.DarkOrange;
                }
                else
                {
                    dgvPlanList.Rows[index].DefaultCellStyle.BackColor = Color.White;
                    dgvPlanList.Rows[index].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
            
            foreach (var tool in tools.OrderBy(t => t.NextMaintenanceDate))
            {
                var daysLeft = (tool.NextMaintenanceDate.Date - DateTime.Now.Date).Days;
                var status = daysLeft < 0 ? "已过期" : daysLeft == 0 ? "今天" : daysLeft + "天后";
                
                var index = dgvPlanList.Rows.Add(
                    "工装",
                    tool.ToolCode,
                    tool.LineLocation,
                    tool.Category,
                    tool.SubCategory,
                    tool.NextMaintenanceDate.ToString("yyyy-MM-dd"),
                    status
                );
                
                if (daysLeft < 0)
                {
                    dgvPlanList.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 210);
                    dgvPlanList.Rows[index].DefaultCellStyle.ForeColor = Color.DarkRed;
                }
                else if (daysLeft <= 3)
                {
                    dgvPlanList.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 196);
                    dgvPlanList.Rows[index].DefaultCellStyle.ForeColor = Color.DarkOrange;
                }
                else
                {
                    dgvPlanList.Rows[index].DefaultCellStyle.BackColor = Color.White;
                    dgvPlanList.Rows[index].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void dgvCalendar_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            
            var dateStr = dgvCalendar.Rows[e.RowIndex].Cells["colDate"].Value?.ToString();
            if (string.IsNullOrEmpty(dateStr)) return;
            
            var date = DateTime.Parse(dateStr);
            ShowDayDetail(date);
        }

        private void ShowDayDetail(DateTime date)
        {
            var equipments = _dataService.LoadEquipments()
                .Where(e => e.NextMaintenanceDate.Date == date.Date)
                .ToList();
            
            var tools = _dataService.LoadTools()
                .Where(t => t.NextMaintenanceDate.Date == date.Date)
                .ToList();
            
            if (equipments.Count == 0 && tools.Count == 0)
            {
                UIMessageBox.ShowInfo("该日期无保养计划");
                return;
            }
            
            var message = date.ToString("yyyy年MM月dd日") + " 保养计划：\n\n";
            
            if (equipments.Count > 0)
            {
                message += "【设备】\n";
                foreach (var eq in equipments)
                {
                    message += "  " + eq.EquipmentId + " - " + eq.Category + " " + eq.SubCategory + "\n";
                }
                message += "\n";
            }
            
            if (tools.Count > 0)
            {
                message += "【工装】\n";
                foreach (var tool in tools)
                {
                    message += "  " + tool.ToolCode + " - " + tool.Category + " " + tool.SubCategory + "\n";
                }
            }
            
            UIMessageBox.Show(message, "保养计划详情", UIStyle.Blue, UIMessageBoxButtons.OK);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadMonthView();
            LoadPlanList();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            UIMessageBox.ShowInfo("导出功能开发中...");
        }

        private string GetChineseDayOfWeek(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => "星期一",
                DayOfWeek.Tuesday => "星期二",
                DayOfWeek.Wednesday => "星期三",
                DayOfWeek.Thursday => "星期四",
                DayOfWeek.Friday => "星期五",
                DayOfWeek.Saturday => "星期六",
                DayOfWeek.Sunday => "星期日",
                _ => ""
            };
        }
    }
}
