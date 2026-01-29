using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsApp1.first_menu.RemoteControl
{
    public class ClipboardManager : IDisposable
    {
        private readonly object lockObj = new object();
        private bool isMonitoring = false;
        private Thread monitorThread;
        private string lastClipboardHash = string.Empty;
        
        public event EventHandler<ClipboardChangedEventArgs> ClipboardChanged;
        
        public void StartMonitoring()
        {
            if (isMonitoring) return;
            
            isMonitoring = true;
            monitorThread = new Thread(MonitorClipboard)
            {
                IsBackground = true,
                Name = "ClipboardMonitor"
            };
            monitorThread.SetApartmentState(ApartmentState.STA);
            monitorThread.Start();
        }
        
        public void StopMonitoring()
        {
            isMonitoring = false;
            monitorThread?.Join(1000);
        }
        
        private void MonitorClipboard()
        {
            while (isMonitoring)
            {
                try
                {
                    if (Clipboard.ContainsData(DataFormats.Text) ||
                        Clipboard.ContainsData(DataFormats.Bitmap) ||
                        Clipboard.ContainsData(DataFormats.FileDrop))
                    {
                        string currentHash = GetClipboardHash();
                        if (currentHash != lastClipboardHash)
                        {
                            lastClipboardHash = currentHash;
                            var data = CaptureClipboardData();
                            if (data != null)
                            {
                                ClipboardChanged?.Invoke(this, new ClipboardChangedEventArgs(data));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"剪贴板监控错误: {ex.Message}");
                }
                
                Thread.Sleep(500);
            }
        }
        
        private string GetClipboardHash()
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    return $"TEXT:{Clipboard.GetText().GetHashCode()}";
                }
                else if (Clipboard.ContainsImage())
                {
                    return $"IMAGE:{DateTime.Now.Ticks}";
                }
                else if (Clipboard.ContainsFileDropList())
                {
                    var files = Clipboard.GetFileDropList();
                    return $"FILES:{string.Join("|", files.Cast<string>()).GetHashCode()}";
                }
            }
            catch { }
            
            return string.Empty;
        }
        
        public ClipboardData CaptureClipboardData()
        {
            lock (lockObj)
            {
                try
                {
                    if (Clipboard.ContainsText())
                    {
                        return new ClipboardData
                        {
                            Type = ClipboardDataType.Text,
                            TextData = Clipboard.GetText()
                        };
                    }
                    else if (Clipboard.ContainsImage())
                    {
                        var image = Clipboard.GetImage();
                        return new ClipboardData
                        {
                            Type = ClipboardDataType.Image,
                            ImageData = ImageToBytes(image)
                        };
                    }
                    else if (Clipboard.ContainsFileDropList())
                    {
                        var files = Clipboard.GetFileDropList();
                        var data = new ClipboardData
                        {
                            Type = ClipboardDataType.Files,
                            FilePaths = files.Cast<string>().ToList()
                        };
                        data.LoadFileContents();
                        return data;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"捕获剪贴板数据失败: {ex.Message}");
                }
                
                return null;
            }
        }
        
        public bool SetClipboardData(ClipboardData data)
        {
            if (data == null) return false;
            
            lock (lockObj)
            {
                try
                {
                    string oldHash = lastClipboardHash;
                    
                    switch (data.Type)
                    {
                        case ClipboardDataType.Text:
                            if (!string.IsNullOrEmpty(data.TextData))
                            {
                                Clipboard.SetText(data.TextData);
                                lastClipboardHash = $"TEXT:{data.TextData.GetHashCode()}";
                                return true;
                            }
                            break;
                            
                        case ClipboardDataType.Image:
                            if (data.ImageData != null && data.ImageData.Length > 0)
                            {
                                var image = BytesToImage(data.ImageData);
                                if (image != null)
                                {
                                    Clipboard.SetImage(image);
                                    lastClipboardHash = $"IMAGE:{DateTime.Now.Ticks}";
                                    return true;
                                }
                            }
                            break;
                            
                        case ClipboardDataType.Files:
                            if (data.FileContents != null && data.FileContents.Count > 0)
                            {
                                string tempDir = Path.Combine(Path.GetTempPath(), "RemoteClipboard_" + Guid.NewGuid().ToString("N"));
                                data.SaveFilesToDirectory(tempDir);
                                
                                var filePaths = new List<string>();
                                
                                foreach (var file in data.FileContents)
                                {
                                    string fullPath = Path.Combine(tempDir, file.RelativePath);
                                    
                                    if (file.IsDirectory)
                                    {
                                        if (Directory.Exists(fullPath))
                                        {
                                            filePaths.Add(fullPath);
                                        }
                                    }
                                    else
                                    {
                                        if (File.Exists(fullPath))
                                        {
                                            filePaths.Add(fullPath);
                                        }
                                    }
                                }
                                
                                if (filePaths.Count > 0)
                                {
                                    var collection = new StringCollection();
                                    collection.AddRange(filePaths.ToArray());
                                    Clipboard.SetFileDropList(collection);
                                    lastClipboardHash = $"FILES:{string.Join("|", filePaths).GetHashCode()}";
                                    return true;
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"设置剪贴板数据失败: {ex.Message}");
                }
                
                return false;
            }
        }
        
        private byte[] ImageToBytes(Image image)
        {
            if (image == null) return null;
            
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
        
        private Image BytesToImage(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            
            using (var ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }
        
        public void ClearClipboard()
        {
            lock (lockObj)
            {
                try
                {
                    Clipboard.Clear();
                    lastClipboardHash = string.Empty;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"清空剪贴板失败: {ex.Message}");
                }
            }
        }
        
        public void Dispose()
        {
            StopMonitoring();
        }
    }
    
    public enum ClipboardDataType : byte
    {
        Text = 0x01,
        Image = 0x02,
        Files = 0x03
    }
    
    public class ClipboardData
    {
        public ClipboardDataType Type { get; set; }
        public string TextData { get; set; }
        public byte[] ImageData { get; set; }
        public List<string> FilePaths { get; set; }
        public List<FileTransferData> FileContents { get; set; }
        
        public void LoadFileContents()
        {
            if (Type != ClipboardDataType.Files || FilePaths == null || FilePaths.Count == 0)
                return;
                
            FileContents = new List<FileTransferData>();
            
            foreach (var path in FilePaths)
            {
                try
                {
                    if (Directory.Exists(path))
                    {
                        LoadDirectory(path, path);
                    }
                    else if (File.Exists(path))
                    {
                        var fileInfo = new FileInfo(path);
                        FileContents.Add(new FileTransferData
                        {
                            FileName = fileInfo.Name,
                            RelativePath = fileInfo.Name,
                            FileSize = fileInfo.Length,
                            IsDirectory = false,
                            Content = File.ReadAllBytes(path)
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"读取文件失败 {path}: {ex.Message}");
                }
            }
        }
        
        private void LoadDirectory(string dirPath, string basePath)
        {
            try
            {
                var dirInfo = new DirectoryInfo(dirPath);
                string relativePath = dirPath.Substring(basePath.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                
                FileContents.Add(new FileTransferData
                {
                    FileName = dirInfo.Name,
                    RelativePath = string.IsNullOrEmpty(relativePath) ? dirInfo.Name : relativePath,
                    FileSize = 0,
                    IsDirectory = true,
                    Content = null
                });
                
                foreach (var file in dirInfo.GetFiles())
                {
                    try
                    {
                        string fileRelativePath = Path.Combine(relativePath, file.Name);
                        FileContents.Add(new FileTransferData
                        {
                            FileName = file.Name,
                            RelativePath = fileRelativePath,
                            FileSize = file.Length,
                            IsDirectory = false,
                            Content = File.ReadAllBytes(file.FullName)
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"读取文件失败 {file.FullName}: {ex.Message}");
                    }
                }
                
                foreach (var subDir in dirInfo.GetDirectories())
                {
                    LoadDirectory(subDir.FullName, basePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取文件夹失败 {dirPath}: {ex.Message}");
            }
        }
        
        public void SaveFilesToDirectory(string targetDirectory)
        {
            if (Type != ClipboardDataType.Files || FileContents == null || FileContents.Count == 0)
                return;
                
            try
            {
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }
                
                foreach (var file in FileContents)
                {
                    string fullPath = Path.Combine(targetDirectory, file.RelativePath);
                    
                    if (file.IsDirectory)
                    {
                        if (!Directory.Exists(fullPath))
                        {
                            Directory.CreateDirectory(fullPath);
                        }
                    }
                    else
                    {
                        string directory = Path.GetDirectoryName(fullPath);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        
                        File.WriteAllBytes(fullPath, file.Content);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存文件失败: {ex.Message}");
                throw;
            }
        }
        
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write((byte)Type);
                
                switch (Type)
                {
                    case ClipboardDataType.Text:
                        writer.Write(TextData ?? string.Empty);
                        break;
                        
                    case ClipboardDataType.Image:
                        writer.Write(ImageData?.Length ?? 0);
                        if (ImageData != null && ImageData.Length > 0)
                        {
                            writer.Write(ImageData);
                        }
                        break;
                        
                    case ClipboardDataType.Files:
                        writer.Write(FileContents?.Count ?? 0);
                        if (FileContents != null)
                        {
                            foreach (var file in FileContents)
                            {
                                writer.Write(file.FileName);
                                writer.Write(file.RelativePath);
                                writer.Write(file.FileSize);
                                writer.Write(file.IsDirectory);
                                if (!file.IsDirectory && file.Content != null)
                                {
                                    writer.Write(file.Content.Length);
                                    writer.Write(file.Content);
                                }
                            }
                        }
                        break;
                }
                
                return ms.ToArray();
            }
        }
        
        public static ClipboardData Deserialize(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            using (var reader = new BinaryReader(ms))
            {
                var result = new ClipboardData
                {
                    Type = (ClipboardDataType)reader.ReadByte()
                };
                
                switch (result.Type)
                {
                    case ClipboardDataType.Text:
                        result.TextData = reader.ReadString();
                        break;
                        
                    case ClipboardDataType.Image:
                        int imageLength = reader.ReadInt32();
                        if (imageLength > 0)
                        {
                            result.ImageData = reader.ReadBytes(imageLength);
                        }
                        break;
                        
                    case ClipboardDataType.Files:
                        int fileCount = reader.ReadInt32();
                        result.FileContents = new List<FileTransferData>();
                        for (int i = 0; i < fileCount; i++)
                        {
                            var file = new FileTransferData
                            {
                                FileName = reader.ReadString(),
                                RelativePath = reader.ReadString(),
                                FileSize = reader.ReadInt64(),
                                IsDirectory = reader.ReadBoolean()
                            };
                            
                            if (!file.IsDirectory)
                            {
                                int contentLength = reader.ReadInt32();
                                file.Content = reader.ReadBytes(contentLength);
                            }
                            
                            result.FileContents.Add(file);
                        }
                        break;
                }
                
                return result;
            }
        }
        
        public string GetSizeDescription()
        {
            long size = 0;
            
            switch (Type)
            {
                case ClipboardDataType.Text:
                    size = Encoding.UTF8.GetByteCount(TextData ?? string.Empty);
                    break;
                case ClipboardDataType.Image:
                    size = ImageData?.Length ?? 0;
                    break;
                case ClipboardDataType.Files:
                    size = FileContents?.Sum(f => f.FileSize) ?? 0;
                    break;
            }
            
            if (size < 1024)
                return $"{size} B";
            else if (size < 1024 * 1024)
                return $"{size / 1024.0:F2} KB";
            else
                return $"{size / (1024.0 * 1024.0):F2} MB";
        }
    }
    
    public class FileTransferData
    {
        public string FileName { get; set; }
        public string RelativePath { get; set; }
        public long FileSize { get; set; }
        public bool IsDirectory { get; set; }
        public byte[] Content { get; set; }
    }
    
    public class ClipboardChangedEventArgs : EventArgs
    {
        public ClipboardData Data { get; }
        
        public ClipboardChangedEventArgs(ClipboardData data)
        {
            Data = data;
        }
    }
}
