using SqlSugar;

namespace WinFormsApp1.Common.Entities.EquipmentMaintenance
{
    /// <summary>
    /// 保养项目关联表实体
    /// </summary>
    [SugarTable("maintenance_item")]
    public class MaintenanceItemEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(Length = 50)]
        public string TargetId { get; set; }

        [SugarColumn(Length = 20)]
        public string TargetType { get; set; }

        [SugarColumn(Length = 200)]
        public string ItemName { get; set; }
    }
}
