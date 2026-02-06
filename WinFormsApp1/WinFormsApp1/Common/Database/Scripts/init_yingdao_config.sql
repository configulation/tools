-- 影刀自动提交配置初始化脚本

-- 插入自动启动配置
INSERT INTO sys_config (config_key, config_name, config_value, data_type, category, sort_order, is_editable, remark)
VALUES ('YINGDAO_AUTO_START', '影刀自动启动', '1', 'BOOLEAN', 'yingdao', 1, 1, '是否启用自动执行，1=启用，0=禁用');

-- 插入执行日期配置
INSERT INTO sys_config (config_key, config_name, config_value, data_type, category, sort_order, is_editable, remark)
VALUES ('YINGDAO_ENABLED_DAYS', '影刀执行日期', '[0,1,2,3,4]', 'STRING', 'yingdao', 2, 1, '执行日期配置，JSON数组，0=周日，1=周一...6=周六');

-- 插入用户配置主记录
INSERT INTO sys_config (config_key, config_name, config_value, data_type, category, sort_order, is_editable, remark)
VALUES ('YINGDAO_USERS', '影刀用户列表', NULL, 'DROPDOWN', 'yingdao', 3, 1, '影刀自动提交用户配置列表');

-- 插入示例用户（可选，根据实际情况修改或删除）
-- INSERT INTO sys_config_option (config_key, option_value, option_label, sort_order, is_default, is_active, extra_data)
-- VALUES ('YINGDAO_USERS', '员工号1', '总厂-员工号1', 1, 0, 1, 
--   '{"Enabled":true,"Factory":"总厂","EmployeeId":"员工号1","Phone":"13800138000","CarNo":"粤A12345","LastSubmittedDate":"","LastResult":""}');
