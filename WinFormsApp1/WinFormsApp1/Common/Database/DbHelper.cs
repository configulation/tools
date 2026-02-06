using SqlSugar;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace WinFormsApp1.Common.Database
{
    public class DbHelper
    {
        private static string _connectionString;

        static DbHelper()
        {
            LoadConnectionString();
        }

        private static void LoadConnectionString()
        {
            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                if (File.Exists(configPath))
                {
                    string json = File.ReadAllText(configPath);
                    var config = JObject.Parse(json);
                    _connectionString = config["ConnectionStrings"]?["MySql"]?.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"读取数据库配置失败: {ex.Message}");
            }
        }

        public static SqlSugarClient GetInstance()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new Exception("数据库连接字符串未配置");
            }

            return new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = _connectionString,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }
    }
}
