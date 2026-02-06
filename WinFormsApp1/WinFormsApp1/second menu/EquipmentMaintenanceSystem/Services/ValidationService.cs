using System;
using System.Text.RegularExpressions;
using WinFormsApp1.EquipmentMaintenanceSystem.Models;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Services
{
    /// <summary>
    /// 数据验证服务
    /// </summary>
    public class ValidationService
    {
        /// <summary>
        /// 验证设备数据
        /// </summary>
        public (bool isValid, string message) ValidateEquipment(Equipment equipment)
        {
            if (string.IsNullOrWhiteSpace(equipment.EquipmentId))
                return (false, "设备ID不能为空");

            if (string.IsNullOrWhiteSpace(equipment.LineLocation))
                return (false, "线别储位不能为空");

            if (string.IsNullOrWhiteSpace(equipment.Category))
                return (false, "类别不能为空");

            if (equipment.MaintenanceIntervalDays <= 0)
                return (false, "保养周期必须大于0");

            if (string.IsNullOrWhiteSpace(equipment.Status))
                return (false, "状态不能为空");

            return (true, "验证通过");
        }

        /// <summary>
        /// 验证工装数据
        /// </summary>
        public (bool isValid, string message) ValidateTool(Tool tool)
        {
            if (string.IsNullOrWhiteSpace(tool.ToolCode))
                return (false, "工装编码不能为空");

            if (string.IsNullOrWhiteSpace(tool.LineLocation))
                return (false, "线别储位不能为空");

            if (string.IsNullOrWhiteSpace(tool.Category))
                return (false, "类别不能为空");

            if (tool.OrderQuantity < 0)
                return (false, "工单数量不能为负数");

            if (tool.PanelQuantity <= 0)
                return (false, "拼料数量必须大于0");

            if (tool.ScraperCount <= 0)
                return (false, "刮刀数量必须大于0");

            if (tool.TotalUsage <= 0)
                return (false, "总使用次数必须大于0");

            if (string.IsNullOrWhiteSpace(tool.Status))
                return (false, "状态不能为空");

            return (true, "验证通过");
        }

        /// <summary>
        /// 验证设备ID格式
        /// </summary>
        public bool ValidateEquipmentIdFormat(string equipmentId)
        {
            if (string.IsNullOrWhiteSpace(equipmentId))
                return false;

            var pattern = @"^[A-Z0-9\-]+$";
            return Regex.IsMatch(equipmentId, pattern);
        }

        /// <summary>
        /// 验证工装编码格式
        /// </summary>
        public bool ValidateToolCodeFormat(string toolCode)
        {
            if (string.IsNullOrWhiteSpace(toolCode))
                return false;

            var pattern = @"^[A-Z0-9\-]+$";
            return Regex.IsMatch(toolCode, pattern);
        }
    }
}
