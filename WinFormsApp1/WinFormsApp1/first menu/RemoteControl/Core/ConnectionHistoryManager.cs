using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace WinFormsApp1.first_menu.RemoteControl
{
    public class ConnectionHistoryManager
    {
        private static ConnectionHistoryManager instance;
        private static readonly object lockObj = new object();
        
        private List<ConnectionHistoryItem> historyItems;
        private readonly string historyFilePath;
        private const int MaxHistoryCount = 20;
        
        public static ConnectionHistoryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new ConnectionHistoryManager();
                        }
                    }
                }
                return instance;
            }
        }
        
        private ConnectionHistoryManager()
        {
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "RemoteControl"
            );
            
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            
            historyFilePath = Path.Combine(appDataPath, "connection_history.json");
            LoadHistory();
        }
        
        private void LoadHistory()
        {
            try
            {
                if (File.Exists(historyFilePath))
                {
                    string json = File.ReadAllText(historyFilePath);
                    historyItems = JsonConvert.DeserializeObject<List<ConnectionHistoryItem>>(json) ?? new List<ConnectionHistoryItem>();
                }
                else
                {
                    historyItems = new List<ConnectionHistoryItem>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载历史记录失败: {ex.Message}");
                historyItems = new List<ConnectionHistoryItem>();
            }
        }
        
        private void SaveHistory()
        {
            try
            {
                string json = JsonConvert.SerializeObject(historyItems, Formatting.Indented);
                File.WriteAllText(historyFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存历史记录失败: {ex.Message}");
            }
        }
        
        public void AddOrUpdateConnection(string deviceCode)
        {
            if (string.IsNullOrWhiteSpace(deviceCode))
                return;
            
            lock (lockObj)
            {
                var existingItem = historyItems.FirstOrDefault(x => x.DeviceCode == deviceCode);
                
                if (existingItem != null)
                {
                    existingItem.UseCount++;
                    existingItem.LastUsedTime = DateTime.Now;
                }
                else
                {
                    historyItems.Add(new ConnectionHistoryItem
                    {
                        DeviceCode = deviceCode,
                        UseCount = 1,
                        LastUsedTime = DateTime.Now
                    });
                }
                
                if (historyItems.Count > MaxHistoryCount)
                {
                    historyItems = historyItems
                        .OrderByDescending(x => x.UseCount)
                        .ThenByDescending(x => x.LastUsedTime)
                        .Take(MaxHistoryCount)
                        .ToList();
                }
                
                SaveHistory();
            }
        }
        
        public List<ConnectionHistoryItem> GetSortedHistory()
        {
            lock (lockObj)
            {
                return historyItems
                    .OrderByDescending(x => x.UseCount)
                    .ThenByDescending(x => x.LastUsedTime)
                    .ToList();
            }
        }
        
        public void RemoveConnection(string deviceCode)
        {
            if (string.IsNullOrWhiteSpace(deviceCode))
                return;
            
            lock (lockObj)
            {
                historyItems.RemoveAll(x => x.DeviceCode == deviceCode);
                SaveHistory();
            }
        }
        
        public void ClearHistory()
        {
            lock (lockObj)
            {
                historyItems.Clear();
                SaveHistory();
            }
        }
    }
    
    public class ConnectionHistoryItem
    {
        public string DeviceCode { get; set; }
        public int UseCount { get; set; }
        public DateTime LastUsedTime { get; set; }
        
        public string DisplayText
        {
            get
            {
                return $"{DeviceCode} (使用{UseCount}次)";
            }
        }
    }
}
