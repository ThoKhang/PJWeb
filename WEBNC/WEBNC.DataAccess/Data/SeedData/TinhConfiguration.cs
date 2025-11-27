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
    public class TinhConfiguration : IEntityTypeConfiguration<Tinh>
    {
        public void Configure(EntityTypeBuilder<Tinh> builder)
        {
            builder.HasData(
                new Tinh { idTinh = "T001", tenTinh = "Hà Nội" },
                new Tinh { idTinh = "T002", tenTinh = "TP Hồ Chí Minh" },
                new Tinh { idTinh = "T003", tenTinh = "Đà Nẵng" },
                new Tinh { idTinh = "T004", tenTinh = "Hải Phòng" },
                new Tinh { idTinh = "T005", tenTinh = "Cần Thơ" }
            );
        }
    }
}
