using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBNC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addYeuCauDoiTraToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YeuCauDoiTra",
                columns: table => new
                {
                    idYeuCau = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idDonDat = table.Column<string>(type: "char(5)", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    lyDo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuCauDoiTra", x => x.idYeuCau);
                    table.ForeignKey(
                        name: "FK_YeuCauDoiTra_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YeuCauDoiTra_DonDatHang_idDonDat",
                        column: x => x.idDonDat,
                        principalTable: "DonDatHang",
                        principalColumn: "idDonDat",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauDoiTra_idDonDat",
                table: "YeuCauDoiTra",
                column: "idDonDat");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauDoiTra_userId",
                table: "YeuCauDoiTra",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YeuCauDoiTra");
        }
    }
}
