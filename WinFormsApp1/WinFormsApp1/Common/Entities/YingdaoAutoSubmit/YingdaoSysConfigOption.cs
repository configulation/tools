using SqlSugar;
using System;

namespace WinFormsApp1.Common.Entities.YingdaoAutoSubmit
{
    [SugarTable("sys_config_option")]
    public class YingdaoSysConfigOption
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "config_key", Length = 100)]
        public string ConfigKey { get; set; }

        [SugarColumn(ColumnName = "option_value", Length = 200)]
        public string OptionValue { get; set; }

        [SugarColumn(ColumnName = "option_label", Length = 200)]
        public string OptionLabel { get; set; }

        [SugarColumn(ColumnName = "sort_order")]
        public int SortOrder { get; set; }

        [SugarColumn(ColumnName = "is_default")]
        public bool IsDefault { get; set; }

        [SugarColumn(ColumnName = "is_active")]
        public bool IsActive { get; set; }

        [SugarColumn(ColumnName = "extra_data", Length = 500)]
        public string ExtraData { get; set; }

        [SugarColumn(ColumnName = "created_time")]
        public DateTime CreatedTime { get; set; }
    }
}
