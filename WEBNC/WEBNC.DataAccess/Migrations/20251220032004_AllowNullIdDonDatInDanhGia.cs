using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBNC.DataAccess.Migrations
{
    public partial class AllowNullIdDonDatInDanhGia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "idDonDat",
                table: "DanhGiaSanPham",
                type: "char(5)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(5)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "idDonDat",
                table: "DanhGiaSanPham",
                type: "char(5)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(5)",
                oldNullable: true);
        }
    }
}
