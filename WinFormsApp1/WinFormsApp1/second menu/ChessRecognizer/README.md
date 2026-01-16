# 中国象棋识别系统

基于 .NET 8 WinForms + SunnyUI 的中国象棋图片识别系统。

## 功能特性

- 支持多种图片输入方式：粘贴、拖拽、文件选择
- 识别象棋棋子的文字和位置
- 输出标准FEN字符串
- 控制台输出可视化棋盘图案
- 支持多种OCR引擎（Tesseract、SimpleOCR）
- 提供微调功能，允许调整识别参数
- 保存和加载识别配置

## 项目结构

```
ChessRecognizer/
├── Forms/                      # 窗体
│   ├── FrmChessRecognizer.cs   # 主界面
│   └── FrmSettings.cs          # 设置窗体
├── Core/                       # 核心模块
│   ├── Interfaces/             # 接口定义
│   │   ├── IChessOCR.cs
│   │   ├── IBoardDetector.cs
│   │   └── IFENGenerator.cs
│   ├── Services/               # 服务实现
│   │   ├── ChessRecognitionEngine.cs  # 核心引擎
│   │   ├── OCR/
│   │   │   ├── TesseractOCRService.cs
│   │   │   └── SimpleOCRService.cs
│   │   ├── Detection/
│   │   │   ├── BoardDetector.cs
│   │   │   └── PieceDetector.cs
│   │   └── FEN/
│   │       ├── FENGenerator.cs
│   │       └── RuleValidator.cs
│   └── Models/                 # 数据模型
│       ├── ChessPiece.cs
│       ├── BoardPosition.cs
│       └── RecognitionResult.cs
├── Utils/                      # 工具类
│   ├── ImageProcessor.cs
│   └── ConsoleRenderer.cs
├── Config/                     # 配置
│   └── AppSettings.cs
└── Tests/                      # 测试
    └── PerformanceTester.cs
```

## 依赖项

项目已包含以下NuGet包：
- OpenCvSharp4 - 图像处理
- Tesseract - OCR识别
- SunnyUI - UI框架
- Newtonsoft.Json - JSON序列化

## 使用Tesseract OCR

需要下载中文语言包：
1. 下载 `chi_sim.traineddata` 文件
2. 放置到 `tessdata` 目录下

下载地址：https://github.com/tesseract-ocr/tessdata

## 快捷键

- `Ctrl+V` - 从剪贴板粘贴图片

## 识别流程

1. 图像预处理（调整大小、去噪、锐化）
2. 棋盘检测（边缘检测、霍夫变换）
3. 提取棋子区域
4. OCR文字识别
5. 颜色分析（红/黑判断）
6. 生成FEN字符串
7. 规则验证

## 配置说明

配置文件位于：`Config/chess_recognizer_settings.json`

主要配置项：
- OCR置信度阈值
- 棋盘检测参数
- FEN生成选项
- 性能设置
- 界面设置

## 注意事项

1. 图片质量越高，识别准确率越高
2. 建议使用标准棋盘截图
3. 倾斜或模糊的图片可能影响识别效果
4. 首次使用需要初始化OCR引擎，可能需要几秒钟
