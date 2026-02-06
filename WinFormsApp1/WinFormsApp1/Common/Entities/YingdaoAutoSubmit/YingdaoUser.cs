using SqlSugar;
using System;

namespace WinFormsApp1.Common.Entities.YingdaoAutoSubmit
{
    [SugarTable("yingdao_users")]
    public class YingdaoUser
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "username", Length = 100)]
        public string Username { get; set; }

        [SugarColumn(ColumnName = "password", Length = 100)]
        public string Password { get; set; }

        [SugarColumn(ColumnName = "factory", Length = 50)]
        public string Factory { get; set; }

        [SugarColumn(ColumnName = "employee_id", Length = 50)]
        public string EmployeeId { get; set; }

        [SugarColumn(ColumnName = "phone", Length = 20)]
        public string Phone { get; set; }

        [SugarColumn(ColumnName = "car_no", Length = 20)]
        public string CarNo { get; set; }

        [SugarColumn(ColumnName = "last_submitted_date", Length = 20)]
        public string LastSubmittedDate { get; set; }

        [SugarColumn(ColumnName = "last_result", Length = 50)]
        public string LastResult { get; set; }

        [SugarColumn(ColumnName = "is_enabled")]
        public bool IsEnabled { get; set; }

        [SugarColumn(ColumnName = "remark", Length = 200)]
        public string Remark { get; set; }

        [SugarColumn(ColumnName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [SugarColumn(ColumnName = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
