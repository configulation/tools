- ============================================================
-- 测试版本拦截功能
-- ============================================================

-- 1. 查看当前配置
SELECT config_key, config_name, config_value, remark, updated_time 
FROM sys_config 
WHERE config_key = 'APP_VERSION';

-- 2. 模拟发布新版本：将要求版本改为 2.0.0，旧程序（1.0.0）会被拦截
UPDATE sys_config 
SET config_value = '2.0.0', updated_time = NOW() 
WHERE config_key = 'APP_VERSION';

-- 3. 恢复正常（取消拦截）
-- UPDATE sys_config SET config_value = '1.0.0', updated_time = NOW() WHERE config_key = 'APP_VERSION';
