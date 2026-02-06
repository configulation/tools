using System;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Models
{
    /// <summary>
    /// 线位储位基础数据
    /// </summary>
    public class LineLocation
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    /// 类别基础数据
    /// </summary>
    public class Category
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // 设备/工装
        public bool IsActive { get; set; } = true;
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    /// 子类别基础数据
    /// </summary>
    public class SubCategory
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    /// 保养项目基础数据
    /// </summary>
    public class MaintenanceItem
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // 设备/工装
        public string CategoryId { get; set; }
        public string Description { get; set; }
        public int StandardDuration { get; set; } // 标准耗时(分钟)
        public bool IsActive { get; set; } = true;
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    /// 操作员基础数据
    /// </summary>
    public class Operator
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
