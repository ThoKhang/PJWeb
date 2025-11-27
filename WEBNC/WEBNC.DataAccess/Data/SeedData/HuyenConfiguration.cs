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
    public class HuyenConfiguration : IEntityTypeConfiguration<Huyen>
    {
        public void Configure(EntityTypeBuilder<Huyen> builder)
        {
            builder.HasData(
                new Huyen { idHuyen = "H001", idTinh = "T001", tenHuyen = "Quận Hoàn Kiếm" },
                new Huyen { idHuyen = "H002", idTinh = "T001", tenHuyen = "Quận Ba Đình" },
                new Huyen { idHuyen = "H003", idTinh = "T002", tenHuyen = "Quận 1" },
                new Huyen { idHuyen = "H004", idTinh = "T002", tenHuyen = "Quận Bình Thạnh" },
                new Huyen { idHuyen = "H005", idTinh = "T003", tenHuyen = "Quận Hải Châu" },
                new Huyen { idHuyen = "H006", idTinh = "T003", tenHuyen = "Quận Thanh Khê" },
                new Huyen { idHuyen = "H007", idTinh = "T004", tenHuyen = "Quận Lê Chân" },
                new Huyen { idHuyen = "H008", idTinh = "T004", tenHuyen = "Quận Hồng Bàng" },
                new Huyen { idHuyen = "H009", idTinh = "T005", tenHuyen = "Quận Ninh Kiều" },
                new Huyen { idHuyen = "H010", idTinh = "T005", tenHuyen = "Quận Bình Thủy" }
            );
        }
    }
}
