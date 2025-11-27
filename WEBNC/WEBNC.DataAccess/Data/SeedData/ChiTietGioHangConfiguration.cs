using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBNC.DataAccess.Data.SeedData
{
    public class ChiTietGioHangConfiguration : IEntityTypeConfiguration<ChiTietGioHang>
    {
        public void Configure(EntityTypeBuilder<ChiTietGioHang> builder)
        {
            const string DEFAULT_USER_ID = "USER1";

            builder.HasData(
                new ChiTietGioHang { idChiTietGioHang = "GH001", idNguoiDung = DEFAULT_USER_ID, idSanPham = "SP01", soLuongTrongGio = 1 },
                new ChiTietGioHang { idChiTietGioHang = "GH002", idNguoiDung = DEFAULT_USER_ID, idSanPham = "SP08", soLuongTrongGio = 2 },
                new ChiTietGioHang { idChiTietGioHang = "GH003", idNguoiDung = DEFAULT_USER_ID, idSanPham = "SP03", soLuongTrongGio = 1 },
                new ChiTietGioHang { idChiTietGioHang = "GH004", idNguoiDung = DEFAULT_USER_ID, idSanPham = "SP05", soLuongTrongGio = 1 },
                new ChiTietGioHang { idChiTietGioHang = "GH005", idNguoiDung = DEFAULT_USER_ID, idSanPham = "SP14", soLuongTrongGio = 1 },
                new ChiTietGioHang { idChiTietGioHang = "GH006", idNguoiDung = DEFAULT_USER_ID, idSanPham = "SP19", soLuongTrongGio = 1 },
                new ChiTietGioHang { idChiTietGioHang = "GH007", idNguoiDung = DEFAULT_USER_ID, idSanPham = "SP11", soLuongTrongGio = 1 },
                new ChiTietGioHang { idChiTietGioHang = "GH008", idNguoiDung = DEFAULT_USER_ID, idSanPham = "SP02", soLuongTrongGio = 1 },
                new ChiTietGioHang { idChiTietGioHang = "GH009", idNguoiDung = DEFAULT_USER_ID, idSanPham = "SP22", soLuongTrongGio = 1 },
                new ChiTietGioHang { idChiTietGioHang = "GH010", idNguoiDung = DEFAULT_USER_ID, idSanPham = "SP16", soLuongTrongGio = 1 }
            );
        }
    }
}
