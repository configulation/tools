using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json; // 使用Newtonsoft.Json替代System.Text.Json
using Newtonsoft.Json.Linq; // 添加JObject支持
using System.Threading;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data.OleDb; // added by cqy at 2025-9-9
using NPOI.SS.UserModel; // added by cqy at 2025-9-9
using NPOI.XSSF.UserModel; // added by cqy at 2025-9-9
using NPOI.HSSF.UserModel; // added by cqy at 2025-9-9
using NPOI.Util; // added by cqy at 2025-9-9
using System.Net; // added by cqy at 2025-9-9
// using EEL.YPD.Web.Modules.YPDWorkflow; // updated by cqy at 2025-9-9: 改为HTTP调用避免引用

namespace WinFormsApp1
{
    public partial class Form1 : Sunny.UI.UIPage
    {
        // 数据库类型列表
        private string[] dbTypes = { "MySQL", "SQL Server", "PostgreSQL", "Oracle" };

        // Excel数据
        private DataTable excelData;

        // 数据库表结构
        private Dictionary<string, Dictionary<string, object>> dbTableStructure;

        // 字段映射
        private Dictionary<string, string> fieldMappings;

        // 值映射配置：Excel列 -> 配置 - added by cqy at 2025-9-9
        private Dictionary<string, ValueMappingConfig> valueMappingConfigs;

        // API基础地址 - added by cqy at 2025-9-9
        private string apiBaseUrl = "http://192.168.27.132/YPD/Modules/YPDWorkflow/SampleMasterYYLibrary.aspx";//"http://localhost/EEL.YPD.Web/Modules/YPDWorkflow/SampleMasterYYLibrary.aspx";

        // 值映射配置结构 - added by cqy at 2025-9-9
        private class ValueMappingConfig
        {
            public bool MapNeeded { get; set; }
            public string MapMode { get; set; } // "手动" 或 "数据库"
            public string ManualPairs { get; set; } // 例如：描述1=ID1;描述2=ID2
            public string MapTable { get; set; }
            public string MapOnDbColumn { get; set; } // 用于与Excel值连接的DB列
            public string MapTargetIdColumn { get; set; } // 返回的ID列
            public string MapFilter { get; set; } // 额外过滤条件，可为空
        }

        public Form1()
        {
            InitializeComponent();

            // 初始化数据
            excelData = new DataTable();
            dbTableStructure = new Dictionary<string, Dictionary<string, object>>();
            fieldMappings = new Dictionary<string, string>();
            valueMappingConfigs = new Dictionary<string, ValueMappingConfig>(); // added by cqy at 2025-9-9

            // 初始化控件
            InitializeControls();

            // 设置页面标题
            this.Text = "Excel数据导入系统 - SunnyUI";

            // 设置页面大小为屏幕的3/5（作为嵌入页仅用于初始布局）
            Screen screen = Screen.PrimaryScreen;
            int width = (int)(screen.WorkingArea.Width * 0.6);
            int height = (int)(screen.WorkingArea.Height * 0.6);
            this.Size = new Size(width, height);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// 测试数据库连接
        /// </summary>
        private bool TestDatabaseConnection()
        {
            string dbType = uiComboBoxDbType.SelectedItem?.ToString();
            string connectionString = uiTextBoxConnString.Text.Trim();

            if (string.IsNullOrEmpty(dbType) || string.IsNullOrEmpty(connectionString))
            {
                ShowWarningMessage("请选择数据库类型并输入连接字符串！");
                return false;
            }

            try
            {
                // 根据不同的数据库类型创建不同的连接对象
                IDbConnection connection = null;

                switch (dbType)
                {
                    case "MySQL":
                        // 需要添加MySql.Data引用
                        // connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                        break;
                    case "SQL Server":
                        // 需要添加System.Data.SqlClient引用
                        // connection = new System.Data.SqlClient.SqlConnection(connectionString);
                        break;
                    case "PostgreSQL":
                        // 需要添加Npgsql引用
                        // connection = new Npgsql.NpgsqlConnection(connectionString);
                        break;
                    case "Oracle":
                        // Oracle连接示例: DATA SOURCE=192.168.27.134:1521/scm2;PASSWORD=pes#8u2e4;PERSIST SECURITY INFO=True;USER ID=ESCMUSER
                        connection = new OracleConnection(connectionString);
                        break;
                    default:
                        ShowErrorMessage("不支持的数据库类型！");
                        return false;
                }

                // 打开连接
                if (connection != null)
                {
                    connection.Open();
                    connection.Close();
                    ShowSuccessMessage("数据库连接成功！");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"数据库连接失败：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitializeControls()
        {
            // 初始化数据库类型下拉框
            uiComboBoxDbType.Items.Clear();
            foreach (var dbType in dbTypes)
            {
                uiComboBoxDbType.Items.Add(dbType);
            }
            uiComboBoxDbType.SelectedIndex = 3; // 默认选择Oracle

            // 初始化表格
            InitializeDataGridViews();

            // 初始化进度条
            uiProcessBar.Visible = false;
            uiProcessBar.Value = 0;

            // 初始化连接字符串示例
            UpdateConnectionStringExample();
        }

        /// <summary>
        /// 初始化DataGridView控件
        /// </summary>
        private void InitializeDataGridViews()
        {
            // 初始化Excel数据预览表格
            uiDataGridViewExcel.AutoGenerateColumns = false;
            uiDataGridViewExcel.AllowUserToAddRows = false;
            uiDataGridViewExcel.AllowUserToDeleteRows = false;
            uiDataGridViewExcel.ReadOnly = true;

            // 初始化字段映射表格
            uiDataGridViewMapping.AutoGenerateColumns = false;
            uiDataGridViewMapping.AllowUserToAddRows = false;
            uiDataGridViewMapping.AllowUserToDeleteRows = false;

            // 添加字段映射表格的列
            if (uiDataGridViewMapping.Columns.Count == 0)
            {
                uiDataGridViewMapping.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Excel列名",
                    Name = "ExcelColumn",
                    ReadOnly = true,
                    Width = 150
                });

                DataGridViewComboBoxColumn dbColumnCombo = new DataGridViewComboBoxColumn
                {
                    HeaderText = "数据库字段",
                    Name = "DbColumn",
                    Width = 150
                };
                uiDataGridViewMapping.Columns.Add(dbColumnCombo);

                DataGridViewComboBoxColumn dataTypeCombo = new DataGridViewComboBoxColumn
                {
                    HeaderText = "数据类型转换",
                    Name = "DataType",
                    Width = 150
                };
                dataTypeCombo.Items.AddRange(new object[] { "文本", "数字", "日期", "布尔值" });
                uiDataGridViewMapping.Columns.Add(dataTypeCombo);

                uiDataGridViewMapping.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    HeaderText = "必填",
                    Name = "Required",
                    Width = 60
                });

                // 是否需要值映射 - added by cqy at 2025-9-9
                uiDataGridViewMapping.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    HeaderText = "需要映射",
                    Name = "MapNeeded",
                    Width = 80
                });

                // 映射模式：手动/数据库 - added by cqy at 2025-9-9
                DataGridViewComboBoxColumn mapModeCombo = new DataGridViewComboBoxColumn
                {
                    HeaderText = "映射模式",
                    Name = "MapMode",
                    Width = 90
                };
                mapModeCombo.Items.AddRange(new object[] { "手动", "数据库" });
                uiDataGridViewMapping.Columns.Add(mapModeCombo);

                // 手动映射对 - added by cqy at 2025-9-9
                uiDataGridViewMapping.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "手动对(描述=ID;...)",
                    Name = "ManualPairs",
                    Width = 200
                });

                // 数据库映射-表名 - added by cqy at 2025-9-9
                uiDataGridViewMapping.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "映射表",
                    Name = "MapTable",
                    Width = 140
                });

                // 数据库映射-连接字段(与Excel描述匹配的DB列) - added by cqy at 2025-9-9
                uiDataGridViewMapping.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "连接字段",
                    Name = "MapOnDbColumn",
                    Width = 120
                });

                // 数据库映射-返回ID列 - added by cqy at 2025-9-9
                uiDataGridViewMapping.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "ID字段",
                    Name = "MapTargetIdColumn",
                    Width = 100
                });

                // 数据库映射-过滤条件 - added by cqy at 2025-9-9
                uiDataGridViewMapping.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "过滤条件",
                    Name = "MapFilter",
                    Width = 180
                });
            }
        }

        /// <summary>
        /// 选择并加载Excel文件
        /// </summary>
        private void SelectExcelFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel文件|*.xlsx;*.xls;*.csv";
                openFileDialog.Title = "选择Excel文件";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    uiTextBoxExcelPath.Text = filePath;

                    try
                    {
                        LoadExcelFile(filePath);
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage($"加载Excel文件失败: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 加载Excel文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        private void LoadExcelFile(string filePath)
        {
            try
            {
                // 加载Excel或CSV数据 - updated by cqy at 2025-9-9
                string ext = Path.GetExtension(filePath).ToLower();
                DataTable dt = new DataTable();

                if (ext == ".csv")
                {
                    dt = LoadCsv(filePath); // added by cqy at 2025-9-9
                }
                else if (ext == ".xls" || ext == ".xlsx")
                {
                    // dt = LoadExcelByOleDb(filePath); // 原OleDb方式，已弃用，避免环境依赖 - updated by cqy at 2025-9-9
                    dt = LoadExcelByNpoi(filePath); // 使用NPOI读取 - added by cqy at 2025-9-9
                }
                else
                {
                    throw new Exception("仅支持CSV/XLS/XLSX文件");
                }

                excelData = dt;

                // 显示Excel数据
                uiDataGridViewExcel.DataSource = excelData;

                // 更新Excel路径显示
                uiTextBoxExcelPath.Text = filePath;

                // 更新映射表
                UpdateMappingTable();

                ShowSuccessMessage("Excel/CSV文件加载成功！");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"加载Excel文件失败：{ex.Message}");
            }
        }

        // CSV加载 - added by cqy at 2025-9-9
        private DataTable LoadCsv(string filePath)
        {
            DataTable table = new DataTable();
            using (var reader = new StreamReader(filePath, Encoding.UTF8, true))
            {
                string headerLine = reader.ReadLine();
                if (headerLine == null) return table;
                var headers = SplitCsvLine(headerLine);
                foreach (var h in headers)
                {
                    table.Columns.Add(h);
                }
                string line;
                int rowNo = 1;
                if (!table.Columns.Contains("_EXCEL_ROW_NO")) table.Columns.Add("_EXCEL_ROW_NO", typeof(int));
                while ((line = reader.ReadLine()) != null)
                {
                    var cells = SplitCsvLine(line);
                    var row = table.NewRow();
                    for (int i = 0; i < table.Columns.Count && i < cells.Count; i++)
                    {
                        row[i] = cells[i];
                    }
                    row["_EXCEL_ROW_NO"] = rowNo++;
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        // 简单CSV分割，处理引号与逗号 - added by cqy at 2025-9-9
        private List<string> SplitCsvLine(string line)
        {
            var result = new List<string>();
            if (line == null) return result;
            StringBuilder current = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }
            result.Add(current.ToString());
            return result;
        }

        // 通过OleDb读取Excel - added by cqy at 2025-9-9
        private DataTable LoadExcelByOleDb(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLower();
            string connStr;
            if (ext == ".xls")
            {
                connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={filePath};Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
            }
            else
            {
                connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";
            }
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                DataTable sheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (sheets == null || sheets.Rows.Count == 0)
                {
                    throw new Exception("未发现Excel工作表");
                }
                // 默认读取第一个sheet
                string firstSheet = sheets.Rows[0]["TABLE_NAME"].ToString();
                using (OleDbDataAdapter da = new OleDbDataAdapter($"SELECT * FROM [{firstSheet}]", conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        // 使用NPOI读取Excel - added by cqy at 2025-9-9
        private DataTable LoadExcelByNpoi(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IWorkbook workbook;
                string ext = Path.GetExtension(filePath).ToLower();
                if (ext == ".xls")
                {
                    workbook = new HSSFWorkbook(fs);
                }
                else
                {
                    workbook = new XSSFWorkbook(fs);
                }

                // 使用 DataFormatter + FormulaEvaluator 以 Excel 显示的文本读取，避免双精度误差
                DataFormatter formatter = new DataFormatter();
                IFormulaEvaluator evaluator = workbook.GetCreationHelper().CreateFormulaEvaluator();

                ISheet sheet = workbook.NumberOfSheets > 0 ? workbook.GetSheetAt(0) : null;
                if (sheet == null) throw new Exception("未发现Excel工作表");

                DataTable dt = new DataTable();
                IRow headerRow = sheet.GetRow(sheet.FirstRowNum);
                if (headerRow == null) throw new Exception("Excel首行为空");
                int cellCount = headerRow.LastCellNum;
                for (int i = 0; i < cellCount; i++)
                {
                    string colName = GetCellString(headerRow.GetCell(i), formatter, evaluator);
                    if (string.IsNullOrEmpty(colName)) colName = $"列{i + 1}";
                    if (!dt.Columns.Contains(colName))
                    {
                        dt.Columns.Add(colName);
                    }
                    else
                    {
                        dt.Columns.Add(colName + "_" + i);
                    }
                }

                // 添加隐藏列保留原始 Excel 行号，便于后续严格按原序导入
                if (!dt.Columns.Contains("_EXCEL_ROW_NO")) dt.Columns.Add("_EXCEL_ROW_NO", typeof(int));
                for (int r = sheet.FirstRowNum + 1; r <= sheet.LastRowNum; r++)
                {
                    IRow row = sheet.GetRow(r);
                    if (row == null) continue;
                    DataRow dr = dt.NewRow();
                    for (int c = 0; c < cellCount; c++)
                    {
                        dr[c] = GetCellString(row.GetCell(c), formatter, evaluator);
                    }
                    dr["_EXCEL_ROW_NO"] = r; // 原始行号
                    dt.Rows.Add(dr);
                }
                return dt;
            }
        }

        private string GetCellString(ICell cell, DataFormatter formatter, IFormulaEvaluator evaluator)
        {
            if (cell == null) return string.Empty;

            try
            {
                // 日期统一格式化
                if (DateUtil.IsCellDateFormatted(cell))
                {
                    return cell.DateCellValue?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty;
                }

                // 统一使用 DataFormatter 获取与 Excel 显示一致的文本，避免数值精度漂移
                string text = formatter.FormatCellValue(cell, evaluator);

                // 统一布尔大小写
                if (string.Equals(text, "TRUE", StringComparison.OrdinalIgnoreCase)) return "true";
                if (string.Equals(text, "FALSE", StringComparison.OrdinalIgnoreCase)) return "false";

                return text;
            }
            catch
            {
                // 兜底：按原有方式处理
                switch (cell.CellType)
                {
                    case CellType.String:
                        return cell.StringCellValue;
                    case CellType.Numeric:
                        if (DateUtil.IsCellDateFormatted(cell))
                        {
                            return cell.DateCellValue?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty;
                        }
                        return cell.NumericCellValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    case CellType.Boolean:
                        return cell.BooleanCellValue ? "true" : "false";
                    case CellType.Formula:
                        try
                        {
                            if (cell.CachedFormulaResultType == CellType.Numeric)
                            {
                                if (DateUtil.IsCellDateFormatted(cell))
                                {
                                    return cell.DateCellValue?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty;
                                }
                                return cell.NumericCellValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else if (cell.CachedFormulaResultType == CellType.String)
                            {
                                return cell.StringCellValue;
                            }
                            else if (cell.CachedFormulaResultType == CellType.Boolean)
                            {
                                return cell.BooleanCellValue ? "true" : "false";
                            }
                            else
                            {
                                return cell.ToString();
                            }
                        }
                        catch
                        {
                            return cell.ToString();
                        }
                    case CellType.Blank:
                        return string.Empty;
                    default:
                        return cell.ToString();
                }
            }
        }

        /// <summary>
        /// 更新字段映射表格
        /// </summary>
        private void UpdateMappingTable()
        {
            uiDataGridViewMapping.Rows.Clear();

            // 添加Excel列到映射表格
            foreach (DataColumn column in excelData.Columns)
            {
                int rowIndex = uiDataGridViewMapping.Rows.Add();
                uiDataGridViewMapping.Rows[rowIndex].Cells["ExcelColumn"].Value = column.ColumnName;

                // 初始化映射配置默认值 - added by cqy at 2025-9-9
                uiDataGridViewMapping.Rows[rowIndex].Cells["MapNeeded"].Value = false;
                uiDataGridViewMapping.Rows[rowIndex].Cells["MapMode"].Value = "手动";
                uiDataGridViewMapping.Rows[rowIndex].Cells["ManualPairs"].Value = "";
                uiDataGridViewMapping.Rows[rowIndex].Cells["MapTable"].Value = "";
                uiDataGridViewMapping.Rows[rowIndex].Cells["MapOnDbColumn"].Value = "";
                uiDataGridViewMapping.Rows[rowIndex].Cells["MapTargetIdColumn"].Value = "";
                uiDataGridViewMapping.Rows[rowIndex].Cells["MapFilter"].Value = "";
            }
        }

        /// <summary>
        /// 加载数据库表结构
        /// </summary>
        private void LoadDatabaseTableStructure()
        {
            string dbType = uiComboBoxDbType.SelectedItem?.ToString();
            string connectionString = uiTextBoxConnString.Text.Trim();
            string tableName = uiComboBoxTableName.Text.Trim();

            if (string.IsNullOrEmpty(dbType) || string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(tableName))
            {
                ShowWarningMessage("请选择数据库类型、输入连接字符串和表名！");
                return;
            }

            try
            {
                // 清空当前表结构
                dbTableStructure.Clear();

                // 根据不同数据库类型获取表结构
                switch (dbType)
                {
                    case "MySQL":
                        LoadMySqlTableStructure(connectionString, tableName);
                        break;
                    case "SQL Server":
                        LoadSqlServerTableStructure(connectionString, tableName);
                        break;
                    case "PostgreSQL":
                        LoadPostgreSqlTableStructure(connectionString, tableName);
                        break;
                    case "Oracle":
                        LoadOracleTableStructure(connectionString, tableName);
                        break;
                    default:
                        ShowErrorMessage("不支持的数据库类型！");
                        return;
                }

                // 更新映射表的数据库字段下拉框
                UpdateDbColumnComboBox();

                ShowSuccessMessage($"成功加载表 {tableName} 的结构！");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"加载表结构失败：{ex.Message}");
            }
        }

        private void LoadOracleTableStructure(string connectionString, string tableName)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                // 首先获取当前连接的用户名（OWNER）
                string ownerQuery = "SELECT USER FROM DUAL";
                string owner = string.Empty;

                using (OracleCommand ownerCommand = new OracleCommand(ownerQuery, connection))
                {
                    owner = ownerCommand.ExecuteScalar()?.ToString() ?? string.Empty;
                }

                // 获取表结构 - 修复SQL查询 - updated by cqy at 2025-9-8
                string query = @"
                    SELECT 
                        C.COLUMN_NAME, 
                        C.DATA_TYPE, 
                        C.NULLABLE,
                        CASE WHEN PK.COLUMN_NAME IS NOT NULL THEN 'YES' ELSE 'NO' END AS IS_PRIMARY_KEY
                    FROM 
                        ALL_TAB_COLUMNS C
                    LEFT JOIN (
                        SELECT 
                            COL.COLUMN_NAME
                        FROM 
                            ALL_CONS_COLUMNS COL
                        JOIN 
                            ALL_CONSTRAINTS CONS ON COL.CONSTRAINT_NAME = CONS.CONSTRAINT_NAME
                                                AND COL.OWNER = CONS.OWNER
                                                AND COL.TABLE_NAME = CONS.TABLE_NAME
                        WHERE 
                            CONS.TABLE_NAME = :tableName
                            AND CONS.OWNER = :owner
                            AND CONS.CONSTRAINT_TYPE = 'P'
                    ) PK ON C.COLUMN_NAME = PK.COLUMN_NAME
                    WHERE 
                        C.TABLE_NAME = :tableName
                        AND C.OWNER = :owner
                    ORDER BY 
                        C.COLUMN_ID";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("tableName", tableName.ToUpper()));
                    command.Parameters.Add(new OracleParameter("owner", owner.ToUpper()));

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string columnName = reader["COLUMN_NAME"].ToString();
                            string dataType = reader["DATA_TYPE"].ToString();
                            bool isNullable = reader["NULLABLE"].ToString() == "Y";
                            bool isPrimaryKey = reader["IS_PRIMARY_KEY"].ToString() == "YES";

                            // 添加到表结构字典
                            dbTableStructure[columnName] = new Dictionary<string, object>
                            {
                                { "DataType", dataType },
                                { "IsNullable", isNullable },
                                { "IsPrimaryKey", isPrimaryKey }
                            };
                        }
                    }
                }

                connection.Close();
            }
        }

        private void LoadSqlServerTableStructure(string connectionString, string tableName)
        {
            // 实现SQL Server表结构加载逻辑
            // 这里只是示例，实际项目中需要根据需求实现
            ShowInfoMessage("SQL Server表结构加载功能尚未实现");
        }

        private void LoadMySqlTableStructure(string connectionString, string tableName)
        {
            // 实现MySQL表结构加载逻辑
            // 这里只是示例，实际项目中需要根据需求实现
            ShowInfoMessage("MySQL表结构加载功能尚未实现");
        }

        private void LoadPostgreSqlTableStructure(string connectionString, string tableName)
        {
            // 实现PostgreSQL表结构加载逻辑
            // 这里只是示例，实际项目中需要根据需求实现
            ShowInfoMessage("PostgreSQL表结构加载功能尚未实现");
        }

        /// <summary>
        /// 更新数据库字段下拉框
        /// </summary>
        private void UpdateDbColumnComboBox()
        {
            // 获取DataGridView中的数据库字段列
            DataGridViewComboBoxColumn dbColumnCombo = uiDataGridViewMapping.Columns["DbColumn"] as DataGridViewComboBoxColumn;
            if (dbColumnCombo == null) return;

            // 清空原有项
            dbColumnCombo.Items.Clear();

            // 添加空选项
            dbColumnCombo.Items.Add("");

            // 添加数据库表的所有字段
            foreach (var kvp in dbTableStructure)
            {
                dbColumnCombo.Items.Add(kvp.Key);
            }
        }

        /// <summary>
        /// 保存字段映射
        /// </summary>
        private void SaveFieldMappings()
        {
            fieldMappings.Clear();
            valueMappingConfigs.Clear(); // added by cqy at 2025-9-9

            foreach (DataGridViewRow row in uiDataGridViewMapping.Rows)
            {
                string excelColumn = row.Cells["ExcelColumn"].Value?.ToString();
                string dbColumn = row.Cells["DbColumn"].Value?.ToString();

                if (!string.IsNullOrEmpty(excelColumn) && !string.IsNullOrEmpty(dbColumn))
                {
                    fieldMappings[excelColumn] = dbColumn;
                }

                // 读取映射配置 - added by cqy at 2025-9-9
                bool mapNeeded = false;
                bool.TryParse(Convert.ToString(row.Cells["MapNeeded"].Value), out mapNeeded);
                string mapMode = row.Cells["MapMode"].Value?.ToString() ?? "手动";
                string manualPairs = row.Cells["ManualPairs"].Value?.ToString() ?? string.Empty;
                string mapTable = row.Cells["MapTable"].Value?.ToString() ?? string.Empty;
                string mapOn = row.Cells["MapOnDbColumn"].Value?.ToString() ?? string.Empty;
                string mapId = row.Cells["MapTargetIdColumn"].Value?.ToString() ?? string.Empty;
                string mapFilter = row.Cells["MapFilter"].Value?.ToString() ?? string.Empty;

                if (!string.IsNullOrEmpty(excelColumn))
                {
                    valueMappingConfigs[excelColumn] = new ValueMappingConfig
                    {
                        MapNeeded = mapNeeded,
                        MapMode = mapMode,
                        ManualPairs = manualPairs,
                        MapTable = mapTable,
                        MapOnDbColumn = mapOn,
                        MapTargetIdColumn = mapId,
                        MapFilter = mapFilter
                    };
                }
            }

            if (fieldMappings.Count > 0)
            {
                ShowSuccessMessage($"成功保存{fieldMappings.Count}个字段映射！");
            }
            else
            {
                ShowWarningMessage("未设置任何字段映射！");
            }
        }

        // 执行值映射：将描述转换为ID - added by cqy at 2025-9-9
        private string MapValueIfNeeded(string excelColumn, object rawValue, string dbType, string connectionString)
        {
            if (rawValue == null || rawValue == DBNull.Value) return null;
            string source = rawValue.ToString();
            if (string.IsNullOrEmpty(source)) return null;

            if (!valueMappingConfigs.ContainsKey(excelColumn)) return source;
            var cfg = valueMappingConfigs[excelColumn];
            if (!cfg.MapNeeded) return source;

            if (cfg.MapMode == "手动")
            {
                // 解析格式：描述=ID;描述2=ID2
                var pairs = (cfg.ManualPairs ?? string.Empty).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in pairs)
                {
                    var kv = p.Split(new[] { '=' }, 2);
                    if (kv.Length == 2)
                    {
                        if (kv[0].Trim() == source)
                        {
                            return kv[1].Trim();
                        }
                    }
                }
                return source; // 找不到则回退原值
            }
            else if (cfg.MapMode == "数据库")
            {
                if (string.IsNullOrEmpty(cfg.MapTable) || string.IsNullOrEmpty(cfg.MapOnDbColumn) || string.IsNullOrEmpty(cfg.MapTargetIdColumn))
                {
                    return source;
                }

                // 仅实现Oracle查询，其他类型预留 - added by cqy at 2025-9-9
                if (dbType == "Oracle")
                {
                    string sql = $"SELECT {cfg.MapTargetIdColumn} FROM {cfg.MapTable} WHERE {cfg.MapOnDbColumn} = '{source.Replace("'", "''")}'";
                    if (!string.IsNullOrEmpty(cfg.MapFilter))
                    {
                        sql += $" AND ({cfg.MapFilter})";
                    }
                    using (OracleConnection conn = new OracleConnection(connectionString))
                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            return result.ToString();
                        }
                    }
                }
                // 其他数据库类型未实现
                return source;
            }
            return source;
        }

        /// <summary>
        /// 导入数据到数据库
        /// </summary>
        private void ImportDataToDatabase()
        {
            if (excelData == null || excelData.Rows.Count == 0)
            {
                ShowWarningMessage("没有Excel数据可导入！");
                return;
            }

            // 保存当前的字段映射
            SaveFieldMappings();

            if (fieldMappings.Count == 0)
            {
                ShowWarningMessage("请先完成字段映射！");
                return;
            }

            string dbType = uiComboBoxDbType.SelectedItem?.ToString();
            string connectionString = uiTextBoxConnString.Text.Trim();
            string tableName = uiComboBoxTableName.Text.Trim();

            if (string.IsNullOrEmpty(dbType) || string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(tableName))
            {
                ShowWarningMessage("请选择数据库类型、输入连接字符串和表名！");
                return;
            }

            try
            {
                // 显示进度条
                uiProcessBar.Visible = true;
                uiProcessBar.Value = 0;

                // 设置进度条最大值
                int totalRows = excelData.Rows.Count;
                if (totalRows > 0)
                {
                    // 使用反射设置Maximum属性
                    var property = uiProcessBar.GetType().GetProperty("Maximum");
                    if (property != null)
                    {
                        property.SetValue(uiProcessBar, totalRows);
                    }
                }

                int successCount = 0;
                int failCount = 0;
                List<string> failedRows = new List<string>();

                // 根据不同数据库类型执行导入
                switch (dbType)
                {
                    case "MySQL":
                        // 实现MySQL导入逻辑
                        ShowInfoMessage("MySQL导入功能尚未实现");
                        break;
                    case "SQL Server":
                        // 实现SQL Server导入逻辑
                        ShowInfoMessage("SQL Server导入功能尚未实现");
                        break;
                    case "PostgreSQL":
                        // 实现PostgreSQL导入逻辑
                        ShowInfoMessage("PostgreSQL导入功能尚未实现");
                        break;
                    case "Oracle":
                        // 执行Oracle导入
                        ImportToOracle(connectionString, tableName, ref successCount, ref failCount, failedRows);
                        break;
                    default:
                        ShowErrorMessage("不支持的数据库类型！");
                        uiProcessBar.Visible = false;
                        return;
                }

                // 隐藏进度条
                uiProcessBar.Visible = false;

                // 显示结果
                string resultMessage = $"导入完成！\r\n成功: {successCount} 行\r\n失败: {failCount} 行";

                if (failCount > 0)
                {
                    resultMessage += "\r\r失败详情:\r" + string.Join("\r\n", failedRows);
                    ShowWarningMessage(resultMessage);
                }
                else
                {
                    ShowSuccessMessage(resultMessage);
                }

                // added by cqy at 2025-9-9: 简易调试输出
                Console.WriteLine("ImportDone-" + new Random().Next(100000, 999999));
            }
            catch (Exception ex)
            {
                uiProcessBar.Visible = false;
                ShowErrorMessage($"导入过程中发生错误: {ex.Message}");
            }
        }

        private void ImportToOracle(string connectionString, string tableName, ref int successCount, ref int failCount, List<string> failedRows)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                // 逐行导入数据 - updated by cqy at 2025-9-9
                var __rowsOrderedA = excelData.AsEnumerable();
                if (excelData.Columns.Contains("_EXCEL_ROW_NO"))
                {
                    __rowsOrderedA = __rowsOrderedA.OrderBy(r =>
                    {
                        int n; return int.TryParse(Convert.ToString(r["_EXCEL_ROW_NO"]), out n) ? n : int.MaxValue;
                    });
                }
                int i = 0;
                foreach (var __row in __rowsOrderedA)
                {
                    try
                    {
                        // 使用字符串拼接SQL而不是StringBuilder - updated by cqy at 2025-9-9
                        string columns = "";
                        string values = "";
                        bool hasIdField = false;
                        string idFieldName = "";

                        // 第一次遍历，检查是否有ID字段及其值
                        foreach (var mapping in fieldMappings)
                        {
                            if (!string.IsNullOrEmpty(mapping.Value))
                            {
                                string dbColumn = mapping.Value;

                                if (dbColumn.ToUpper() == "ID" || dbColumn.EndsWith("_ID", StringComparison.OrdinalIgnoreCase))
                                {
                                    idFieldName = dbColumn;
                                    string excelColumn = mapping.Key;
                                    object value = excelData.Rows[i][excelColumn];

                                    if (value != null && value != DBNull.Value && !string.IsNullOrEmpty(value.ToString()))
                                    {
                                        hasIdField = true;
                                    }
                                    break;
                                }
                            }
                        }

                        // 如果没有ID字段或ID字段为空，需要添加ID列
                        if (!hasIdField && !string.IsNullOrEmpty(idFieldName))
                        {
                            if (columns.Length > 0)
                            {
                                columns = idFieldName + ", " + columns;
                                values = $"'{Guid.NewGuid().ToString()}', " + values;
                            }
                            else
                            {
                                columns = idFieldName;
                                values = $"'{Guid.NewGuid().ToString()}'";
                            }
                        }

                        foreach (var mapping in fieldMappings)
                        {
                            if (!string.IsNullOrEmpty(mapping.Value))
                            {
                                string dbColumn = mapping.Value;
                                // 如果是ID字段且已经处理过，则跳过
                                if ((dbColumn.ToUpper() == "ID" || dbColumn.EndsWith("_ID", StringComparison.OrdinalIgnoreCase)) && !hasIdField)
                                {
                                    continue;
                                }

                                if (columns.Length > 0)
                                {
                                    columns += ", ";
                                    values += ", ";
                                }
                                columns += dbColumn;

                                string excelColumn = mapping.Key;

                                // 获取Excel中的值
                                object rawValue = __row[excelColumn];

                                // 先进行值映射：描述 -> ID 或 原值 - added by cqy at 2025-9-9
                                string mapped = MapValueIfNeeded(excelColumn, rawValue, uiComboBoxDbType.SelectedItem?.ToString(), connectionString);
                                object value = string.IsNullOrEmpty(mapped) ? rawValue : (object)mapped;

                                // 处理空值
                                if (value == null || value == DBNull.Value || string.IsNullOrEmpty(value.ToString()))
                                {
                                    values += "NULL";
                                    continue;
                                }

                                // 根据数据库字段类型进行转换
                                if (dbTableStructure.ContainsKey(dbColumn))
                                {
                                    string dataType = dbTableStructure[dbColumn]["DataType"].ToString().ToLower();
                                    bool isPrimaryKey = (bool)dbTableStructure[dbColumn]["IsPrimaryKey"];

                                    // 根据数据类型进行转换
                                    if (dataType.Contains("number") || dataType.Contains("float") || dataType.Contains("decimal"))
                                    {
                                        decimal decValue;
                                        if (decimal.TryParse(value.ToString(), out decValue))
                                        {
                                            values += decValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                                        }
                                        else
                                        {
                                            values += "NULL";
                                        }
                                    }
                                    else if (dataType.Contains("date") || dataType.Contains("timestamp"))
                                    {
                                        DateTime dateValue;
                                        if (DateTime.TryParse(value.ToString(), out dateValue))
                                        {
                                            values += $"TO_DATE('{dateValue:yyyy-MM-dd HH:mm:ss}', 'YYYY-MM-DD HH24:MI:SS')";
                                        }
                                        else
                                        {
                                            values += "NULL";
                                        }
                                    }
                                    else if (dataType.Contains("raw") && isPrimaryKey) // 处理GUID/RAW类型的主键
                                    {
                                        // 对于Oracle中的RAW(16)类型，通常用于存储GUID
                                        Guid guidValue;
                                        if (Guid.TryParse(value.ToString(), out guidValue))
                                        {
                                            // 将GUID转换为十六进制字符串
                                            string hexString = BitConverter.ToString(guidValue.ToByteArray()).Replace("-", "");
                                            values += $"HEXTORAW('{hexString}')";
                                        }
                                        else
                                        {
                                            // 如果不是有效的GUID，生成一个新的
                                            string hexString = BitConverter.ToString(Guid.NewGuid().ToByteArray()).Replace("-", "");
                                            values += $"HEXTORAW('{hexString}')";
                                        }
                                    }
                                    else if (isPrimaryKey && (dbColumn.ToUpper() == "ID" || dbColumn.EndsWith("_ID", StringComparison.OrdinalIgnoreCase)))
                                    {
                                        // 对于主键ID字段，根据数据类型处理 - updated by cqy at 2025-9-9
                                        if (dataType.Contains("number"))
                                        {
                                            // 如果是数字类型的ID，可能是自增的，使用序列
                                            values += $"{tableName}_SEQ.NEXTVAL";
                                        }
                                        else if (dataType.Contains("varchar") || dataType.Contains("char"))
                                        {
                                            // 如果是字符串类型的ID，使用GUID字符串
                                            values += $"'{Guid.NewGuid().ToString()}'";
                                        }
                                        else
                                        {
                                            // 其他类型的ID，按字符串处理
                                            values += $"'{value.ToString().Replace("'", "''")}'";
                                        }
                                    }
                                    else
                                    {
                                        // 默认作为字符串处理
                                        values += $"'{value.ToString().Replace("'", "''")}'";
                                    }
                                }
                                else
                                {
                                    // 如果没有找到字段类型信息，默认作为字符串处理
                                    values += $"'{value.ToString().Replace("'", "''")}'";
                                }
                            }
                        }

                        // 使用字符串插值构建SQL - added by cqy at 2025-9-9
                        string insertSql = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

                        // 执行插入
                        using (OracleCommand command = new OracleCommand(insertSql, connection))
                        {
                            command.ExecuteNonQuery();
                            successCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        failedRows.Add($"第{i + 1}行: {ex.Message}");
                    }

                    // 更新进度条
                    uiProcessBar.Value = i + 1;
                    Application.DoEvents();
                    i++;
                }
            }
        }

        /// <summary>
        /// 显示成功消息
        /// </summary>
        private void ShowSuccessMessage(string message)
        {
            UIMessageTip.ShowOk(message);
        }

        /// <summary>
        /// 显示错误消息
        /// </summary>
        private void ShowErrorMessage(string message)
        {
            UIMessageTip.ShowError(message);
        }

        /// <summary>
        /// 显示警告消息
        /// </summary>
        private void ShowWarningMessage(string message)
        {
            UIMessageTip.ShowWarning(message);
        }

        /// <summary>
        /// 显示信息消息
        /// </summary>
        private void ShowInfoMessage(string message)
        {
            UIMessageTip.Show(message);
        }

        private void UpdateConnectionStringExample()
        {
            string dbType = uiComboBoxDbType.SelectedItem?.ToString();
            string example = string.Empty;

            switch (dbType)
            {
                case "MySQL":
                    example = "Server=localhost;Database=mydb;Uid=root;Pwd=password;";
                    break;
                case "SQL Server":
                    example = "Data Source=localhost;Initial Catalog=mydb;Integrated Security=True;";
                    break;
                case "PostgreSQL":
                    example = "Host=localhost;Database=mydb;Username=postgres;Password=password;";
                    break;
                case "Oracle":
                    example = "DATA SOURCE=192.168.27.134:1521/scm2;PASSWORD=pes#8u2e4;PERSIST SECURITY INFO=True;USER ID=ESCMUSER";
                    break;
                default:
                    example = "请选择数据库类型";
                    break;
            }

            uiLabelConnStringExample.Text = $"示例: {example}";

            // 如果文本框为空，则自动填入示例
            if (string.IsNullOrEmpty(uiTextBoxConnString.Text))
            {
                uiTextBoxConnString.Text = example;
            }
        }

        #region 事件处理方法

        /// <summary>
        /// 数据库连接测试按钮点击事件
        /// </summary>
        private void uiButtonTestConnection_Click(object sender, EventArgs e)
        {
            TestDatabaseConnection();
        }

        /// <summary>
        /// 数据库类型选择变更事件
        /// </summary>
        private void uiComboBoxDbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateConnectionStringExample();
        }

        /// <summary>
        /// 显示/隐藏密码按钮点击事件
        /// </summary>
        private void uiButtonTogglePassword_Click(object sender, EventArgs e)
        {
            if (uiTextBoxConnString.PasswordChar == '*')
            {
                uiTextBoxConnString.PasswordChar = '\0'; // 显示密码
                uiButtonTogglePassword.Text = "隐藏密码";
            }
            else
            {
                uiTextBoxConnString.PasswordChar = '*'; // 隐藏密码
                uiButtonTogglePassword.Text = "显示密码";
            }
        }

        /// <summary>
        /// 加载表结构按钮点击事件
        /// </summary>
        private void uiButtonLoadTable_Click(object sender, EventArgs e)
        {
            LoadDatabaseTableStructure();
        }

        /// <summary>
        /// 选择Excel文件按钮点击事件
        /// </summary>
        private void uiButtonSelectExcel_Click(object sender, EventArgs e)
        {
            SelectExcelFile();
        }

        /// <summary>
        /// 保存映射按钮点击事件
        /// </summary>
        private void uiButtonSaveMapping_Click(object sender, EventArgs e)
        {
            SaveFieldMappings();
        }

        /// <summary>
        /// 导入数据按钮点击事件
        /// </summary>
        private void uiButtonImport_Click(object sender, EventArgs e)
        {
            ImportDataToDatabase();
        }

        /// <summary>
        /// 字段映射表格单元格值变更事件
        /// </summary>
        private void uiDataGridViewMapping_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // 当选择了数据库字段时，自动设置数据类型和是否必填
            if (e.RowIndex >= 0 && e.ColumnIndex == uiDataGridViewMapping.Columns["DbColumn"].Index)
            {
                string dbColumnName = uiDataGridViewMapping.Rows[e.RowIndex].Cells["DbColumn"].Value?.ToString();

                if (!string.IsNullOrEmpty(dbColumnName) && dbTableStructure != null && dbTableStructure.ContainsKey(dbColumnName))
                {
                    var columnInfo = dbTableStructure[dbColumnName];

                    // 设置数据类型
                    string dataType = columnInfo["DataType"].ToString().ToLower();
                    string uiDataType = "文本";

                    if (dataType.Contains("int") || dataType.Contains("number") || dataType.Contains("decimal") || dataType.Contains("float") || dataType.Contains("double"))
                    {
                        uiDataType = "数字";
                    }
                    else if (dataType.Contains("date") || dataType.Contains("time"))
                    {
                        uiDataType = "日期";
                    }
                    else if (dataType.Contains("bit") || dataType.Contains("bool"))
                    {
                        uiDataType = "布尔值";
                    }

                    uiDataGridViewMapping.Rows[e.RowIndex].Cells["DataType"].Value = uiDataType;

                    // 设置是否必填
                    bool isRequired = !(bool)columnInfo["IsNullable"];
                    uiDataGridViewMapping.Rows[e.RowIndex].Cells["Required"].Value = isRequired;
                }
            }

            // 自动控制MapNeeded - added by cqy at 2025-9-9
            if (e.RowIndex >= 0)
            {
                var row = uiDataGridViewMapping.Rows[e.RowIndex];
                bool hasManual = !string.IsNullOrEmpty(Convert.ToString(row.Cells["ManualPairs"].Value));
                bool hasDb = !string.IsNullOrEmpty(Convert.ToString(row.Cells["MapTable"].Value))
                            && !string.IsNullOrEmpty(Convert.ToString(row.Cells["MapOnDbColumn"].Value))
                            && !string.IsNullOrEmpty(Convert.ToString(row.Cells["MapTargetIdColumn"].Value));
                if (hasManual || hasDb)
                {
                    row.Cells["MapNeeded"].Value = true;
                }
            }
        }

        /// <summary>
        /// 保存配置按钮点击事件
        /// </summary>
        private void uiButtonSaveConfig_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "配置文件|*.json";
                saveFileDialog.Title = "保存导入配置";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 创建配置对象
                        var config = new
                        {
                            DbType = uiComboBoxDbType.SelectedItem?.ToString(),
                            ConnectionString = uiTextBoxConnString.Text,
                            TableName = uiComboBoxTableName.Text,
                            Mappings = fieldMappings,
                            ValueMappings = valueMappingConfigs // added by cqy at 2025-9-9
                        };

                        // 序列化为JSON
                        string json = JsonConvert.SerializeObject(config, Formatting.Indented);

                        // 写入文件
                        File.WriteAllText(saveFileDialog.FileName, json);

                        ShowSuccessMessage("配置保存成功！");
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage($"保存配置失败: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 加载配置按钮点击事件
        /// </summary>
        private void uiButtonLoadConfig_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "配置文件|*.json";
                openFileDialog.Title = "加载导入配置";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 读取JSON文件
                        string json = File.ReadAllText(openFileDialog.FileName);

                        // 反序列化
                        JObject config = JObject.Parse(json);

                        // 设置控件值
                        string dbType = config["DbType"].ToString();
                        uiComboBoxDbType.SelectedItem = dbType;

                        uiTextBoxConnString.Text = config["ConnectionString"].ToString();
                        uiComboBoxTableName.Text = config["TableName"].ToString();

                        // 加载字段映射
                        var mappings = config["Mappings"] as JObject;
                        fieldMappings.Clear();

                        if (mappings != null)
                        {
                            foreach (var property in mappings.Properties())
                            {
                                fieldMappings[property.Name] = property.Value.ToString();
                            }
                        }

                        // 加载值映射配置 - added by cqy at 2025-9-9
                        valueMappingConfigs.Clear();
                        var valueMaps = config["ValueMappings"] as JObject;
                        if (valueMaps != null)
                        {
                            foreach (var prop in valueMaps.Properties())
                            {
                                var obj = prop.Value as JObject;
                                if (obj != null)
                                {
                                    valueMappingConfigs[prop.Name] = new ValueMappingConfig
                                    {
                                        MapNeeded = obj["MapNeeded"]?.ToObject<bool>() ?? false,
                                        MapMode = obj["MapMode"]?.ToString(),
                                        ManualPairs = obj["ManualPairs"]?.ToString(),
                                        MapTable = obj["MapTable"]?.ToString(),
                                        MapOnDbColumn = obj["MapOnDbColumn"]?.ToString(),
                                        MapTargetIdColumn = obj["MapTargetIdColumn"]?.ToString(),
                                        MapFilter = obj["MapFilter"]?.ToString()
                                    };
                                }
                            }
                        }

                        ShowSuccessMessage("配置加载成功！");
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage($"加载配置失败: {ex.Message}");
                    }
                }
            }
        }

        #endregion

        private void btnCustomerInput_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel文件|*.xlsx;*.xls;*.csv";
                openFileDialog.Title = "选择Excel文件";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    uiTextBoxExcelPath.Text = filePath;

                    try
                    {
                        // 加载Excel或CSV数据 - updated by cqy at 2025-9-9
                        string ext = Path.GetExtension(filePath).ToLower();
                        DataTable dt = new DataTable();

                        if (ext == ".csv")
                        {
                            dt = LoadCsv(filePath); // added by cqy at 2025-9-9
                        }
                        else if (ext == ".xls" || ext == ".xlsx")
                        {
                            // dt = LoadExcelByOleDb(filePath); // 原OleDb方式，已弃用，避免环境依赖 - updated by cqy at 2025-9-9
                            dt = LoadExcelByNpoi(filePath); // 使用NPOI读取 - added by cqy at 2025-9-9
                        }
                        else
                        {
                            throw new Exception("仅支持CSV/XLS/XLSX文件");
                        }

                        // 将数据绑定到预览表格
                        excelData = dt;

                        // 分隔符分隔符 - 访问页面静态方法获取选项 - 
                        string sleeveJson = CallPageMethod("GetSleeveOptions", null);
                        string fabricPatternJson = CallPageMethod("GetFabricPatternOptions", null);
                        string styleDescJson = CallPageMethod("GetStyleDescOptions", null);
                        string featureJson = CallPageMethod("GetFeatureOptions", new { garmentType = "", productType = "" });

                        // 字符串转变量 - updated by cqy at 2025-9-9
                        var sleeveArr = ExtractArrayFromWebMethodResponse(sleeveJson);
                        var fabricPatternArr = ExtractArrayFromWebMethodResponse(fabricPatternJson);
                        var styleDescArr = ExtractArrayFromWebMethodResponse(styleDescJson);
                        var featureArr = ExtractArrayFromWebMethodResponse(featureJson);

                        //excelData里面的STYLE_DESC	Sleeve	Feature	Fabric Pattern是描述，数据表里是存id(_CD之类的)，需要将描述转换为id
                        // 遍历excelData，将STYLE_DESC、Sleeve、Feature、Fabric Pattern的描述转换为id（倒序遍历，删除不跳行，保持原始顺序）
                        for (int i = excelData.Rows.Count - 1; i >= 0; i--)
                        {
                            // 获取STYLE_DESC、Sleeve、Feature、Fabric Pattern的描述
                            string styleDesc = excelData.Rows[i]["STYLE_DESC"].ToString();
                            string sleeve = excelData.Rows[i]["Sleeve"].ToString();
                            string feature = excelData.Rows[i]["Feature"].ToString();
                            string fabricPattern = excelData.Rows[i]["Fabric Pattern"].ToString();
                            string garment_type_cd = excelData.Rows[i]["GARMENT_TYPE_CD"].ToString();

                            if (string.IsNullOrEmpty(garment_type_cd) || string.IsNullOrEmpty(styleDesc))
                            {
                                //excelData去掉这一行
                                excelData.Rows.RemoveAt(i);
                                continue;
                            }

                            // 将描述转换为id
                            string styleDescId = FindIdByDescriptionGeneric(styleDescArr, styleDesc);
                            string sleeveId = FindIdByDescriptionGeneric(sleeveArr, sleeve);
                            string featureId = FindIdByDescriptionGeneric(featureArr, feature);
                            string fabricPatternId = FindIdByDescriptionGeneric(fabricPatternArr, fabricPattern);

                            // 将id赋值给excelData
                            excelData.Rows[i]["STYLE_DESC"] = styleDescId;
                            excelData.Rows[i]["Sleeve"] = sleeveId;
                            //excelData.Rows[i]["Feature"] = featureId;
                            excelData.Rows[i]["Fabric Pattern"] = fabricPatternId;
                        }

                        //将excelData导入数据表SampleMasterYYLibrary
                        ImportDataToOracle(uiTextBoxConnString.Text, "GEN_SAMPLE_MASTER_YY");

                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage($"加载Excel文件失败: {ex.Message}");
                    }
                }
            }
        }


        // 将数据导入数据表 - updated by cqy at 2025-9-9
        private void ImportDataToOracle(string connectionString, string tableName)
        {
            if (excelData == null || excelData.Rows.Count == 0)
            {
                ShowWarningMessage("Excel没有可导入的数据！");
                return;
            }

            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(tableName))
            {
                ShowWarningMessage("连接字符串或表名为空！");
                return;
            }

            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                OracleTransaction tran = conn.BeginTransaction();
                try
                {
                    // 获取当前用户与目标表OWNER
                    string owner = string.Empty;
                    using (var cmd = new OracleCommand("SELECT USER FROM DUAL", conn))
                    {
                        cmd.Transaction = tran;
                        owner = Convert.ToString(cmd.ExecuteScalar());
                    }
                    owner = string.IsNullOrEmpty(owner) ? "ESCMUSER" : owner.ToUpper();
                    string upperTable = tableName.ToUpper();

                    // 解析同义词/跨库，获取真实表OWNER
                    string tableOwner = owner;
                    using (var cmd = new OracleCommand("SELECT OWNER FROM ALL_TAB_COLUMNS WHERE TABLE_NAME = :tn GROUP BY OWNER", conn))
                    {
                        cmd.Transaction = tran;
                        cmd.Parameters.Add(new OracleParameter("tn", upperTable));
                        var o = cmd.ExecuteScalar();
                        if (o != null) tableOwner = Convert.ToString(o).ToUpper();
                    }

                    // 自动判断ID策略
                    string idDataType = string.Empty;
                    using (var cmd = new OracleCommand("SELECT DATA_TYPE FROM ALL_TAB_COLUMNS WHERE OWNER = :own AND TABLE_NAME = :tn AND COLUMN_NAME = 'ID'", conn))
                    {
                        cmd.Transaction = tran;
                        cmd.Parameters.Add(new OracleParameter("own", tableOwner));
                        cmd.Parameters.Add(new OracleParameter("tn", upperTable));
                        var o = cmd.ExecuteScalar();
                        idDataType = o == null ? string.Empty : Convert.ToString(o).ToUpper();
                    }

                    // 触发器检测（是否自动给ID赋值） - updated by cqy at 2025-9-9: 避免对LONG列TRIGGER_BODY做LIKE导致 ORA-00932
                    int triggerCount = 0;
                    using (var cmd = new OracleCommand("SELECT COUNT(1) FROM ALL_TRIGGERS WHERE OWNER = :own AND TABLE_NAME = :tn AND STATUS='ENABLED' AND TRIGGERING_EVENT LIKE '%INSERT%'", conn))
                    {
                        cmd.Transaction = tran;
                        cmd.Parameters.Add(new OracleParameter("own", tableOwner));
                        cmd.Parameters.Add(new OracleParameter("tn", upperTable));
                        triggerCount = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // 序列检测
                    string seqName = $"{upperTable}_SEQ";
                    int seqCount = 0;
                    using (var cmd = new OracleCommand("SELECT COUNT(1) FROM ALL_SEQUENCES WHERE SEQUENCE_OWNER = :own AND SEQUENCE_NAME = :seq", conn))
                    {
                        cmd.Transaction = tran;
                        cmd.Parameters.Add(new OracleParameter("own", tableOwner));
                        cmd.Parameters.Add(new OracleParameter("seq", seqName));
                        seqCount = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // ID处理策略
                    // 优先级：触发器 > 序列(NUMBER) > RAW GUID > CHAR GUID > NUMBER默认序列 > 其他GUID
                    // none: 不插入ID列（依赖触发器/标识列）；seq: 使用表名_SEQ.NEXTVAL；guid: 使用GUID字符串；rawGuid: 使用RAW(16) HEXTORAW(GUID)
                    string idStrategy = "none";
                    if (triggerCount > 0)
                    {
                        idStrategy = "none";
                    }
                    else if (idDataType.Contains("NUMBER") && seqCount > 0)
                    {
                        idStrategy = "seq";
                    }
                    else if (idDataType.Contains("RAW"))
                    {
                        idStrategy = "rawGuid";
                    }
                    else if (idDataType.Contains("CHAR")) // VARCHAR2/NVARCHAR2
                    {
                        idStrategy = "guid";
                    }
                    else if (idDataType.Contains("NUMBER"))
                    {
                        idStrategy = "seq";
                    }
                    else
                    {
                        idStrategy = "guid";
                    }

                    // 列映射：Excel列 -> 目标表字段
                    Func<string, string[], string> findExcelCol = (fallback, candidates) =>
                    {
                        // 优先精确匹配
                        foreach (var c in candidates)
                        {
                            if (!string.IsNullOrEmpty(c) && excelData.Columns.Contains(c)) return c;
                        }
                        // 模糊包含匹配（去除空格与下划线）
                        foreach (var c in candidates)
                        {
                            if (string.IsNullOrEmpty(c)) continue;
                            string norm = c.Replace(" ", string.Empty).Replace("_", string.Empty).ToLower();
                            foreach (DataColumn col in excelData.Columns)
                            {
                                string cur = col.ColumnName.Replace(" ", string.Empty).Replace("_", string.Empty).ToLower();
                                if (cur.Contains(norm)) return col.ColumnName;
                            }
                        }
                        return fallback;
                    };

                    // 预定义映射候选
                    var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "GARMENT_TYPE_CD",    findExcelCol(null, new []{ "GARMENT_TYPE_CD", "GARMENT_TYPE" }) },
                        { "PRODUCT_GENDER_CD", findExcelCol(null, new []{ "PRODUCT_GENDER_CD", "STYLE_DESC", "GENDER_CD" }) },
                        { "PATTERN_TYPE_CD",   findExcelCol(null, new []{ "PATTERN_TYPE_CD", "Fabric Pattern" }) },
                        { "WIDTH",             findExcelCol(null, new []{ "WIDTH", "Fabric Width" }) },
                        { "SLEEVE_CD",         findExcelCol(null, new []{ "SLEEVE_CD", "Sleeve" }) },
                        { "FEATURE_DESC",      findExcelCol(null, new []{ "FEATURE_DESC", "Feature" }) },
                        { "MASTER_YY",         findExcelCol(null, new []{ "MASTER_YY", "Master YY" }) },
                        { "IS_BASIC",          findExcelCol(null, new []{ "IS_BASIC", "Remark" }) }
                    };

                    // 过滤掉Excel中不存在的映射项
                    var finalMap = map.Where(kv => !string.IsNullOrEmpty(kv.Value) && excelData.Columns.Contains(kv.Value))
                                      .ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase);

                    // 事务内批量导入
                    int success = 0;
                    int failed = 0;
                    List<string> errors = new List<string>();

                    var __rowsOrderedB = excelData.AsEnumerable();
                    if (excelData.Columns.Contains("_EXCEL_ROW_NO"))
                    {
                        __rowsOrderedB = __rowsOrderedB.OrderBy(r =>
                        {
                            int n; return int.TryParse(Convert.ToString(r["_EXCEL_ROW_NO"]), out n) ? n : int.MaxValue;
                        });
                    }
                    int i = 0;
                    foreach (var row in __rowsOrderedB)
                    {
                        try
                        {
                            string columns = string.Empty;
                            string values = string.Empty;

                            // 处理ID
                            if (idStrategy == "seq")
                            {
                                columns = "ID";
                                values = $"{seqName}.NEXTVAL";
                            }
                            else if (idStrategy == "guid")
                            {
                                columns = "ID";
                                values = $"'{Guid.NewGuid().ToString()}'";
                            }
                            else if (idStrategy == "rawGuid")
                            {
                                string hex = BitConverter.ToString(Guid.NewGuid().ToByteArray()).Replace("-", "");
                                columns = "ID";
                                values = $"HEXTORAW('{hex}')";
                            }
                            // idStrategy == none 则不写入ID列

                            // 常规列
                            Action<string, object> addKV = (col, val) =>
                            {
                                if (columns.Length > 0)
                                {
                                    columns += ", ";
                                    values += ", ";
                                }
                                columns += col;
                                if (val == null || val == DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(val)))
                                {
                                    values += "NULL";
                                }
                                else if (col.Equals("MASTER_YY", StringComparison.OrdinalIgnoreCase) || col.Equals("ALLOWANCE", StringComparison.OrdinalIgnoreCase))
                                {
                                    decimal d;
                                    if (decimal.TryParse(Convert.ToString(val), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out d))
                                    {
                                        values += d.ToString(System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        values += "NULL";
                                    }
                                }
                                else if (col.Equals("IS_BASIC", StringComparison.OrdinalIgnoreCase))
                                {
                                    var s = Convert.ToString(val)?.Trim();
                                    if (!string.IsNullOrEmpty(s))
                                    {
                                        if (string.Equals(s, "basic", StringComparison.OrdinalIgnoreCase) || s == "Y" || s == "1" || s == "是")
                                            values += "'Y'";
                                        else
                                            values += "'N'";
                                    }
                                    else
                                    {
                                        values += "NULL";
                                    }
                                }
                                /* 合并至上方 MASTER_YY 分支，避免重复 */
                                else
                                {
                                    values += $"'{Convert.ToString(val).Replace("'", "''")}'";
                                }
                            };

                            foreach (var kv in finalMap)
                            {
                                string targetCol = kv.Key;
                                string excelCol = kv.Value;
                                object val = row[excelCol];

                                // 固定 MASTER_YY 保留小数位的显示值（若为文本），不改自然数值
                                if (targetCol.Equals("MASTER_YY", StringComparison.OrdinalIgnoreCase))
                                {
                                    decimal d;
                                    if (decimal.TryParse(Convert.ToString(val), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out d))
                                    {
                                        val = d.ToString(System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                }

                                //特殊处理WIDTH字段
                                if (targetCol.Equals("WIDTH", StringComparison.OrdinalIgnoreCase))
                                {
                                    //如果row["Fabric Width"]，row["列7"]不空 - updated by cqy at 2025-9-9
                                    if (row["Fabric Width"] != null && row["Fabric Width"] != DBNull.Value && !string.IsNullOrEmpty(row["Fabric Width"].ToString())
                                        && row["列7"] != null && row["列7"] != DBNull.Value && !string.IsNullOrEmpty(row["列7"].ToString()))
                                    {
                                        val = row["Fabric Width"].ToString().Replace("cm", "") + "-" + row["列7"].ToString().Replace("cm", "");
                                    }
                                }


                                addKV(targetCol, val);
                            }

                            // 系统列（不写入时间列，让数据库默认 SYSDATE 生效）
                            addKV("CREATE_USER_ID", "SYSTEM");
                            addKV("LAST_MODI_USER_ID", "SYSTEM");

                            string insertSql = $"INSERT INTO {upperTable} ({columns}) VALUES ({values})";
                            using (var cmd = new OracleCommand(insertSql, conn))
                            {
                                cmd.Transaction = tran;
                                cmd.ExecuteNonQuery();
                            }
                            success++;
                        }
                        catch (Exception exRow)
                        {
                            failed++;
                            errors.Add($"第{i + 1}行: {exRow.Message}");
                        }
                    }

                    if (failed > 0)
                    {
                        tran.Rollback();
                        ShowErrorMessage($"导入失败，已回滚。成功{success}，失败{failed}\r\n" + string.Join("\r\n", errors.Take(10)) + (errors.Count > 10 ? "\r\n…" : string.Empty));
                        return;
                    }

                    tran.Commit();
                    ShowSuccessMessage($"导入成功，共{success}行！(顺序与Excel一致)");
                }
                catch (Exception ex)
                {
                    try { tran.Rollback(); } catch { }
                    ShowErrorMessage($"导入过程中发生异常，已回滚：{ex.Message}");
                }
            }
        }

        // 调用WebForms页面的静态[WebMethod]，POST JSON 返回字符串 - updated by cqy at 2025-9-9
        private string CallPageMethod(string methodName, object payload)
        {
            try
            {
                if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException("methodName");
                string url = apiBaseUrl.TrimEnd('/') + "/" + methodName;
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string body = payload == null ? "{}" : JsonConvert.SerializeObject(payload);
                    return client.UploadString(url, "POST", body);
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }

        // 从WebMethod返回的JSON字符串中提取JArray - updated by cqy at 2025-9-9
        private JArray ExtractArrayFromWebMethodResponse(string jsonString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonString)) return new JArray();
                var root = JObject.Parse(jsonString); // { d: "[...]" } 或 { d: "{...}" }
                var dToken = root["d"];
                if (dToken == null) return new JArray();
                string inner = dToken.Type == JTokenType.String ? dToken.ToString() : dToken.ToString(Formatting.None);
                // 内层字符串是一个JSON数组字符串
                var arr = JArray.Parse(inner);
                return arr ?? new JArray();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"提取JSON数组失败: {ex.Message}");
                return new JArray();
            }
        }

        // 根据描述查找ID（兼容多个实体的常见字段） - updated by cqy at 2025-9-9
        private string FindIdByDescriptionGeneric(JArray array, string description)
        {
            if (array == null || array.Count == 0 || string.IsNullOrEmpty(description)) return null;
            // Sleeve: SLEEVE_CD/SLEEVE_DESC
            var item = array.FirstOrDefault(x => (x["SLEEVE_DESC"]?.ToString() ?? "") == description);
            if (item != null) return item["SLEEVE_CD"]?.ToString();
            // Feature: FEATURE_CD/FEATURE_DESC
            item = array.FirstOrDefault(x => (x["FEATURE_DESC"]?.ToString() ?? "") == description);
            if (item != null) return item["FEATURE_CD"]?.ToString();
            // Pattern: Pattern_Type_CD / Pattern_Type_Desc 这里返回CD=Desc（仓库实现）
            item = array.FirstOrDefault(x => (x["Pattern_Type_Desc"]?.ToString() ?? "") == description || (x["Pattern_Type_CD"]?.ToString() ?? "") == description);
            if (item != null) return item["Pattern_Type_CD"]?.ToString();
            // Gender: StyleNo / StyleDescription（GetBindDataSourceGender）
            item = array.FirstOrDefault(x => (x["StyleDescription"]?.ToString() ?? "").Contains(description));
            if (item != null) return item["StyleNo"]?.ToString();
            return null;
        }









    }
}
