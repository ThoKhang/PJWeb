using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBNC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addThanhToanTodb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "daThanhToan",
                table: "DonDatHang",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ThanhToan",
                columns: table => new
                {
                    idThanhToan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idDonDat = table.Column<string>(type: "char(5)", nullable: false),
                    phuongThuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    soTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    daThanhToan = table.Column<bool>(type: "bit", nullable: false),
                    ngayThanhToan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    maGiaoDich = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToan", x => x.idThanhToan);
                    table.ForeignKey(
                        name: "FK_ThanhToan_DonDatHang_idDonDat",
                        column: x => x.idDonDat,
                        principalTable: "DonDatHang",
                        principalColumn: "idDonDat",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH001",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH002",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH003",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH004",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH005",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH006",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH007",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH008",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH009",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH010",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH011",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH012",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH013",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH014",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH015",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH016",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH017",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH018",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH019",
                column: "daThanhToan",
                value: false);

            migrationBuilder.UpdateData(
                table: "DonDatHang",
                keyColumn: "idDonDat",
                keyValue: "DH020",
                column: "daThanhToan",
                value: false);

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_idDonDat",
                table: "ThanhToan",
                column: "idDonDat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "daThanhToan",
                table: "DonDatHang");
        }
    }
}
