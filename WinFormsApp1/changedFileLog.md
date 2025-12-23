D:\ProgramCode\PROD\EEL.YPD\branches\sprint107\Web\EEL.YPD.Web\Modules\cankao\winfrom\WinFormsApp1\WinFormsApp1\Program.cs
- 修复HighDpiMode相关错误
- updated by cqy at 2025-9-8

D:\ProgramCode\PROD\EEL.YPD\branches\sprint107\Web\EEL.YPD.Web\Modules\cankao\winfrom\WinFormsApp1\WinFormsApp1\Form1.cs
- 修复字符串转义问题
- 修复Newtonsoft.Json反序列化问题，使用JObject.Parse替代
- 添加JObject支持
- 添加Oracle.ManagedDataAccess.Client引用
- 修改TestDatabaseConnection方法，支持Oracle连接
- 添加UpdateConnectionStringExample方法，提供Oracle连接字符串示例
- 默认选择Oracle数据库类型
- 修复InitializeControls方法中的类型转换错误
- 修复UIProcessBar属性设置问题，使用反射设置Maximum属性
- 修改数据库表结构存储方式，使用Dictionary<string, Dictionary<string, object>>替代DataTable
- 添加LoadOracleTableStructure方法，实现Oracle表结构加载
- 修改UpdateDbColumnComboBox和CellValueChanged方法，适应新的表结构格式
- 实现ImportToOracle方法，支持Oracle数据导入
- 添加SampleMasterYYLibrary表的示例数据
- 设置窗体大小为屏幕的3/5，并居中显示
- 修复LoadOracleTableStructure方法中的SQL查询，解决ORA-00904错误
- 添加OWNER参数到SQL查询中，确保能正确查询到表结构
- 修改ImportToOracle方法，使用字符串拼接SQL而不是StringBuilder和参数化
- 优化ID字段处理逻辑，支持不同类型的ID字段（自增、GUID等）
- 修复VARCHAR2类型ID字段处理，确保ID字段总是有值
- updated by cqy at 2025-9-9

D:\ProgramCode\PROD\EEL.YPD\branches\sprint107\Web\EEL.YPD.Web\Modules\cankao\winfrom\WinFormsApp1\WinFormsApp1\Form1.Designer.cs
- 简化Designer代码，解决ContentAlignment引用问题
- 完善UI布局，修复控件位置和大小设置
- 修复UI错乱问题
- 将保存配置和加载配置按钮移动到数据库配置标签页中
- 设置窗体可调整大小，添加FormBorderStyle.Sizable和MinimumSize属性
- 调整所有按钮大小，从120x40调整为100x30，字体从12F调整为10.5F
- 为主控件添加Anchor属性，使其支持自适应大小
- 为数据网格和进度条添加Anchor属性，使其支持自适应大小
- 统一文本框大小为500x35或250x35
- 统一按钮大小为120x35
- 调整控件位置，使布局更加整齐美观
- updated by cqy at 2025-9-8

D:\ProgramCode\PROD\EEL.YPD\branches\sprint107\Web\EEL.YPD.Web\Modules\cankao\winfrom\WinFormsApp1\WinFormsApp1\WinFormsApp1.csproj
- 添加必要的NuGet包引用
- 设置目标框架为net472
- 添加Microsoft.CSharp引用，解决动态绑定问题
- 添加设计器支持配置
- 添加Oracle.ManagedDataAccess引用，支持Oracle数据库连接
- updated by cqy at 2025-9-8

D:\ProgramCode\PROD\EEL.YPD\branches\sprint107\Web\EEL.YPD.Web\Modules\cankao\winfrom\WinFormsApp1\WinFormsApp1\Properties\Settings.settings
- 创建Settings.settings文件，支持设计器
- updated by cqy at 2025-9-8

D:\ProgramCode\PROD\EEL.YPD\branches\sprint107\Web\EEL.YPD.Web\Modules\cankao\winfrom\WinFormsApp1\WinFormsApp1\Properties\Settings.Designer.cs
- 创建Settings.Designer.cs文件，支持设计器
- updated by cqy at 2025-9-8

D:\ProgramCode\PROD\EEL.YPD\branches\sprint107\Web\EEL.YPD.Web\Modules\cankao\winfrom\WinFormsApp1\WinFormsApp1\Properties\AssemblyInfo.cs
- 创建AssemblyInfo.cs文件，提供程序集信息
- updated by cqy at 2025-9-8 