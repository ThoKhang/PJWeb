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
    public class XaPhuongConfiguration : IEntityTypeConfiguration<XaPhuong>
    {
        public void Configure(EntityTypeBuilder<XaPhuong> builder)
        {
            builder.HasData(
                // Hà Nội - Hoàn Kiếm
                new XaPhuong { idXaPhuong = "XP001", idHuyen = "H001", tenXaPhuong = "Phường Hàng Bạc" },
                new XaPhuong { idXaPhuong = "XP002", idHuyen = "H001", tenXaPhuong = "Phường Hàng Buồm" },
                new XaPhuong { idXaPhuong = "XP003", idHuyen = "H001", tenXaPhuong = "Phường Tràng Tiền" },
                // Hà Nội - Ba Đình
                new XaPhuong { idXaPhuong = "XP004", idHuyen = "H002", tenXaPhuong = "Phường Ngọc Hà" },
                new XaPhuong { idXaPhuong = "XP005", idHuyen = "H002", tenXaPhuong = "Phường Kim Mã" },
                new XaPhuong { idXaPhuong = "XP006", idHuyen = "H002", tenXaPhuong = "Phường Điện Biên" },
                // TP HCM - Quận 1
                new XaPhuong { idXaPhuong = "XP007", idHuyen = "H003", tenXaPhuong = "Phường Bến Nghé" },
                new XaPhuong { idXaPhuong = "XP008", idHuyen = "H003", tenXaPhuong = "Phường Bến Thành" },
                new XaPhuong { idXaPhuong = "XP009", idHuyen = "H003", tenXaPhuong = "Phường Nguyễn Thái Bình" },
                // TP HCM - Bình Thạnh
                new XaPhuong { idXaPhuong = "XP010", idHuyen = "H004", tenXaPhuong = "Phường 1" },
                new XaPhuong { idXaPhuong = "XP011", idHuyen = "H004", tenXaPhuong = "Phường 2" },
                new XaPhuong { idXaPhuong = "XP012", idHuyen = "H004", tenXaPhuong = "Phường 3" },
                // Đà Nẵng - Hải Châu
                new XaPhuong { idXaPhuong = "XP013", idHuyen = "H005", tenXaPhuong = "Phường Hải Châu I" },
                new XaPhuong { idXaPhuong = "XP014", idHuyen = "H005", tenXaPhuong = "Phường Hải Châu II" },
                new XaPhuong { idXaPhuong = "XP015", idHuyen = "H005", tenXaPhuong = "Phường Bình Hiên" },
                // Đà Nẵng - Thanh Khê
                new XaPhuong { idXaPhuong = "XP016", idHuyen = "H006", tenXaPhuong = "Phường An Khê" },
                new XaPhuong { idXaPhuong = "XP017", idHuyen = "H006", tenXaPhuong = "Phường Chính Gián" },
                new XaPhuong { idXaPhuong = "XP018", idHuyen = "H006", tenXaPhuong = "Phường Tam Thuận" },
                // Hải Phòng - Lê Chân
                new XaPhuong { idXaPhuong = "XP019", idHuyen = "H007", tenXaPhuong = "Phường An Biên" },
                new XaPhuong { idXaPhuong = "XP020", idHuyen = "H007", tenXaPhuong = "Phường An Dương" },
                new XaPhuong { idXaPhuong = "XP021", idHuyen = "H007", tenXaPhuong = "Phường Dư Hàng" },
                // Hải Phòng - Hồng Bàng
                new XaPhuong { idXaPhuong = "XP022", idHuyen = "H008", tenXaPhuong = "Phường Hoàng Văn Thụ" },
                new XaPhuong { idXaPhuong = "XP023", idHuyen = "H008", tenXaPhuong = "Phường Hạ Lý" },
                new XaPhuong { idXaPhuong = "XP024", idHuyen = "H008", tenXaPhuong = "Phường Quán Toan" },
                // Cần Thơ - Ninh Kiều
                new XaPhuong { idXaPhuong = "XP025", idHuyen = "H009", tenXaPhuong = "Phường An Hòa" },
                new XaPhuong { idXaPhuong = "XP026", idHuyen = "H009", tenXaPhuong = "Phường An Nghiệp" },
                new XaPhuong { idXaPhuong = "XP027", idHuyen = "H009", tenXaPhuong = "Phường Tân An" },
                // Cần Thơ - Bình Thủy
                new XaPhuong { idXaPhuong = "XP028", idHuyen = "H010", tenXaPhuong = "Phường An Thới" },
                new XaPhuong { idXaPhuong = "XP029", idHuyen = "H010", tenXaPhuong = "Phường Bình Thủy" },
                new XaPhuong { idXaPhuong = "XP030", idHuyen = "H010", tenXaPhuong = "Phường Trà An" }
            );
        }
    }
}
