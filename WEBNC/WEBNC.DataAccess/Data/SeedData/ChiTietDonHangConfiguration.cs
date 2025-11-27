using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBNC.DataAccess.Data.SeedData
{
    public class ChiTietDonHangConfiguration : IEntityTypeConfiguration<ChiTietDonHang>
    {
        public void Configure(EntityTypeBuilder<ChiTietDonHang> builder)
        {
            builder.Property(c => c.donGia).HasColumnType("money");

            builder.HasData(
                new ChiTietDonHang { idDonDat = "DH001", idSanPham = "SP01", soluong = 1, donGia = 4500000m },
                new ChiTietDonHang { idDonDat = "DH001", idSanPham = "SP08", soluong = 2, donGia = 1200000m },
                new ChiTietDonHang { idDonDat = "DH002", idSanPham = "SP03", soluong = 1, donGia = 5200000m },
                new ChiTietDonHang { idDonDat = "DH002", idSanPham = "SP05", soluong = 1, donGia = 3500000m },
                new ChiTietDonHang { idDonDat = "DH003", idSanPham = "SP14", soluong = 1, donGia = 8500000m },
                new ChiTietDonHang { idDonDat = "DH004", idSanPham = "SP02", soluong = 1, donGia = 9500000m },
                new ChiTietDonHang { idDonDat = "DH004", idSanPham = "SP06", soluong = 1, donGia = 3800000m },
                new ChiTietDonHang { idDonDat = "DH005", idSanPham = "SP15", soluong = 1, donGia = 13500000m },
                new ChiTietDonHang { idDonDat = "DH005", idSanPham = "SP10", soluong = 2, donGia = 3100000m },
                new ChiTietDonHang { idDonDat = "DH006", idSanPham = "SP17", soluong = 1, donGia = 1500000m },
                new ChiTietDonHang { idDonDat = "DH007", idSanPham = "SP11", soluong = 1, donGia = 1100000m },
                new ChiTietDonHang { idDonDat = "DH007", idSanPham = "SP19", soluong = 1, donGia = 1800000m },
                new ChiTietDonHang { idDonDat = "DH008", idSanPham = "SP04", soluong = 1, donGia = 8700000m },
                new ChiTietDonHang { idDonDat = "DH008", idSanPham = "SP07", soluong = 1, donGia = 4200000m },
                new ChiTietDonHang { idDonDat = "DH009", idSanPham = "SP12", soluong = 1, donGia = 2300000m },
                new ChiTietDonHang { idDonDat = "DH009", idSanPham = "SP22", soluong = 1, donGia = 3400000m },
                new ChiTietDonHang { idDonDat = "DH010", idSanPham = "SP13", soluong = 1, donGia = 1800000m },
                new ChiTietDonHang { idDonDat = "DH011", idSanPham = "SP16", soluong = 1, donGia = 18500000m },
                new ChiTietDonHang { idDonDat = "DH012", idSanPham = "SP18", soluong = 1, donGia = 2800000m },
                new ChiTietDonHang { idDonDat = "DH013", idSanPham = "SP20", soluong = 1, donGia = 2200000m },
                new ChiTietDonHang { idDonDat = "DH014", idSanPham = "SP21", soluong = 1, donGia = 900000m },
                new ChiTietDonHang { idDonDat = "DH015", idSanPham = "SP09", soluong = 2, donGia = 1350000m },
                new ChiTietDonHang { idDonDat = "DH016", idSanPham = "SP01", soluong = 1, donGia = 4500000m },
                new ChiTietDonHang { idDonDat = "DH017", idSanPham = "SP05", soluong = 1, donGia = 3500000m },
                new ChiTietDonHang { idDonDat = "DH018", idSanPham = "SP14", soluong = 1, donGia = 8500000m },
                new ChiTietDonHang { idDonDat = "DH019", idSanPham = "SP11", soluong = 1, donGia = 1100000m },
                new ChiTietDonHang { idDonDat = "DH020", idSanPham = "SP03", soluong = 1, donGia = 5200000m }
            );
        }
    }
}
