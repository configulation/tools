using SqlSugar;
using System;

namespace WinFormsApp1.Common.Entities.EquipmentMaintenance
{
    /// <summary>
    /// 设备表实体
    /// </summary>
    [SugarTable("equipment")]
    public class EquipmentEntity
    {
        [SugarColumn(IsPrimaryKey = true, Length = 50)]
        public string EquipmentId { get; set; }

        [SugarColumn(Length = 100)]
        public string LineLocation { get; set; }

        [SugarColumn(Length = 50)]
        public string Category { get; set; }

        [SugarColumn(Length = 50)]
        public string SubCategory { get; set; }

        public int MaintenanceIntervalDays { get; set; }

        public DateTime NextMaintenanceDate { get; set; }

        [SugarColumn(Length = 50)]
        public string Status { get; set; }

        [SugarColumn(Length = 50)]
        public string OperatorId { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string Notes { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
