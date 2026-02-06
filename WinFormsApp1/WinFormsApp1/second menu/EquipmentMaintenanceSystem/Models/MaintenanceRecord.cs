using System;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Models
{
    /// <summary>
    /// 保养记录模型
    /// </summary>
    public class MaintenanceRecord
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public string RecordId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 设备ID或工装编码
        /// </summary>
        public string TargetId { get; set; }

        /// <summary>
        /// 目标类型（Equipment/Tool）
        /// </summary>
        public string TargetType { get; set; }

        /// <summary>
        /// 保养时间
        /// </summary>
        public DateTime MaintenanceTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 操作员
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 保养项目
        /// </summary>
        public string MaintenanceItems { get; set; }

        /// <summary>
        /// 保养结果（正常/异常）
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// 下次保养日期
        /// </summary>
        public DateTime NextMaintenanceDate { get; set; }
    }
}
