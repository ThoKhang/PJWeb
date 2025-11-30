using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WEBNC.Models;

namespace WEBNC.DataAccess.Data.SeedData
{
    public class XaPhuongConfiguration : IEntityTypeConfiguration<XaPhuong>
    {
        public void Configure(EntityTypeBuilder<XaPhuong> builder)
        {
            builder.HasData(
                // Hà Nội – T001
                new XaPhuong { idXaPhuong = "XP001", idTinh = "T001", tenXaPhuong = "Phường Hàng Bạc" },
                new XaPhuong { idXaPhuong = "XP002", idTinh = "T001", tenXaPhuong = "Phường Hàng Buồm" },
                new XaPhuong { idXaPhuong = "XP003", idTinh = "T001", tenXaPhuong = "Phường Tràng Tiền" },

                new XaPhuong { idXaPhuong = "XP004", idTinh = "T001", tenXaPhuong = "Phường Ngọc Hà" },
                new XaPhuong { idXaPhuong = "XP005", idTinh = "T001", tenXaPhuong = "Phường Kim Mã" },
                new XaPhuong { idXaPhuong = "XP006", idTinh = "T001", tenXaPhuong = "Phường Điện Biên" },

                // TP HCM – T002
                new XaPhuong { idXaPhuong = "XP007", idTinh = "T002", tenXaPhuong = "Phường Bến Nghé" },
                new XaPhuong { idXaPhuong = "XP008", idTinh = "T002", tenXaPhuong = "Phường Bến Thành" },
                new XaPhuong { idXaPhuong = "XP009", idTinh = "T002", tenXaPhuong = "Phường Nguyễn Thái Bình" },

                new XaPhuong { idXaPhuong = "XP010", idTinh = "T002", tenXaPhuong = "Phường 1" },
                new XaPhuong { idXaPhuong = "XP011", idTinh = "T002", tenXaPhuong = "Phường 2" },
                new XaPhuong { idXaPhuong = "XP012", idTinh = "T002", tenXaPhuong = "Phường 3" },

                // Đà Nẵng – T003
                new XaPhuong { idXaPhuong = "XP013", idTinh = "T003", tenXaPhuong = "Phường Hải Châu I" },
                new XaPhuong { idXaPhuong = "XP014", idTinh = "T003", tenXaPhuong = "Phường Hải Châu II" },
                new XaPhuong { idXaPhuong = "XP015", idTinh = "T003", tenXaPhuong = "Phường Bình Hiên" },

                new XaPhuong { idXaPhuong = "XP016", idTinh = "T003", tenXaPhuong = "Phường An Khê" },
                new XaPhuong { idXaPhuong = "XP017", idTinh = "T003", tenXaPhuong = "Phường Chính Gián" },
                new XaPhuong { idXaPhuong = "XP018", idTinh = "T003", tenXaPhuong = "Phường Tam Thuận" },

                // Hải Phòng – T004
                new XaPhuong { idXaPhuong = "XP019", idTinh = "T004", tenXaPhuong = "Phường An Biên" },
                new XaPhuong { idXaPhuong = "XP020", idTinh = "T004", tenXaPhuong = "Phường An Dương" },
                new XaPhuong { idXaPhuong = "XP021", idTinh = "T004", tenXaPhuong = "Phường Dư Hàng" },

                new XaPhuong { idXaPhuong = "XP022", idTinh = "T004", tenXaPhuong = "Phường Hoàng Văn Thụ" },
                new XaPhuong { idXaPhuong = "XP023", idTinh = "T004", tenXaPhuong = "Phường Hạ Lý" },
                new XaPhuong { idXaPhuong = "XP024", idTinh = "T004", tenXaPhuong = "Phường Quán Toan" },

                // Cần Thơ – T005
                new XaPhuong { idXaPhuong = "XP025", idTinh = "T005", tenXaPhuong = "Phường An Hòa" },
                new XaPhuong { idXaPhuong = "XP026", idTinh = "T005", tenXaPhuong = "Phường An Nghiệp" },
                new XaPhuong { idXaPhuong = "XP027", idTinh = "T005", tenXaPhuong = "Phường Tân An" },

                new XaPhuong { idXaPhuong = "XP028", idTinh = "T005", tenXaPhuong = "Phường An Thới" },
                new XaPhuong { idXaPhuong = "XP029", idTinh = "T005", tenXaPhuong = "Phường Bình Thủy" },
                new XaPhuong { idXaPhuong = "XP030", idTinh = "T005", tenXaPhuong = "Phường Trà An" }
            );
        }
    }
}
