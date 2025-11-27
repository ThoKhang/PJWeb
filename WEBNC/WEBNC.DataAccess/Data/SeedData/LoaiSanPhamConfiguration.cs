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
    public class LoaiSanPhamConfiguration : IEntityTypeConfiguration<LoaiSanPham>
    {
        public void Configure(EntityTypeBuilder<LoaiSanPham> builder)
        {
            builder.HasData(
                new LoaiSanPham { idLoaiSanPham = "LSP01", tenLoaiSanPham = "CPU" },
                new LoaiSanPham { idLoaiSanPham = "LSP02", tenLoaiSanPham = "Mainboard" },
                new LoaiSanPham { idLoaiSanPham = "LSP03", tenLoaiSanPham = "RAM" },
                new LoaiSanPham { idLoaiSanPham = "LSP04", tenLoaiSanPham = "Ổ cứng SSD/HDD" },
                new LoaiSanPham { idLoaiSanPham = "LSP05", tenLoaiSanPham = "Card đồ họa (GPU)" },
                new LoaiSanPham { idLoaiSanPham = "LSP06", tenLoaiSanPham = "Nguồn (PSU)" },
                new LoaiSanPham { idLoaiSanPham = "LSP07", tenLoaiSanPham = "Vỏ Case" },
                new LoaiSanPham { idLoaiSanPham = "LSP08", tenLoaiSanPham = "Tản nhiệt CPU" },
                new LoaiSanPham { idLoaiSanPham = "LSP09", tenLoaiSanPham = "Màn hình" }
            );
        }
    }
}
