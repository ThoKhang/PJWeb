using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBNC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addDanhGiaSanPhamToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhGiaSanPham",
                columns: table => new
                {
                    idDanhGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idSanPham = table.Column<string>(type: "char(5)", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idDonDat = table.Column<string>(type: "char(5)", nullable: false),
                    soSao = table.Column<int>(type: "int", nullable: false),
                    noiDung = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ngayDanhGia = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGiaSanPham", x => x.idDanhGia);
                    table.ForeignKey(
                        name: "FK_DanhGiaSanPham_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGiaSanPham_DonDatHang_idDonDat",
                        column: x => x.idDonDat,
                        principalTable: "DonDatHang",
                        principalColumn: "idDonDat",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGiaSanPham_SanPham_idSanPham",
                        column: x => x.idSanPham,
                        principalTable: "SanPham",
                        principalColumn: "idSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaSanPham_idDonDat",
                table: "DanhGiaSanPham",
                column: "idDonDat");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaSanPham_idSanPham",
                table: "DanhGiaSanPham",
                column: "idSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaSanPham_userId",
                table: "DanhGiaSanPham",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DanhGiaSanPham");
        }
    }
}
