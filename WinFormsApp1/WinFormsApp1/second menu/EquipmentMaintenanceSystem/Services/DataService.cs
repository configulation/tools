using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsApp1.EquipmentMaintenanceSystem.Models;
using WinFormsApp1.Common.Database.Services;
using WinFormsApp1.Common.Entities.EquipmentMaintenance;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using WinFormsApp1.Common.Database;

namespace WinFormsApp1.EquipmentMaintenanceSystem.Services
{
    /// <summary>
    /// 数据库操作服务(使用SqlSugar)
    /// </summary>
    public class DataService
    {
        public DataService()
        {
        }

        #region 设备数据操作

        public List<Equipment> LoadEquipments()
        {
            using var db = DbHelper.GetInstance();
            var entities = db.Queryable<EquipmentEntity>().ToList();
            
            return entities.Select(e => new Equipment
            {
                EquipmentId = e.EquipmentId,
                LineLocation = e.LineLocation,
                Category = e.Category,
                SubCategory = e.SubCategory,
                MaintenanceIntervalDays = e.MaintenanceIntervalDays,
                NextMaintenanceDate = e.NextMaintenanceDate,
                Status = e.Status,
                OperatorId = e.OperatorId,
                Notes = e.Notes,
                CreateTime = e.CreateTime,
                UpdateTime = e.UpdateTime,
                MaintenanceItems = GetMaintenanceItems(e.EquipmentId, "Equipment")
            }).ToList();
        }

        public void SaveEquipments(List<Equipment> equipments)
        {
            using var db = DbHelper.GetInstance();
            db.BeginTran();
            try
            {
                db.Deleteable<EquipmentEntity>().ExecuteCommand();
                
                var entities = equipments.Select(e => new EquipmentEntity
                {
                    EquipmentId = e.EquipmentId,
                    LineLocation = e.LineLocation,
                    Category = e.Category,
                    SubCategory = e.SubCategory,
                    MaintenanceIntervalDays = e.MaintenanceIntervalDays,
                    NextMaintenanceDate = e.NextMaintenanceDate,
                    Status = e.Status,
                    OperatorId = e.OperatorId,
                    Notes = e.Notes,
                    CreateTime = e.CreateTime,
                    UpdateTime = e.UpdateTime
                }).ToList();
                
                if (entities.Count > 0)
                {
                    db.Insertable(entities).ExecuteCommand();
                }
                
                db.CommitTran();
            }
            catch
            {
                db.RollbackTran();
                throw;
            }
        }

        public bool AddEquipment(Equipment equipment)
        {
            using var db = DbHelper.GetInstance();
            
            var exists = db.Queryable<EquipmentEntity>()
                .Where(e => e.EquipmentId == equipment.EquipmentId)
                .Any();
            
            if (exists) return false;

            equipment.CreateTime = DateTime.Now;
            equipment.UpdateTime = DateTime.Now;
            
            var entity = new EquipmentEntity
            {
                EquipmentId = equipment.EquipmentId,
                LineLocation = equipment.LineLocation,
                Category = equipment.Category,
                SubCategory = equipment.SubCategory,
                MaintenanceIntervalDays = equipment.MaintenanceIntervalDays,
                NextMaintenanceDate = equipment.NextMaintenanceDate,
                Status = equipment.Status,
                OperatorId = equipment.OperatorId,
                Notes = equipment.Notes,
                CreateTime = equipment.CreateTime,
                UpdateTime = equipment.UpdateTime
            };
            
            db.Insertable(entity).ExecuteCommand();
            SaveMaintenanceItems(equipment.EquipmentId, "Equipment", equipment.MaintenanceItems);
            
            return true;
        }

        public bool UpdateEquipment(Equipment equipment)
        {
            using var db = DbHelper.GetInstance();
            
            var entity = db.Queryable<EquipmentEntity>()
                .Where(e => e.EquipmentId == equipment.EquipmentId)
                .First();
            
            if (entity == null) return false;

            equipment.UpdateTime = DateTime.Now;
            
            entity.LineLocation = equipment.LineLocation;
            entity.Category = equipment.Category;
            entity.SubCategory = equipment.SubCategory;
            entity.MaintenanceIntervalDays = equipment.MaintenanceIntervalDays;
            entity.NextMaintenanceDate = equipment.NextMaintenanceDate;
            entity.Status = equipment.Status;
            entity.OperatorId = equipment.OperatorId;
            entity.Notes = equipment.Notes;
            entity.UpdateTime = equipment.UpdateTime;
            
            db.Updateable(entity).ExecuteCommand();
            SaveMaintenanceItems(equipment.EquipmentId, "Equipment", equipment.MaintenanceItems);
            
            return true;
        }

        public bool DeleteEquipment(string equipmentId)
        {
            using var db = DbHelper.GetInstance();
            db.BeginTran();
            try
            {
                db.Deleteable<EquipmentEntity>()
                    .Where(e => e.EquipmentId == equipmentId)
                    .ExecuteCommand();
                
                db.Deleteable<MaintenanceItemEntity>()
                    .Where(m => m.TargetId == equipmentId && m.TargetType == "Equipment")
                    .ExecuteCommand();
                
                db.CommitTran();
                return true;
            }
            catch
            {
                db.RollbackTran();
                return false;
            }
        }

        public Equipment GetEquipment(string equipmentId)
        {
            using var db = DbHelper.GetInstance();
            var entity = db.Queryable<EquipmentEntity>()
                .Where(e => e.EquipmentId == equipmentId)
                .First();
            
            if (entity == null) return null;
            
            return new Equipment
            {
                EquipmentId = entity.EquipmentId,
                LineLocation = entity.LineLocation,
                Category = entity.Category,
                SubCategory = entity.SubCategory,
                MaintenanceIntervalDays = entity.MaintenanceIntervalDays,
                NextMaintenanceDate = entity.NextMaintenanceDate,
                Status = entity.Status,
                OperatorId = entity.OperatorId,
                Notes = entity.Notes,
                CreateTime = entity.CreateTime,
                UpdateTime = entity.UpdateTime,
                MaintenanceItems = GetMaintenanceItems(equipmentId, "Equipment")
            };
        }

        #endregion

        #region 工装数据操作

        public List<Tool> LoadTools()
        {
            using var db = DbHelper.GetInstance();
            var entities = db.Queryable<ToolEntity>().ToList();
            
            return entities.Select(t => new Tool
            {
                ToolCode = t.ToolCode,
                LineLocation = t.LineLocation,
                Category = t.Category,
                SubCategory = t.SubCategory,
                WorkOrder = t.WorkOrder,
                OrderQuantity = t.OrderQuantity,
                PanelQuantity = t.PanelQuantity,
                ScraperCount = t.ScraperCount,
                UsageCount = t.UsageCount,
                TotalUsage = t.TotalUsage,
                MaintenanceInterval = t.MaintenanceInterval,
                NextMaintenanceDate = t.NextMaintenanceDate,
                IssueTime = t.IssueTime,
                ReturnTime = t.ReturnTime,
                Status = t.Status,
                Notes = t.Notes,
                CreateTime = t.CreateTime,
                UpdateTime = t.UpdateTime,
                MaintenanceItems = GetMaintenanceItems(t.ToolCode, "Tool")
            }).ToList();
        }

        public void SaveTools(List<Tool> tools)
        {
            using var db = DbHelper.GetInstance();
            db.BeginTran();
            try
            {
                db.Deleteable<ToolEntity>().ExecuteCommand();
                
                var entities = tools.Select(t => new ToolEntity
                {
                    ToolCode = t.ToolCode,
                    LineLocation = t.LineLocation,
                    Category = t.Category,
                    SubCategory = t.SubCategory,
                    WorkOrder = t.WorkOrder,
                    OrderQuantity = t.OrderQuantity,
                    PanelQuantity = t.PanelQuantity,
                    ScraperCount = t.ScraperCount,
                    UsageCount = t.UsageCount,
                    TotalUsage = t.TotalUsage,
                    MaintenanceInterval = t.MaintenanceInterval,
                    NextMaintenanceDate = t.NextMaintenanceDate,
                    IssueTime = t.IssueTime,
                    ReturnTime = t.ReturnTime,
                    Status = t.Status,
                    Notes = t.Notes,
                    CreateTime = t.CreateTime,
                    UpdateTime = t.UpdateTime
                }).ToList();
                
                if (entities.Count > 0)
                {
                    db.Insertable(entities).ExecuteCommand();
                }
                
                db.CommitTran();
            }
            catch
            {
                db.RollbackTran();
                throw;
            }
        }

        public bool AddTool(Tool tool)
        {
            using var db = DbHelper.GetInstance();
            
            var exists = db.Queryable<ToolEntity>()
                .Where(t => t.ToolCode == tool.ToolCode)
                .Any();
            
            if (exists) return false;

            tool.CreateTime = DateTime.Now;
            tool.UpdateTime = DateTime.Now;
            
            var entity = new ToolEntity
            {
                ToolCode = tool.ToolCode,
                LineLocation = tool.LineLocation,
                Category = tool.Category,
                SubCategory = tool.SubCategory,
                WorkOrder = tool.WorkOrder,
                OrderQuantity = tool.OrderQuantity,
                PanelQuantity = tool.PanelQuantity,
                ScraperCount = tool.ScraperCount,
                UsageCount = tool.UsageCount,
                TotalUsage = tool.TotalUsage,
                MaintenanceInterval = tool.MaintenanceInterval,
                NextMaintenanceDate = tool.NextMaintenanceDate,
                IssueTime = tool.IssueTime,
                ReturnTime = tool.ReturnTime,
                Status = tool.Status,
                Notes = tool.Notes,
                CreateTime = tool.CreateTime,
                UpdateTime = tool.UpdateTime
            };
            
            db.Insertable(entity).ExecuteCommand();
            SaveMaintenanceItems(tool.ToolCode, "Tool", tool.MaintenanceItems);
            
            return true;
        }

        public bool UpdateTool(Tool tool)
        {
            using var db = DbHelper.GetInstance();
            
            var entity = db.Queryable<ToolEntity>()
                .Where(t => t.ToolCode == tool.ToolCode)
                .First();
            
            if (entity == null) return false;

            tool.UpdateTime = DateTime.Now;
            
            entity.LineLocation = tool.LineLocation;
            entity.Category = tool.Category;
            entity.SubCategory = tool.SubCategory;
            entity.WorkOrder = tool.WorkOrder;
            entity.OrderQuantity = tool.OrderQuantity;
            entity.PanelQuantity = tool.PanelQuantity;
            entity.ScraperCount = tool.ScraperCount;
            entity.UsageCount = tool.UsageCount;
            entity.TotalUsage = tool.TotalUsage;
            entity.MaintenanceInterval = tool.MaintenanceInterval;
            entity.NextMaintenanceDate = tool.NextMaintenanceDate;
            entity.IssueTime = tool.IssueTime;
            entity.ReturnTime = tool.ReturnTime;
            entity.Status = tool.Status;
            entity.Notes = tool.Notes;
            entity.UpdateTime = tool.UpdateTime;
            
            db.Updateable(entity).ExecuteCommand();
            SaveMaintenanceItems(tool.ToolCode, "Tool", tool.MaintenanceItems);
            
            return true;
        }

        public bool DeleteTool(string toolCode)
        {
            using var db = DbHelper.GetInstance();
            db.BeginTran();
            try
            {
                db.Deleteable<ToolEntity>()
                    .Where(t => t.ToolCode == toolCode)
                    .ExecuteCommand();
                
                db.Deleteable<MaintenanceItemEntity>()
                    .Where(m => m.TargetId == toolCode && m.TargetType == "Tool")
                    .ExecuteCommand();
                
                db.CommitTran();
                return true;
            }
            catch
            {
                db.RollbackTran();
                return false;
            }
        }

        public Tool GetTool(string toolCode)
        {
            using var db = DbHelper.GetInstance();
            var entity = db.Queryable<ToolEntity>()
                .Where(t => t.ToolCode == toolCode)
                .First();
            
            if (entity == null) return null;
            
            return new Tool
            {
                ToolCode = entity.ToolCode,
                LineLocation = entity.LineLocation,
                Category = entity.Category,
                SubCategory = entity.SubCategory,
                WorkOrder = entity.WorkOrder,
                OrderQuantity = entity.OrderQuantity,
                PanelQuantity = entity.PanelQuantity,
                ScraperCount = entity.ScraperCount,
                UsageCount = entity.UsageCount,
                TotalUsage = entity.TotalUsage,
                MaintenanceInterval = entity.MaintenanceInterval,
                NextMaintenanceDate = entity.NextMaintenanceDate,
                IssueTime = entity.IssueTime,
                ReturnTime = entity.ReturnTime,
                Status = entity.Status,
                Notes = entity.Notes,
                CreateTime = entity.CreateTime,
                UpdateTime = entity.UpdateTime,
                MaintenanceItems = GetMaintenanceItems(toolCode, "Tool")
            };
        }

        #endregion

        #region 保养记录操作

        public List<MaintenanceRecord> LoadRecords()
        {
            using var db = DbHelper.GetInstance();
            var entities = db.Queryable<MaintenanceRecordEntity>().ToList();
            
            return entities.Select(r => new MaintenanceRecord
            {
                RecordId = r.RecordId,
                TargetId = r.TargetId,
                TargetType = r.TargetType,
                MaintenanceTime = r.MaintenanceTime,
                Operator = r.Operator,
                MaintenanceItems = r.MaintenanceItems,
                Result = r.Result,
                Notes = r.Notes,
                NextMaintenanceDate = r.NextMaintenanceDate
            }).ToList();
        }

        public void SaveRecords(List<MaintenanceRecord> records)
        {
            using var db = DbHelper.GetInstance();
            db.BeginTran();
            try
            {
                db.Deleteable<MaintenanceRecordEntity>().ExecuteCommand();
                
                var entities = records.Select(r => new MaintenanceRecordEntity
                {
                    RecordId = r.RecordId,
                    TargetId = r.TargetId,
                    TargetType = r.TargetType,
                    MaintenanceTime = r.MaintenanceTime,
                    Operator = r.Operator,
                    MaintenanceItems = r.MaintenanceItems,
                    Result = r.Result,
                    Notes = r.Notes,
                    NextMaintenanceDate = r.NextMaintenanceDate
                }).ToList();
                
                if (entities.Count > 0)
                {
                    db.Insertable(entities).ExecuteCommand();
                }
                
                db.CommitTran();
            }
            catch
            {
                db.RollbackTran();
                throw;
            }
        }

        public void AddRecord(MaintenanceRecord record)
        {
            using var db = DbHelper.GetInstance();
            
            var entity = new MaintenanceRecordEntity
            {
                RecordId = record.RecordId,
                TargetId = record.TargetId,
                TargetType = record.TargetType,
                MaintenanceTime = record.MaintenanceTime,
                Operator = record.Operator,
                MaintenanceItems = record.MaintenanceItems,
                Result = record.Result,
                Notes = record.Notes,
                NextMaintenanceDate = record.NextMaintenanceDate
            };
            
            db.Insertable(entity).ExecuteCommand();
        }

        public List<MaintenanceRecord> GetRecordsByTarget(string targetId, string targetType)
        {
            using var db = DbHelper.GetInstance();
            var entities = db.Queryable<MaintenanceRecordEntity>()
                .Where(r => r.TargetId == targetId && r.TargetType == targetType)
                .OrderBy(r => r.MaintenanceTime, SqlSugar.OrderByType.Desc)
                .ToList();
            
            return entities.Select(r => new MaintenanceRecord
            {
                RecordId = r.RecordId,
                TargetId = r.TargetId,
                TargetType = r.TargetType,
                MaintenanceTime = r.MaintenanceTime,
                Operator = r.Operator,
                MaintenanceItems = r.MaintenanceItems,
                Result = r.Result,
                Notes = r.Notes,
                NextMaintenanceDate = r.NextMaintenanceDate
            }).ToList();
        }

        #endregion

        #region 保养项目辅助方法

        private List<string> GetMaintenanceItems(string targetId, string targetType)
        {
            using var db = DbHelper.GetInstance();
            return db.Queryable<MaintenanceItemEntity>()
                .Where(m => m.TargetId == targetId && m.TargetType == targetType)
                .Select(m => m.ItemName)
                .ToList();
        }

        private void SaveMaintenanceItems(string targetId, string targetType, List<string> items)
        {
            using var db = DbHelper.GetInstance();
            db.BeginTran();
            try
            {
                db.Deleteable<MaintenanceItemEntity>()
                    .Where(m => m.TargetId == targetId && m.TargetType == targetType)
                    .ExecuteCommand();
                
                if (items != null && items.Count > 0)
                {
                    var entities = items.Select(item => new MaintenanceItemEntity
                    {
                        TargetId = targetId,
                        TargetType = targetType,
                        ItemName = item
                    }).ToList();
                    
                    db.Insertable(entities).ExecuteCommand();
                }
                
                db.CommitTran();
            }
            catch
            {
                db.RollbackTran();
                throw;
            }
        }

        #endregion

        #region Excel导入导出

        /// <summary>
        /// 从Excel导入设备数据
        /// </summary>
        public int ImportEquipmentFromExcel(string filePath)
        {
            var equipments = LoadEquipments();
            int count = 0;

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheetAt(0);

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null) continue;

                    var equipment = new Equipment
                    {
                        EquipmentId = GetCellValue(row.GetCell(0)),
                        LineLocation = GetCellValue(row.GetCell(1)),
                        Category = GetCellValue(row.GetCell(2)),
                        SubCategory = GetCellValue(row.GetCell(3)),
                        MaintenanceIntervalDays = int.TryParse(GetCellValue(row.GetCell(4)), out int interval) ? interval : 30,
                        NextMaintenanceDate = DateTime.TryParse(GetCellValue(row.GetCell(5)), out DateTime nextDate) ? nextDate : DateTime.Now.AddDays(30),
                        Status = GetCellValue(row.GetCell(6)),
                        OperatorId = GetCellValue(row.GetCell(7)),
                        Notes = GetCellValue(row.GetCell(8)),
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    };

                    if (!string.IsNullOrEmpty(equipment.EquipmentId) && !equipments.Any(e => e.EquipmentId == equipment.EquipmentId))
                    {
                        equipments.Add(equipment);
                        count++;
                    }
                }
            }

            SaveEquipments(equipments);
            return count;
        }

        /// <summary>
        /// 导出设备数据到Excel
        /// </summary>
        public void ExportEquipmentToExcel(string filePath)
        {
            var equipments = LoadEquipments();
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("设备数据");

            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("设备ID");
            headerRow.CreateCell(1).SetCellValue("线别储位");
            headerRow.CreateCell(2).SetCellValue("类别");
            headerRow.CreateCell(3).SetCellValue("子类别");
            headerRow.CreateCell(4).SetCellValue("保养周期(天)");
            headerRow.CreateCell(5).SetCellValue("下次保养日期");
            headerRow.CreateCell(6).SetCellValue("状态");
            headerRow.CreateCell(7).SetCellValue("操作员ID");
            headerRow.CreateCell(8).SetCellValue("备注");

            for (int i = 0; i < equipments.Count; i++)
            {
                var equipment = equipments[i];
                var row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(equipment.EquipmentId);
                row.CreateCell(1).SetCellValue(equipment.LineLocation);
                row.CreateCell(2).SetCellValue(equipment.Category);
                row.CreateCell(3).SetCellValue(equipment.SubCategory);
                row.CreateCell(4).SetCellValue(equipment.MaintenanceIntervalDays);
                row.CreateCell(5).SetCellValue(equipment.NextMaintenanceDate.ToString("yyyy-MM-dd"));
                row.CreateCell(6).SetCellValue(equipment.Status);
                row.CreateCell(7).SetCellValue(equipment.OperatorId);
                row.CreateCell(8).SetCellValue(equipment.Notes);
            }

            for (int i = 0; i < 9; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
            }
        }

        /// <summary>
        /// 从Excel导入工装数据
        /// </summary>
        public int ImportToolFromExcel(string filePath)
        {
            var tools = LoadTools();
            int count = 0;

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheetAt(0);

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null) continue;

                    var tool = new Tool
                    {
                        ToolCode = GetCellValue(row.GetCell(0)),
                        LineLocation = GetCellValue(row.GetCell(1)),
                        Category = GetCellValue(row.GetCell(2)),
                        SubCategory = GetCellValue(row.GetCell(3)),
                        WorkOrder = GetCellValue(row.GetCell(4)),
                        OrderQuantity = int.TryParse(GetCellValue(row.GetCell(5)), out int orderQty) ? orderQty : 0,
                        PanelQuantity = int.TryParse(GetCellValue(row.GetCell(6)), out int panelQty) ? panelQty : 0,
                        ScraperCount = int.TryParse(GetCellValue(row.GetCell(7)), out int scraperCnt) ? scraperCnt : 0,
                        UsageCount = int.TryParse(GetCellValue(row.GetCell(8)), out int usageCnt) ? usageCnt : 0,
                        TotalUsage = int.TryParse(GetCellValue(row.GetCell(9)), out int totalUsage) ? totalUsage : 0,
                        MaintenanceInterval = GetCellValue(row.GetCell(10)),
                        NextMaintenanceDate = DateTime.TryParse(GetCellValue(row.GetCell(11)), out DateTime nextDate) ? nextDate : DateTime.Now.AddDays(30),
                        Status = GetCellValue(row.GetCell(12)),
                        Notes = GetCellValue(row.GetCell(13)),
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    };

                    if (!string.IsNullOrEmpty(tool.ToolCode) && !tools.Any(t => t.ToolCode == tool.ToolCode))
                    {
                        tools.Add(tool);
                        count++;
                    }
                }
            }

            SaveTools(tools);
            return count;
        }

        /// <summary>
        /// 导出工装数据到Excel
        /// </summary>
        public void ExportToolToExcel(string filePath)
        {
            var tools = LoadTools();
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("工装数据");

            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("工装编码");
            headerRow.CreateCell(1).SetCellValue("线别储位");
            headerRow.CreateCell(2).SetCellValue("类别");
            headerRow.CreateCell(3).SetCellValue("子类别");
            headerRow.CreateCell(4).SetCellValue("工单号");
            headerRow.CreateCell(5).SetCellValue("工单数量");
            headerRow.CreateCell(6).SetCellValue("拼料数量");
            headerRow.CreateCell(7).SetCellValue("刮刀数量");
            headerRow.CreateCell(8).SetCellValue("当前使用次数");
            headerRow.CreateCell(9).SetCellValue("总使用次数");
            headerRow.CreateCell(10).SetCellValue("保养间隔");
            headerRow.CreateCell(11).SetCellValue("下次保养日期");
            headerRow.CreateCell(12).SetCellValue("状态");
            headerRow.CreateCell(13).SetCellValue("备注");

            for (int i = 0; i < tools.Count; i++)
            {
                var tool = tools[i];
                var row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(tool.ToolCode);
                row.CreateCell(1).SetCellValue(tool.LineLocation);
                row.CreateCell(2).SetCellValue(tool.Category);
                row.CreateCell(3).SetCellValue(tool.SubCategory);
                row.CreateCell(4).SetCellValue(tool.WorkOrder);
                row.CreateCell(5).SetCellValue(tool.OrderQuantity);
                row.CreateCell(6).SetCellValue(tool.PanelQuantity);
                row.CreateCell(7).SetCellValue(tool.ScraperCount);
                row.CreateCell(8).SetCellValue(tool.UsageCount);
                row.CreateCell(9).SetCellValue(tool.TotalUsage);
                row.CreateCell(10).SetCellValue(tool.MaintenanceInterval);
                row.CreateCell(11).SetCellValue(tool.NextMaintenanceDate.ToString("yyyy-MM-dd"));
                row.CreateCell(12).SetCellValue(tool.Status);
                row.CreateCell(13).SetCellValue(tool.Notes);
            }

            for (int i = 0; i < 14; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
            }
        }

        /// <summary>
        /// 从Excel导入保养计划数据（实际是导入设备和工装的下次保养日期）
        /// </summary>
        public int ImportMaintenancePlanFromExcel(string filePath)
        {
            int count = 0;

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheetAt(0);

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null) continue;

                    var targetType = GetCellValue(row.GetCell(0));
                    var targetId = GetCellValue(row.GetCell(1));
                    var nextMaintenanceDateStr = GetCellValue(row.GetCell(2));

                    if (string.IsNullOrEmpty(targetType) || string.IsNullOrEmpty(targetId)) continue;

                    if (!DateTime.TryParse(nextMaintenanceDateStr, out DateTime nextDate))
                        continue;

                    if (targetType == "设备")
                    {
                        var equipment = GetEquipment(targetId);
                        if (equipment != null)
                        {
                            equipment.NextMaintenanceDate = nextDate;
                            UpdateEquipment(equipment);
                            count++;
                        }
                    }
                    else if (targetType == "工装")
                    {
                        var tool = GetTool(targetId);
                        if (tool != null)
                        {
                            tool.NextMaintenanceDate = nextDate;
                            UpdateTool(tool);
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// 导出保养计划到Excel
        /// </summary>
        public void ExportMaintenancePlanToExcel(string filePath)
        {
            var equipments = LoadEquipments();
            var tools = LoadTools();

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("保养计划");

            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("类型");
            headerRow.CreateCell(1).SetCellValue("编号");
            headerRow.CreateCell(2).SetCellValue("下次保养日期");
            headerRow.CreateCell(3).SetCellValue("线别储位");
            headerRow.CreateCell(4).SetCellValue("类别");
            headerRow.CreateCell(5).SetCellValue("子类别");
            headerRow.CreateCell(6).SetCellValue("状态");

            int rowIndex = 1;

            foreach (var equipment in equipments.OrderBy(e => e.NextMaintenanceDate))
            {
                var row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue("设备");
                row.CreateCell(1).SetCellValue(equipment.EquipmentId);
                row.CreateCell(2).SetCellValue(equipment.NextMaintenanceDate.ToString("yyyy-MM-dd"));
                row.CreateCell(3).SetCellValue(equipment.LineLocation);
                row.CreateCell(4).SetCellValue(equipment.Category);
                row.CreateCell(5).SetCellValue(equipment.SubCategory);
                row.CreateCell(6).SetCellValue(equipment.Status);
            }

            foreach (var tool in tools.OrderBy(t => t.NextMaintenanceDate))
            {
                var row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue("工装");
                row.CreateCell(1).SetCellValue(tool.ToolCode);
                row.CreateCell(2).SetCellValue(tool.NextMaintenanceDate.ToString("yyyy-MM-dd"));
                row.CreateCell(3).SetCellValue(tool.LineLocation);
                row.CreateCell(4).SetCellValue(tool.Category);
                row.CreateCell(5).SetCellValue(tool.SubCategory);
                row.CreateCell(6).SetCellValue(tool.Status);
            }

            for (int i = 0; i < 7; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
            }
        }

        /// <summary>
        /// 从Excel导入保养记录
        /// </summary>
        public int ImportMaintenanceRecordFromExcel(string filePath)
        {
            var records = LoadRecords();
            int count = 0;

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheetAt(0);

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null) continue;

                    var record = new MaintenanceRecord
                    {
                        RecordId = Guid.NewGuid().ToString(),
                        TargetType = GetCellValue(row.GetCell(0)),
                        TargetId = GetCellValue(row.GetCell(1)),
                        MaintenanceTime = DateTime.TryParse(GetCellValue(row.GetCell(2)), out DateTime mainTime) ? mainTime : DateTime.Now,
                        Operator = GetCellValue(row.GetCell(3)),
                        MaintenanceItems = GetCellValue(row.GetCell(4)),
                        Result = GetCellValue(row.GetCell(5)),
                        Notes = GetCellValue(row.GetCell(6)),
                        NextMaintenanceDate = DateTime.TryParse(GetCellValue(row.GetCell(7)), out DateTime nextDate) ? nextDate : DateTime.Now.AddDays(30)
                    };

                    if (!string.IsNullOrEmpty(record.TargetId))
                    {
                        records.Add(record);
                        count++;
                    }
                }
            }

            SaveRecords(records);
            return count;
        }

        /// <summary>
        /// 导出保养记录到Excel
        /// </summary>
        public void ExportMaintenanceRecordToExcel(string filePath)
        {
            var records = LoadRecords();
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("保养记录");

            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("类型");
            headerRow.CreateCell(1).SetCellValue("编号");
            headerRow.CreateCell(2).SetCellValue("保养时间");
            headerRow.CreateCell(3).SetCellValue("操作员");
            headerRow.CreateCell(4).SetCellValue("保养项目");
            headerRow.CreateCell(5).SetCellValue("保养结果");
            headerRow.CreateCell(6).SetCellValue("备注");
            headerRow.CreateCell(7).SetCellValue("下次保养日期");

            for (int i = 0; i < records.Count; i++)
            {
                var record = records[i];
                var row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(record.TargetType);
                row.CreateCell(1).SetCellValue(record.TargetId);
                row.CreateCell(2).SetCellValue(record.MaintenanceTime.ToString("yyyy-MM-dd HH:mm:ss"));
                row.CreateCell(3).SetCellValue(record.Operator);
                row.CreateCell(4).SetCellValue(record.MaintenanceItems);
                row.CreateCell(5).SetCellValue(record.Result);
                row.CreateCell(6).SetCellValue(record.Notes);
                row.CreateCell(7).SetCellValue(record.NextMaintenanceDate.ToString("yyyy-MM-dd"));
            }

            for (int i = 0; i < 8; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
            }
        }

        /// <summary>
        /// 生成Excel模板文件
        /// </summary>
        public void GenerateTemplates(string folderPath)
        {
            GenerateEquipmentTemplate(Path.Combine(folderPath, "设备数据模板.xlsx"));
            GenerateToolTemplate(Path.Combine(folderPath, "工装数据模板.xlsx"));
            GenerateMaintenancePlanTemplate(Path.Combine(folderPath, "保养计划模板.xlsx"));
            GenerateMaintenanceRecordTemplate(Path.Combine(folderPath, "保养记录模板.xlsx"));
        }

        private void GenerateEquipmentTemplate(string filePath)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("设备数据");

            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("设备ID");
            headerRow.CreateCell(1).SetCellValue("线别储位");
            headerRow.CreateCell(2).SetCellValue("类别");
            headerRow.CreateCell(3).SetCellValue("子类别");
            headerRow.CreateCell(4).SetCellValue("保养周期(天)");
            headerRow.CreateCell(5).SetCellValue("下次保养日期");
            headerRow.CreateCell(6).SetCellValue("状态");
            headerRow.CreateCell(7).SetCellValue("操作员ID");
            headerRow.CreateCell(8).SetCellValue("备注");

            var exampleRow = sheet.CreateRow(1);
            exampleRow.CreateCell(0).SetCellValue("EQ001");
            exampleRow.CreateCell(1).SetCellValue("A线-01");
            exampleRow.CreateCell(2).SetCellValue("生产设备");
            exampleRow.CreateCell(3).SetCellValue("印刷机");
            exampleRow.CreateCell(4).SetCellValue("30");
            exampleRow.CreateCell(5).SetCellValue(DateTime.Now.AddDays(30).ToString("yyyy-MM-dd"));
            exampleRow.CreateCell(6).SetCellValue("正常使用");
            exampleRow.CreateCell(7).SetCellValue("OP001");
            exampleRow.CreateCell(8).SetCellValue("示例数据");

            for (int i = 0; i < 9; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
            }
        }

        private void GenerateToolTemplate(string filePath)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("工装数据");

            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("工装编码");
            headerRow.CreateCell(1).SetCellValue("线别储位");
            headerRow.CreateCell(2).SetCellValue("类别");
            headerRow.CreateCell(3).SetCellValue("子类别");
            headerRow.CreateCell(4).SetCellValue("工单号");
            headerRow.CreateCell(5).SetCellValue("工单数量");
            headerRow.CreateCell(6).SetCellValue("拼料数量");
            headerRow.CreateCell(7).SetCellValue("刮刀数量");
            headerRow.CreateCell(8).SetCellValue("当前使用次数");
            headerRow.CreateCell(9).SetCellValue("总使用次数");
            headerRow.CreateCell(10).SetCellValue("保养间隔");
            headerRow.CreateCell(11).SetCellValue("下次保养日期");
            headerRow.CreateCell(12).SetCellValue("状态");
            headerRow.CreateCell(13).SetCellValue("备注");

            var exampleRow = sheet.CreateRow(1);
            exampleRow.CreateCell(0).SetCellValue("TOOL001");
            exampleRow.CreateCell(1).SetCellValue("B线-02");
            exampleRow.CreateCell(2).SetCellValue("钢网");
            exampleRow.CreateCell(3).SetCellValue("标准钢网");
            exampleRow.CreateCell(4).SetCellValue("WO20240101");
            exampleRow.CreateCell(5).SetCellValue("10000");
            exampleRow.CreateCell(6).SetCellValue("2");
            exampleRow.CreateCell(7).SetCellValue("5");
            exampleRow.CreateCell(8).SetCellValue("500");
            exampleRow.CreateCell(9).SetCellValue("200000");
            exampleRow.CreateCell(10).SetCellValue("200000次");
            exampleRow.CreateCell(11).SetCellValue(DateTime.Now.AddDays(30).ToString("yyyy-MM-dd"));
            exampleRow.CreateCell(12).SetCellValue("正常使用");
            exampleRow.CreateCell(13).SetCellValue("示例数据");

            for (int i = 0; i < 14; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
            }
        }

        private void GenerateMaintenancePlanTemplate(string filePath)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("保养计划");

            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("类型");
            headerRow.CreateCell(1).SetCellValue("编号");
            headerRow.CreateCell(2).SetCellValue("下次保养日期");

            var exampleRow1 = sheet.CreateRow(1);
            exampleRow1.CreateCell(0).SetCellValue("设备");
            exampleRow1.CreateCell(1).SetCellValue("EQ001");
            exampleRow1.CreateCell(2).SetCellValue(DateTime.Now.AddDays(30).ToString("yyyy-MM-dd"));

            var exampleRow2 = sheet.CreateRow(2);
            exampleRow2.CreateCell(0).SetCellValue("工装");
            exampleRow2.CreateCell(1).SetCellValue("TOOL001");
            exampleRow2.CreateCell(2).SetCellValue(DateTime.Now.AddDays(15).ToString("yyyy-MM-dd"));

            for (int i = 0; i < 3; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
            }
        }

        private void GenerateMaintenanceRecordTemplate(string filePath)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("保养记录");

            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("类型");
            headerRow.CreateCell(1).SetCellValue("编号");
            headerRow.CreateCell(2).SetCellValue("保养时间");
            headerRow.CreateCell(3).SetCellValue("操作员");
            headerRow.CreateCell(4).SetCellValue("保养项目");
            headerRow.CreateCell(5).SetCellValue("保养结果");
            headerRow.CreateCell(6).SetCellValue("备注");
            headerRow.CreateCell(7).SetCellValue("下次保养日期");

            var exampleRow = sheet.CreateRow(1);
            exampleRow.CreateCell(0).SetCellValue("设备");
            exampleRow.CreateCell(1).SetCellValue("EQ001");
            exampleRow.CreateCell(2).SetCellValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            exampleRow.CreateCell(3).SetCellValue("张三");
            exampleRow.CreateCell(4).SetCellValue("清洁、润滑、检查");
            exampleRow.CreateCell(5).SetCellValue("正常");
            exampleRow.CreateCell(6).SetCellValue("保养完成");
            exampleRow.CreateCell(7).SetCellValue(DateTime.Now.AddDays(30).ToString("yyyy-MM-dd"));

            for (int i = 0; i < 8; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
            }
        }

        private string GetCellValue(ICell cell)
        {
            if (cell == null) return string.Empty;

            return cell.CellType switch
            {
                CellType.String => cell.StringCellValue,
                CellType.Numeric => cell.NumericCellValue.ToString(),
                CellType.Boolean => cell.BooleanCellValue.ToString(),
                CellType.Formula => cell.StringCellValue,
                _ => string.Empty
            };
        }

        #endregion
    }
}
