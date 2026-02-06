-- =============================================
-- 影刀用户表初始化脚本
-- 支持多用户配置，替代 JSON 文件存储
-- =============================================

-- 创建影刀用户表
CREATE TABLE IF NOT EXISTS yingdao_users (
    id INT AUTO_INCREMENT PRIMARY KEY COMMENT '主键ID',
    username VARCHAR(100) NOT NULL COMMENT '用户名',
    password VARCHAR(100) DEFAULT '' COMMENT '密码（预留）',
    factory VARCHAR(50) DEFAULT '' COMMENT '厂区',
    employee_id VARCHAR(50) NOT NULL COMMENT '员工号',
    phone VARCHAR(20) DEFAULT '' COMMENT '手机号',
    car_no VARCHAR(20) DEFAULT '' COMMENT '车牌号',
    last_submitted_date VARCHAR(20) DEFAULT '' COMMENT '上次提交日期',
    last_result VARCHAR(50) DEFAULT '' COMMENT '上次执行结果',
    is_enabled TINYINT(1) DEFAULT 1 COMMENT '是否启用',
    remark VARCHAR(200) DEFAULT '' COMMENT '备注',
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    UNIQUE KEY uk_employee_id (employee_id),
    KEY idx_is_enabled (is_enabled),
    KEY idx_last_submitted_date (last_submitted_date)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='影刀自动提交用户表';

-- 插入示例数据（可选）
-- INSERT INTO yingdao_users (username, password, factory, employee_id, phone, car_no, is_enabled, remark)
-- VALUES 
-- ('user1', '', '总厂', 'EMP001', '13800138000', '粤A12345', 1, '测试用户1'),
-- ('user2', '', '分厂', 'EMP002', '13800138001', '粤B67890', 1, '测试用户2');

-- 查询所有用户
-- SELECT * FROM yingdao_users ORDER BY id;

-- 查询启用的用户
-- SELECT * FROM yingdao_users WHERE is_enabled = 1 ORDER BY id;
