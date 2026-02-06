using System;
using System.Collections.Generic;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Models
{
    /// <summary>
    /// 设备数据模型
    /// </summary>
    public class Equipment
    {
        /// <summary>
        /// 设备ID（唯一标识）
        /// </summary>
        public string EquipmentId { get; set; }

        /// <summary>
        /// 线别储位
        /// </summary>
        public string LineLocation { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 子类别
        /// </summary>
        public string SubCategory { get; set; }

        /// <summary>
        /// 保养周期（天）
        /// </summary>
        public int MaintenanceIntervalDays { get; set; }

        /// <summary>
        /// 下次保养日期
        /// </summary>
        public DateTime NextMaintenanceDate { get; set; }

        /// <summary>
        /// 保养历史记录
        /// </summary>
        public List<MaintenanceRecord> MaintenanceHistory { get; set; } = new List<MaintenanceRecord>();

        /// <summary>
        /// 状态（新购、正常使用、保养中、维修中）
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 操作员ID
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 保养项目
        /// </summary>
        public List<string> MaintenanceItems { get; set; } = new List<string>();

        /// <summary>
        /// 备注
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}
