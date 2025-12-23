# 远程控制模块 - 文件结构说明

## 📁 目录结构

```
RemoteControl/
├── Core/                    # 核心功能模块
│   ├── ConfigManager.cs     # 配置管理器
│   └── RemoteControlManager.cs  # 远程控制核心管理器
│
├── Network/                 # 网络通信模块
│   ├── NetworkManager.cs    # 网络连接管理
│   ├── NetworkProtocol.cs   # 网络协议实现
│   ├── NetworkHelper.cs     # 网络工具类（IP获取等）
│   └── NetworkDiagnostics.cs # 网络诊断工具
│
├── Capture/                 # 屏幕捕获模块
│   ├── ScreenCaptureManager.cs  # 屏幕捕获管理器
│   ├── DifferentialCapture.cs   # 差异捕获引擎
│   └── AdaptiveFrameController.cs # 自适应帧率控制器
│
├── UI/                      # 用户界面
│   ├── FrmRemoteControl.cs       # 主窗体代码
│   ├── FrmRemoteControl.Designer.cs # 窗体设计器生成代码
│   └── FrmRemoteControl.resx     # 窗体资源文件
│
├── Utils/                   # 工具类
│   └── WinAPI.cs           # Windows API调用封装
│
├── Test/                    # 测试相关
│   ├── TestHelper.cs       # 测试辅助工具（双窗口测试等）
│   └── QuickTest.cs        # 快速测试工具
│
├── Config/                  # 配置文件
│   └── RemoteControlConfig.json  # 程序配置文件
│
└── README.md               # 本说明文档
```

## 🔧 功能模块说明

### Core（核心功能）
- **ConfigManager**: 管理程序配置，包括网络端口、屏幕质量、FPS等
- **RemoteControlManager**: 处理远程控制的核心逻辑，包括鼠标键盘事件

### Network（网络通信）
- **NetworkManager**: 处理TCP连接、数据收发、连接管理
- **NetworkProtocol**: 专业级网络协议，支持数据压缩、批处理、确认机制
- **NetworkHelper**: 网络工具，获取本机IP、解析设备码等
- **NetworkDiagnostics**: 网络连接诊断，Ping测试、端口检测等

### Capture（屏幕捕获）
- **ScreenCaptureManager**: 基础屏幕捕获，支持JPEG压缩
- **DifferentialCapture**: 差异捕获引擎，只传输变化的屏幕区域
- **AdaptiveFrameController**: 根据网络状况自动调整帧率

### UI（用户界面）
- **FrmRemoteControl**: 主窗体，包含所有用户交互逻辑

### Utils（工具类）
- **WinAPI**: Windows API封装，用于鼠标键盘控制

### Test（测试工具）
- **TestHelper**: 支持单机双窗口测试
- **QuickTest**: 快速网络信息测试

## 📝 命名空间约定

基础命名空间：`WinFormsApp1.first_menu.RemoteControl`

各模块子命名空间：
- Core: `.Core`
- Network: `.Network`
- Capture: `.Capture`
- UI: `.UI`
- Utils: `.Utils`
- Test: `.Test`

## 🚀 使用说明

### 局域网连接
1. **受控端**：点击"开始受控"，获取设备码和IP
2. **控制端**：输入格式`设备码#IP地址`（如：123456#192.168.1.7）
3. 点击"连接"

### 本地测试
1. 只输入6位设备码（不加IP）
2. 程序会连接到本地（127.0.0.1）

### 防火墙设置
```batch
# 添加入站规则（管理员权限）
netsh advfirewall firewall add rule name="RemoteControl" dir=in action=allow protocol=TCP localport=8888
```

## ⚠️ 注意事项

1. **编译时注意**：如果出现命名空间错误，需要更新文件中的using引用
2. **防火墙**：确保端口8888已开放
3. **网络环境**：两台电脑必须在同一局域网
4. **虚拟网卡**：程序会自动过滤VMware等虚拟网卡

## 🔍 问题排查

如果连接失败：
1. 使用内置的网络诊断工具（连接失败时自动提示）
2. 检查防火墙设置
3. 确认IP地址正确
4. 检查端口是否被占用

## 📊 性能优化

- 差异捕获：只传输变化的屏幕区域
- 自适应帧率：根据网络状况动态调整
- JPEG压缩：减少传输数据量
- 批处理发送：提高网络效率

## 🎯 最新更新（2024-12-03 23:45）

### 已修复问题
1. **鼠标坐标映射精度问题**
   - 问题：控制端鼠标位置与被控端不一致
   - 原因：未考虑PictureBox的Zoom模式下图片实际显示区域
   - 解决：实现`GetImageDisplayRectangle`方法，精确计算图片显示边界

2. **添加全屏功能**
   - 新增全屏按钮（位于右上角）
   - 支持ESC键退出全屏
   - 全屏时隐藏所有控制面板，最大化显示区域

3. **弹性布局优化**
   - PictureBox改为Zoom模式（保持图片比例）
   - 左侧面板添加AutoScroll（内容过多时自动滚动）
   - 全屏按钮使用Anchor定位（自适应窗口大小）

## 📖 使用说明

### 基础操作

#### 作为被控端（被控制的电脑）
1. 点击【开始受控】按钮
2. 记下显示的6位设备码（如：666666）
3. 告诉控制方你的IP地址（显示在标题栏）

#### 作为控制端（控制别人的电脑）
1. 获取对方的设备码和IP地址
2. 在输入框中输入：`设备码#IP地址`
   - 例如：`666666#192.168.1.6`
3. 点击【连接】按钮

### 高级功能

#### 全屏模式
- **进入全屏**：点击右上角【全屏】按钮
- **退出全屏**：按ESC键或再次点击全屏按钮
- 全屏时自动隐藏所有控制面板

#### 画质调整（仅受控端可用）
- **画面质量**：10-100%，默认70%
- **传输帧率**：5-60 FPS，默认30 FPS
- 根据网络状况调整以获得最佳体验

#### 测试模式
- 点击左上角【测试模式】按钮
- 选择测试方式：
  - 双窗口测试：同时打开两个窗口模拟
  - 双进程测试：启动两个独立进程
  - 环回测试：本机自连接

## 🔧 故障排除

### 连接失败
1. **检查防火墙**
   ```batch
   # 管理员CMD执行
   netsh advfirewall firewall add rule name="RemoteControl" dir=in action=allow protocol=TCP localport=8888
   ```

2. **检查IP格式**
   - ✅ 正确：`123456#192.168.1.7`
   - ❌ 错误：`123456`（局域网必须带IP）

3. **使用诊断工具**
   - 连接失败时会自动提示运行诊断
   - 诊断内容：Ping测试、端口检测、子网分析

### 鼠标不准确
- 确保PictureBox的SizeMode为Zoom
- 检查屏幕分辨率是否一致
- 尝试重新连接

### 画面卡顿
- 降低画面质量（50-60%）
- 降低帧率（15-20 FPS）
- 检查网络带宽

## 📊 性能优化建议

### 局域网环境
- 画质：80-100%
- 帧率：30-60 FPS
- 延迟：< 50ms

### 低带宽环境
- 画质：40-60%
- 帧率：10-20 FPS
- 启用差异捕获

## 更新日志

### 2024-12-04 00:10
- ✅ **彻底修复鼠标精度问题**（腾讯电脑管家方式）
  - 自动交换屏幕分辨率信息
  - 精确计算Zoom模式下的显示区域
  - 添加鼠标移动限流（20ms间隔）
  - 实时显示鼠标坐标（调试用）
- ✅ **全屏体验升级**
  - 新增独立全屏窗口覆盖任务栏
  - 支持多屏幕自动选择当前屏幕
  - ESC/双击退出，全屏提示自动隐藏

### 2024-12-03 23:45
- ✅ 修复鼠标坐标映射精度问题
- ✅ 添加全屏显示功能
- ✅ 实现弹性布局
- ✅ 优化PictureBox显示模式

### 2024-12-03 23:24
- ✅ 修复局域网连接问题
- ✅ 添加网络诊断工具
- ✅ 整理文件结构
- ✅ 修复屏幕传输问题
