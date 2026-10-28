CREATE DATABASE IF NOT EXISTS ql_dangnhap CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE ql_dangnhap;

CREATE TABLE IF NOT EXISTS NguoiDung (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(100) NOT NULL
);

INSERT IGNORE INTO NguoiDung (username, password) VALUES
('admin', '123456'),
('nguyen', '123456');
