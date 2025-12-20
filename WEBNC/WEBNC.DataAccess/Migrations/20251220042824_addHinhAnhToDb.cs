using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBNC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addHinhAnhToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HinhAnhDanhGia",
                columns: table => new
                {
                    idHinhAnh = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idDanhGia = table.Column<int>(nullable: true),
                    imageUrl = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnhDanhGia", x => x.idHinhAnh);
                    table.ForeignKey(
                        name: "FK_HinhAnhDanhGia_DanhGiaSanPham_idDanhGia",
                        column: x => x.idDanhGia,
                        principalTable: "DanhGiaSanPham",
                        principalColumn: "idDanhGia");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhDanhGia_idDanhGia",
                table: "HinhAnhDanhGia",
                column: "idDanhGia");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HinhAnhDanhGia");
        }
    }
}
