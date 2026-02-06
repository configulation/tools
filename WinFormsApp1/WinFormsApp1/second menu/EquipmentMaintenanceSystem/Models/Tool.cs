using System;
using System.Collections.Generic;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Models
{
    /// <summary>
    /// 工装数据模型
    /// </summary>
    public class Tool
    {
        /// <summary>
        /// 工装编码（唯一标识）
        /// </summary>
        public string ToolCode { get; set; }

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
        /// 工单号
        /// </summary>
        public string WorkOrder { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        public int OrderQuantity { get; set; }

        /// <summary>
        /// 拼料数量
        /// </summary>
        public int PanelQuantity { get; set; }

        /// <summary>
        /// 刮刀数量
        /// </summary>
        public int ScraperCount { get; set; }

        /// <summary>
        /// 当前使用次数
        /// </summary>
        public int UsageCount { get; set; }

        /// <summary>
        /// 总使用次数限制
        /// </summary>
        public int TotalUsage { get; set; }

        /// <summary>
        /// 保养间隔（描述，如"200000次"）
        /// </summary>
        public string MaintenanceInterval { get; set; }

        /// <summary>
        /// 下次保养日期
        /// </summary>
        public DateTime NextMaintenanceDate { get; set; }

        /// <summary>
        /// 发放时间
        /// </summary>
        public DateTime? IssueTime { get; set; }

        /// <summary>
        /// 退回时间
        /// </summary>
        public DateTime? ReturnTime { get; set; }

        /// <summary>
        /// 状态（新购、正常使用、保养中、维修中）
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 保养历史记录
        /// </summary>
        public List<MaintenanceRecord> MaintenanceHistory { get; set; } = new List<MaintenanceRecord>();

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

        /// <summary>
        /// 计算使用次数：工单数量 / 拼料数量 / 刮刀数量
        /// </summary>
        public int CalculateUsageCount()
        {
            if (PanelQuantity <= 0 || ScraperCount <= 0)
                return 0;
            return OrderQuantity / PanelQuantity / ScraperCount;
        }
    }
}
