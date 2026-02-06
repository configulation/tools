-- 设备维护系统数据库初始化脚本

-- 1. 设备表
CREATE TABLE IF NOT EXISTS `equipment` (
  `EquipmentId` VARCHAR(50) NOT NULL COMMENT '设备ID',
  `LineLocation` VARCHAR(100) NOT NULL COMMENT '线别储位',
  `Category` VARCHAR(50) NOT NULL COMMENT '类别',
  `SubCategory` VARCHAR(50) NOT NULL COMMENT '子类别',
  `MaintenanceIntervalDays` INT NOT NULL DEFAULT 30 COMMENT '保养周期(天)',
  `NextMaintenanceDate` DATETIME NOT NULL COMMENT '下次保养日期',
  `Status` VARCHAR(50) NOT NULL DEFAULT '正常使用' COMMENT '状态',
  `OperatorId` VARCHAR(50) NULL COMMENT '操作员ID',
  `Notes` VARCHAR(500) NULL COMMENT '备注',
  `CreateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`EquipmentId`),
  INDEX `idx_line_location` (`LineLocation`),
  INDEX `idx_category` (`Category`),
  INDEX `idx_next_maintenance` (`NextMaintenanceDate`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='设备表';

-- 2. 工装表
CREATE TABLE IF NOT EXISTS `tool` (
  `ToolCode` VARCHAR(50) NOT NULL COMMENT '工装编码',
  `LineLocation` VARCHAR(100) NOT NULL COMMENT '线别储位',
  `Category` VARCHAR(50) NOT NULL COMMENT '类别',
  `SubCategory` VARCHAR(50) NOT NULL COMMENT '子类别',
  `WorkOrder` VARCHAR(50) NULL COMMENT '工单号',
  `OrderQuantity` INT NOT NULL DEFAULT 0 COMMENT '工单数量',
  `PanelQuantity` INT NOT NULL DEFAULT 0 COMMENT '拼料数量',
  `ScraperCount` INT NOT NULL DEFAULT 0 COMMENT '刮刀数量',
  `UsageCount` INT NOT NULL DEFAULT 0 COMMENT '当前使用次数',
  `TotalUsage` INT NOT NULL DEFAULT 0 COMMENT '总使用次数',
  `MaintenanceInterval` VARCHAR(50) NULL COMMENT '保养间隔',
  `NextMaintenanceDate` DATETIME NOT NULL COMMENT '下次保养日期',
  `IssueTime` DATETIME NULL COMMENT '发放时间',
  `ReturnTime` DATETIME NULL COMMENT '退回时间',
  `Status` VARCHAR(50) NOT NULL DEFAULT '正常使用' COMMENT '状态',
  `Notes` VARCHAR(500) NULL COMMENT '备注',
  `CreateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`ToolCode`),
  INDEX `idx_line_location` (`LineLocation`),
  INDEX `idx_category` (`Category`),
  INDEX `idx_next_maintenance` (`NextMaintenanceDate`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='工装表';

-- 3. 保养记录表
CREATE TABLE IF NOT EXISTS `maintenance_record` (
  `RecordId` VARCHAR(50) NOT NULL COMMENT '记录ID',
  `TargetId` VARCHAR(50) NOT NULL COMMENT '目标ID(设备ID或工装编码)',
  `TargetType` VARCHAR(20) NOT NULL COMMENT '目标类型(Equipment/Tool)',
  `MaintenanceTime` DATETIME NOT NULL COMMENT '保养时间',
  `Operator` VARCHAR(50) NOT NULL COMMENT '操作员',
  `MaintenanceItems` VARCHAR(500) NULL COMMENT '保养项目',
  `Result` VARCHAR(50) NOT NULL COMMENT '保养结果',
  `Notes` VARCHAR(500) NULL COMMENT '备注',
  `NextMaintenanceDate` DATETIME NOT NULL COMMENT '下次保养日期',
  PRIMARY KEY (`RecordId`),
  INDEX `idx_target` (`TargetId`, `TargetType`),
  INDEX `idx_maintenance_time` (`MaintenanceTime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='保养记录表';

-- 4. 保养项目关联表
CREATE TABLE IF NOT EXISTS `maintenance_item` (
  `Id` INT NOT NULL AUTO_INCREMENT COMMENT '自增ID',
  `TargetId` VARCHAR(50) NOT NULL COMMENT '目标ID(设备ID或工装编码)',
  `TargetType` VARCHAR(20) NOT NULL COMMENT '目标类型(Equipment/Tool)',
  `ItemName` VARCHAR(200) NOT NULL COMMENT '保养项目名称',
  PRIMARY KEY (`Id`),
  INDEX `idx_target` (`TargetId`, `TargetType`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='保养项目关联表';
