using Sunny.UI;
using System;
using System.Windows.Forms;
using WinFormsApp1.EquipmentMaintenanceSystem.Services;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Forms
{
    public partial class FrmDataImportExport : UIForm
    {
        private readonly DataService dataService;

        public FrmDataImportExport()
        {
            InitializeComponent();
            dataService = new DataService();
            InitializeUI();
        }

        private void InitializeUI()
        {
            Text = "数据导入导出";
            ShowRadius = false;
        }

        private void btnImportEquipment_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Excel文件|*.xlsx;*.xls";
                dialog.Title = "选择设备数据文件";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        int count = dataService.ImportEquipmentFromExcel(dialog.FileName);
                        UIMessageBox.ShowSuccess($"成功导入 {count} 条设备数据！");
                    }
                    catch (Exception ex)
                    {
                        UIMessageBox.ShowError($"导入失败：{ex.Message}");
                    }
                }
            }
        }

        private void btnExportEquipment_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Excel文件|*.xlsx";
                dialog.Title = "保存设备数据";
                dialog.FileName = $"设备数据_{DateTime.Now:yyyyMMdd}.xlsx";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        dataService.ExportEquipmentToExcel(dialog.FileName);
                        UIMessageBox.ShowSuccess("设备数据导出成功！");
                    }
                    catch (Exception ex)
                    {
                        UIMessageBox.ShowError($"导出失败：{ex.Message}");
                    }
                }
            }
        }

        private void btnImportTool_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Excel文件|*.xlsx;*.xls";
                dialog.Title = "选择工装数据文件";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        int count = dataService.ImportToolFromExcel(dialog.FileName);
                        UIMessageBox.ShowSuccess($"成功导入 {count} 条工装数据！");
                    }
                    catch (Exception ex)
                    {
                        UIMessageBox.ShowError($"导入失败：{ex.Message}");
                    }
                }
            }
        }

        private void btnExportTool_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Excel文件|*.xlsx";
                dialog.Title = "保存工装数据";
                dialog.FileName = $"工装数据_{DateTime.Now:yyyyMMdd}.xlsx";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        dataService.ExportToolToExcel(dialog.FileName);
                        UIMessageBox.ShowSuccess("工装数据导出成功！");
                    }
                    catch (Exception ex)
                    {
                        UIMessageBox.ShowError($"导出失败：{ex.Message}");
                    }
                }
            }
        }

        private void btnImportMaintenancePlan_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Excel文件|*.xlsx;*.xls";
                dialog.Title = "选择保养计划数据文件";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        int count = dataService.ImportMaintenancePlanFromExcel(dialog.FileName);
                        UIMessageBox.ShowSuccess($"成功导入 {count} 条保养计划数据！");
                    }
                    catch (Exception ex)
                    {
                        UIMessageBox.ShowError($"导入失败：{ex.Message}");
                    }
                }
            }
        }

        private void btnExportMaintenancePlan_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Excel文件|*.xlsx";
                dialog.Title = "保存保养计划数据";
                dialog.FileName = $"保养计划_{DateTime.Now:yyyyMMdd}.xlsx";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        dataService.ExportMaintenancePlanToExcel(dialog.FileName);
                        UIMessageBox.ShowSuccess("保养计划数据导出成功！");
                    }
                    catch (Exception ex)
                    {
                        UIMessageBox.ShowError($"导出失败：{ex.Message}");
                    }
                }
            }
        }

        private void btnImportMaintenanceRecord_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Excel文件|*.xlsx;*.xls";
                dialog.Title = "选择保养记录数据文件";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        int count = dataService.ImportMaintenanceRecordFromExcel(dialog.FileName);
                        UIMessageBox.ShowSuccess($"成功导入 {count} 条保养记录数据！");
                    }
                    catch (Exception ex)
                    {
                        UIMessageBox.ShowError($"导入失败：{ex.Message}");
                    }
                }
            }
        }

        private void btnExportMaintenanceRecord_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Excel文件|*.xlsx";
                dialog.Title = "保存保养记录数据";
                dialog.FileName = $"保养记录_{DateTime.Now:yyyyMMdd}.xlsx";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        dataService.ExportMaintenanceRecordToExcel(dialog.FileName);
                        UIMessageBox.ShowSuccess("保养记录数据导出成功！");
                    }
                    catch (Exception ex)
                    {
                        UIMessageBox.ShowError($"导出失败：{ex.Message}");
                    }
                }
            }
        }

        private void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择模板保存位置";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        dataService.GenerateTemplates(dialog.SelectedPath);
                        UIMessageBox.ShowSuccess($"模板文件已生成到：{dialog.SelectedPath}");
                    }
                    catch (Exception ex)
                    {
                        UIMessageBox.ShowError($"生成模板失败：{ex.Message}");
                    }
                }
            }
        }

        private void btnExportEquipments_Click(object sender, EventArgs e)
        {
            btnExportEquipment_Click(sender, e);
        }

        private void btnExportTools_Click(object sender, EventArgs e)
        {
            btnExportTool_Click(sender, e);
        }

        private void btnExportRecords_Click(object sender, EventArgs e)
        {
            btnExportMaintenanceRecord_Click(sender, e);
        }

        private void btnImportEquipments_Click(object sender, EventArgs e)
        {
            btnImportEquipment_Click(sender, e);
        }

        private void btnImportTools_Click(object sender, EventArgs e)
        {
            btnImportTool_Click(sender, e);
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择备份保存位置";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string backupPath = System.IO.Path.Combine(dialog.SelectedPath, $"Backup_{DateTime.Now:yyyyMMddHHmmss}");
                        System.IO.Directory.CreateDirectory(backupPath);
                        
                        dataService.ExportEquipmentToExcel(System.IO.Path.Combine(backupPath, "设备数据.xlsx"));
                        dataService.ExportToolToExcel(System.IO.Path.Combine(backupPath, "工装数据.xlsx"));
                        dataService.ExportMaintenancePlanToExcel(System.IO.Path.Combine(backupPath, "保养计划.xlsx"));
                        dataService.ExportMaintenanceRecordToExcel(System.IO.Path.Combine(backupPath, "保养记录.xlsx"));
                        
                        UIMessageBox.ShowSuccess($"数据备份成功！\n备份位置：{backupPath}");
                    }
                    catch (Exception ex)
                    {
                        UIMessageBox.ShowError($"备份失败：{ex.Message}");
                    }
                }
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择备份文件夹";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        int totalCount = 0;
                        string backupPath = dialog.SelectedPath;
                        
                        string equipmentFile = System.IO.Path.Combine(backupPath, "设备数据.xlsx");
                        if (System.IO.File.Exists(equipmentFile))
                        {
                            totalCount += dataService.ImportEquipmentFromExcel(equipmentFile);
                        }
                        
                        string toolFile = System.IO.Path.Combine(backupPath, "工装数据.xlsx");
                        if (System.IO.File.Exists(toolFile))
                        {
                            totalCount += dataService.ImportToolFromExcel(toolFile);
                        }
                        
                        string planFile = System.IO.Path.Combine(backupPath, "保养计划.xlsx");
                        if (System.IO.File.Exists(planFile))
                        {
                            totalCount += dataService.ImportMaintenancePlanFromExcel(planFile);
                        }
                        
                        string recordFile = System.IO.Path.Combine(backupPath, "保养记录.xlsx");
                        if (System.IO.File.Exists(recordFile))
                        {
                            totalCount += dataService.ImportMaintenanceRecordFromExcel(recordFile);
                        }
                        
                        UIMessageBox.ShowSuccess($"数据恢复成功！共恢复 {totalCount} 条数据。");
                    }
                    catch (Exception ex)
                    {
                        UIMessageBox.ShowError($"恢复失败：{ex.Message}");
                    }
                }
            }
        }
    }
}
