-- ============================================
-- 设备维护系统完整初始化脚本
-- 创建日期: 2026-02-06
-- 说明: 此脚本包含所有表的创建和初始化数据
-- 使用方法: 在MySQL中执行此脚本即可完成数据库初始化
-- ============================================

-- ============================================
-- 第一部分: 创建数据库表
-- ============================================

-- 1. 设备表
DROP TABLE IF EXISTS `equipment`;
CREATE TABLE `equipment` (
  `EquipmentId` VARCHAR(50) NOT NULL COMMENT '设备ID（唯一标识）',
  `LineLocation` VARCHAR(100) NOT NULL COMMENT '线别储位',
  `Category` VARCHAR(50) NOT NULL COMMENT '类别',
  `SubCategory` VARCHAR(50) NOT NULL COMMENT '子类别',
  `MaintenanceIntervalDays` INT NOT NULL DEFAULT 30 COMMENT '保养周期（天）',
  `NextMaintenanceDate` DATETIME NOT NULL COMMENT '下次保养日期',
  `Status` VARCHAR(50) NOT NULL DEFAULT '正常使用' COMMENT '状态（新购、正常使用、保养中、维修中）',
  `OperatorId` VARCHAR(50) DEFAULT NULL COMMENT '操作员ID',
  `Notes` VARCHAR(500) DEFAULT NULL COMMENT '备注',
  `CreateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`EquipmentId`),
  INDEX `idx_line_location` (`LineLocation`),
  INDEX `idx_category` (`Category`),
  INDEX `idx_status` (`Status`),
  INDEX `idx_next_maintenance` (`NextMaintenanceDate`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='设备信息表';

-- 2. 工装表
DROP TABLE IF EXISTS `tool`;
CREATE TABLE `tool` (
  `ToolCode` VARCHAR(50) NOT NULL COMMENT '工装编码（唯一标识）',
  `LineLocation` VARCHAR(100) NOT NULL COMMENT '线别储位',
  `Category` VARCHAR(50) NOT NULL COMMENT '类别',
  `SubCategory` VARCHAR(50) NOT NULL COMMENT '子类别',
  `WorkOrder` VARCHAR(50) DEFAULT NULL COMMENT '工单号',
  `OrderQuantity` INT NOT NULL DEFAULT 0 COMMENT '工单数量',
  `PanelQuantity` INT NOT NULL DEFAULT 1 COMMENT '拼料数量',
  `ScraperCount` INT NOT NULL DEFAULT 1 COMMENT '刮刀数量',
  `UsageCount` INT NOT NULL DEFAULT 0 COMMENT '当前使用次数',
  `TotalUsage` INT NOT NULL DEFAULT 0 COMMENT '总使用次数限制',
  `MaintenanceInterval` VARCHAR(50) DEFAULT NULL COMMENT '保养间隔（描述，如"200000次"）',
  `NextMaintenanceDate` DATETIME NOT NULL COMMENT '下次保养日期',
  `IssueTime` DATETIME DEFAULT NULL COMMENT '发放时间',
  `ReturnTime` DATETIME DEFAULT NULL COMMENT '退回时间',
  `Status` VARCHAR(50) NOT NULL DEFAULT '正常使用' COMMENT '状态（新购、正常使用、保养中、维修中）',
  `Notes` VARCHAR(500) DEFAULT NULL COMMENT '备注',
  `CreateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`ToolCode`),
  INDEX `idx_line_location` (`LineLocation`),
  INDEX `idx_category` (`Category`),
  INDEX `idx_status` (`Status`),
  INDEX `idx_work_order` (`WorkOrder`),
  INDEX `idx_next_maintenance` (`NextMaintenanceDate`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='工装信息表';

-- 3. 保养记录表
DROP TABLE IF EXISTS `maintenance_record`;
CREATE TABLE `maintenance_record` (
  `RecordId` VARCHAR(50) NOT NULL COMMENT '记录ID',
  `TargetId` VARCHAR(50) NOT NULL COMMENT '目标ID（设备ID或工装编码）',
  `TargetType` VARCHAR(20) NOT NULL COMMENT '目标类型（Equipment/Tool）',
  `MaintenanceTime` DATETIME NOT NULL COMMENT '保养时间',
  `Operator` VARCHAR(50) NOT NULL COMMENT '操作员',
  `MaintenanceItems` VARCHAR(500) DEFAULT NULL COMMENT '保养项目（逗号分隔）',
  `Result` VARCHAR(50) NOT NULL COMMENT '保养结果',
  `Notes` VARCHAR(500) DEFAULT NULL COMMENT '备注',
  `NextMaintenanceDate` DATETIME NOT NULL COMMENT '下次保养日期',
  PRIMARY KEY (`RecordId`),
  INDEX `idx_target` (`TargetId`, `TargetType`),
  INDEX `idx_maintenance_time` (`MaintenanceTime`),
  INDEX `idx_operator` (`Operator`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='保养记录表';

-- 4. 保养项目关联表
DROP TABLE IF EXISTS `maintenance_item`;
CREATE TABLE `maintenance_item` (
  `Id` INT NOT NULL AUTO_INCREMENT COMMENT '自增ID',
  `TargetId` VARCHAR(50) NOT NULL COMMENT '目标ID（设备ID或工装编码）',
  `TargetType` VARCHAR(20) NOT NULL COMMENT '目标类型（Equipment/Tool）',
  `ItemName` VARCHAR(200) NOT NULL COMMENT '保养项目名称',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `uk_target_item` (`TargetId`, `TargetType`, `ItemName`),
  INDEX `idx_target` (`TargetId`, `TargetType`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='保养项目关联表';

-- 5. 线别储位基础数据表
DROP TABLE IF EXISTS `base_line_location`;
CREATE TABLE `base_line_location` (
  `Id` VARCHAR(50) NOT NULL COMMENT '主键ID',
  `Code` VARCHAR(50) NOT NULL COMMENT '编码',
  `Name` VARCHAR(100) NOT NULL COMMENT '名称',
  `Description` VARCHAR(200) DEFAULT NULL COMMENT '描述',
  `IsActive` TINYINT(1) NOT NULL DEFAULT 1 COMMENT '是否启用',
  `CreateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `uk_code` (`Code`),
  INDEX `idx_is_active` (`IsActive`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='线别储位基础数据表';

-- 6. 类别基础数据表
DROP TABLE IF EXISTS `base_category`;
CREATE TABLE `base_category` (
  `Id` VARCHAR(50) NOT NULL COMMENT '主键ID',
  `Code` VARCHAR(50) NOT NULL COMMENT '编码',
  `Name` VARCHAR(100) NOT NULL COMMENT '名称',
  `Type` VARCHAR(20) NOT NULL COMMENT '类型（Equipment/Tool）',
  `IsActive` TINYINT(1) NOT NULL DEFAULT 1 COMMENT '是否启用',
  `CreateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `uk_code_type` (`Code`, `Type`),
  INDEX `idx_type` (`Type`),
  INDEX `idx_is_active` (`IsActive`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='类别基础数据表';

-- 7. 子类别基础数据表
DROP TABLE IF EXISTS `base_sub_category`;
CREATE TABLE `base_sub_category` (
  `Id` VARCHAR(50) NOT NULL COMMENT '主键ID',
  `CategoryId` VARCHAR(50) NOT NULL COMMENT '所属类别ID',
  `Code` VARCHAR(50) NOT NULL COMMENT '编码',
  `Name` VARCHAR(100) NOT NULL COMMENT '名称',
  `IsActive` TINYINT(1) NOT NULL DEFAULT 1 COMMENT '是否启用',
  `CreateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `uk_category_code` (`CategoryId`, `Code`),
  INDEX `idx_category` (`CategoryId`),
  INDEX `idx_is_active` (`IsActive`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='子类别基础数据表';

-- 8. 保养项目基础数据表
DROP TABLE IF EXISTS `base_maintenance_item`;
CREATE TABLE `base_maintenance_item` (
  `Id` VARCHAR(50) NOT NULL COMMENT '主键ID',
  `Code` VARCHAR(50) NOT NULL COMMENT '编码',
  `Name` VARCHAR(100) NOT NULL COMMENT '名称',
  `Type` VARCHAR(20) NOT NULL COMMENT '类型（Equipment/Tool）',
  `CategoryId` VARCHAR(50) DEFAULT NULL COMMENT '所属类别ID',
  `Description` VARCHAR(200) DEFAULT NULL COMMENT '描述',
  `StandardDuration` INT NOT NULL DEFAULT 0 COMMENT '标准耗时（分钟）',
  `IsActive` TINYINT(1) NOT NULL DEFAULT 1 COMMENT '是否启用',
  `CreateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `uk_code_type` (`Code`, `Type`),
  INDEX `idx_type` (`Type`),
  INDEX `idx_category` (`CategoryId`),
  INDEX `idx_is_active` (`IsActive`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='保养项目基础数据表';

-- 9. 操作员基础数据表
DROP TABLE IF EXISTS `base_operator`;
CREATE TABLE `base_operator` (
  `Id` VARCHAR(50) NOT NULL COMMENT '主键ID',
  `Code` VARCHAR(50) NOT NULL COMMENT '工号',
  `Name` VARCHAR(100) NOT NULL COMMENT '姓名',
  `Department` VARCHAR(100) DEFAULT NULL COMMENT '部门',
  `Phone` VARCHAR(20) DEFAULT NULL COMMENT '电话',
  `IsActive` TINYINT(1) NOT NULL DEFAULT 1 COMMENT '是否启用',
  `CreateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `uk_code` (`Code`),
  INDEX `idx_name` (`Name`),
  INDEX `idx_is_active` (`IsActive`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='操作员基础数据表';

-- ============================================
-- 第二部分: 插入基础数据
-- ============================================

-- 1. 线别储位基础数据
INSERT INTO `base_line_location` (`Id`, `Code`, `Name`, `Description`, `IsActive`, `CreateTime`, `UpdateTime`)
VALUES
('LL001', 'A-01', 'A线-01工位', 'A生产线第1工位', 1, NOW(), NOW()),
('LL002', 'A-02', 'A线-02工位', 'A生产线第2工位', 1, NOW(), NOW()),
('LL003', 'A-03', 'A线-03工位', 'A生产线第3工位', 1, NOW(), NOW()),
('LL004', 'B-01', 'B线-01工位', 'B生产线第1工位', 1, NOW(), NOW()),
('LL005', 'B-02', 'B线-02工位', 'B生产线第2工位', 1, NOW(), NOW()),
('LL006', 'B-03', 'B线-03工位', 'B生产线第3工位', 1, NOW(), NOW()),
('LL007', 'C-01', 'C线-01工位', 'C生产线第1工位', 1, NOW(), NOW()),
('LL008', 'C-02', 'C线-02工位', 'C生产线第2工位', 1, NOW(), NOW()),
('LL009', 'STORE-01', '仓库-01区', '仓库存储区域01', 1, NOW(), NOW()),
('LL010', 'STORE-02', '仓库-02区', '仓库存储区域02', 1, NOW(), NOW());

-- 2. 设备类别基础数据
INSERT INTO `base_category` (`Id`, `Code`, `Name`, `Type`, `IsActive`, `CreateTime`, `UpdateTime`)
VALUES
('EC001', 'PROD', '生产设备', 'Equipment', 1, NOW(), NOW()),
('EC002', 'TEST', '检测设备', 'Equipment', 1, NOW(), NOW()),
('EC003', 'PACK', '包装设备', 'Equipment', 1, NOW(), NOW()),
('EC004', 'TRANS', '运输设备', 'Equipment', 1, NOW(), NOW()),
('EC005', 'AUX', '辅助设备', 'Equipment', 1, NOW(), NOW());

-- 3. 工装类别基础数据
INSERT INTO `base_category` (`Id`, `Code`, `Name`, `Type`, `IsActive`, `CreateTime`, `UpdateTime`)
VALUES
('TC001', 'STENCIL', '钢网', 'Tool', 1, NOW(), NOW()),
('TC002', 'FIXTURE', '治具', 'Tool', 1, NOW(), NOW()),
('TC003', 'MOLD', '模具', 'Tool', 1, NOW(), NOW()),
('TC004', 'BLADE', '刀具', 'Tool', 1, NOW(), NOW()),
('TC005', 'GAUGE', '量具', 'Tool', 1, NOW(), NOW());

-- 4. 设备子类别基础数据
INSERT INTO `base_sub_category` (`Id`, `CategoryId`, `Code`, `Name`, `IsActive`, `CreateTime`, `UpdateTime`)
VALUES
-- 生产设备子类别
('ESC001', 'EC001', 'PRINT', '印刷机', 1, NOW(), NOW()),
('ESC002', 'EC001', 'MOUNT', '贴片机', 1, NOW(), NOW()),
('ESC003', 'EC001', 'REFLOW', '回流焊', 1, NOW(), NOW()),
('ESC004', 'EC001', 'WAVE', '波峰焊', 1, NOW(), NOW()),
-- 检测设备子类别
('ESC005', 'EC002', 'AOI', 'AOI检测仪', 1, NOW(), NOW()),
('ESC006', 'EC002', 'XRAY', 'X-RAY检测仪', 1, NOW(), NOW()),
('ESC007', 'EC002', 'ICT', 'ICT测试仪', 1, NOW(), NOW()),
('ESC008', 'EC002', 'FCT', 'FCT测试仪', 1, NOW(), NOW()),
-- 包装设备子类别
('ESC009', 'EC003', 'SEAL', '封口机', 1, NOW(), NOW()),
('ESC010', 'EC003', 'LABEL', '贴标机', 1, NOW(), NOW()),
-- 运输设备子类别
('ESC011', 'EC004', 'CONV', '传送带', 1, NOW(), NOW()),
('ESC012', 'EC004', 'LIFT', '升降机', 1, NOW(), NOW()),
-- 辅助设备子类别
('ESC013', 'EC005', 'COMP', '空压机', 1, NOW(), NOW()),
('ESC014', 'EC005', 'COOL', '冷却机', 1, NOW(), NOW());

-- 5. 工装子类别基础数据
INSERT INTO `base_sub_category` (`Id`, `CategoryId`, `Code`, `Name`, `IsActive`, `CreateTime`, `UpdateTime`)
VALUES
-- 钢网子类别
('TSC001', 'TC001', 'STD', '标准钢网', 1, NOW(), NOW()),
('TSC002', 'TC001', 'PREC', '精密钢网', 1, NOW(), NOW()),
('TSC003', 'TC001', 'STEP', '阶梯钢网', 1, NOW(), NOW()),
-- 治具子类别
('TSC004', 'TC002', 'TEST', '测试治具', 1, NOW(), NOW()),
('TSC005', 'TC002', 'HOLD', '夹持治具', 1, NOW(), NOW()),
('TSC006', 'TC002', 'POS', '定位治具', 1, NOW(), NOW()),
-- 模具子类别
('TSC007', 'TC003', 'INJ', '注塑模具', 1, NOW(), NOW()),
('TSC008', 'TC003', 'STAMP', '冲压模具', 1, NOW(), NOW()),
-- 刀具子类别
('TSC009', 'TC004', 'CUT', '切割刀', 1, NOW(), NOW()),
('TSC010', 'TC004', 'MILL', '铣刀', 1, NOW(), NOW()),
-- 量具子类别
('TSC011', 'TC005', 'CALI', '卡尺', 1, NOW(), NOW()),
('TSC012', 'TC005', 'MICRO', '千分尺', 1, NOW(), NOW());

-- 6. 设备保养项目基础数据
INSERT INTO `base_maintenance_item` (`Id`, `Code`, `Name`, `Type`, `CategoryId`, `Description`, `StandardDuration`, `IsActive`, `CreateTime`, `UpdateTime`)
VALUES
-- 通用设备保养项目
('EMI001', 'CLEAN', '清洁', 'Equipment', NULL, '设备外观及内部清洁', 30, 1, NOW(), NOW()),
('EMI002', 'LUBR', '润滑', 'Equipment', NULL, '运动部件润滑保养', 20, 1, NOW(), NOW()),
('EMI003', 'CHECK', '检查', 'Equipment', NULL, '设备功能检查', 15, 1, NOW(), NOW()),
('EMI004', 'CALIB', '校准', 'Equipment', NULL, '设备精度校准', 45, 1, NOW(), NOW()),
('EMI005', 'TIGHT', '紧固', 'Equipment', NULL, '螺丝紧固检查', 10, 1, NOW(), NOW()),
-- 生产设备专用
('EMI006', 'NOZZLE', '喷嘴清洁', 'Equipment', 'EC001', '贴片机喷嘴清洁', 25, 1, NOW(), NOW()),
('EMI007', 'BLADE', '刮刀更换', 'Equipment', 'EC001', '印刷机刮刀更换', 15, 1, NOW(), NOW()),
('EMI008', 'TEMP', '温度校准', 'Equipment', 'EC001', '回流焊温度校准', 60, 1, NOW(), NOW()),
-- 检测设备专用
('EMI009', 'LENS', '镜头清洁', 'Equipment', 'EC002', 'AOI镜头清洁', 20, 1, NOW(), NOW()),
('EMI010', 'LIGHT', '光源检查', 'Equipment', 'EC002', '检测设备光源检查', 15, 1, NOW(), NOW());

-- 7. 工装保养项目基础数据
INSERT INTO `base_maintenance_item` (`Id`, `Code`, `Name`, `Type`, `CategoryId`, `Description`, `StandardDuration`, `IsActive`, `CreateTime`, `UpdateTime`)
VALUES
-- 通用工装保养项目
('TMI001', 'WASH', '清洗', 'Tool', NULL, '工装清洗', 20, 1, NOW(), NOW()),
('TMI002', 'WEAR', '检查磨损', 'Tool', NULL, '工装磨损检查', 15, 1, NOW(), NOW()),
('TMI003', 'PREC', '检查精度', 'Tool', NULL, '工装精度检查', 30, 1, NOW(), NOW()),
('TMI004', 'REPAIR', '修复', 'Tool', NULL, '工装修复', 60, 1, NOW(), NOW()),
-- 钢网专用
('TMI005', 'ULTRA', '超声波清洗', 'Tool', 'TC001', '钢网超声波清洗', 25, 1, NOW(), NOW()),
('TMI006', 'HOLE', '开孔检查', 'Tool', 'TC001', '钢网开孔检查', 20, 1, NOW(), NOW()),
('TMI007', 'THICK', '厚度测量', 'Tool', 'TC001', '钢网厚度测量', 10, 1, NOW(), NOW()),
-- 治具专用
('TMI008', 'PIN', '定位销检查', 'Tool', 'TC002', '治具定位销检查', 15, 1, NOW(), NOW()),
('TMI009', 'SPRING', '弹簧检查', 'Tool', 'TC002', '治具弹簧检查', 10, 1, NOW(), NOW()),
-- 刀具专用
('TMI010', 'SHARP', '刃口检查', 'Tool', 'TC004', '刀具刃口检查', 10, 1, NOW(), NOW()),
('TMI011', 'GRIND', '研磨', 'Tool', 'TC004', '刀具研磨', 30, 1, NOW(), NOW());

-- 8. 操作员基础数据
INSERT INTO `base_operator` (`Id`, `Code`, `Name`, `Department`, `Phone`, `IsActive`, `CreateTime`, `UpdateTime`)
VALUES
('OP001', 'E001', '张三', '生产部', '13800138001', 1, NOW(), NOW()),
('OP002', 'E002', '李四', '生产部', '13800138002', 1, NOW(), NOW()),
('OP003', 'E003', '王五', '设备部', '13800138003', 1, NOW(), NOW()),
('OP004', 'E004', '赵六', '设备部', '13800138004', 1, NOW(), NOW()),
('OP005', 'E005', '钱七', '质量部', '13800138005', 1, NOW(), NOW()),
('OP006', 'E006', '孙八', '质量部', '13800138006', 1, NOW(), NOW()),
('OP007', 'E007', '周九', '工程部', '13800138007', 1, NOW(), NOW()),
('OP008', 'E008', '吴十', '工程部', '13800138008', 1, NOW(), NOW()),
('OP009', 'E009', '郑十一', '仓库', '13800138009', 1, NOW(), NOW()),
('OP010', 'E010', '陈十二', '仓库', '13800138010', 1, NOW(), NOW());

-- ============================================
-- 第三部分: 插入示例数据（可选）
-- ============================================

-- 1. 插入示例设备数据
INSERT INTO `equipment` (`EquipmentId`, `LineLocation`, `Category`, `SubCategory`, `MaintenanceIntervalDays`, `NextMaintenanceDate`, `Status`, `OperatorId`, `Notes`, `CreateTime`, `UpdateTime`)
VALUES
('EQ001', 'A-01', '生产设备', '印刷机', 30, DATE_ADD(NOW(), INTERVAL 30 DAY), '正常使用', 'E001', 'DEK印刷机，型号：Horizon 03iX', NOW(), NOW()),
('EQ002', 'A-02', '生产设备', '贴片机', 30, DATE_ADD(NOW(), INTERVAL 25 DAY), '正常使用', 'E002', 'JUKI贴片机，型号：RS-1R', NOW(), NOW()),
('EQ003', 'A-03', '生产设备', '回流焊', 45, DATE_ADD(NOW(), INTERVAL 40 DAY), '正常使用', 'E001', 'BTU回流焊，型号：Pyramax 150N', NOW(), NOW()),
('EQ004', 'B-01', '检测设备', 'AOI检测仪', 15, DATE_ADD(NOW(), INTERVAL 15 DAY), '正常使用', 'E003', 'SAKI AOI，型号：BF-3Di', NOW(), NOW()),
('EQ005', 'B-02', '检测设备', 'X-RAY检测仪', 20, DATE_ADD(NOW(), INTERVAL 18 DAY), '正常使用', 'E003', 'Nordson X-RAY，型号：Dage XD7600NT', NOW(), NOW());

-- 2. 插入示例工装数据
INSERT INTO `tool` (`ToolCode`, `LineLocation`, `Category`, `SubCategory`, `WorkOrder`, `OrderQuantity`, `PanelQuantity`, `ScraperCount`, `UsageCount`, `TotalUsage`, `MaintenanceInterval`, `NextMaintenanceDate`, `Status`, `Notes`, `CreateTime`, `UpdateTime`)
VALUES
('TOOL001', 'A-01', '钢网', '标准钢网', 'WO20260201001', 10000, 2, 5, 500, 200000, '200000次', DATE_ADD(NOW(), INTERVAL 30 DAY), '正常使用', '标准钢网，厚度：0.12mm', NOW(), NOW()),
('TOOL002', 'A-01', '钢网', '精密钢网', 'WO20260201002', 8000, 2, 4, 400, 150000, '150000次', DATE_ADD(NOW(), INTERVAL 20 DAY), '正常使用', '精密钢网，厚度：0.10mm', NOW(), NOW()),
('TOOL003', 'B-01', '治具', '测试治具', 'WO20260201004', 5000, 1, 1, 100, 100000, '100000次', DATE_ADD(NOW(), INTERVAL 10 DAY), '正常使用', 'ICT测试治具，编号：JIG-001', NOW(), NOW());

-- 3. 插入示例保养项目关联
INSERT INTO `maintenance_item` (`TargetId`, `TargetType`, `ItemName`)
VALUES
('EQ001', 'Equipment', '清洁'),
('EQ001', 'Equipment', '润滑'),
('EQ001', 'Equipment', '检查'),
('EQ001', 'Equipment', '刮刀更换'),
('EQ002', 'Equipment', '清洁'),
('EQ002', 'Equipment', '润滑'),
('EQ002', 'Equipment', '校准'),
('EQ002', 'Equipment', '喷嘴清洁'),
('TOOL001', 'Tool', '清洗'),
('TOOL001', 'Tool', '检查磨损'),
('TOOL001', 'Tool', '超声波清洗'),
('TOOL001', 'Tool', '开孔检查'),
('TOOL002', 'Tool', '清洗'),
('TOOL002', 'Tool', '检查精度'),
('TOOL003', 'Tool', '清洗'),
('TOOL003', 'Tool', '检查磨损');

-- 4. 插入示例保养记录
INSERT INTO `maintenance_record` (`RecordId`, `TargetId`, `TargetType`, `MaintenanceTime`, `Operator`, `MaintenanceItems`, `Result`, `Notes`, `NextMaintenanceDate`)
VALUES
(UUID(), 'EQ001', 'Equipment', DATE_SUB(NOW(), INTERVAL 30 DAY), '张三', '清洁、润滑、检查、刮刀更换', '正常', '保养完成，设备运行正常', DATE_ADD(NOW(), INTERVAL 30 DAY)),
(UUID(), 'EQ002', 'Equipment', DATE_SUB(NOW(), INTERVAL 25 DAY), '李四', '清洁、润滑、校准、喷嘴清洁', '正常', '保养完成，校准数据已记录', DATE_ADD(NOW(), INTERVAL 25 DAY)),
(UUID(), 'TOOL001', 'Tool', DATE_SUB(NOW(), INTERVAL 30 DAY), '周九', '清洗、检查磨损、超声波清洗、开孔检查', '正常', '保养完成，钢网状态良好', DATE_ADD(NOW(), INTERVAL 30 DAY));

-- ============================================
-- 初始化完成
-- ============================================
