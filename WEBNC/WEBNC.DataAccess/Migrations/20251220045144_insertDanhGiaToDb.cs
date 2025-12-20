using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBNC.DataAccess.Migrations
{
    public partial class insertDanhGiaToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "USER1",
                columns: new[] { "Email", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[]
                {
                    "minhhuy91@gmail.com",
                    "MINHHUY91@GMAIL.COM",
                    "MINHHUY91@GMAIL.COM",
                    "minhhuy91@gmail.com"
                });

            migrationBuilder.InsertData(
                table: "DanhGiaSanPham",
                columns: new[]
                {
                    "idDonDat",
                    "idSanPham",
                    "ngayDanhGia",
                    "noiDung",
                    "soSao",
                    "userId"
                },
                values: new object[,]
                {
                    { null, "SP01", new DateTime(2025, 1, 1), "Sản phẩm rất tốt, đúng mô tả.", 5, "USER1" },
                    { null, "SP02", new DateTime(2025, 1, 2), "Chất lượng ổn, giao hàng nhanh.", 4, "USER1" },
                    { null, "SP03", new DateTime(2025, 1, 3), "Rất hài lòng, sẽ ủng hộ tiếp.", 5, "USER1" },
                    { null, "SP04", new DateTime(2025, 1, 4), "Sản phẩm tạm ổn trong tầm giá.", 3, "USER1" },
                    { null, "SP05", new DateTime(2025, 1, 5), "Dùng ổn, đóng gói cẩn thận.", 4, "USER1" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.DeleteData(
                table: "DanhGiaSanPham",
                keyColumn: "idSanPham",
                keyValues: new object[]
                {
                    "SP01", "SP02", "SP03", "SP04", "SP05"
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "USER1",
                columns: new[] { "Email", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[]
                {
                    "minhhuy91@gmail.com",
                    "MINHHUY91@GMAIL.COM",
                    "MINHHUY91@GMAIL.COM",
                    "minhhuy91@gmail.com"
                });
        }
    }
}
