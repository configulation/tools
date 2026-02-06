using SqlSugar;
using System;

namespace WinFormsApp1.Common.Entities.EquipmentMaintenance
{
    /// <summary>
    /// 工装表实体
    /// </summary>
    [SugarTable("tool")]
    public class ToolEntity
    {
        [SugarColumn(IsPrimaryKey = true, Length = 50)]
        public string ToolCode { get; set; }

        [SugarColumn(Length = 100)]
        public string LineLocation { get; set; }

        [SugarColumn(Length = 50)]
        public string Category { get; set; }

        [SugarColumn(Length = 50)]
        public string SubCategory { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string WorkOrder { get; set; }

        public int OrderQuantity { get; set; }

        public int PanelQuantity { get; set; }

        public int ScraperCount { get; set; }

        public int UsageCount { get; set; }

        public int TotalUsage { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string MaintenanceInterval { get; set; }

        public DateTime NextMaintenanceDate { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? IssueTime { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? ReturnTime { get; set; }

        [SugarColumn(Length = 50)]
        public string Status { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string Notes { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
