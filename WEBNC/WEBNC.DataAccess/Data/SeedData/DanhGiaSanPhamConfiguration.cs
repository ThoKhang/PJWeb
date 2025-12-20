using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WEBNC.Models;
using System;

namespace WEBNC.DataAccess.Data.SeedData
{
    public class DanhGiaSanPhamConfiguration : IEntityTypeConfiguration<DanhGiaSanPham>
    {
        public void Configure(EntityTypeBuilder<DanhGiaSanPham> builder)
        {
            builder.HasData(
                new DanhGiaSanPham
                {
                    idDanhGia = 1,
                    idSanPham = "SP01",
                    userId = "USER1",
                    soSao = 5,
                    noiDung = "Sản phẩm rất tốt, đúng mô tả.",
                    ngayDanhGia = new DateTime(2025, 1, 1)
                },
                new DanhGiaSanPham
                {
                    idDanhGia = 2,
                    idSanPham = "SP02",
                    userId = "USER1",
                    soSao = 4,
                    noiDung = "Chất lượng ổn, giao hàng nhanh.",
                    ngayDanhGia = new DateTime(2025, 1, 2)
                },
                new DanhGiaSanPham
                {
                    idDanhGia = 3,
                    idSanPham = "SP03",
                    userId = "USER1",
                    soSao = 5,
                    noiDung = "Rất hài lòng, sẽ ủng hộ tiếp.",
                    ngayDanhGia = new DateTime(2025, 1, 3)
                },
                new DanhGiaSanPham
                {
                    idDanhGia = 4,
                    idSanPham = "SP04",
                    userId = "USER1",
                    soSao = 3,
                    noiDung = "Sản phẩm tạm ổn trong tầm giá.",
                    ngayDanhGia = new DateTime(2025, 1, 4)
                },
                new DanhGiaSanPham
                {
                    idDanhGia = 5,
                    idSanPham = "SP05",
                    userId = "USER1",
                    soSao = 4,
                    noiDung = "Dùng ổn, đóng gói cẩn thận.",
                    ngayDanhGia = new DateTime(2025, 1, 5)
                }
            );
        }
    }
}
