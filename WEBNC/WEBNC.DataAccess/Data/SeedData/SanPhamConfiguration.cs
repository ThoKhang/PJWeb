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
    public class SanPhamConfiguration : IEntityTypeConfiguration<SanPham>
    {
        public void Configure(EntityTypeBuilder<SanPham> builder)
        {
            // Định nghĩa lại kiểu dữ liệu cho cột gia để khớp hoàn toàn với snapshot
            builder.Property(s => s.gia).HasPrecision(18, 2).HasColumnType("decimal(18,2)");

            // Hàm chuyển đổi chuỗi SQL đa dòng thành chuỗi C# hợp lệ
            string GetThongSo(string[] lines) => string.Join("\r\n", lines);

            builder.HasData(
                // CPU
                new SanPham
                {
                    idSanPham = "SP01",
                    idCongTy = "CT01",
                    idLoaiSanPham = "LSP01",
                    tenSanPham = "CPU_12400F_1",
                    imageURL = "CPU_12400F_1.png",
                    gia = 4300000m,
                    soLuongHienCon = 45,
                    soLuongCanDuoi = 10,
                    moTa = "Intel Core i5-12400F mang lại hiệu năng tuyệt vời cho nhu cầu chơi game và làm việc văn phòng. Sở hữu 6 nhân 12 luồng, tốc độ xử lý cao và tương thích với socket LGA 1700.",
                    imageLienQuan = "[\"CPU_12400F_1.png\", \"CPU_12400F_2.jpg\", \"CPU_12400F_3.png\", \"CPU_12400F_4.png\"]",
                    thongSoSanPham = GetThongSo(new[] { "- 6 nhân 12 luồng", "- Xung nhịp 2.5GHz (Boost 4.4GHz)", "- Socket LGA 1700", "- Bộ nhớ đệm 18MB" })
                },
                new SanPham
                {
                    idSanPham = "SP02",
                    idCongTy = "CT01",
                    idLoaiSanPham = "LSP01",
                    tenSanPham = "Intel Core i7-12700K",
                    imageURL = "CPU_i7-12700k.jpg",
                    gia = 7500000m,
                    soLuongHienCon = 20,
                    soLuongCanDuoi = 5,
                    moTa = "CPU Intel Core i7-12700K thuộc dòng Alder Lake mạnh mẽ, hỗ trợ ép xung, 12 nhân 20 luồng, mang đến hiệu năng vượt trội cho cả gaming và công việc sáng tạo.",
                    imageLienQuan = "[\"CPU_12700k_1.png\", \"CPU_12700k_2.png\", \"CPU_12700k_3.png\", \"CPU_12700k_4.png\"]",
                    thongSoSanPham = GetThongSo(new[] { "- 12 nhân 20 luồng", "- Xung nhịp 3.6GHz (Boost 5.0GHz)", "- Socket LGA 1700", "- Hỗ trợ ép xung" })
                },
                new SanPham
                {
                    idSanPham = "SP03",
                    idCongTy = "CT02",
                    idLoaiSanPham = "LSP01",
                    tenSanPham = "AMD Ryzen 5 5600X",
                    imageURL = "CPU_Ryzen_5_5600X.jpg",
                    gia = 4200000m,
                    soLuongHienCon = 50,
                    soLuongCanDuoi = 15,
                    moTa = "AMD Ryzen 5 5600X sử dụng kiến trúc Zen 3 mới, mang lại hiệu năng đơn nhân và đa nhân vượt trội. Lựa chọn tối ưu cho game thủ và dân đồ họa.",
                    imageLienQuan = "[\"CPU_5600x_1.png\", \"CPU_5600x_2.jpg\", \"CPU_5600x_3.jpg\", \"CPU_5600x_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- 6 nhân 12 luồng", "- Kiến trúc Zen 3", "- Xung nhịp 3.7GHz (Boost 4.6GHz)", "- Socket AM4" })
                },
                new SanPham
                {
                    idSanPham = "SP04",
                    idCongTy = "CT02",
                    idLoaiSanPham = "LSP01",
                    tenSanPham = "AMD Ryzen 7 5800X3D",
                    imageURL = "CPU_Ryzen_7_5800X3D.jpg",
                    gia = 8900000m,
                    soLuongHienCon = 12,
                    soLuongCanDuoi = 5,
                    moTa = "Ryzen 7 5800X3D với công nghệ 3D V-Cache độc quyền của AMD giúp tăng hiệu năng game đáng kể, là CPU lý tưởng cho trải nghiệm chơi game cao cấp.",
                    imageLienQuan = "[\"CPU_7800x3d_1.jpg\", \"CPU_7800x3d_2.jpg\", \"CPU_7800x3d_3.jpg\", \"CPU_7800x3d_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- 8 nhân 16 luồng", "- Xung nhịp 3.4GHz (Boost 4.5GHz)", "- Công nghệ 3D V-Cache", "- Socket AM4" })
                },

                // Mainboard
                new SanPham
                {
                    idSanPham = "SP05",
                    idCongTy = "CT03",
                    idLoaiSanPham = "LSP02",
                    tenSanPham = "ASUS ROG Strix B550-F",
                    imageURL = "MAIN_ASUS_ROG_Strix_B550-F.jpg",
                    gia = 4500000m,
                    soLuongHienCon = 35,
                    soLuongCanDuoi = 10,
                    moTa = "Mainboard ASUS ROG Strix B550-F Gaming hỗ trợ các CPU Ryzen thế hệ mới, trang bị tản nhiệt VRM cao cấp và nhiều cổng kết nối hiện đại như PCIe 4.0 và USB 3.2 Gen2.",
                    imageLienQuan = "[\"MAIN_b550f_1.jpg\", \"MAIN_b550f_2.jpg\", \"MAIN_b550f_3.jpg\", \"MAIN_b550f_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Chipset B550", "- Socket AM4", "- Hỗ trợ PCIe 4.0", "- 4 khe RAM DDR4 (Tối đa 128GB)" })
                },
                new SanPham
                {
                    idSanPham = "SP06",
                    idCongTy = "CT04",
                    idLoaiSanPham = "LSP02",
                    tenSanPham = "MSI B660 Tomahawk",
                    imageURL = "MAIN_B660_1.png",
                    gia = 4900000m,
                    soLuongHienCon = 40,
                    soLuongCanDuoi = 15,
                    moTa = "MSI B660 Tomahawk mang thiết kế mạnh mẽ, hỗ trợ CPU Intel thế hệ 12, có khe PCIe Gen4 và cổng LAN 2.5G, phù hợp cho cấu hình tầm trung và cao cấp.",
                    imageLienQuan = "[\"MAIN_B660_1.png\", \"MAIN_B660_2.jpg\", \"MAIN_B660_3.jpg\", \"MAIN_B660_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Chipset B660", "- Socket LGA 1700", "- Hỗ trợ PCIe Gen4", "- LAN 2.5G / USB 3.2 Gen2" })
                },
                new SanPham
                {
                    idSanPham = "SP07",
                    idCongTy = "CT05",
                    idLoaiSanPham = "LSP02",
                    tenSanPham = "Gigabyte Z690 Aorus Elite",
                    imageURL = "MAIN_Gigabyte_Z690_Aorus_Elite.png",
                    gia = 5600000m,
                    soLuongHienCon = 18,
                    soLuongCanDuoi = 5,
                    moTa = "Bo mạch chủ Gigabyte Z690 Aorus Elite hỗ trợ DDR5, nhiều cổng M.2 tốc độ cao và thiết kế tản nhiệt mạnh mẽ, đảm bảo hoạt động ổn định trong thời gian dài.",
                    imageLienQuan = "[\"MAIN_z690_1.jpg\", \"MAIN_z690_2.jpg\", \"MAIN_z690_3.jpg\", \"MAIN_z690_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Chipset Z690", "- Socket LGA 1700", "- Hỗ trợ DDR5 / PCIe 5.0", "- 3 khe M.2 NVMe" })
                },

                // RAM
                new SanPham
                {
                    idSanPham = "SP08",
                    idCongTy = "CT08",
                    idLoaiSanPham = "LSP03",
                    tenSanPham = "Kingston Fury Beast 16GB DDR4 3200",
                    imageURL = "RAM_Kingston_Fury_Beast_16GB_DDR4_3200.jpg",
                    gia = 1100000m,
                    soLuongHienCon = 60,
                    soLuongCanDuoi = 20,
                    moTa = "RAM Kingston Fury Beast 16GB DDR4 3200 mang lại hiệu năng ổn định, khả năng tương thích cao và thiết kế mạnh mẽ, lý tưởng cho các game thủ và dân văn phòng.",
                    imageLienQuan = "[\"RAM_fury_1.jpg\", \"RAM_fury_2.jpg\", \"RAM_fury_3.jpg\", \"RAM_fury_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Dung lượng 16GB (1x16GB)", "- Bus 3200MHz", "- Loại DDR4", "- Điện áp 1.35V" })
                },
                new SanPham
                {
                    idSanPham = "SP09",
                    idCongTy = "CT06",
                    idLoaiSanPham = "LSP03",
                    tenSanPham = "Corsair Vengeance LPX 16GB DDR4 3600",
                    imageURL = "RAM_corsairN_1.jpg",
                    gia = 1150000m,
                    soLuongHienCon = 55,
                    soLuongCanDuoi = 15,
                    moTa = "Corsair Vengeance LPX 16GB DDR4 3600 được thiết kế để ép xung tối đa, tản nhiệt nhôm cao cấp, đảm bảo tốc độ và độ bền vượt trội cho hệ thống.",
                    imageLienQuan = "[\"RAM_corsairN_1.jpg\", \"RAM_corsairN_2.jpg\", \"RAM_corsairN_3.jpg\", \"RAM_corsairN_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Dung lượng 16GB (2x8GB)", "- Bus 3600MHz", "- Loại DDR4", "- Tản nhiệt nhôm cao cấp" })
                },
                new SanPham
                {
                    idSanPham = "SP10",
                    idCongTy = "CT06",
                    idLoaiSanPham = "LSP03",
                    tenSanPham = "Corsair Vengeance RGB 32GB DDR5 5600",
                    imageURL = "RAM_corsair_3.png",
                    gia = 3200000m,
                    soLuongHienCon = 25,
                    soLuongCanDuoi = 10,
                    moTa = "Dòng RAM DDR5 cao cấp với dải đèn RGB rực rỡ, tốc độ siêu nhanh 5600MHz, mang lại hiệu năng đỉnh cao cho các dàn máy chơi game hiện đại.",
                    imageLienQuan = "[\"RAM_corsair_1.png\", \"RAM_corsair_2.png\", \"RAM_corsair_3.png\", \"RAM_corsair_4.png\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Dung lượng 32GB (2x16GB)", "- Bus 5600MHz", "- Loại DDR5", "- RGB tùy chỉnh" })
                },

                // SSD / HDD
                new SanPham
                {
                    idSanPham = "SP11",
                    idCongTy = "CT08",
                    idLoaiSanPham = "LSP04",
                    tenSanPham = "Kingston NV2 1TB NVMe SSD",
                    imageURL = "SSD_KT1T_1.jpg",
                    gia = 1600000m,
                    soLuongHienCon = 70,
                    soLuongCanDuoi = 25,
                    moTa = "SSD Kingston NV2 1TB sử dụng chuẩn NVMe Gen4 tốc độ cao, mang đến khả năng khởi động và tải ứng dụng cực nhanh, tiết kiệm điện năng và bền bỉ.",
                    imageLienQuan = "[\"SSD_KT1T_1.jpg\", \"SSD_KT1T_2.jpg\", \"SSD_KT1T_3.png\", \"SSD_KT1T_4.jpeg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Dung lượng 1TB", "- Chuẩn NVMe PCIe 4.0", "- Đọc: 3500MB/s, Ghi: 2100MB/s", "- Hệ số hình dạng M.2 2280" })
                },
                new SanPham
                {
                    idSanPham = "SP12",
                    idCongTy = "CT06",
                    idLoaiSanPham = "LSP04",
                    tenSanPham = "Corsair MP600 1TB NVMe SSD",
                    imageURL = "SSD_Corsair_MP600_1TB_NVMe_SSD.jpg",
                    gia = 2500000m,
                    soLuongHienCon = 30,
                    soLuongCanDuoi = 10,
                    moTa = "Corsair MP600 1TB là dòng SSD PCIe Gen4 hiệu suất cực cao, đạt tốc độ đọc ghi lên đến 4950MB/s, phù hợp cho dân chơi PC hiệu năng.",
                    imageLienQuan = "[\"SSD_CS1T_1.jpg\", \"SSD_CS1T_2.jpg\", \"SSD_CS1T_3.jpg\", \"SSD_CS1T_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Dung lượng 1TB", "- Chuẩn NVMe PCIe 4.0 x4", "- Tốc độ đọc: 4950MB/s, ghi: 4250MB/s", "- Tản nhiệt nhôm" })
                },
                new SanPham
                {
                    idSanPham = "SP13",
                    idCongTy = "CT05",
                    idLoaiSanPham = "LSP04",
                    tenSanPham = "Gigabyte 2TB HDD 7200rpm",
                    imageURL = "HDD_WD2T_1.jpg",
                    gia = 1400000m,
                    soLuongHienCon = 45,
                    soLuongCanDuoi = 15,
                    moTa = "Ổ cứng HDD Gigabyte 2TB tốc độ 7200 vòng/phút, cung cấp dung lượng lưu trữ lớn, độ bền cao, thích hợp cho việc lưu trữ dữ liệu lâu dài.",
                    imageLienQuan = "[\"HDD_WD2T_1.jpg\", \"HDD_WD2T_2.jpg\", \"HDD_WD2T_3.jpeg\", \"HDD_WD2T_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Dung lượng 2TB", "- Loại HDD 3.5 inch", "- Tốc độ quay 7200rpm", "- Cache 64MB" })
                },

                // GPU
                new SanPham
                {
                    idSanPham = "SP14",
                    idCongTy = "CT04",
                    idLoaiSanPham = "LSP05",
                    tenSanPham = "MSI RTX 3060 Ventus 2X 12GB",
                    imageURL = "VGA_MSI_RTX_3060_Ventus_2X_12GB.jpg",
                    gia = 8700000m,
                    soLuongHienCon = 28,
                    soLuongCanDuoi = 10,
                    moTa = "MSI RTX 3060 Ventus 2X trang bị 12GB VRAM GDDR6, mang lại khả năng xử lý đồ họa ấn tượng, chơi tốt các tựa game AAA ở độ phân giải Full HD và 2K.",
                    imageLienQuan = "[\"VGA_30602x_1.jpg\", \"VGA_30602x_2.jpg\", \"VGA_30602x_3.jpg\", \"VGA_30602x_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- GPU NVIDIA RTX 3060", "- VRAM 12GB GDDR6", "- Giao tiếp PCIe 4.0", "- Nguồn khuyến nghị 550W" })
                },
                new SanPham
                {
                    idSanPham = "SP15",
                    idCongTy = "CT05",
                    idLoaiSanPham = "LSP05",
                    tenSanPham = "Gigabyte RTX 3070 Gaming OC 8GB",
                    imageURL = "VGA_Gigabyte_RTX_3070_Gaming_OC_8GB.jpg",
                    gia = 11300000m,
                    soLuongHienCon = 18,
                    soLuongCanDuoi = 5,
                    moTa = "Gigabyte RTX 3070 Gaming OC là GPU mạnh mẽ với hiệu năng cao, hỗ trợ Ray Tracing, DLSS, mang lại trải nghiệm gaming đỉnh cao.",
                    imageLienQuan = "[\"VGA_30703x_1.jpg\", \"VGA_30703x_2.jpg\", \"VGA_30703x_3.jpg\", \"VGA_30703x_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- GPU NVIDIA RTX 3070", "- VRAM 8GB GDDR6", "- Ray Tracing / DLSS 2.0", "- Nguồn khuyến nghị 650W" })
                },
                new SanPham
                {
                    idSanPham = "SP16",
                    idCongTy = "CT03",
                    idLoaiSanPham = "LSP05",
                    tenSanPham = "ASUS TUF Gaming RTX 3080 10GB",
                    imageURL = "VGA_ASUS_TUF_Gaming_RTX_3080_10GB.jpg",
                    gia = 16900000m,
                    soLuongHienCon = 8,
                    soLuongCanDuoi = 3,
                    moTa = "ASUS TUF Gaming RTX 3080 10GB có hiệu năng cực mạnh cho gaming 4K, trang bị hệ thống tản nhiệt tối ưu và thiết kế bền bỉ đạt chuẩn quân đội.",
                    imageLienQuan = "[\"VGA_30803x_1.jpg\", \"VGA_30803x_2.jpg\", \"VGA_30803x_3.jpg\", \"VGA_30803x_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- GPU NVIDIA RTX 3080", "- VRAM 10GB GDDR6X", "- Tản nhiệt 3 quạt TUF", "- Nguồn khuyến nghị 750W" })
                },

                // PSU
                new SanPham
                {
                    idSanPham = "SP17",
                    idCongTy = "CT07",
                    idLoaiSanPham = "LSP06",
                    tenSanPham = "Cooler Master MWE 650W 80+ Bronze",
                    imageURL = "PSU_650w_3.png",
                    gia = 1300000m,
                    soLuongHienCon = 40,
                    soLuongCanDuoi = 15,
                    moTa = "Cooler Master MWE 650W đạt chứng nhận 80+ Bronze, hiệu suất ổn định, hoạt động êm ái và an toàn cho toàn bộ hệ thống.",
                    imageLienQuan = "[\"PSU_650w_1.png\", \"PSU_650w_2.png\", \"PSU_650w_3.png\", \"PSU_650w_4.png\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Công suất 650W", "- Chứng nhận 80+ Bronze", "- Quạt 120mm yên tĩnh", "- Bảo vệ OVP / OCP / SCP" })
                },
                new SanPham
                {
                    idSanPham = "SP18",
                    idCongTy = "CT06",
                    idLoaiSanPham = "LSP06",
                    tenSanPham = "Corsair RM750x 750W 80+ Gold",
                    imageURL = "PSU_Corsair_RM750x_750W_80+_Gold.jpg",
                    gia = 2600000m,
                    soLuongHienCon = 20,
                    soLuongCanDuoi = 5,
                    moTa = "Corsair RM750x 750W cung cấp công suất ổn định, hiệu suất cao 80+ Gold, dây cáp bọc lưới mềm, hoạt động yên tĩnh với quạt Zero RPM.",
                    imageLienQuan = "[\"PSU_Corsair_RM750x_750W_80+_Gold.jpg\", \"PSU_650w_2.png\", \"PSU_Corsair_RM750x_750W_80+_Gold.jpg\", \"PSU_750w_4.png\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Công suất 750W", "- Chứng nhận 80+ Gold", "- Dây cáp rời mềm", "- Quạt Zero RPM" })
                },

                // Case
                new SanPham
                {
                    idSanPham = "SP19",
                    idCongTy = "CT07",
                    idLoaiSanPham = "LSP07",
                    tenSanPham = "Cooler Master MasterBox TD500",
                    imageURL = "CASE_Cooler_Master_MasterBox_TD500.jpg",
                    gia = 1800000m,
                    soLuongHienCon = 30,
                    soLuongCanDuoi = 10,
                    moTa = "Thiết kế góc cạnh ấn tượng, mặt kính cường lực và hệ thống đèn RGB nổi bật, MasterBox TD500 mang lại vẻ đẹp hiện đại cho dàn PC.",
                    imageLienQuan = "[\"CASE_td500_1.jpg\", \"CASE_td500_2.jpg\", \"CASE_td500_3.jpg\", \"CASE_td500_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Hỗ trợ main ATX / mATX / ITX", "- 3 quạt RGB đi kèm", "- Mặt kính cường lực", "- Kích thước: Mid Tower" })
                },
                new SanPham
                {
                    idSanPham = "SP20",
                    idCongTy = "CT03",
                    idLoaiSanPham = "LSP07",
                    tenSanPham = "ASUS TUF Gaming GT301",
                    imageURL = "CASE_tuf_1.jpg",
                    gia = 2100000m,
                    soLuongHienCon = 25,
                    soLuongCanDuoi = 10,
                    moTa = "ASUS TUF Gaming GT301 có thiết kế bền bỉ, hỗ trợ nhiều quạt làm mát, dễ dàng lắp đặt và phù hợp với các bo mạch chủ ATX, Micro-ATX.",
                    imageLienQuan = "[\"CASE_tuf_1.jpg\", \"CASE_tuf_2.jpg\", \"CASE_tuf_3.jpg\", \"CASE_tuf_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Hỗ trợ main ATX / Micro-ATX", "- Lưới tản nhiệt mặt trước", "- Kính cường lực bên hông", "- 3 quạt LED TUF kèm theo" })
                },

                // Tản nhiệt CPU
                new SanPham
                {
                    idSanPham = "SP21",
                    idCongTy = "CT07",
                    idLoaiSanPham = "LSP08",
                    tenSanPham = "Cooler Master Hyper 212 Black Edition",
                    imageURL = "TNHIET_Cooler_Master_Hyper_212_Black_Edition.jpg",
                    gia = 950000m,
                    soLuongHienCon = 50,
                    soLuongCanDuoi = 20,
                    moTa = "Cooler Master Hyper 212 Black Edition là tản khí huyền thoại với khả năng làm mát hiệu quả, hoạt động êm ái, phù hợp cho các CPU tầm trung.",
                    imageLienQuan = "[\"TNHIET_212_1.png\", \"TNHIET_212_2.png\", \"TNHIET_212_3.png\", \"TNHIET_212_4.png\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Loại tản khí", "- Quạt 120mm PWM", "- Độ ồn thấp", "- Hỗ trợ Intel / AMD" })
                },
                new SanPham
                {
                    idSanPham = "SP22",
                    idCongTy = "CT06",
                    idLoaiSanPham = "LSP08",
                    tenSanPham = "Corsair iCUE H100i Elite Liquid Cooler",
                    imageURL = "TNHIET_Corsair_iCUE_H100i_Elite_Liquid_Cooler.jpg",
                    gia = 3200000m,
                    soLuongHienCon = 20,
                    soLuongCanDuoi = 5,
                    moTa = "Tản nhiệt nước Corsair iCUE H100i Elite với đèn RGB tùy chỉnh, hiệu suất làm mát cao, điều khiển thông minh qua phần mềm iCUE.",
                    imageLienQuan = "[\"TNHIET_h100i_1.jpg\", \"TNHIET_h100i_2.jpg\", \"TNHIET_h100i_3.jpg\", \"TNHIET_h100i_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Loại tản nước AIO 240mm", "- 2 quạt 120mm RGB", "- Điều khiển qua phần mềm iCUE", "- Hỗ trợ Intel & AMD" })
                },

                // Màn hình
                new SanPham
                {
                    idSanPham = "SP23",
                    idCongTy = "CT03",
                    idLoaiSanPham = "LSP09",
                    tenSanPham = "ASUS TUF Gaming VG259QM 24.5 inch, FHD, IPS, 280Hz, 1ms",
                    imageURL = "MHinh_MASUS_TUF_Gaming_VG259QM.jpg",
                    gia = 5600000m,
                    soLuongHienCon = 15,
                    soLuongCanDuoi = 5,
                    moTa = "Màn hình ASUS TUF VG259QM với tần số quét 280Hz và thời gian phản hồi 1ms, hiển thị mượt mà cho các game eSports.",
                    imageLienQuan = "[\"MHINH_TUF_1.jpg\", \"MHINH_TUF_2.jpg\", \"MHINH_TUF_3.jpg\", \"MHINH_TUF_4.jpg\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Kích thước 24.5 inch", "- Tấm nền IPS", "- Tần số quét 280Hz", "- Thời gian phản hồi 1ms" })
                },
                new SanPham
                {
                    idSanPham = "SP24",
                    idCongTy = "CT04",
                    idLoaiSanPham = "LSP09",
                    tenSanPham = "MSI G2712FDE (27 inch, Full HD, 180Hz, Rapid IPS, 1ms, Black)",
                    imageURL = "MHinh_MSI_G2712FDE.jpg",
                    gia = 4700000m,
                    soLuongHienCon = 35,
                    soLuongCanDuoi = 10,
                    moTa = "MSI G2712FDE mang đến trải nghiệm hình ảnh sống động, tấm nền IPS cao cấp và tần số quét 180Hz cực nhanh.",
                    imageLienQuan = "[\"MHINH_MSI_1.png\", \"MHINH_MSI_2.png\", \"MHINH_MSI_3.png\", \"MHINH_MSI_4.png\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Kích thước 27 inch", "- Độ phân giải Full HD", "- Tần số quét 180Hz", "- Tấm nền Rapid IPS" })
                },
                new SanPham
                {
                    idSanPham = "SP25",
                    idCongTy = "CT05",
                    idLoaiSanPham = "LSP09",
                    tenSanPham = "Gigabyte G24F2 (23.8inch, FHD, IPS, 165Hz, 180Hz(OC), 1ms)",
                    imageURL = "MHinh_Gigabyte_G24F2.jpg",
                    gia = 3600000m,
                    soLuongHienCon = 40,
                    soLuongCanDuoi = 15,
                    moTa = "Màn hình Gigabyte G24F2 có thiết kế mỏng nhẹ, tần số quét 165Hz, hỗ trợ ép xung 180Hz, rất lý tưởng cho game thủ FPS.",
                    imageLienQuan = "[\"MHINH_GGB_1.png\", \"MHINH_GGB_2.png\", \"MHINH_GGB_3.png\", \"MHINH_GGB_4.png\"]",
                    thongSoSanPham = GetThongSo(new[] { "- Kích thước 23.8 inch", "- Độ phân giải Full HD", "- Tần số quét 165Hz (OC 180Hz)", "- Tấm nền IPS / Thời gian phản hồi 1ms" })
                }
            );
        }
    }
}
