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
-- ràng buộc mật khẩu: tối thiểu 8 số, có chữ Hoa, số, ký tự đặc biệt.
-- số lượng >= 0
-- số điện thoại từ 9 -> 10 số (viettel hoặc mobile)
-- trạng thái: đang giao, đã giao, đã hủy.
-- thanhToan: đã thanh toán, chưa thanh toán.
-- dongia >= 0
-- còn nhiều điều kiện khác........


