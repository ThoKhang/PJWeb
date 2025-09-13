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
			update cascade
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
	tenSanPham nvarchar(50) not null,
	imageURL varchar(30)
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
	trangThai nvarchar(10) not null,
	thanhToan nvarchar(10) not null,
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
ADD CONSTRAINT chk_TrangThai CHECK (trangThai IN ('Đang giao', 'Đã giao', 'Đã hủy'));

-- thanhToan: đã thanh toán, chưa thanh toán.
ALTER TABLE DonDatHang
ADD CONSTRAINT chk_ThanhToan CHECK (thanhToan IN ('Đã thanh toán', 'Chưa thanh toán'));
-- dongia >= 0
ALTER TABLE ChiTietDonHang
ADD CONSTRAINT chk_DonGia CHECK (donGia >= 0);
--Ràng buộc ngày đặt hàng và ngày thanh toán
ALTER TABLE DonDatHang
ADD CONSTRAINT chk_NgayDat_NgayThanhToan CHECK (ngayDat <= ngayThanhToan),
 CONSTRAINT chk_NgayThanhToan_NgayDuKienNhan CHECK (ngayThanhToan <= ngayGiaoDuKien),
 CONSTRAINT chk_NgayDat_NgayDuKienNhan CHECK (ngayDat <= ngayGiaoDuKien);





