using System;
using System.IO;
using System.Reflection;

namespace WinFormsApp1.Common
{
    /// <summary>
    /// 路径辅助类 - 统一管理项目数据文件路径
    /// 数据文件保存在项目源码目录，避免编译后丢失
    /// </summary>
    public static class PathHelper
    {
        private static string _projectRoot;

        /// <summary>
        /// 获取项目根目录（WinFormsApp1项目文件夹）
        /// </summary>
        public static string ProjectRoot
        {
            get
            {
                if (_projectRoot == null)
                {
                    // 获取当前程序集所在目录
                    string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                    string binDir = Path.GetDirectoryName(assemblyLocation);

                    // 从 bin/Debug/net8.0-windows7.0 向上找到项目根目录
                    DirectoryInfo dir = new DirectoryInfo(binDir);
                    while (dir != null && dir.Name != "WinFormsApp1")
                    {
                        dir = dir.Parent;
                    }

                    if (dir != null)
                    {
                        _projectRoot = dir.FullName;
                    }
                    else
                    {
                        // 如果找不到，使用当前目录
                        _projectRoot = AppDomain.CurrentDomain.BaseDirectory;
                    }
                }
                return _projectRoot;
            }
        }

        /// <summary>
        /// 获取通用数据目录：Common/Data
        /// </summary>
        public static string CommonDataFolder => Path.Combine(ProjectRoot, "Common", "Data");

        /// <summary>
        /// 获取设备维护系统数据目录
        /// </summary>
        public static string EquipmentMaintenanceDataFolder => Path.Combine(CommonDataFolder, "EquipmentMaintenance");

        /// <summary>
        /// 获取设备维护系统基础数据目录
        /// </summary>
        public static string EquipmentMaintenanceBaseDataFolder => Path.Combine(EquipmentMaintenanceDataFolder, "BaseData");

        /// <summary>
        /// 确保目录存在
        /// </summary>
        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 获取配置文件目录
        /// </summary>
        public static string ConfigFolder => Path.Combine(ProjectRoot, "Common", "Config");
    }
}
