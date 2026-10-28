CREATE DATABASE IF NOT EXISTS ql_hocvien CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE ql_hocvien;

CREATE TABLE IF NOT EXISTS HocVien (
    id INT AUTO_INCREMENT PRIMARY KEY,
    ho_ten VARCHAR(100) NOT NULL,
    sdt VARCHAR(20),
    ngay_sinh DATE,
    khoa_hoc VARCHAR(100),
    gioi_tinh VARCHAR(10),
    da_dong_hoc_phi TINYINT(1) NOT NULL DEFAULT 0
);

INSERT INTO HocVien (ho_ten, sdt, ngay_sinh, khoa_hoc, gioi_tinh, da_dong_hoc_phi)
SELECT 'Nguyễn Văn An', '0912345678', '2005-05-12', 'Lập trình C#', 'Nam', 1
WHERE NOT EXISTS (SELECT 1 FROM HocVien WHERE ho_ten = 'Nguyễn Văn An' AND sdt = '0912345678');

INSERT INTO HocVien (ho_ten, sdt, ngay_sinh, khoa_hoc, gioi_tinh, da_dong_hoc_phi)
SELECT 'Trần Minh Anh', '0987654321', '2004-10-20', 'Cơ sở dữ liệu', 'Nữ', 0
WHERE NOT EXISTS (SELECT 1 FROM HocVien WHERE ho_ten = 'Trần Minh Anh' AND sdt = '0987654321');

SELECT * FROM HocVien;
