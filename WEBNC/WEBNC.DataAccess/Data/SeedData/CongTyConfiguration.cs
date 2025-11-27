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
    public class CongTyConfiguration : IEntityTypeConfiguration<CongTy>
    {
        public void Configure(EntityTypeBuilder<CongTy> builder)
        {
            builder.HasData(
                new CongTy { idCongTy = "CT01", tenCongTy = "Intel" },
                new CongTy { idCongTy = "CT02", tenCongTy = "AMD" },
                new CongTy { idCongTy = "CT03", tenCongTy = "ASUS" },
                new CongTy { idCongTy = "CT04", tenCongTy = "MSI" },
                new CongTy { idCongTy = "CT05", tenCongTy = "Gigabyte" },
                new CongTy { idCongTy = "CT06", tenCongTy = "Corsair" },
                new CongTy { idCongTy = "CT07", tenCongTy = "Cooler Master" },
                new CongTy { idCongTy = "CT08", tenCongTy = "Kingston" }
            );
        }
    }
}
