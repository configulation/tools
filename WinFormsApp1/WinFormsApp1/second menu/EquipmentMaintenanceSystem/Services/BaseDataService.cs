using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using WinFormsApp1.EquipmentMaintenanceSystem.Models;
using WinFormsApp1.Common;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Services
{
    /// <summary>
    /// 基础数据服务
    /// </summary>
    public class BaseDataService
    {
        private readonly string _dataFolder;
        private readonly string _lineLocationFile;
        private readonly string _categoryFile;
        private readonly string _subCategoryFile;
        private readonly string _maintenanceItemFile;
        private readonly string _operatorFile;

        public BaseDataService()
        {
            _dataFolder = PathHelper.EquipmentMaintenanceBaseDataFolder;
            _lineLocationFile = Path.Combine(_dataFolder, "line_locations.json");
            _categoryFile = Path.Combine(_dataFolder, "categories.json");
            _subCategoryFile = Path.Combine(_dataFolder, "sub_categories.json");
            _maintenanceItemFile = Path.Combine(_dataFolder, "maintenance_items.json");
            _operatorFile = Path.Combine(_dataFolder, "operators.json");

            PathHelper.EnsureDirectoryExists(_dataFolder);
            if (!File.Exists(_lineLocationFile) || !File.Exists(_categoryFile))
            {
                InitializeDefaultData();
            }
        }

        #region 线位储位

        public List<LineLocation> LoadLineLocations()
        {
            if (!File.Exists(_lineLocationFile))
                return new List<LineLocation>();

            var json = File.ReadAllText(_lineLocationFile);
            return JsonConvert.DeserializeObject<List<LineLocation>>(json) ?? new List<LineLocation>();
        }

        public void SaveLineLocations(List<LineLocation> data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_lineLocationFile, json);
        }

        public bool AddLineLocation(LineLocation item)
        {
            var list = LoadLineLocations();
            if (list.Any(x => x.Code == item.Code))
                return false;

            item.Id = Guid.NewGuid().ToString();
            item.CreateTime = DateTime.Now;
            item.UpdateTime = DateTime.Now;
            list.Add(item);
            SaveLineLocations(list);
            return true;
        }

        public bool UpdateLineLocation(LineLocation item)
        {
            var list = LoadLineLocations();
            var index = list.FindIndex(x => x.Id == item.Id);
            if (index == -1)
                return false;

            item.UpdateTime = DateTime.Now;
            list[index] = item;
            SaveLineLocations(list);
            return true;
        }

        public bool DeleteLineLocation(string id)
        {
            var list = LoadLineLocations();
            var removed = list.RemoveAll(x => x.Id == id);
            if (removed > 0)
            {
                SaveLineLocations(list);
                return true;
            }
            return false;
        }

        public List<string> GetActiveLineLocationNames()
        {
            return LoadLineLocations()
                .Where(x => x.IsActive)
                .Select(x => x.Name)
                .OrderBy(x => x)
                .ToList();
        }

        #endregion

        #region 类别

        public List<Category> LoadCategories()
        {
            if (!File.Exists(_categoryFile))
                return new List<Category>();

            var json = File.ReadAllText(_categoryFile);
            return JsonConvert.DeserializeObject<List<Category>>(json) ?? new List<Category>();
        }

        public void SaveCategories(List<Category> data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_categoryFile, json);
        }

        public bool AddCategory(Category item)
        {
            var list = LoadCategories();
            if (list.Any(x => x.Code == item.Code))
                return false;

            item.Id = Guid.NewGuid().ToString();
            item.CreateTime = DateTime.Now;
            item.UpdateTime = DateTime.Now;
            list.Add(item);
            SaveCategories(list);
            return true;
        }

        public bool UpdateCategory(Category item)
        {
            var list = LoadCategories();
            var index = list.FindIndex(x => x.Id == item.Id);
            if (index == -1)
                return false;

            item.UpdateTime = DateTime.Now;
            list[index] = item;
            SaveCategories(list);
            return true;
        }

        public bool DeleteCategory(string id)
        {
            var list = LoadCategories();
            var removed = list.RemoveAll(x => x.Id == id);
            if (removed > 0)
            {
                SaveCategories(list);
                return true;
            }
            return false;
        }

        public List<string> GetActiveCategoryNames(string type = null)
        {
            var query = LoadCategories().Where(x => x.IsActive);
            if (!string.IsNullOrEmpty(type))
                query = query.Where(x => x.Type == type);

            return query.Select(x => x.Name)
                .OrderBy(x => x)
                .ToList();
        }

        #endregion

        #region 子类别

        public List<SubCategory> LoadSubCategories()
        {
            if (!File.Exists(_subCategoryFile))
                return new List<SubCategory>();

            var json = File.ReadAllText(_subCategoryFile);
            return JsonConvert.DeserializeObject<List<SubCategory>>(json) ?? new List<SubCategory>();
        }

        public void SaveSubCategories(List<SubCategory> data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_subCategoryFile, json);
        }

        public bool AddSubCategory(SubCategory item)
        {
            var list = LoadSubCategories();
            if (list.Any(x => x.Code == item.Code))
                return false;

            item.Id = Guid.NewGuid().ToString();
            item.CreateTime = DateTime.Now;
            item.UpdateTime = DateTime.Now;
            list.Add(item);
            SaveSubCategories(list);
            return true;
        }

        public bool UpdateSubCategory(SubCategory item)
        {
            var list = LoadSubCategories();
            var index = list.FindIndex(x => x.Id == item.Id);
            if (index == -1)
                return false;

            item.UpdateTime = DateTime.Now;
            list[index] = item;
            SaveSubCategories(list);
            return true;
        }

        public bool DeleteSubCategory(string id)
        {
            var list = LoadSubCategories();
            var removed = list.RemoveAll(x => x.Id == id);
            if (removed > 0)
            {
                SaveSubCategories(list);
                return true;
            }
            return false;
        }

        public List<string> GetActiveSubCategoryNames(string categoryName = null)
        {
            var subCategories = LoadSubCategories().Where(x => x.IsActive);
            
            if (!string.IsNullOrEmpty(categoryName))
            {
                var category = LoadCategories().FirstOrDefault(x => x.Name == categoryName);
                if (category != null)
                {
                    subCategories = subCategories.Where(x => x.CategoryId == category.Id);
                }
            }

            return subCategories.Select(x => x.Name)
                .OrderBy(x => x)
                .ToList();
        }

        #endregion

        #region 保养项目

        public List<MaintenanceItem> LoadMaintenanceItems()
        {
            if (!File.Exists(_maintenanceItemFile))
                return new List<MaintenanceItem>();

            var json = File.ReadAllText(_maintenanceItemFile);
            return JsonConvert.DeserializeObject<List<MaintenanceItem>>(json) ?? new List<MaintenanceItem>();
        }

        public void SaveMaintenanceItems(List<MaintenanceItem> data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_maintenanceItemFile, json);
        }

        public bool AddMaintenanceItem(MaintenanceItem item)
        {
            var list = LoadMaintenanceItems();
            if (list.Any(x => x.Code == item.Code))
                return false;

            item.Id = Guid.NewGuid().ToString();
            item.CreateTime = DateTime.Now;
            item.UpdateTime = DateTime.Now;
            list.Add(item);
            SaveMaintenanceItems(list);
            return true;
        }

        public bool UpdateMaintenanceItem(MaintenanceItem item)
        {
            var list = LoadMaintenanceItems();
            var index = list.FindIndex(x => x.Id == item.Id);
            if (index == -1)
                return false;

            item.UpdateTime = DateTime.Now;
            list[index] = item;
            SaveMaintenanceItems(list);
            return true;
        }

        public bool DeleteMaintenanceItem(string id)
        {
            var list = LoadMaintenanceItems();
            var removed = list.RemoveAll(x => x.Id == id);
            if (removed > 0)
            {
                SaveMaintenanceItems(list);
                return true;
            }
            return false;
        }

        public List<MaintenanceItem> GetActiveMaintenanceItems(string type = null, string categoryName = null)
        {
            var query = LoadMaintenanceItems().Where(x => x.IsActive);
            
            if (!string.IsNullOrEmpty(type))
                query = query.Where(x => x.Type == type);

            if (!string.IsNullOrEmpty(categoryName))
            {
                var category = LoadCategories().FirstOrDefault(x => x.Name == categoryName);
                if (category != null)
                {
                    query = query.Where(x => x.CategoryId == category.Id);
                }
            }

            return query.OrderBy(x => x.Name).ToList();
        }

        public List<string> GetMaintenanceItems(string type = null)
        {
            var query = LoadMaintenanceItems().Where(x => x.IsActive);
            
            if (!string.IsNullOrEmpty(type))
                query = query.Where(x => x.Type == type);

            return query.Select(x => x.Name).OrderBy(x => x).ToList();
        }

        public List<string> GetActiveMaintenanceItemNames(string type = null)
        {
            return GetMaintenanceItems(type);
        }

        #endregion

        #region 操作员

        public List<Operator> LoadOperators()
        {
            if (!File.Exists(_operatorFile))
                return new List<Operator>();

            var json = File.ReadAllText(_operatorFile);
            return JsonConvert.DeserializeObject<List<Operator>>(json) ?? new List<Operator>();
        }

        public void SaveOperators(List<Operator> data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_operatorFile, json);
        }

        public bool AddOperator(Operator item)
        {
            var list = LoadOperators();
            if (list.Any(x => x.Code == item.Code))
                return false;

            item.Id = Guid.NewGuid().ToString();
            item.CreateTime = DateTime.Now;
            item.UpdateTime = DateTime.Now;
            list.Add(item);
            SaveOperators(list);
            return true;
        }

        public bool UpdateOperator(Operator item)
        {
            var list = LoadOperators();
            var index = list.FindIndex(x => x.Id == item.Id);
            if (index == -1)
                return false;

            item.UpdateTime = DateTime.Now;
            list[index] = item;
            SaveOperators(list);
            return true;
        }

        public bool DeleteOperator(string id)
        {
            var list = LoadOperators();
            var removed = list.RemoveAll(x => x.Id == id);
            if (removed > 0)
            {
                SaveOperators(list);
                return true;
            }
            return false;
        }

        public List<string> GetActiveOperatorNames()
        {
            return LoadOperators()
                .Where(x => x.IsActive)
                .Select(x => $"{x.Code} - {x.Name}")
                .OrderBy(x => x)
                .ToList();
        }

        #endregion

        #region 初始化默认数据

        private void InitializeDefaultData()
        {
            var lineLocations = new List<LineLocation>();
            for (int i = 1; i <= 15; i++)
            {
                var line = (char)('A' + (i - 1) / 5);
                var pos = (i - 1) % 5 + 1;
                lineLocations.Add(new LineLocation
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = $"{line}{pos:D2}",
                    Name = $"{line}线-{pos:D2}储位",
                    IsActive = true,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                });
            }
            SaveLineLocations(lineLocations);

            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid().ToString(), Code = "EQ01", Name = "生产设备", Type = "设备", IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new Category { Id = Guid.NewGuid().ToString(), Code = "EQ02", Name = "检测设备", Type = "设备", IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new Category { Id = Guid.NewGuid().ToString(), Code = "EQ03", Name = "辅助设备", Type = "设备", IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new Category { Id = Guid.NewGuid().ToString(), Code = "EQ04", Name = "包装设备", Type = "设备", IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new Category { Id = Guid.NewGuid().ToString(), Code = "TL01", Name = "钢网", Type = "工装", IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new Category { Id = Guid.NewGuid().ToString(), Code = "TL02", Name = "治具", Type = "工装", IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new Category { Id = Guid.NewGuid().ToString(), Code = "TL03", Name = "夹具", Type = "工装", IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new Category { Id = Guid.NewGuid().ToString(), Code = "TL04", Name = "模具", Type = "工装", IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now }
            };
            SaveCategories(categories);

            var subCategories = new List<SubCategory>
            {
                new SubCategory { Id = Guid.NewGuid().ToString(), Code = "SUB01", Name = "印刷机", CategoryId = categories[0].Id, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new SubCategory { Id = Guid.NewGuid().ToString(), Code = "SUB02", Name = "贴片机", CategoryId = categories[0].Id, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new SubCategory { Id = Guid.NewGuid().ToString(), Code = "SUB03", Name = "回流焊", CategoryId = categories[0].Id, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new SubCategory { Id = Guid.NewGuid().ToString(), Code = "SUB04", Name = "AOI检测", CategoryId = categories[1].Id, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new SubCategory { Id = Guid.NewGuid().ToString(), Code = "SUB05", Name = "X-RAY检测", CategoryId = categories[1].Id, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new SubCategory { Id = Guid.NewGuid().ToString(), Code = "SUB06", Name = "空压机", CategoryId = categories[2].Id, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new SubCategory { Id = Guid.NewGuid().ToString(), Code = "SUB07", Name = "除湿机", CategoryId = categories[2].Id, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new SubCategory { Id = Guid.NewGuid().ToString(), Code = "SUB08", Name = "标准钢网", CategoryId = categories[4].Id, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new SubCategory { Id = Guid.NewGuid().ToString(), Code = "SUB09", Name = "精密钢网", CategoryId = categories[4].Id, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new SubCategory { Id = Guid.NewGuid().ToString(), Code = "SUB10", Name = "测试治具", CategoryId = categories[5].Id, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now }
            };
            SaveSubCategories(subCategories);

            var operators = new List<Operator>();
            string[] names = { "张三", "李四", "王五", "赵六", "钱七", "孙八", "周九", "吴十", "郑十一", "陈十二", "刘十三", "杨十四", "黄十五" };
            string[] departments = { "生产部", "品质部", "工程部", "设备部" };
            for (int i = 0; i < names.Length; i++)
            {
                operators.Add(new Operator
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = $"OP{(i + 1):D3}",
                    Name = names[i],
                    Department = departments[i % departments.Length],
                    IsActive = true,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                });
            }
            SaveOperators(operators);

            var maintenanceItems = new List<MaintenanceItem>
            {
                new MaintenanceItem { Id = Guid.NewGuid().ToString(), Code = "MT001", Name = "清洁保养", Type = "设备", Description = "清洁设备表面及内部", StandardDuration = 30, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new MaintenanceItem { Id = Guid.NewGuid().ToString(), Code = "MT002", Name = "润滑保养", Type = "设备", Description = "添加润滑油", StandardDuration = 15, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new MaintenanceItem { Id = Guid.NewGuid().ToString(), Code = "MT003", Name = "精度校准", Type = "设备", Description = "校准设备精度", StandardDuration = 60, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new MaintenanceItem { Id = Guid.NewGuid().ToString(), Code = "MT004", Name = "电气检查", Type = "设备", Description = "检查电气连接", StandardDuration = 45, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new MaintenanceItem { Id = Guid.NewGuid().ToString(), Code = "MT005", Name = "气路检查", Type = "设备", Description = "检查气路系统", StandardDuration = 30, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new MaintenanceItem { Id = Guid.NewGuid().ToString(), Code = "MT006", Name = "传动系统检查", Type = "设备", Description = "检查传动部件", StandardDuration = 40, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new MaintenanceItem { Id = Guid.NewGuid().ToString(), Code = "MT007", Name = "安全装置检查", Type = "设备", Description = "检查安全装置", StandardDuration = 20, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new MaintenanceItem { Id = Guid.NewGuid().ToString(), Code = "MT008", Name = "钢网清洗", Type = "工装", Description = "清洗钢网", StandardDuration = 20, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new MaintenanceItem { Id = Guid.NewGuid().ToString(), Code = "MT009", Name = "钢网检查", Type = "工装", Description = "检查钢网开孔", StandardDuration = 15, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new MaintenanceItem { Id = Guid.NewGuid().ToString(), Code = "MT010", Name = "治具清洁", Type = "工装", Description = "清洁治具", StandardDuration = 25, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new MaintenanceItem { Id = Guid.NewGuid().ToString(), Code = "MT011", Name = "治具校准", Type = "工装", Description = "校准治具精度", StandardDuration = 35, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new MaintenanceItem { Id = Guid.NewGuid().ToString(), Code = "MT012", Name = "夹具检查", Type = "工装", Description = "检查夹具状态", StandardDuration = 20, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new MaintenanceItem { Id = Guid.NewGuid().ToString(), Code = "MT013", Name = "模具保养", Type = "工装", Description = "模具清洁润滑", StandardDuration = 40, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new MaintenanceItem { Id = Guid.NewGuid().ToString(), Code = "MT014", Name = "刮刀更换", Type = "工装", Description = "更换钢网刮刀", StandardDuration = 10, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now },
                new MaintenanceItem { Id = Guid.NewGuid().ToString(), Code = "MT015", Name = "外观检查", Type = "工装", Description = "检查外观损伤", StandardDuration = 15, IsActive = true, CreateTime = DateTime.Now, UpdateTime = DateTime.Now }
            };
            SaveMaintenanceItems(maintenanceItems);
        }

        #endregion
    }
}
