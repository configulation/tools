using SqlSugar;

namespace WinFormsApp1.Common.Database.Entities
{
    [SugarTable("sys_config")]
    public class YingdaoSysConfig
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "config_key", Length = 100)]
        public string ConfigKey { get; set; }

        [SugarColumn(ColumnName = "config_name", Length = 200)]
        public string ConfigName { get; set; }

        [SugarColumn(ColumnName = "config_value", ColumnDataType = "text")]
        public string ConfigValue { get; set; }

        [SugarColumn(ColumnName = "data_type", Length = 50)]
        public string DataType { get; set; }

        [SugarColumn(ColumnName = "category", Length = 50)]
        public string Category { get; set; }

        [SugarColumn(ColumnName = "sort_order")]
        public int SortOrder { get; set; }

        [SugarColumn(ColumnName = "is_editable")]
        public bool IsEditable { get; set; }

        [SugarColumn(ColumnName = "remark", Length = 500)]
        public string Remark { get; set; }

        [SugarColumn(ColumnName = "created_time")]
        public DateTime CreatedTime { get; set; }

        [SugarColumn(ColumnName = "updated_time")]
        public DateTime UpdatedTime { get; set; }
    }
}
