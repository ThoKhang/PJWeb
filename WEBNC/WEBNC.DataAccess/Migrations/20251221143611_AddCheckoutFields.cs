using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBNC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckoutFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThanhToan_DonDatHang_idDonDat",
                table: "ThanhToan");

            migrationBuilder.AlterColumn<string>(
                name: "idDonDat",
                table: "ThanhToan",
                type: "char(5)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(5)");

            migrationBuilder.AddForeignKey(
                name: "FK_ThanhToan_DonDatHang_idDonDat",
                table: "ThanhToan",
                column: "idDonDat",
                principalTable: "DonDatHang",
                principalColumn: "idDonDat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThanhToan_DonDatHang_idDonDat",
                table: "ThanhToan");

            migrationBuilder.AlterColumn<string>(
                name: "idDonDat",
                table: "ThanhToan",
                type: "char(5)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "char(5)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ThanhToan_DonDatHang_idDonDat",
                table: "ThanhToan",
                column: "idDonDat",
                principalTable: "DonDatHang",
                principalColumn: "idDonDat",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
