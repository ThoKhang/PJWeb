using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBNC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSanPhamToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    idSanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    tenSanPham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idCongTy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idLoaiSanPham = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham", x => x.idSanPham);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SanPham");
        }
    }
}
