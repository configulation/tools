using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinFormsApp1.first_menu.RemoteControl
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    public class ConfigManager
    {
        private static ConfigManager instance;
        private JObject configData;
        private string configPath;
        
        public static ConfigManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigManager();
                }
                return instance;
            }
        }
        
        private ConfigManager()
        {
            configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RemoteControlConfig.json");
            LoadConfig();
        }
        
        /// <summary>
        /// 加载配置文件
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                if (File.Exists(configPath))
                {
                    string json = File.ReadAllText(configPath);
                    configData = JObject.Parse(json);
                }
                else
                {
                    // 如果配置文件不存在，创建默认配置
                    CreateDefaultConfig();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载配置文件失败: {ex.Message}");
                CreateDefaultConfig();
            }
        }
        
        /// <summary>
        /// 创建默认配置
        /// </summary>
        private void CreateDefaultConfig()
        {
            configData = new JObject
            {
                ["RemoteControl"] = new JObject
                {
                    ["Network"] = new JObject
                    {
                        ["DefaultPort"] = 8888,
                        ["MaxConnections"] = 5,
                        ["ConnectionTimeout"] = 30000,
                        ["HeartbeatInterval"] = 5000,
                        ["BufferSize"] = 4096
                    },
                    ["Screen"] = new JObject
                    {
                        ["DefaultQuality"] = 70,
                        ["MinQuality"] = 10,
                        ["MaxQuality"] = 100,
                        ["DefaultFPS"] = 30,
                        ["MinFPS"] = 5,
                        ["MaxFPS"] = 60
                    },
                    ["Security"] = new JObject
                    {
                        ["DeviceCodeLength"] = 6,
                        ["DeviceCodeExpiration"] = 300
                    },
                    ["Advanced"] = new JObject
                    {
                        ["UseRelay"] = true,
                        ["RelayServer"] = "103.112.185.127:9100"
                    }
                }
            };
            
            SaveConfig();
        }
        
        /// <summary>
        /// 保存配置
        /// </summary>
        public void SaveConfig()
        {
            try
            {
                string json = configData.ToString(Formatting.Indented);
                File.WriteAllText(configPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存配置文件失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 获取配置值
        /// </summary>
        public T GetValue<T>(string path, T defaultValue = default(T))
        {
            try
            {
                var token = configData.SelectToken(path);
                if (token != null)
                {
                    if (typeof(T) == typeof(string))
                    {
                        string value = token.ToObject<string>();
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            return defaultValue;
                        }
                    }
                    return token.ToObject<T>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取配置值失败: {ex.Message}");
            }
            
            return defaultValue;
        }
        
        /// <summary>
        /// 设置配置值
        /// </summary>
        public void SetValue(string path, object value)
        {
            try
            {
                var tokens = path.Split('.');
                JToken current = configData;
                
                for (int i = 0; i < tokens.Length - 1; i++)
                {
                    if (current[tokens[i]] == null)
                    {
                        current[tokens[i]] = new JObject();
                    }
                    current = current[tokens[i]];
                }
                
                current[tokens[tokens.Length - 1]] = JToken.FromObject(value);
                SaveConfig();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"设置配置值失败: {ex.Message}");
            }
        }
        
        // 便捷属性访问器
        public int NetworkPort => GetValue<int>("RemoteControl.Network.DefaultPort", 8888);
        public int ScreenQuality => GetValue<int>("RemoteControl.Screen.DefaultQuality", 70);
        public int ScreenFPS => GetValue<int>("RemoteControl.Screen.DefaultFPS", 30);
        public int DeviceCodeLength => GetValue<int>("RemoteControl.Security.DeviceCodeLength", 6);
        public int ConnectionTimeout => GetValue<int>("RemoteControl.Network.ConnectionTimeout", 30000);
        public int BufferSize => GetValue<int>("RemoteControl.Network.BufferSize", 4096);
        public bool EnableLogging => GetValue<bool>("RemoteControl.Logging.EnableLogging", true);
        public string LogLevel => GetValue<string>("RemoteControl.Logging.LogLevel", "Info");

        public bool RelayEnabled => GetValue<bool>("RemoteControl.Advanced.UseRelay", true);
        public string RelayServer => GetValue<string>("RemoteControl.Advanced.RelayServer", "103.112.185.127:9100");
    }
}
