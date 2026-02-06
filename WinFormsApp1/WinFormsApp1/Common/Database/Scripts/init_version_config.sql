-- ============================================================
-- 设备工装保养系统 - 版本控制配置
-- 只需在 sys_config 表中插入一行数据即可
-- ============================================================

-- 先清理旧数据（幂等）
DELETE FROM sys_config 
WHERE config_key IN ('APP_VERSION', 'EQUIPMENT_SYSTEM_CURRENT_VERSION', 'EQUIPMENT_SYSTEM_LATEST_VERSION', 'EQUIPMENT_SYSTEM_REQUIRED_VERSION');

-- 插入版本控制配置（一行数据）
INSERT INTO sys_config (config_key, config_name, config_value, data_type, category, sort_order, is_editable, remark, created_time, updated_time)
VALUES (
    'APP_VERSION',                          -- config_key：程序用这个 key 去查
    '设备保养系统要求版本',                    -- config_name：配置项中文名
    '1.0.0',                                -- config_value：要求的最低版本号（与程序内 APP_VERSION 比对）
    'STRING',                               -- data_type
    'system',                               -- category：归类为系统级配置
    0,                                      -- sort_order
    1,                                      -- is_editable：允许修改
    '控制设备保养系统的准入版本。程序启动时会用内置版本号与此值比对，若程序版本低于此值则拦截禁止使用。发布新版本后将此值改为新版本号即可强制用户升级。',
    NOW(),
    NOW()
);

-- ============================================================
-- 使用说明：
--   正常运行：程序内 APP_VERSION = "1.0.0"，数据库 config_value = "1.0.0" → 放行
--   强制升级：UPDATE sys_config SET config_value = '2.0.0', updated_time = NOW() WHERE config_key = 'APP_VERSION';
--            → 所有 APP_VERSION < 2.0.0 的旧程序都会被拦截
--   恢复放行：UPDATE sys_config SET config_value = '1.0.0', updated_time = NOW() WHERE config_key = 'APP_VERSION';
-- ============================================================
