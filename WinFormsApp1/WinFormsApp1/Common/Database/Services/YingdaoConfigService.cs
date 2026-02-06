using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsApp1.Common.Entities.YingdaoAutoSubmit;
using Newtonsoft.Json;
using SqlSugar;

namespace WinFormsApp1.Common.Database.Services
{
    public class YingdaoConfigService
    {
        private readonly bool useNewTable;

        public YingdaoConfigService()
        {
            useNewTable = CheckIfNewTableExists();
        }

        private bool CheckIfNewTableExists()
        {
            try
            {
                using var db = DbHelper.GetInstance();
                return db.DbMaintenance.IsAnyTable("yingdao_users", false);
            }
            catch
            {
                return false;
            }
        }

        public bool GetAutoStartEnabled()
        {
            using var db = DbHelper.GetInstance();
            var config = db.Queryable<YingdaoSysConfig>()
                .Where(c => c.ConfigKey == "YINGDAO_AUTO_START")
                .First();
            
            return config != null && config.ConfigValue == "1";
        }

        public void SetAutoStartEnabled(bool enabled)
        {
            using var db = DbHelper.GetInstance();
            var config = db.Queryable<YingdaoSysConfig>()
                .Where(c => c.ConfigKey == "YINGDAO_AUTO_START")
                .First();
            
            if (config != null)
            {
                config.ConfigValue = enabled ? "1" : "0";
                config.UpdatedTime = DateTime.Now;
                db.Updateable(config).ExecuteCommand();
            }
        }

        public List<int> GetEnabledDays()
        {
            using var db = DbHelper.GetInstance();
            var config = db.Queryable<YingdaoSysConfig>()
                .Where(c => c.ConfigKey == "YINGDAO_ENABLED_DAYS")
                .First();
            
            if (config == null || string.IsNullOrWhiteSpace(config.ConfigValue))
                return new List<int> { 0, 1, 2, 3, 4 };
            
            return JsonConvert.DeserializeObject<List<int>>(config.ConfigValue);
        }

        public void SetEnabledDays(List<int> days)
        {
            using var db = DbHelper.GetInstance();
            string jsonValue = JsonConvert.SerializeObject(days);
            
            var config = db.Queryable<YingdaoSysConfig>()
                .Where(c => c.ConfigKey == "YINGDAO_ENABLED_DAYS")
                .First();
            
            if (config != null)
            {
                config.ConfigValue = jsonValue;
                config.UpdatedTime = DateTime.Now;
                db.Updateable(config).ExecuteCommand();
            }
        }

        public List<YingdaoUserConfig> GetAllUsers()
        {
            if (useNewTable)
            {
                return GetAllUsersFromNewTable();
            }
            else
            {
                return GetAllUsersFromOldTable();
            }
        }

        private List<YingdaoUserConfig> GetAllUsersFromNewTable()
        {
            using var db = DbHelper.GetInstance();
            var users = db.Queryable<YingdaoUser>()
                .OrderBy(u => u.Id)
                .ToList();
            
            return users.Select(u => new YingdaoUserConfig
            {
                Enabled = u.IsEnabled,
                Factory = u.Factory ?? "",
                EmployeeId = u.EmployeeId ?? "",
                Phone = u.Phone ?? "",
                CarNo = u.CarNo ?? "",
                LastSubmittedDate = u.LastSubmittedDate ?? "",
                LastResult = u.LastResult ?? ""
            }).ToList();
        }

        private List<YingdaoUserConfig> GetAllUsersFromOldTable()
        {
            using var db = DbHelper.GetInstance();
            var options = db.Queryable<YingdaoSysConfigOption>()
                .Where(o => o.ConfigKey == "YINGDAO_USERS" && o.IsActive)
                .OrderBy(o => o.SortOrder)
                .ToList();
            
            return options.Select(o => JsonConvert.DeserializeObject<YingdaoUserConfig>(o.ExtraData)).ToList();
        }

        public void SaveUser(YingdaoUserConfig user)
        {
            if (useNewTable)
            {
                SaveUserToNewTable(user);
            }
            else
            {
                SaveUserToOldTable(user);
            }
        }

        private void SaveUserToNewTable(YingdaoUserConfig user)
        {
            using var db = DbHelper.GetInstance();
            
            var existing = db.Queryable<YingdaoUser>()
                .Where(u => u.EmployeeId == user.EmployeeId)
                .First();
            
            if (existing != null)
            {
                existing.Factory = user.Factory;
                existing.Phone = user.Phone;
                existing.CarNo = user.CarNo;
                existing.LastSubmittedDate = user.LastSubmittedDate;
                existing.LastResult = user.LastResult;
                existing.IsEnabled = user.Enabled;
                existing.UpdatedAt = DateTime.Now;
                db.Updateable(existing).ExecuteCommand();
            }
            else
            {
                var newUser = new YingdaoUser
                {
                    Username = user.EmployeeId,
                    Password = "",
                    Factory = user.Factory,
                    EmployeeId = user.EmployeeId,
                    Phone = user.Phone,
                    CarNo = user.CarNo,
                    LastSubmittedDate = user.LastSubmittedDate,
                    LastResult = user.LastResult,
                    IsEnabled = user.Enabled,
                    Remark = "",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                db.Insertable(newUser).ExecuteCommand();
            }
        }

        private void SaveUserToOldTable(YingdaoUserConfig user)
        {
            using var db = DbHelper.GetInstance();
            string jsonData = JsonConvert.SerializeObject(user);
            
            var existing = db.Queryable<YingdaoSysConfigOption>()
                .Where(o => o.ConfigKey == "YINGDAO_USERS" && o.OptionValue == user.EmployeeId)
                .First();
            
            if (existing != null)
            {
                existing.OptionLabel = $"{user.Factory}-{user.EmployeeId}";
                existing.ExtraData = jsonData;
                existing.IsActive = user.Enabled;
                db.Updateable(existing).ExecuteCommand();
            }
            else
            {
                var maxSortOption = db.Queryable<YingdaoSysConfigOption>()
                    .Where(o => o.ConfigKey == "YINGDAO_USERS")
                    .OrderBy(o => o.SortOrder, OrderByType.Desc)
                    .First();
                
                int maxSort = maxSortOption?.SortOrder ?? 0;
                
                db.Insertable(new YingdaoSysConfigOption
                {
                    ConfigKey = "YINGDAO_USERS",
                    OptionValue = user.EmployeeId,
                    OptionLabel = $"{user.Factory}-{user.EmployeeId}",
                    SortOrder = maxSort + 1,
                    IsDefault = false,
                    IsActive = user.Enabled,
                    ExtraData = jsonData,
                    CreatedTime = DateTime.Now
                }).ExecuteCommand();
            }
        }

        public void DeleteUser(string employeeId)
        {
            if (useNewTable)
            {
                DeleteUserFromNewTable(employeeId);
            }
            else
            {
                DeleteUserFromOldTable(employeeId);
            }
        }

        private void DeleteUserFromNewTable(string employeeId)
        {
            using var db = DbHelper.GetInstance();
            db.Deleteable<YingdaoUser>()
                .Where(u => u.EmployeeId == employeeId)
                .ExecuteCommand();
        }

        private void DeleteUserFromOldTable(string employeeId)
        {
            using var db = DbHelper.GetInstance();
            db.Deleteable<YingdaoSysConfigOption>()
                .Where(o => o.ConfigKey == "YINGDAO_USERS" && o.OptionValue == employeeId)
                .ExecuteCommand();
        }
    }

    public class YingdaoUserConfig
    {
        public bool Enabled { get; set; } = true;
        public string Factory { get; set; } = "";
        public string EmployeeId { get; set; } = "";
        public string Phone { get; set; } = "";
        public string CarNo { get; set; } = "";
        public string LastSubmittedDate { get; set; } = "";
        public string LastResult { get; set; } = "";
    }
}
