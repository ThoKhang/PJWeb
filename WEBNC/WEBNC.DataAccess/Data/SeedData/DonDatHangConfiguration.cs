using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBNC.Models;

namespace WEBNC.DataAccess.Data.SeedData
{
    public class DonDatHangConfiguration : IEntityTypeConfiguration<DonDatHang>
    {
        public void Configure(EntityTypeBuilder<DonDatHang> builder)
        {
            const string DEFAULT_USER_ID = "USER1";

            builder.HasData(
                new DonDatHang { idDonDat = "DH001", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0912345678", soNha = "12 Hàng Bạc", trangThai = "Đang giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 9, 1), ngayThanhToan = new DateTime(2025, 9, 2), ngayGiaoDuKien = new DateTime(2025, 9, 5) },
                new DonDatHang { idDonDat = "DH002", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0898765432", soNha = "34 Hàng Buồm", trangThai = "Đã giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 9, 2), ngayThanhToan = new DateTime(2025, 9, 3), ngayGiaoDuKien = new DateTime(2025, 9, 6) },
                new DonDatHang { idDonDat = "DH003", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0913344556", soNha = "56 Bến Nghé", trangThai = "Đang giao", thanhToan = "Chưa thanh toán", ngayDat = new DateTime(2025, 9, 4), ngayThanhToan = null, ngayGiaoDuKien = new DateTime(2025, 9, 8) },
                new DonDatHang { idDonDat = "DH004", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0822233345", soNha = "78 Bến Thành", trangThai = "Đã giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 8, 30), ngayThanhToan = new DateTime(2025, 9, 1), ngayGiaoDuKien = new DateTime(2025, 9, 4) },
                new DonDatHang { idDonDat = "DH005", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0933344455", soNha = "23 An Biên", trangThai = "Đang giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 9, 5), ngayThanhToan = new DateTime(2025, 9, 5), ngayGiaoDuKien = new DateTime(2025, 9, 9) },
                new DonDatHang { idDonDat = "DH006", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0834455667", soNha = "45 An Dương", trangThai = "Đã hủy", thanhToan = "Chưa thanh toán", ngayDat = new DateTime(2025, 9, 6), ngayThanhToan = null, ngayGiaoDuKien = new DateTime(2025, 9, 10) },
                new DonDatHang { idDonDat = "DH007", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0915566778", soNha = "12 An Hòa", trangThai = "Đang giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 9, 7), ngayThanhToan = new DateTime(2025, 9, 8), ngayGiaoDuKien = new DateTime(2025, 9, 12) },
                new DonDatHang { idDonDat = "DH008", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0891122334", soNha = "34 An Nghiệp", trangThai = "Đã giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 9, 1), ngayThanhToan = new DateTime(2025, 9, 1), ngayGiaoDuKien = new DateTime(2025, 9, 3) },
                new DonDatHang { idDonDat = "DH009", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0912233445", soNha = "12 Hải Châu I", trangThai = "Đang giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 9, 8), ngayThanhToan = new DateTime(2025, 9, 9), ngayGiaoDuKien = new DateTime(2025, 9, 13) },
                new DonDatHang { idDonDat = "DH010", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0891122335", soNha = "34 Hải Châu I", trangThai = "Đã giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 8, 28), ngayThanhToan = new DateTime(2025, 8, 29), ngayGiaoDuKien = new DateTime(2025, 9, 2) },
                new DonDatHang { idDonDat = "DH011", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0919988777", soNha = "56 Hải Châu II", trangThai = "Đang giao", thanhToan = "Chưa thanh toán", ngayDat = new DateTime(2025, 9, 9), ngayThanhToan = null, ngayGiaoDuKien = new DateTime(2025, 9, 14) },
                new DonDatHang { idDonDat = "DH012", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0812345679", soNha = "78 Hải Châu II", trangThai = "Đã giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 8, 25), ngayThanhToan = new DateTime(2025, 8, 26), ngayGiaoDuKien = new DateTime(2025, 8, 30) },
                new DonDatHang { idDonDat = "DH013", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0922233345", soNha = "23 Bình Hiên", trangThai = "Đang giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 9, 10), ngayThanhToan = new DateTime(2025, 9, 11), ngayGiaoDuKien = new DateTime(2025, 9, 15) },
                new DonDatHang { idDonDat = "DH014", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0834455668", soNha = "45 Bình Hiên", trangThai = "Đã giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 9, 3), ngayThanhToan = new DateTime(2025, 9, 4), ngayGiaoDuKien = new DateTime(2025, 9, 7) },
                new DonDatHang { idDonDat = "DH015", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0912345567", soNha = "12 An Khê", trangThai = "Đang giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 9, 11), ngayThanhToan = new DateTime(2025, 9, 11), ngayGiaoDuKien = new DateTime(2025, 9, 16) },
                new DonDatHang { idDonDat = "DH016", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0892345567", soNha = "34 An Khê", trangThai = "Đã hủy", thanhToan = "Chưa thanh toán", ngayDat = new DateTime(2025, 9, 7), ngayThanhToan = null, ngayGiaoDuKien = new DateTime(2025, 9, 18) },
                new DonDatHang { idDonDat = "DH017", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0913344557", soNha = "56 Chính Gián", trangThai = "Đang giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 9, 2), ngayThanhToan = new DateTime(2025, 9, 2), ngayGiaoDuKien = new DateTime(2025, 9, 19) },
                new DonDatHang { idDonDat = "DH018", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0823344557", soNha = "78 Chính Gián", trangThai = "Đã giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 9, 4), ngayThanhToan = new DateTime(2025, 9, 5), ngayGiaoDuKien = new DateTime(2025, 9, 9) },
                new DonDatHang { idDonDat = "DH019", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0912233445", soNha = "12 Hải Châu I", trangThai = "Đang giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 9, 10), ngayThanhToan = new DateTime(2025, 9, 11), ngayGiaoDuKien = new DateTime(2025, 9, 15) },
                new DonDatHang { idDonDat = "DH020", idNguoiDung = DEFAULT_USER_ID, sdtGiaoHang = "0891122335", soNha = "34 Hải Châu I", trangThai = "Đã giao", thanhToan = "Đã thanh toán", ngayDat = new DateTime(2025, 9, 7), ngayThanhToan = new DateTime(2025, 9, 8), ngayGiaoDuKien = new DateTime(2025, 9, 12) }
            );
        }
    }
}
