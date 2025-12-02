using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBNC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class thayDoiIDNguoiDungtrongDONDATHANG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "idNguoiDung",
                table: "DonDatHang",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(5)");

            migrationBuilder.UpdateData(
                table: "SanPham",
                keyColumn: "idSanPham",
                keyValue: "SP01",
                column: "imageURL",
                value: "CPU_12400F_1.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "idNguoiDung",
                table: "DonDatHang",
                type: "char(5)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "SanPham",
                keyColumn: "idSanPham",
                keyValue: "SP01",
                column: "imageURL",
                value: "CPU_12400F_1");
        }
    }
}
