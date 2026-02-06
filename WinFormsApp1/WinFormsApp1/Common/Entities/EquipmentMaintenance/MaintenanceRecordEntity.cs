using SqlSugar;
using System;

namespace WinFormsApp1.Common.Entities.EquipmentMaintenance
{
    /// <summary>
    /// 保养记录表实体
    /// </summary>
    [SugarTable("maintenance_record")]
    public class MaintenanceRecordEntity
    {
        [SugarColumn(IsPrimaryKey = true, Length = 50)]
        public string RecordId { get; set; }

        [SugarColumn(Length = 50)]
        public string TargetId { get; set; }

        [SugarColumn(Length = 20)]
        public string TargetType { get; set; }

        public DateTime MaintenanceTime { get; set; }

        [SugarColumn(Length = 50)]
        public string Operator { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string MaintenanceItems { get; set; }

        [SugarColumn(Length = 50)]
        public string Result { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string Notes { get; set; }

        public DateTime NextMaintenanceDate { get; set; }
    }
}
