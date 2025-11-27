using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WEBNC.Models;

namespace WEBNC.DataAccess.Data.SeedData
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            const string USER_ID = "USER1";
            // [BẮT BUỘC] Hash mật khẩu tĩnh cho "Pass123."
            const string STATIC_PASSWORD_HASH = "AQAAAAIAAYagAAAAEPLiGv3yqV5d5TzFv2n7hR3N4Q2O1FjM4U7C9Vd3w0g5O8kR8J5Xo5D3/Kq7X"; // Dùng giá trị bạn đã tạo

            // [SỬA LỖI] Sử dụng GUID tĩnh cho SecurityStamp và ConcurrencyStamp
            const string STATIC_SECURITY_STAMP = "A2C1D2A4-F2D5-4E80-9A1F-A6B3A9B2F2A1"; // GUID tĩnh 1
            const string STATIC_CONCURRENCY_STAMP = "B3D2E5C5-A1E4-4D71-8B2E-C5D4B3A2C1D2"; // GUID tĩnh 2

            builder.HasData(
                new ApplicationUser
                {
                    Id = USER_ID,
                    UserName = "admin@webnc.com",
                    NormalizedUserName = "ADMIN@WEBNC.COM",
                    Email = "admin@webnc.com",
                    NormalizedEmail = "ADMIN@WEBNC.COM",
                    EmailConfirmed = true,

                    // SỬ DỤNG GUID TĨNH ĐỂ KHÔNG BỊ LỖI NON-DETERMINISTIC
                    SecurityStamp = STATIC_SECURITY_STAMP,
                    ConcurrencyStamp = STATIC_CONCURRENCY_STAMP,

                    idPhuongXa = "XP001",
                    hoTen = "Admin Hệ Thống",
                    soNha = "123 Ngõ Test",
                    PhoneNumber = "0901234567",

                    PasswordHash = STATIC_PASSWORD_HASH
                }
            );
        }
    }
}