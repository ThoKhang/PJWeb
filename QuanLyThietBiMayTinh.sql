--lệnh if exists để xóa database nếu đã tồn tại trước đó 
USE master;
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'QuanLyThietBiMayTinh')
BEGIN
    ALTER DATABASE QuanLyThietBiMayTinh SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyThietBiMayTinh;
END;
--Tạo database
create database QuanLyThietBiMayTinh
go
use QuanLyThietBiMayTinh
go
--===========================================Table====================================--
create table Tinh
(
	idTinh char(5) primary key,
	tenTinh nvarchar(30) not null,
)
create table Huyen 
(
	idHuyen char(5) primary key,
	idTinh char(5) not null foreign key references Tinh(idTinh)
	on 
			delete cascade
		on 
			update cascade,
	tenHuyen nvarchar(30) not null
)
create table XaPhuong 
(
	idXaPhuong char(5) primary key,
	idHuyen char(5) not null foreign key references Huyen(idHuyen)
	on 
			delete cascade
		on 
			update cascade,
	tenXaPhuong nvarchar(30) not null
)
create table NguoiDung
(
	idNguoiDung char(5) primary key,
	idXaPhuong char(5) not null foreign key references XaPhuong(idXaPhuong)
	on 
			delete cascade
		on 
			update cascade,
	tenDangNhap nvarchar(50) not null,
	matKhau varchar(30) not null,
	email varchar(30) not null,
	sdt char(11) not null,
	soNha nvarchar(50) not null
)
create table PhanQuyen
(
	idPhanQuyen char(5) primary key,
	tenPhanQuyen nvarchar(30) not null
)
create table LoaiSanPham
(
	idLoaiSanPham char(5) primary key,
	tenLoaiSanPham nvarchar(50) not null
)
create table CongTy
(
	idCongTy char(5) primary key,
	tenCongTy nvarchar(50) not null
)
create table SanPham 
(
	idSanPham char(5) primary key,
	idCongTy char(5) not null foreign key references CongTy(idCongTy)
	on 
			delete cascade
		on 
			update cascade,
	idLoaiSanPham char(5) not null foreign key references LoaiSanPham(idLoaiSanPham)
	on 
			delete cascade
		on 
			update cascade,
	tenSanPham nvarchar(70) not null,
	imageURL varchar(70)
)
create table DonDatHang
(
	idDonDat char(5) primary key,
	idNguoiDung char(5) not null foreign key references NguoiDung(idNguoiDung)
	on 
			delete cascade
		on 
			update cascade,
	sdtGiaoHang char(11) not null,
	soNha nvarchar(50) not null,
	trangThai nvarchar(20) not null,
	thanhToan nvarchar(15) not null,
	ngayDat datetime,
	ngayThanhToan datetime,
	ngayGiaoDuKien datetime
)
create table ChiTietDonHang
(
	idDonDat char(5) not null foreign key references DonDatHang(idDonDat)
	on 
			delete cascade
		on 
			update cascade,
	idSanPham char(5) not null foreign key references SanPham(idSanPham)
	on 
			delete cascade
		on 
			update cascade,
	primary key (idDonDat, idSanPham),
	soluong int not null,
	donGia money
)
create table PhanQuyenNguoiDung
(
	idNguoiDung char(5) not null foreign key references NguoiDung(idNguoiDung)

	on 
			delete cascade
		on 
			update cascade,
	idPhanQuyen char(5) not null foreign key references PhanQuyen(idPhanQuyen)
	on 
			delete cascade
		on 
			update cascade,
	primary key (idNguoiDung, idPhanQuyen)
)
create table ChiTietGioHang
(
	idChiTietGioHang char(5) not null,
	idNguoiDung char(5) not null foreign key references NguoiDung(idNguoiDung)
	on 
			delete cascade
		on 
			update cascade,
	idSanPham char(5) not null foreign key references SanPham(idSanPham)
		on 
			delete no action
		on 
			update cascade,
	primary key (idChiTietGioHang, idNguoiDung, idSanPham),
	soLuongTrongGio int
)
--========================================Ràng buộc=======================================-
-- ràng buộc email có @gmail.com
ALTER TABLE NguoiDung
ADD CONSTRAINT chk_Email CHECK (email LIKE '%@gmail.com');

-- ràng buộc mật khẩu: tối thiểu 8 số, có chữ Hoa, số, ký tự đặc biệt.
ALTER TABLE NguoiDung
ADD CONSTRAINT chk_MatKhau CHECK (LEN(matKhau) >= 8 AND matKhau LIKE '%[A-Z]%' AND matKhau LIKE '%[0-9]%' AND matKhau LIKE '%[^a-zA-Z0-9]%');

-- số lượng >= 0
ALTER TABLE ChiTietDonHang
ADD CONSTRAINT chk_SoLuong CHECK (soluong >= 0);

-- số điện thoại từ 9 -> 10 số (viettel hoặc mobile)
ALTER TABLE NguoiDung
ADD CONSTRAINT chk_SDT CHECK (LEN(sdt) BETWEEN 9 AND 10 AND (sdt LIKE '09%' OR sdt LIKE '08%'));
-- trạng thái: đang giao, đã giao, đã hủy.
ALTER TABLE DonDatHang
ADD CONSTRAINT chk_TrangThai CHECK (trangThai IN (N'Đang giao', N'Đã giao', N'Đã hủy'));

-- thanhToan: đã thanh toán, chưa thanh toán.
ALTER TABLE DonDatHang
ADD CONSTRAINT chk_ThanhToan CHECK (thanhToan IN (N'Đã thanh toán', N'Chưa thanh toán'));
-- dongia >= 0
ALTER TABLE ChiTietDonHang
ADD CONSTRAINT chk_DonGia CHECK (donGia >= 0);
--Ràng buộc ngày đặt hàng và ngày thanh toán
ALTER TABLE DonDatHang
ADD CONSTRAINT chk_NgayDat_NgayThanhToan CHECK (ngayDat <= ngayThanhToan),
 CONSTRAINT chk_NgayThanhToan_NgayDuKienNhan CHECK (ngayThanhToan <= ngayGiaoDuKien),
 CONSTRAINT chk_NgayDat_NgayDuKienNhan CHECK (ngayDat <= ngayGiaoDuKien);
-- Ràng buộc tên đăng nhập không trùng lặp
ALTER TABLE NguoiDung
ADD CONSTRAINT uq_Email UNIQUE (email);
-- Ràng buộc link ảnh
ALTER TABLE SanPham
ADD CONSTRAINT chk_ImageURL CHECK (imageURL IS NULL OR imageURL LIKE '%.jpg' OR imageURL LIKE '%.png' OR imageURL LIKE '%.jpeg');
-- Ràng buộc tránh cho tên công ty và tên sản phẩm chỉ là 1 dấu cách
ALTER TABLE CongTy
ADD CONSTRAINT chk_TenCongTy_NotEmpty CHECK (LEN(LTRIM(RTRIM(tenCongTy))) > 0);
ALTER TABLE LoaiSanPham
ADD CONSTRAINT chk_TenLoaiSanPham_NotEmpty CHECK (LEN(LTRIM(RTRIM(tenLoaiSanPham))) > 0);
--=======================================ALTER TABLE=======================================
GO
ALTER TABLE SanPham
ADD gia DECIMAL(18,2) NOT NULL DEFAULT 0;
GO
ALTER TABLE SanPham
ADD moTa NVARCHAR(MAX);
GO
ALTER TABLE SanPham
ADD imageLienQuan NVARCHAR(MAX);
GO

--========================================INSERT DATA=======================================-
INSERT INTO Tinh (idTinh, tenTinh) VALUES
('T001', N'Hà Nội'),
('T002', N'TP Hồ Chí Minh'),
('T003', N'Đà Nẵng'),
('T004', N'Hải Phòng'),
('T005', N'Cần Thơ');

INSERT INTO Huyen (idHuyen, idTinh, tenHuyen) VALUES
('H001', 'T001', N'Quận Hoàn Kiếm'),
('H002', 'T001', N'Quận Ba Đình'),

('H003', 'T002', N'Quận 1'),
('H004', 'T002', N'Quận Bình Thạnh'),

('H005', 'T003', N'Quận Hải Châu'),
('H006', 'T003', N'Quận Thanh Khê'),

('H007', 'T004', N'Quận Lê Chân'),
('H008', 'T004', N'Quận Hồng Bàng'),

('H009', 'T005', N'Quận Ninh Kiều'),
('H010', 'T005', N'Quận Bình Thủy');

INSERT INTO XaPhuong (idXaPhuong, idHuyen, tenXaPhuong) VALUES
-- Hà Nội - Hoàn Kiếm
('XP001', 'H001', N'Phường Hàng Bạc'),
('XP002', 'H001', N'Phường Hàng Buồm'),
('XP003', 'H001', N'Phường Tràng Tiền'),

-- Hà Nội - Ba Đình
('XP004', 'H002', N'Phường Ngọc Hà'),
('XP005', 'H002', N'Phường Kim Mã'),
('XP006', 'H002', N'Phường Điện Biên'),

-- TP HCM - Quận 1
('XP007', 'H003', N'Phường Bến Nghé'),
('XP008', 'H003', N'Phường Bến Thành'),
('XP009', 'H003', N'Phường Nguyễn Thái Bình'),

-- TP HCM - Bình Thạnh
('XP010', 'H004', N'Phường 1'),
('XP011', 'H004', N'Phường 2'),
('XP012', 'H004', N'Phường 3'),

-- Đà Nẵng - Hải Châu
('XP013', 'H005', N'Phường Hải Châu I'),
('XP014', 'H005', N'Phường Hải Châu II'),
('XP015', 'H005', N'Phường Bình Hiên'),

-- Đà Nẵng - Thanh Khê
('XP016', 'H006', N'Phường An Khê'),
('XP017', 'H006', N'Phường Chính Gián'),
('XP018', 'H006', N'Phường Tam Thuận'),

-- Hải Phòng - Lê Chân
('XP019', 'H007', N'Phường An Biên'),
('XP020', 'H007', N'Phường An Dương'),
('XP021', 'H007', N'Phường Dư Hàng'),

-- Hải Phòng - Hồng Bàng
('XP022', 'H008', N'Phường Hoàng Văn Thụ'),
('XP023', 'H008', N'Phường Hạ Lý'),
('XP024', 'H008', N'Phường Quán Toan'),

-- Cần Thơ - Ninh Kiều
('XP025', 'H009', N'Phường An Hòa'),
('XP026', 'H009', N'Phường An Nghiệp'),
('XP027', 'H009', N'Phường Tân An'),

-- Cần Thơ - Bình Thủy
('XP028', 'H010', N'Phường An Thới'),
('XP029', 'H010', N'Phường Bình Thủy'),
('XP030', 'H010', N'Phường Trà An');

INSERT INTO NguoiDung (idNguoiDung, idXaPhuong, tenDangNhap, matKhau, email, sdt, soNha) VALUES
-- Hà Nội
('ND001', 'XP001', N'Nguyễn Văn An', 'An11111@', 'nguyenvanan.hn@gmail.com', '0912345678', N'12 Hàng Bạc'),
('ND002', 'XP002', N'Trần Thị Bích', 'Bich11111@', 'tranthibich.hn@gmail.com', '0898765432', N'34 Hàng Buồm'),

-- TP HCM
('ND003', 'XP007', N'Lê Minh Phúc', 'Phuc11111@', 'leminhphuc.hcm@gmail.com', '0913344556', N'56 Bến Nghé'),
('ND004', 'XP008', N'Phạm Thị Quỳnh', 'Quynh11111@', 'phamthiquynh.hcm@gmail.com', '0822233345', N'78 Bến Thành'),

-- Hải Phòng
('ND005', 'XP019', N'Hoàng Văn Dũng', 'Dung11111@', 'hoangvandung.hp@gmail.com', '0933344455', N'23 An Biên'),
('ND006', 'XP020', N'Ngô Thị Hường', 'Huong11111@', 'ngothihuong.hp@gmail.com', '0834455667', N'45 An Dương'),

-- Cần Thơ
('ND007', 'XP025', N'Nguyễn Văn Hùng', 'Hung11111@', 'nguyenvanhung.ct@gmail.com', '0915566778', N'12 An Hòa'),
('ND008', 'XP026', N'Trần Thị Hoa', 'Hoa11111@', 'tranthihoa.ct@gmail.com', '0891122334', N'34 An Nghiệp'),

-- Đà Nẵng (10 người)
('ND009', 'XP013', N'Nguyễn Văn Yên', 'Yen11111@', 'nguyenvanyen.dn@gmail.com', '0912233445', N'12 Hải Châu I'),
('ND010', 'XP013', N'Trần Thị Ánh', 'Anh11111@', 'tranthianh.dn@gmail.com', '0891122335', N'34 Hải Châu I'),

('ND011', 'XP014', N'Lê Văn Bình', 'Binh11111@', 'levanbinh.dn@gmail.com', '0919988777', N'56 Hải Châu II'),
('ND012', 'XP014', N'Phạm Thị Cúc', 'Cuc11111@', 'phamthicuc.dn@gmail.com', '0812345679', N'78 Hải Châu II'),

('ND013', 'XP015', N'Hoàng Văn Dũng', 'Dung11111@', 'hoangvandung.dn@gmail.com', '0922233345', N'23 Bình Hiên'),
('ND014', 'XP015', N'Ngô Thị Vy', 'Vy11111@', 'ngothihuong.dn@gmail.com', '0834455668', N'45 Bình Hiên'),

('ND015', 'XP016', N'Nguyễn Văn Khoa', 'Khoa11111@', 'nguyenvankhoa.dn@gmail.com', '0912345567', N'12 An Khê'),
('ND016', 'XP016', N'Trần Thị Liên', 'Lien11111@', 'tranthilien.dn@gmail.com', '0892345567', N'34 An Khê'),

('ND017', 'XP017', N'Lê Văn Mạnh', 'Manh11111@', 'levanmanh.dn@gmail.com', '0913344557', N'56 Chính Gián'),
('ND018', 'XP017', N'Phạm Thị Nga', 'Nga11111@', 'phamthinga.dn@gmail.com', '0823344557', N'78 Chính Gián');

INSERT INTO LoaiSanPham (idLoaiSanPham, tenLoaiSanPham) VALUES
('LSP01', N'CPU'),
('LSP02', N'Mainboard'),
('LSP03', N'RAM'),
('LSP04', N'Ổ cứng SSD/HDD'),
('LSP05', N'Card đồ họa (GPU)'),
('LSP06', N'Nguồn (PSU)'),
('LSP07', N'Vỏ Case'),
('LSP08', N'Tản nhiệt CPU'),
('LSP09', N'Màn hình');

INSERT INTO CongTy (idCongTy, tenCongTy) VALUES
('CT01', N'Intel'),
('CT02', N'AMD'),
('CT03', N'ASUS'),
('CT04', N'MSI'),
('CT05', N'Gigabyte'),
('CT06', N'Corsair'),
('CT07', N'Cooler Master'),
('CT08', N'Kingston');


INSERT INTO SanPham (idSanPham, idCongTy, idLoaiSanPham, tenSanPham, imageURL, gia, moTa, imageLienQuan) VALUES
-- CPU
('SP01', 'CT01', 'LSP01', N'Intel Core i5-12400F', 'CPU_i5-12400f.jpg', 4300000, 
 N'Intel Core i5-12400F mang lại hiệu năng tuyệt vời cho nhu cầu chơi game và làm việc văn phòng. Sở hữu 6 nhân 12 luồng, tốc độ xử lý cao và tương thích với socket LGA 1700.', NULL),
('SP02', 'CT01', 'LSP01', N'Intel Core i7-12700K', 'CPU_i7-12700k.jpg', 7500000,
 N'CPU Intel Core i7-12700K thuộc dòng Alder Lake mạnh mẽ, hỗ trợ ép xung, 12 nhân 20 luồng, mang đến hiệu năng vượt trội cho cả gaming và công việc sáng tạo.', NULL),
('SP03', 'CT02', 'LSP01', N'AMD Ryzen 5 5600X', 'CPU_Ryzen_5_5600X.jpg', 4200000,
 N'AMD Ryzen 5 5600X sử dụng kiến trúc Zen 3 mới, mang lại hiệu năng đơn nhân và đa nhân vượt trội. Lựa chọn tối ưu cho game thủ và dân đồ họa.', NULL),
('SP04', 'CT02', 'LSP01', N'AMD Ryzen 7 5800X3D', 'CPU_Ryzen_7_5800X3D.jpg', 8900000,
 N'Ryzen 7 5800X3D với công nghệ 3D V-Cache độc quyền của AMD giúp tăng hiệu năng game đáng kể, là CPU lý tưởng cho trải nghiệm chơi game cao cấp.', NULL),

-- Mainboard
('SP05', 'CT03', 'LSP02', N'ASUS ROG Strix B550-F', 'MAIN_ASUS_ROG_Strix_B550-F.jpg', 4500000,
 N'Mainboard ASUS ROG Strix B550-F Gaming hỗ trợ các CPU Ryzen thế hệ mới, trang bị tản nhiệt VRM cao cấp và nhiều cổng kết nối hiện đại như PCIe 4.0 và USB 3.2 Gen2.', NULL),
('SP06', 'CT04', 'LSP02', N'MSI B660 Tomahawk', 'MAIN_MSI_B660_Tomahawk.jpg', 4900000,
 N'MSI B660 Tomahawk mang thiết kế mạnh mẽ, hỗ trợ CPU Intel thế hệ 12, có khe PCIe Gen4 và cổng LAN 2.5G, phù hợp cho cấu hình tầm trung và cao cấp.', NULL),
('SP07', 'CT05', 'LSP02', N'Gigabyte Z690 Aorus Elite', 'MAIN_Gigabyte_Z690_Aorus_Elite.png', 5600000,
 N'Bo mạch chủ Gigabyte Z690 Aorus Elite hỗ trợ DDR5, nhiều cổng M.2 tốc độ cao và thiết kế tản nhiệt mạnh mẽ, đảm bảo hoạt động ổn định trong thời gian dài.', NULL),

-- RAM
('SP08', 'CT08', 'LSP03', N'Kingston Fury Beast 16GB DDR4 3200', 'RAM_Kingston_Fury_Beast_16GB_DDR4_3200.jpg', 1100000,
 N'RAM Kingston Fury Beast 16GB DDR4 3200 mang lại hiệu năng ổn định, khả năng tương thích cao và thiết kế mạnh mẽ, lý tưởng cho các game thủ và dân văn phòng.', NULL),
('SP09', 'CT06', 'LSP03', N'Corsair Vengeance LPX 16GB DDR4 3600', 'RAM_Corsair_Vengeance_LPX_16GB_DDR4_3600.jpg', 1150000,
 N'Corsair Vengeance LPX 16GB DDR4 3600 được thiết kế để ép xung tối đa, tản nhiệt nhôm cao cấp, đảm bảo tốc độ và độ bền vượt trội cho hệ thống.', NULL),
('SP10', 'CT06', 'LSP03', N'Corsair Vengeance RGB 32GB DDR5 5600', 'RAM_Corsair_Vengeance_RGB_32GB_DDR5_5600.jpg', 3200000,
 N'Dòng RAM DDR5 cao cấp với dải đèn RGB rực rỡ, tốc độ siêu nhanh 5600MHz, mang lại hiệu năng đỉnh cao cho các dàn máy chơi game hiện đại.', NULL),

-- SSD / HDD
('SP11', 'CT08', 'LSP04', N'Kingston NV2 1TB NVMe SSD', 'SSD_Kingston_NV2_1TB_NVMe_SSD.jpg', 1600000,
 N'SSD Kingston NV2 1TB sử dụng chuẩn NVMe Gen4 tốc độ cao, mang đến khả năng khởi động và tải ứng dụng cực nhanh, tiết kiệm điện năng và bền bỉ.', NULL),
('SP12', 'CT06', 'LSP04', N'Corsair MP600 1TB NVMe SSD', 'SSD_Corsair_MP600_1TB_NVMe_SSD.jpg', 2500000,
 N'Corsair MP600 1TB là dòng SSD PCIe Gen4 hiệu suất cực cao, đạt tốc độ đọc ghi lên đến 4950MB/s, phù hợp cho dân chơi PC hiệu năng.', NULL),
('SP13', 'CT05', 'LSP04', N'Gigabyte 2TB HDD 7200rpm', 'HDD_Gigabyte_2TB_HDD_7200rpm.jpg', 1400000,
 N'Ổ cứng HDD Gigabyte 2TB tốc độ 7200 vòng/phút, cung cấp dung lượng lưu trữ lớn, độ bền cao, thích hợp cho việc lưu trữ dữ liệu lâu dài.', NULL),

-- GPU
('SP14', 'CT04', 'LSP05', N'MSI RTX 3060 Ventus 2X 12GB', 'VGA_MSI_RTX_3060_Ventus_2X_12GB.jpg', 8700000,
 N'MSI RTX 3060 Ventus 2X trang bị 12GB VRAM GDDR6, mang lại khả năng xử lý đồ họa ấn tượng, chơi tốt các tựa game AAA ở độ phân giải Full HD và 2K.', NULL),
('SP15', 'CT05', 'LSP05', N'Gigabyte RTX 3070 Gaming OC 8GB', 'VGA_Gigabyte_RTX_3070_Gaming_OC_8GB.jpg', 11300000,
 N'Gigabyte RTX 3070 Gaming OC là GPU mạnh mẽ với hiệu năng cao, hỗ trợ Ray Tracing, DLSS, mang lại trải nghiệm gaming đỉnh cao.', NULL),
('SP16', 'CT03', 'LSP05', N'ASUS TUF Gaming RTX 3080 10GB', 'VGA_ASUS_TUF_Gaming_RTX_3080_10GB.jpg', 16900000,
 N'ASUS TUF Gaming RTX 3080 10GB có hiệu năng cực mạnh cho gaming 4K, trang bị hệ thống tản nhiệt tối ưu và thiết kế bền bỉ đạt chuẩn quân đội.', NULL),

-- PSU
('SP17', 'CT07', 'LSP06', N'Cooler Master MWE 650W 80+ Bronze', 'PSU_Cooler_Master_MWE_650W_80+_Bronze.jpg', 1300000,
 N'Cooler Master MWE 650W đạt chứng nhận 80+ Bronze, hiệu suất ổn định, hoạt động êm ái và an toàn cho toàn bộ hệ thống.', NULL),
('SP18', 'CT06', 'LSP06', N'Corsair RM750x 750W 80+ Gold', 'PSU_Corsair_RM750x_750W_80+_Gold.jpg', 2600000,
 N'Corsair RM750x 750W cung cấp công suất ổn định, hiệu suất cao 80+ Gold, dây cáp bọc lưới mềm, hoạt động yên tĩnh với quạt Zero RPM.', NULL),

-- Case
('SP19', 'CT07', 'LSP07', N'Cooler Master MasterBox TD500', 'CASE_Cooler_Master_MasterBox_TD500.jpg', 1800000,
 N'Thiết kế góc cạnh ấn tượng, mặt kính cường lực và hệ thống đèn RGB nổi bật, MasterBox TD500 mang lại vẻ đẹp hiện đại cho dàn PC.', NULL),
('SP20', 'CT03', 'LSP07', N'ASUS TUF Gaming GT301', 'CASE_ASUS_TUF_Gaming_GT301.jpg', 2100000,
 N'ASUS TUF Gaming GT301 có thiết kế bền bỉ, hỗ trợ nhiều quạt làm mát, dễ dàng lắp đặt và phù hợp với các bo mạch chủ ATX, Micro-ATX.', NULL),

-- Tản nhiệt CPU
('SP21', 'CT07', 'LSP08', N'Cooler Master Hyper 212 Black Edition', 'TNHIET_Cooler_Master_Hyper_212_Black_Edition.jpg', 950000,
 N'Cooler Master Hyper 212 Black Edition là tản khí huyền thoại với khả năng làm mát hiệu quả, hoạt động êm ái, phù hợp cho các CPU tầm trung.', NULL),
('SP22', 'CT06', 'LSP08', N'Corsair iCUE H100i Elite Liquid Cooler', 'TNHIET_Corsair_iCUE_H100i_Elite_Liquid_Cooler.jpg', 3200000,
 N'Tản nhiệt nước Corsair iCUE H100i Elite với đèn RGB tùy chỉnh, hiệu suất làm mát cao, điều khiển thông minh qua phần mềm iCUE.', NULL),

-- Màn hình
('SP23', 'CT03', 'LSP09', N'ASUS TUF Gaming VG259QM 24.5 inch, FHD, IPS, 280Hz, 1ms', 'MHinh_MASUS_TUF_Gaming_VG259QM.jpg', 5600000,
 N'Màn hình ASUS TUF VG259QM với tần số quét 280Hz và thời gian phản hồi 1ms, hiển thị mượt mà cho các game eSports.', NULL),
('SP24', 'CT04', 'LSP09', N'MSI G2712FDE (27 inch, Full HD, 180Hz, Rapid IPS, 1ms, Black)', 'MHinh_MSI_G2712FDE.jpg', 4700000,
 N'MSI G2712FDE mang đến trải nghiệm hình ảnh sống động, tấm nền IPS cao cấp và tần số quét 180Hz cực nhanh.', NULL),
('SP25', 'CT05', 'LSP09', N'Gigabyte G24F2 (23.8inch, FHD, IPS, 165Hz, 180Hz(OC), 1ms)', 'MHinh_Gigabyte_G24F2.jpg', 3600000,
 N'Màn hình Gigabyte G24F2 có thiết kế mỏng nhẹ, tần số quét 165Hz, hỗ trợ ép xung 180Hz, rất lý tưởng cho game thủ FPS.', NULL);


INSERT INTO PhanQuyen (idPhanQuyen, tenPhanQuyen) VALUES
('PQ01', N'Quản trị viên'),
('PQ02', N'Nhân viên'),
('PQ03', N'Khách hàng');

INSERT INTO PhanQuyenNguoiDung (idNguoiDung, idPhanQuyen) VALUES
('ND001', 'PQ03'), ('ND002', 'PQ03'),
('ND003', 'PQ03'), ('ND004', 'PQ03'),
('ND005', 'PQ03'), ('ND006', 'PQ03'),
('ND007', 'PQ03'), ('ND008', 'PQ03'),
('ND009', 'PQ03'), ('ND010', 'PQ03'),
('ND011', 'PQ03'), ('ND012', 'PQ03'),
('ND013', 'PQ03'), ('ND014', 'PQ03'),
('ND015', 'PQ03'), ('ND016', 'PQ03'),
('ND017', 'PQ03'), ('ND018', 'PQ03');

INSERT INTO DonDatHang (idDonDat, idNguoiDung, sdtGiaoHang, soNha, trangThai, thanhToan, ngayDat, ngayThanhToan, ngayGiaoDuKien) VALUES
('DH001', 'ND001', '0912345678', N'12 Hàng Bạc', N'Đang giao', N'Đã thanh toán', '2025-09-01', '2025-09-02', '2025-09-05'),
('DH002', 'ND002', '0898765432', N'34 Hàng Buồm', N'Đã giao', N'Đã thanh toán', '2025-09-02', '2025-09-03', '2025-09-06'),
('DH003', 'ND003', '0913344556', N'56 Bến Nghé', N'Đang giao', N'Chưa thanh toán', '2025-09-04', NULL, '2025-09-08'),
('DH004', 'ND004', '0822233345', N'78 Bến Thành', N'Đã giao', N'Đã thanh toán', '2025-08-30', '2025-09-01', '2025-09-04'),
('DH005', 'ND005', '0933344455', N'23 An Biên', N'Đang giao', N'Đã thanh toán', '2025-09-05', '2025-09-05', '2025-09-09'),
('DH006', 'ND006', '0834455667', N'45 An Dương', N'Đã hủy', N'Chưa thanh toán', '2025-09-06', NULL, '2025-09-10'),
('DH007', 'ND007', '0915566778', N'12 An Hòa', N'Đang giao', N'Đã thanh toán', '2025-09-07', '2025-09-08', '2025-09-12'),
('DH008', 'ND008', '0891122334', N'34 An Nghiệp', N'Đã giao', N'Đã thanh toán', '2025-09-01', '2025-09-01', '2025-09-03'),
('DH009', 'ND009', '0912233445', N'12 Hải Châu I', N'Đang giao', N'Đã thanh toán', '2025-09-08', '2025-09-09', '2025-09-13'),
('DH010', 'ND010', '0891122335', N'34 Hải Châu I', N'Đã giao', N'Đã thanh toán', '2025-08-28', '2025-08-29', '2025-09-02'),
('DH011', 'ND011', '0919988777', N'56 Hải Châu II', N'Đang giao', N'Chưa thanh toán', '2025-09-09', NULL, '2025-09-14'),
('DH012', 'ND012', '0812345679', N'78 Hải Châu II', N'Đã giao', N'Đã thanh toán', '2025-08-25', '2025-08-26', '2025-08-30'),
('DH013', 'ND013', '0922233345', N'23 Bình Hiên', N'Đang giao', N'Đã thanh toán', '2025-09-10', '2025-09-11', '2025-09-15'),
('DH014', 'ND014', '0834455668', N'45 Bình Hiên', N'Đã giao', N'Đã thanh toán', '2025-09-03', '2025-09-04', '2025-09-07'),
('DH015', 'ND015', '0912345567', N'12 An Khê', N'Đang giao', N'Đã thanh toán', '2025-09-11', '2025-09-11', '2025-09-16'),
('DH016', 'ND016', '0892345567', N'34 An Khê', N'Đã hủy', N'Chưa thanh toán', '2025-09-07', NULL, '2025-09-18'),
('DH017', 'ND017', '0913344557', N'56 Chính Gián', N'Đang giao', N'Đã thanh toán', '2025-09-02', '2025-09-02', '2025-09-19'),
('DH018', 'ND018', '0823344557', N'78 Chính Gián', N'Đã giao', N'Đã thanh toán', '2025-09-04', '2025-09-05', '2025-09-09'),
('DH019', 'ND009', '0912233445', N'12 Hải Châu I', N'Đang giao', N'Đã thanh toán', '2025-09-10', '2025-09-11', '2025-09-15'),
('DH020', 'ND010', '0891122335', N'34 Hải Châu I', N'Đã giao', N'Đã thanh toán', '2025-09-07', '2025-09-08', '2025-09-12');

INSERT INTO ChiTietDonHang (idDonDat, idSanPham, soluong, donGia) VALUES
('DH001', 'SP01', 1, 4500000), -- CPU i5
('DH001', 'SP08', 2, 1200000), -- RAM
('DH002', 'SP03', 1, 5200000), -- Ryzen 5600X
('DH002', 'SP05', 1, 3500000), -- Mainboard ASUS B550
('DH003', 'SP14', 1, 8500000), -- RTX 3060
('DH004', 'SP02', 1, 9500000), -- i7-12700K
('DH004', 'SP06', 1, 3800000), -- MSI B660
('DH005', 'SP15', 1, 13500000), -- RTX 3070
('DH005', 'SP10', 2, 3100000), -- RAM DDR5
('DH006', 'SP17', 1, 1500000), -- PSU Cooler Master
('DH007', 'SP11', 1, 1100000), -- Kingston NV2 SSD
('DH007', 'SP19', 1, 1800000), -- Case Cooler Master
('DH008', 'SP04', 1, 8700000), -- Ryzen 5800X3D
('DH008', 'SP07', 1, 4200000), -- Gigabyte Z690
('DH009', 'SP12', 1, 2300000), -- Corsair MP600
('DH009', 'SP22', 1, 3400000), -- Corsair AIO
('DH010', 'SP13', 1, 1800000), -- HDD 2TB
('DH011', 'SP16', 1, 18500000), -- RTX 3080
('DH012', 'SP18', 1, 2800000), -- Corsair RM750x
('DH013', 'SP20', 1, 2200000), -- Case ASUS
('DH014', 'SP21', 1, 900000), -- Hyper 212
('DH015', 'SP09', 2, 1350000), -- RAM Corsair LPX
('DH016', 'SP01', 1, 4500000), -- i5
('DH017', 'SP05', 1, 3500000), -- ASUS B550
('DH018', 'SP14', 1, 8500000), -- RTX 3060
('DH019', 'SP11', 1, 1100000), -- SSD Kingston
('DH020', 'SP03', 1, 5200000); -- Ryzen 5600X

INSERT INTO ChiTietGioHang (idChiTietGioHang, idNguoiDung, idSanPham, soLuongTrongGio) VALUES
('GH001', 'ND001', 'SP01', 1),
('GH002', 'ND001', 'SP08', 2),
('GH003', 'ND002', 'SP03', 1),
('GH004', 'ND002', 'SP05', 1),
('GH005', 'ND009', 'SP14', 1),
('GH006', 'ND009', 'SP19', 1),
('GH007', 'ND010', 'SP11', 1),
('GH008', 'ND011', 'SP02', 1),
('GH009', 'ND012', 'SP22', 1),
('GH010', 'ND013', 'SP16', 1);

select * from sanpham
select* from loaisanpham
select * from congty
