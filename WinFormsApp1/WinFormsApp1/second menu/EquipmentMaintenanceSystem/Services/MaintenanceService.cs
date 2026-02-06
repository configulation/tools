using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsApp1.EquipmentMaintenanceSystem.Models;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Services
{
    /// <summary>
    /// 保养业务逻辑服务
    /// </summary>
    public class MaintenanceService
    {
        private readonly DataService _dataService;

        public MaintenanceService(DataService dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// 获取数据服务实例
        /// </summary>
        public DataService GetDataService()
        {
            return _dataService;
        }

        /// <summary>
        /// 计算下次保养日期
        /// </summary>
        public DateTime CalculateNextMaintenanceDate(DateTime lastMaintenanceDate, int intervalDays)
        {
            return lastMaintenanceDate.AddDays(intervalDays);
        }

        /// <summary>
        /// 执行设备保养
        /// </summary>
        public bool PerformEquipmentMaintenance(string equipmentId, string operatorName, string items, string result, string notes)
        {
            var equipment = _dataService.GetEquipment(equipmentId);
            if (equipment == null)
                return false;

            var record = new MaintenanceRecord
            {
                TargetId = equipmentId,
                TargetType = "Equipment",
                MaintenanceTime = DateTime.Now,
                Operator = operatorName,
                MaintenanceItems = items,
                Result = result,
                Notes = notes,
                NextMaintenanceDate = CalculateNextMaintenanceDate(DateTime.Now, equipment.MaintenanceIntervalDays)
            };

            equipment.MaintenanceHistory.Add(record);
            equipment.NextMaintenanceDate = record.NextMaintenanceDate;
            equipment.Status = "正常使用";
            equipment.UpdateTime = DateTime.Now;

            _dataService.UpdateEquipment(equipment);
            _dataService.AddRecord(record);

            return true;
        }

        /// <summary>
        /// 执行工装保养
        /// </summary>
        public bool PerformToolMaintenance(string toolCode, string operatorName, string items, string result, string notes, int nextIntervalDays)
        {
            var tool = _dataService.GetTool(toolCode);
            if (tool == null)
                return false;

            var record = new MaintenanceRecord
            {
                TargetId = toolCode,
                TargetType = "Tool",
                MaintenanceTime = DateTime.Now,
                Operator = operatorName,
                MaintenanceItems = items,
                Result = result,
                Notes = notes,
                NextMaintenanceDate = CalculateNextMaintenanceDate(DateTime.Now, nextIntervalDays)
            };

            tool.MaintenanceHistory.Add(record);
            tool.NextMaintenanceDate = record.NextMaintenanceDate;
            tool.UsageCount = 0;
            tool.Status = "正常使用";
            tool.UpdateTime = DateTime.Now;

            _dataService.UpdateTool(tool);
            _dataService.AddRecord(record);

            return true;
        }

        /// <summary>
        /// 获取即将到期的设备列表（7天内）
        /// </summary>
        public List<Equipment> GetUpcomingEquipments(int daysAhead = 7)
        {
            var equipments = _dataService.LoadEquipments();
            var targetDate = DateTime.Now.AddDays(daysAhead);
            return equipments.Where(e => e.NextMaintenanceDate <= targetDate && e.NextMaintenanceDate >= DateTime.Now)
                           .OrderBy(e => e.NextMaintenanceDate)
                           .ToList();
        }

        /// <summary>
        /// 获取已过期的设备列表
        /// </summary>
        public List<Equipment> GetOverdueEquipments()
        {
            var equipments = _dataService.LoadEquipments();
            return equipments.Where(e => e.NextMaintenanceDate < DateTime.Now)
                           .OrderBy(e => e.NextMaintenanceDate)
                           .ToList();
        }

        /// <summary>
        /// 获取即将到期的工装列表（7天内）
        /// </summary>
        public List<Tool> GetUpcomingTools(int daysAhead = 7)
        {
            var tools = _dataService.LoadTools();
            var targetDate = DateTime.Now.AddDays(daysAhead);
            return tools.Where(t => t.NextMaintenanceDate <= targetDate && t.NextMaintenanceDate >= DateTime.Now)
                       .OrderBy(t => t.NextMaintenanceDate)
                       .ToList();
        }

        /// <summary>
        /// 获取已过期的工装列表
        /// </summary>
        public List<Tool> GetOverdueTools()
        {
            var tools = _dataService.LoadTools();
            return tools.Where(t => t.NextMaintenanceDate < DateTime.Now)
                       .OrderBy(t => t.NextMaintenanceDate)
                       .ToList();
        }

        /// <summary>
        /// 检查工装使用次数是否需要保养
        /// </summary>
        public bool CheckToolUsageLimit(Tool tool)
        {
            return tool.UsageCount >= tool.TotalUsage;
        }

        /// <summary>
        /// 获取保养记录列表
        /// </summary>
        public List<MaintenanceRecord> GetMaintenanceRecords(string targetType = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var records = _dataService.LoadRecords();

            if (!string.IsNullOrEmpty(targetType))
            {
                records = records.Where(r => r.TargetType == targetType).ToList();
            }

            if (startDate.HasValue)
            {
                records = records.Where(r => r.MaintenanceTime >= startDate.Value).ToList();
            }

            if (endDate.HasValue)
            {
                records = records.Where(r => r.MaintenanceTime <= endDate.Value).ToList();
            }

            return records.OrderByDescending(r => r.MaintenanceTime).ToList();
        }
    }
}
