using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WEBNC.Models;

namespace WEBNC.DataAccess.Data.SeedData
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // ID user
            const string USER_ID = "USER1";

            // Email
            const string EMAIL = "minhhuy91@gmail.com";
            const string NORMALIZED_EMAIL = "MINHHUY91@GMAIL.COM";

            // GUID chuẩn (KHÔNG được bỏ dấu '-')
            const string STATIC_SECURITY_STAMP = "A2C1D2A4-F2D5-4E80-9A1F-A6B3A9B2F2A1";
            const string STATIC_CONCURRENCY_STAMP = "B3D2E5C5-A1E4-4D71-8B2E-C5D4B3A2C1D2";

            // HASH CHUẨN CHO MẬT KHẨU: Abc123!@#
            const string STATIC_PASSWORD_HASH =
                "AQAAAAIAAYagAAAAECostYXDzWMxjeRK8BZV9Y2l5j9jgqJ8h65CSvX0UQnI657xBoFczZpIOGj8p8Fm1Q==";

            builder.HasData(
                new ApplicationUser
                {
                    Id = USER_ID,
                    UserName = EMAIL,
                    NormalizedUserName = NORMALIZED_EMAIL,
                    Email = EMAIL,
                    NormalizedEmail = NORMALIZED_EMAIL,
                    EmailConfirmed = true,

                    SecurityStamp = STATIC_SECURITY_STAMP,
                    ConcurrencyStamp = STATIC_CONCURRENCY_STAMP,

                    // Info
                    idPhuongXa = "XP001",
                    hoTen = "Phạm Minh Huy",
                    soNha = "24 Bắc Đẩu",
                    PhoneNumber = "0987654321",

                    // Password
                    PasswordHash = STATIC_PASSWORD_HASH
                }
            );
        }
    }
}
