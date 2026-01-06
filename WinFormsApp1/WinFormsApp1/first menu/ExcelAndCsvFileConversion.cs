using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Sunny.UI;

namespace WinFormsApp1.first_menu
{
    public partial class ExcelAndCsvFileConversion : UIForm
    {
        public ExcelAndCsvFileConversion()
        {
            InitializeComponent();
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            InitDefaults();
        }

        private void InitDefaults()
        {
            cboTargetFormat.Items.Clear();
            cboTargetFormat.Items.AddRange(new object[] { "CSV", "Excel" });
            cboTargetFormat.SelectedIndex = 0;

            cboCsvSeparator.Items.Clear();
            cboCsvSeparator.Items.AddRange(new object[] { ", (逗号)", "\t (制表符)", "; (分号)" });
            cboCsvSeparator.SelectedIndex = 0;

            cboEncoding.Items.Clear();
            cboEncoding.Items.AddRange(new object[] { "UTF-8", "UTF-8 BOM", "GB2312", "GBK" });
            cboEncoding.SelectedIndex = 0;

            cboExcelFormat.Items.Clear();
            cboExcelFormat.Items.AddRange(new object[] { ".xlsx", ".xls" });
            cboExcelFormat.SelectedIndex = 0;

            chkExcelMergeSheets.Checked = false;
            chkCombineCsvToWorkbook.Checked = false;
            chkIncludeSubfolders.Checked = false;
            ToggleOptions();
        }

        private void ToggleOptions()
        {
            bool toCsv = string.Equals(Convert.ToString(cboTargetFormat.SelectedItem), "CSV", StringComparison.OrdinalIgnoreCase);
            // CSV 相关
            lblCsvSeparator.Enabled = toCsv;
            cboCsvSeparator.Enabled = toCsv;
            lblEncoding.Enabled = toCsv;
            cboEncoding.Enabled = toCsv;
            chkExcelMergeSheets.Enabled = toCsv;

            // Excel 相关
            lblExcelFormat.Enabled = !toCsv;
            cboExcelFormat.Enabled = !toCsv;
            chkCombineCsvToWorkbook.Enabled = !toCsv;
        }

        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtSourceDir.Text = fbd.SelectedPath;
                }
            }
        }

        private void ConvertCsvFileToExcel(string csvFile, string outDir, bool xlsx, char separator, Encoding encoding)
        {
            try
            {
                IWorkbook wb = xlsx ? (IWorkbook)new XSSFWorkbook() : new HSSFWorkbook();
                string sheetName = Path.GetFileNameWithoutExtension(csvFile);
                if (string.IsNullOrWhiteSpace(sheetName)) sheetName = "Sheet";
                sheetName = SanitizeSheetName(sheetName);
                ISheet sheet = wb.CreateSheet(sheetName);
                int r = 0;
                foreach (var row in ReadCsvRows(csvFile, separator, encoding))
                {
                    var ir = sheet.CreateRow(r++);
                    for (int c = 0; c < row.Count; c++)
                    {
                        ir.CreateCell(c).SetCellValue(row[c] ?? string.Empty);
                    }
                }
                string baseName = Path.GetFileNameWithoutExtension(csvFile);
                string outPath = Path.Combine(outDir, SanitizeFileName(baseName) + (xlsx ? ".xlsx" : ".xls"));
                using (var fs = new FileStream(outPath, FileMode.Create, FileAccess.Write))
                {
                    wb.Write(fs);
                }
                Log($"[OK] {Path.GetFileName(csvFile)} => {Path.GetFileName(outPath)}");
            }
            catch (Exception ex)
            {
                Log($"[失败] {csvFile}: {ex.Message}");
                throw;
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtOutputDir.Text = fbd.SelectedPath;
                }
            }
        }

        private void cboTargetFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleOptions();
        }

        private async void btnConvert_Click(object sender, EventArgs e)
        {
            string src = txtSourceDir.Text?.Trim();
            string dst = txtOutputDir.Text?.Trim();
            if (string.IsNullOrWhiteSpace(src) || !Directory.Exists(src))
            {
                this.ShowWarningTip("请选择有效的源文件夹");
                return;
            }
            if (string.IsNullOrWhiteSpace(dst))
            {
                this.ShowWarningTip("请选择有效的输出文件夹");
                return;
            }
            Directory.CreateDirectory(dst);

            bool includeSub = chkIncludeSubfolders.Checked;
            bool toCsv = string.Equals(Convert.ToString(cboTargetFormat.SelectedItem), "CSV", StringComparison.OrdinalIgnoreCase);

            btnConvert.Enabled = false;
            try
            {
                rtbLog.Text = string.Empty;
                if (toCsv)
                {
                    char sep = GetSeparator();
                    Encoding enc = GetEncoding();
                    bool mergeSheets = chkExcelMergeSheets.Checked;
                    await Task.Run(() => ConvertAllExcelToCsv(src, dst, includeSub, sep, enc, mergeSheets));
                }
                else
                {
                    string fmt = Convert.ToString(cboExcelFormat.SelectedItem);
                    bool xlsx = string.Equals(fmt, ".xlsx", StringComparison.OrdinalIgnoreCase);
                    bool combine = chkCombineCsvToWorkbook.Checked;
                    char sep = GetSeparator();
                    Encoding enc = GetEncoding();
                    await Task.Run(() => ConvertAllCsvToExcel(src, dst, includeSub, xlsx, combine, sep, enc));
                }
                this.ShowSuccessTip("转换完成");
            }
            catch (Exception ex)
            {
                this.ShowErrorTip($"转换失败: {ex.Message}");
                Log($"[错误] {ex}");
            }
            finally
            {
                btnConvert.Enabled = true;
            }
        }

        private async void btnConvertSingle_Click(object sender, EventArgs e)
        {
            bool toCsv = string.Equals(Convert.ToString(cboTargetFormat.SelectedItem), "CSV", StringComparison.OrdinalIgnoreCase);
            char separator = GetSeparator();
            Encoding encoding = GetEncoding();
            string outputDir = txtOutputDir.Text?.Trim();

            string selectedFile;
            if (toCsv)
            {
                using (var ofd = new OpenFileDialog
                {
                    Title = "选择需要转换的 Excel 文件",
                    Filter = "Excel 文件 (*.xlsx;*.xls)|*.xlsx;*.xls",
                    Multiselect = false
                })
                {
                    if (ofd.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    selectedFile = ofd.FileName;
                }
            }
            else
            {
                using (var ofd = new OpenFileDialog
                {
                    Title = "选择需要转换的 CSV 文件",
                    Filter = "CSV 文件 (*.csv)|*.csv",
                    Multiselect = false
                })
                {
                    if (ofd.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    selectedFile = ofd.FileName;
                }
            }

            if (string.IsNullOrWhiteSpace(outputDir))
            {
                outputDir = Path.GetDirectoryName(selectedFile);
            }
            else
            {
                Directory.CreateDirectory(outputDir);
            }

            btnConvertSingle.Enabled = false;
            try
            {
                rtbLog.Text = string.Empty;
                if (toCsv)
                {
                    bool mergeSheets = chkExcelMergeSheets.Checked;
                    await Task.Run(() => ConvertExcelFileToCsv(selectedFile, outputDir, separator, encoding, mergeSheets));
                }
                else
                {
                    string fmt = Convert.ToString(cboExcelFormat.SelectedItem);
                    bool xlsx = string.Equals(fmt, ".xlsx", StringComparison.OrdinalIgnoreCase);
                    await Task.Run(() => ConvertCsvFileToExcel(selectedFile, outputDir, xlsx, separator, encoding));
                }
                this.ShowSuccessTip("单文件转换完成");
            }
            catch (Exception ex)
            {
                this.ShowErrorTip($"转换失败: {ex.Message}");
                Log($"[错误] {ex}");
            }
            finally
            {
                btnConvertSingle.Enabled = true;
            }
        }

        private char GetSeparator()
        {
            switch (Convert.ToString(cboCsvSeparator.SelectedItem))
            {
                case "\t (制表符)": return '\t';
                case "; (分号)": return ';';
                default: return ',';
            }
        }

        private Encoding GetEncoding()
        {
            string name = Convert.ToString(cboEncoding.SelectedItem);
            if (string.Equals(name, "UTF-8 BOM", StringComparison.OrdinalIgnoreCase)) return new UTF8Encoding(true);
            if (string.Equals(name, "UTF-8", StringComparison.OrdinalIgnoreCase)) return new UTF8Encoding(false);
            if (string.Equals(name, "GB2312", StringComparison.OrdinalIgnoreCase)) return Encoding.GetEncoding("GB2312");
            if (string.Equals(name, "GBK", StringComparison.OrdinalIgnoreCase)) return Encoding.GetEncoding("GBK");
            return new UTF8Encoding(false);
        }

        private void ConvertAllExcelToCsv(string srcDir, string outDir, bool includeSub, char separator, Encoding encoding, bool mergeSheets)
        {
            var option = includeSub ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var excels = Directory.EnumerateFiles(srcDir, "*.xlsx", option).Concat(Directory.EnumerateFiles(srcDir, "*.xls", option)).ToList();
            if (excels.Count == 0)
            {
                Log("未找到Excel文件");
                return;
            }
            foreach (var file in excels)
            {
                try
                {
                    using (var fs = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var reader = ExcelReaderFactory.CreateReader(fs))
                    {
                        var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = false
                            }
                        });
                        if (dataSet == null || dataSet.Tables.Count == 0)
                        {
                            Log($"[跳过] {file} 无数据");
                            continue;
                        }

                        if (mergeSheets)
                        {
                            string baseName = Path.GetFileNameWithoutExtension(file);
                            string outPath = Path.Combine(outDir, SanitizeFileName(baseName) + ".csv");
                            using (var sw = new StreamWriter(outPath, false, encoding))
                            {
                                foreach (DataTable table in dataSet.Tables)
                                {
                                    string sheetName = string.IsNullOrWhiteSpace(table.TableName) ? "Sheet" : table.TableName;
                                    for (int r = 0; r < table.Rows.Count; r++)
                                    {
                                        var row = table.Rows[r];
                                        // SheetName + row values
                                        var fields = new List<string> { EscapeCsv(sheetName, separator) };
                                        for (int c = 0; c < table.Columns.Count; c++)
                                        {
                                            string val = Convert.ToString(row[c]);
                                            fields.Add(EscapeCsv(val, separator));
                                        }
                                        sw.WriteLine(string.Join(separator.ToString(), fields));
                                    }
                                }
                            }
                            Log($"[OK] 合并输出 => {outPath}");
                        }
                        else
                        {
                            foreach (DataTable table in dataSet.Tables)
                            {
                                string sheetName = string.IsNullOrWhiteSpace(table.TableName) ? "Sheet" : table.TableName;
                                string baseName = Path.GetFileNameWithoutExtension(file);
                                string outPath = Path.Combine(outDir, SanitizeFileName($"{baseName}_{sheetName}") + ".csv");
                                WriteDataTableToCsv(table, outPath, separator, encoding);
                                Log($"[OK] {Path.GetFileName(file)}::{sheetName} => {Path.GetFileName(outPath)}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log($"[失败] {file}: {ex.Message}");
                }
            }
        }

        private void ConvertExcelFileToCsv(string excelFile, string outDir, char separator, Encoding encoding, bool mergeSheets)
        {
            try
            {
                using (var fs = File.Open(excelFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = ExcelReaderFactory.CreateReader(fs))
                {
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = false
                        }
                    });
                    if (dataSet == null || dataSet.Tables.Count == 0)
                    {
                        Log($"[跳过] {excelFile} 无数据");
                        return;
                    }

                    if (mergeSheets)
                    {
                        string baseName = Path.GetFileNameWithoutExtension(excelFile);
                        string outPath = Path.Combine(outDir, SanitizeFileName(baseName) + ".csv");
                        using (var sw = new StreamWriter(outPath, false, encoding))
                        {
                            foreach (DataTable table in dataSet.Tables)
                            {
                                string sheetName = string.IsNullOrWhiteSpace(table.TableName) ? "Sheet" : table.TableName;
                                for (int r = 0; r < table.Rows.Count; r++)
                                {
                                    var row = table.Rows[r];
                                    var fields = new List<string> { EscapeCsv(sheetName, separator) };
                                    for (int c = 0; c < table.Columns.Count; c++)
                                    {
                                        string val = Convert.ToString(row[c]);
                                        fields.Add(EscapeCsv(val, separator));
                                    }
                                    sw.WriteLine(string.Join(separator.ToString(), fields));
                                }
                            }
                        }
                        Log($"[OK] {Path.GetFileName(excelFile)} 合并输出 => {Path.GetFileName(outPath)}");
                    }
                    else
                    {
                        foreach (DataTable table in dataSet.Tables)
                        {
                            string sheetName = string.IsNullOrWhiteSpace(table.TableName) ? "Sheet" : table.TableName;
                            string baseName = Path.GetFileNameWithoutExtension(excelFile);
                            string outPath = Path.Combine(outDir, SanitizeFileName($"{baseName}_{sheetName}") + ".csv");
                            WriteDataTableToCsv(table, outPath, separator, encoding);
                            Log($"[OK] {Path.GetFileName(excelFile)}::{sheetName} => {Path.GetFileName(outPath)}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"[失败] {excelFile}: {ex.Message}");
                throw;
            }
        }

        private void ConvertAllCsvToExcel(string srcDir, string outDir, bool includeSub, bool xlsx, bool combineToOneWorkbook, char separator, Encoding encoding)
        {
            var option = includeSub ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var csvs = Directory.EnumerateFiles(srcDir, "*.csv", option).ToList();
            if (csvs.Count == 0)
            {
                Log("未找到CSV文件");
                return;
            }

            if (combineToOneWorkbook)
            {
                IWorkbook wb = xlsx ? (IWorkbook)new XSSFWorkbook() : new HSSFWorkbook();
                foreach (var file in csvs)
                {
                    try
                    {
                        string sheetName = Path.GetFileNameWithoutExtension(file);
                        if (string.IsNullOrWhiteSpace(sheetName)) sheetName = "Sheet";
                        sheetName = SanitizeSheetName(sheetName);
                        ISheet sheet = wb.CreateSheet(sheetName);
                        int r = 0;
                        foreach (var row in ReadCsvRows(file, separator, encoding))
                        {
                            var ir = sheet.CreateRow(r++);
                            for (int c = 0; c < row.Count; c++)
                            {
                                ir.CreateCell(c).SetCellValue(row[c] ?? string.Empty);
                            }
                        }
                        Log($"[OK] {Path.GetFileName(file)} 已作为工作表加入");
                    }
                    catch (Exception ex)
                    {
                        Log($"[失败] {file}: {ex.Message}");
                    }
                }
                string outName = Path.Combine(outDir, "combined" + (xlsx ? ".xlsx" : ".xls"));
                using (var fs = new FileStream(outName, FileMode.Create, FileAccess.Write))
                {
                    wb.Write(fs);
                }
                Log($"[OK] 合并输出 => {outName}");
            }
            else
            {
                foreach (var file in csvs)
                {
                    try
                    {
                        IWorkbook wb = xlsx ? (IWorkbook)new XSSFWorkbook() : new HSSFWorkbook();
                        string sheetName = Path.GetFileNameWithoutExtension(file);
                        if (string.IsNullOrWhiteSpace(sheetName)) sheetName = "Sheet";
                        sheetName = SanitizeSheetName(sheetName);
                        ISheet sheet = wb.CreateSheet(sheetName);
                        int r = 0;
                        foreach (var row in ReadCsvRows(file, separator, encoding))
                        {
                            var ir = sheet.CreateRow(r++);
                            for (int c = 0; c < row.Count; c++)
                            {
                                ir.CreateCell(c).SetCellValue(row[c] ?? string.Empty);
                            }
                        }
                        string baseName = Path.GetFileNameWithoutExtension(file);
                        string outPath = Path.Combine(outDir, SanitizeFileName(baseName) + (xlsx ? ".xlsx" : ".xls"));
                        using (var fs = new FileStream(outPath, FileMode.Create, FileAccess.Write)) wb.Write(fs);
                        Log($"[OK] {Path.GetFileName(file)} => {Path.GetFileName(outPath)}");
                    }
                    catch (Exception ex)
                    {
                        Log($"[失败] {file}: {ex.Message}");
                    }
                }
            }
        }

        private IEnumerable<List<string>> ReadCsvRows(string file, char separator, Encoding encoding)
        {
            // 先尝试检测文件编码
            Encoding detectedEncoding = DetectFileEncoding(file, encoding);
            using (var sr = new StreamReader(file, detectedEncoding))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return ParseCsvLine(line, separator);
                }
            }
        }

        /// <summary>
        /// 检测文件编码，优先检测 BOM，否则使用用户指定的编码
        /// </summary>
        private Encoding DetectFileEncoding(string file, Encoding fallbackEncoding)
        {
            byte[] bom = new byte[4];
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Read(bom, 0, 4);
            }

            // 检测 BOM
            if (bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF)
                return new UTF8Encoding(true);
            if (bom[0] == 0xFF && bom[1] == 0xFE)
                return Encoding.Unicode; // UTF-16 LE
            if (bom[0] == 0xFE && bom[1] == 0xFF)
                return Encoding.BigEndianUnicode; // UTF-16 BE

            // 没有 BOM，使用用户选择的编码
            return fallbackEncoding;
        }

        private List<string> ParseCsvLine(string line, char separator)
        {
            var result = new List<string>();
            if (line == null) return result;
            var sb = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                char ch = line[i];
                if (inQuotes)
                {
                    if (ch == '"')
                    {
                        if (i + 1 < line.Length && line[i + 1] == '"')
                        {
                            sb.Append('"');
                            i++; // skip escaped quote
                        }
                        else
                        {
                            inQuotes = false; // end quote
                        }
                    }
                    else
                    {
                        sb.Append(ch);
                    }
                }
                else
                {
                    if (ch == '"')
                    {
                        inQuotes = true;
                    }
                    else if (ch == separator)
                    {
                        result.Add(sb.ToString());
                        sb.Clear();
                    }
                    else
                    {
                        sb.Append(ch);
                    }
                }
            }
            result.Add(sb.ToString());
            return result;
        }

        private void WriteDataTableToCsv(DataTable table, string outPath, char separator, Encoding encoding)
        {
            using (var sw = new StreamWriter(outPath, false, encoding))
            {
                for (int r = 0; r < table.Rows.Count; r++)
                {
                    var row = table.Rows[r];
                    var fields = new List<string>(table.Columns.Count);
                    for (int c = 0; c < table.Columns.Count; c++)
                    {
                        string val = Convert.ToString(row[c]);
                        fields.Add(EscapeCsv(val, separator));
                    }
                    sw.WriteLine(string.Join(separator.ToString(), fields));
                }
            }
        }

        private string EscapeCsv(string value, char separator)
        {
            if (value == null) return string.Empty;
            bool needQuote = value.Contains('"') || value.Contains('\n') || value.Contains('\r') || value.Contains(separator);
            if (needQuote)
            {
                var v = value.Replace("\"", "\"\"");
                return "\"" + v + "\"";
            }
            return value;
        }

        private string SanitizeFileName(string name)
        {
            foreach (var ch in Path.GetInvalidFileNameChars()) name = name.Replace(ch, '_');
            return name;
        }

        private string SanitizeSheetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "Sheet";
            var invalid = new char[] { ':', '\\', '/', '?', '*', '[', ']' };
            foreach (var ch in invalid) name = name.Replace(ch, '_');
            return name.Length > 31 ? name.Substring(0, 31) : name;
        }

        private void Log(string message)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action<string>(Log), message);
                return;
            }
            rtbLog.AppendText($"{DateTime.Now:HH:mm:ss} {message}\r\n");
        }
    }
}
