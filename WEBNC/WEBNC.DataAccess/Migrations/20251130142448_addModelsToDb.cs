using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WEBNC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addModelsToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CongTy",
                columns: table => new
                {
                    idCongTy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    tenCongTy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CongTy", x => x.idCongTy);
                });

            migrationBuilder.CreateTable(
                name: "DonDatHang",
                columns: table => new
                {
                    idDonDat = table.Column<string>(type: "char(5)", nullable: false),
                    idNguoiDung = table.Column<string>(type: "char(5)", nullable: false),
                    sdtGiaoHang = table.Column<string>(type: "char(11)", nullable: false),
                    soNha = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    thanhToan = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ngayDat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ngayThanhToan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ngayGiaoDuKien = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonDatHang", x => x.idDonDat);
                });

            migrationBuilder.CreateTable(
                name: "LoaiSanPham",
                columns: table => new
                {
                    idLoaiSanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    tenLoaiSanPham = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiSanPham", x => x.idLoaiSanPham);
                });

            migrationBuilder.CreateTable(
                name: "Tinh",
                columns: table => new
                {
                    idTinh = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    tenTinh = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tinh", x => x.idTinh);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    idSanPham = table.Column<string>(type: "char(5)", nullable: false),
                    tenSanPham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idCongTy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idLoaiSanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    gia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    moTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imageLienQuan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    thongSoSanPham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    soLuongCanDuoi = table.Column<int>(type: "int", nullable: false),
                    soLuongHienCon = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham", x => x.idSanPham);
                    table.ForeignKey(
                        name: "FK_SanPham_CongTy_idCongTy",
                        column: x => x.idCongTy,
                        principalTable: "CongTy",
                        principalColumn: "idCongTy",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPham_LoaiSanPham_idLoaiSanPham",
                        column: x => x.idLoaiSanPham,
                        principalTable: "LoaiSanPham",
                        principalColumn: "idLoaiSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "XaPhuong",
                columns: table => new
                {
                    idXaPhuong = table.Column<string>(type: "char(5)", nullable: false),
                    idTinh = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    tenXaPhuong = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XaPhuong", x => x.idXaPhuong);
                    table.ForeignKey(
                        name: "FK_XaPhuong_Tinh_idTinh",
                        column: x => x.idTinh,
                        principalTable: "Tinh",
                        principalColumn: "idTinh",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHang",
                columns: table => new
                {
                    idDonDat = table.Column<string>(type: "char(5)", nullable: false),
                    idSanPham = table.Column<string>(type: "char(5)", nullable: false),
                    soluong = table.Column<int>(type: "int", nullable: false),
                    donGia = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHang", x => new { x.idDonDat, x.idSanPham });
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_DonDatHang_idDonDat",
                        column: x => x.idDonDat,
                        principalTable: "DonDatHang",
                        principalColumn: "idDonDat",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_SanPham_idSanPham",
                        column: x => x.idSanPham,
                        principalTable: "SanPham",
                        principalColumn: "idSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    hoTen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    soNha = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idPhuongXa = table.Column<string>(type: "char(5)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_XaPhuong_idPhuongXa",
                        column: x => x.idPhuongXa,
                        principalTable: "XaPhuong",
                        principalColumn: "idXaPhuong",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietGioHang",
                columns: table => new
                {
                    idChiTietGioHang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idSanPham = table.Column<string>(type: "char(5)", nullable: false),
                    soLuongTrongGio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietGioHang", x => x.idChiTietGioHang);
                    table.ForeignKey(
                        name: "FK_ChiTietGioHang_AspNetUsers_idNguoiDung",
                        column: x => x.idNguoiDung,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietGioHang_SanPham_idSanPham",
                        column: x => x.idSanPham,
                        principalTable: "SanPham",
                        principalColumn: "idSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CongTy",
                columns: new[] { "idCongTy", "tenCongTy" },
                values: new object[,]
                {
                    { "CT01", "Intel" },
                    { "CT02", "AMD" },
                    { "CT03", "ASUS" },
                    { "CT04", "MSI" },
                    { "CT05", "Gigabyte" },
                    { "CT06", "Corsair" },
                    { "CT07", "Cooler Master" },
                    { "CT08", "Kingston" }
                });

            migrationBuilder.InsertData(
                table: "DonDatHang",
                columns: new[] { "idDonDat", "idNguoiDung", "ngayDat", "ngayGiaoDuKien", "ngayThanhToan", "sdtGiaoHang", "soNha", "thanhToan", "trangThai" },
                values: new object[,]
                {
                    { "DH001", "USER1", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345678", "12 Hàng Bạc", "Đã thanh toán", "Đang giao" },
                    { "DH002", "USER1", new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "0898765432", "34 Hàng Buồm", "Đã thanh toán", "Đã giao" },
                    { "DH003", "USER1", new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "0913344556", "56 Bến Nghé", "Chưa thanh toán", "Đang giao" },
                    { "DH004", "USER1", new DateTime(2025, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0822233345", "78 Bến Thành", "Đã thanh toán", "Đã giao" },
                    { "DH005", "USER1", new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "0933344455", "23 An Biên", "Đã thanh toán", "Đang giao" },
                    { "DH006", "USER1", new DateTime(2025, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "0834455667", "45 An Dương", "Chưa thanh toán", "Đã hủy" },
                    { "DH007", "USER1", new DateTime(2025, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "0915566778", "12 An Hòa", "Đã thanh toán", "Đang giao" },
                    { "DH008", "USER1", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0891122334", "34 An Nghiệp", "Đã thanh toán", "Đã giao" },
                    { "DH009", "USER1", new DateTime(2025, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912233445", "12 Hải Châu I", "Đã thanh toán", "Đang giao" },
                    { "DH010", "USER1", new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "0891122335", "34 Hải Châu I", "Đã thanh toán", "Đã giao" },
                    { "DH011", "USER1", new DateTime(2025, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "0919988777", "56 Hải Châu II", "Chưa thanh toán", "Đang giao" },
                    { "DH012", "USER1", new DateTime(2025, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "0812345679", "78 Hải Châu II", "Đã thanh toán", "Đã giao" },
                    { "DH013", "USER1", new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "0922233345", "23 Bình Hiên", "Đã thanh toán", "Đang giao" },
                    { "DH014", "USER1", new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "0834455668", "45 Bình Hiên", "Đã thanh toán", "Đã giao" },
                    { "DH015", "USER1", new DateTime(2025, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345567", "12 An Khê", "Đã thanh toán", "Đang giao" },
                    { "DH016", "USER1", new DateTime(2025, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "0892345567", "34 An Khê", "Chưa thanh toán", "Đã hủy" },
                    { "DH017", "USER1", new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "0913344557", "56 Chính Gián", "Đã thanh toán", "Đang giao" },
                    { "DH018", "USER1", new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "0823344557", "78 Chính Gián", "Đã thanh toán", "Đã giao" },
                    { "DH019", "USER1", new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912233445", "12 Hải Châu I", "Đã thanh toán", "Đang giao" },
                    { "DH020", "USER1", new DateTime(2025, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "0891122335", "34 Hải Châu I", "Đã thanh toán", "Đã giao" }
                });

            migrationBuilder.InsertData(
                table: "LoaiSanPham",
                columns: new[] { "idLoaiSanPham", "tenLoaiSanPham" },
                values: new object[,]
                {
                    { "LSP01", "CPU" },
                    { "LSP02", "Mainboard" },
                    { "LSP03", "RAM" },
                    { "LSP04", "Ổ cứng SSD/HDD" },
                    { "LSP05", "Card đồ họa (GPU)" },
                    { "LSP06", "Nguồn (PSU)" },
                    { "LSP07", "Vỏ Case" },
                    { "LSP08", "Tản nhiệt CPU" },
                    { "LSP09", "Màn hình" }
                });

            migrationBuilder.InsertData(
                table: "Tinh",
                columns: new[] { "idTinh", "tenTinh" },
                values: new object[,]
                {
                    { "T001", "Hà Nội" },
                    { "T002", "TP Hồ Chí Minh" },
                    { "T003", "Đà Nẵng" },
                    { "T004", "Hải Phòng" },
                    { "T005", "Cần Thơ" }
                });

            migrationBuilder.InsertData(
                table: "SanPham",
                columns: new[] { "idSanPham", "gia", "idCongTy", "idLoaiSanPham", "imageLienQuan", "imageURL", "moTa", "soLuongCanDuoi", "soLuongHienCon", "tenSanPham", "thongSoSanPham" },
                values: new object[,]
                {
                    { "SP01", 4300000m, "CT01", "LSP01", "[\"CPU_12400F_1.png\", \"CPU_12400F_2.jpg\", \"CPU_12400F_3.png\", \"CPU_12400F_4.png\"]", "CPU_12400F_1", "Intel Core i5-12400F mang lại hiệu năng tuyệt vời cho nhu cầu chơi game và làm việc văn phòng. Sở hữu 6 nhân 12 luồng, tốc độ xử lý cao và tương thích với socket LGA 1700.", 10, 45, "CPU_12400F_1", "- 6 nhân 12 luồng\r\n- Xung nhịp 2.5GHz (Boost 4.4GHz)\r\n- Socket LGA 1700\r\n- Bộ nhớ đệm 18MB" },
                    { "SP02", 7500000m, "CT01", "LSP01", "[\"CPU_12700k_1.png\", \"CPU_12700k_2.png\", \"CPU_12700k_3.png\", \"CPU_12700k_4.png\"]", "CPU_i7-12700k.jpg", "CPU Intel Core i7-12700K thuộc dòng Alder Lake mạnh mẽ, hỗ trợ ép xung, 12 nhân 20 luồng, mang đến hiệu năng vượt trội cho cả gaming và công việc sáng tạo.", 5, 20, "Intel Core i7-12700K", "- 12 nhân 20 luồng\r\n- Xung nhịp 3.6GHz (Boost 5.0GHz)\r\n- Socket LGA 1700\r\n- Hỗ trợ ép xung" },
                    { "SP03", 4200000m, "CT02", "LSP01", "[\"CPU_5600x_1.png\", \"CPU_5600x_2.jpg\", \"CPU_5600x_3.jpg\", \"CPU_5600x_4.jpg\"]", "CPU_Ryzen_5_5600X.jpg", "AMD Ryzen 5 5600X sử dụng kiến trúc Zen 3 mới, mang lại hiệu năng đơn nhân và đa nhân vượt trội. Lựa chọn tối ưu cho game thủ và dân đồ họa.", 15, 50, "AMD Ryzen 5 5600X", "- 6 nhân 12 luồng\r\n- Kiến trúc Zen 3\r\n- Xung nhịp 3.7GHz (Boost 4.6GHz)\r\n- Socket AM4" },
                    { "SP04", 8900000m, "CT02", "LSP01", "[\"CPU_7800x3d_1.jpg\", \"CPU_7800x3d_2.jpg\", \"CPU_7800x3d_3.jpg\", \"CPU_7800x3d_4.jpg\"]", "CPU_Ryzen_7_5800X3D.jpg", "Ryzen 7 5800X3D với công nghệ 3D V-Cache độc quyền của AMD giúp tăng hiệu năng game đáng kể, là CPU lý tưởng cho trải nghiệm chơi game cao cấp.", 5, 12, "AMD Ryzen 7 5800X3D", "- 8 nhân 16 luồng\r\n- Xung nhịp 3.4GHz (Boost 4.5GHz)\r\n- Công nghệ 3D V-Cache\r\n- Socket AM4" },
                    { "SP05", 4500000m, "CT03", "LSP02", "[\"MAIN_b550f_1.jpg\", \"MAIN_b550f_2.jpg\", \"MAIN_b550f_3.jpg\", \"MAIN_b550f_4.jpg\"]", "MAIN_ASUS_ROG_Strix_B550-F.jpg", "Mainboard ASUS ROG Strix B550-F Gaming hỗ trợ các CPU Ryzen thế hệ mới, trang bị tản nhiệt VRM cao cấp và nhiều cổng kết nối hiện đại như PCIe 4.0 và USB 3.2 Gen2.", 10, 35, "ASUS ROG Strix B550-F", "- Chipset B550\r\n- Socket AM4\r\n- Hỗ trợ PCIe 4.0\r\n- 4 khe RAM DDR4 (Tối đa 128GB)" },
                    { "SP06", 4900000m, "CT04", "LSP02", "[\"MAIN_B660_1.png\", \"MAIN_B660_2.jpg\", \"MAIN_B660_3.jpg\", \"MAIN_B660_4.jpg\"]", "MAIN_B660_1.png", "MSI B660 Tomahawk mang thiết kế mạnh mẽ, hỗ trợ CPU Intel thế hệ 12, có khe PCIe Gen4 và cổng LAN 2.5G, phù hợp cho cấu hình tầm trung và cao cấp.", 15, 40, "MSI B660 Tomahawk", "- Chipset B660\r\n- Socket LGA 1700\r\n- Hỗ trợ PCIe Gen4\r\n- LAN 2.5G / USB 3.2 Gen2" },
                    { "SP07", 5600000m, "CT05", "LSP02", "[\"MAIN_z690_1.jpg\", \"MAIN_z690_2.jpg\", \"MAIN_z690_3.jpg\", \"MAIN_z690_4.jpg\"]", "MAIN_Gigabyte_Z690_Aorus_Elite.png", "Bo mạch chủ Gigabyte Z690 Aorus Elite hỗ trợ DDR5, nhiều cổng M.2 tốc độ cao và thiết kế tản nhiệt mạnh mẽ, đảm bảo hoạt động ổn định trong thời gian dài.", 5, 18, "Gigabyte Z690 Aorus Elite", "- Chipset Z690\r\n- Socket LGA 1700\r\n- Hỗ trợ DDR5 / PCIe 5.0\r\n- 3 khe M.2 NVMe" },
                    { "SP08", 1100000m, "CT08", "LSP03", "[\"RAM_fury_1.jpg\", \"RAM_fury_2.jpg\", \"RAM_fury_3.jpg\", \"RAM_fury_4.jpg\"]", "RAM_Kingston_Fury_Beast_16GB_DDR4_3200.jpg", "RAM Kingston Fury Beast 16GB DDR4 3200 mang lại hiệu năng ổn định, khả năng tương thích cao và thiết kế mạnh mẽ, lý tưởng cho các game thủ và dân văn phòng.", 20, 60, "Kingston Fury Beast 16GB DDR4 3200", "- Dung lượng 16GB (1x16GB)\r\n- Bus 3200MHz\r\n- Loại DDR4\r\n- Điện áp 1.35V" },
                    { "SP09", 1150000m, "CT06", "LSP03", "[\"RAM_corsairN_1.jpg\", \"RAM_corsairN_2.jpg\", \"RAM_corsairN_3.jpg\", \"RAM_corsairN_4.jpg\"]", "RAM_corsairN_1.jpg", "Corsair Vengeance LPX 16GB DDR4 3600 được thiết kế để ép xung tối đa, tản nhiệt nhôm cao cấp, đảm bảo tốc độ và độ bền vượt trội cho hệ thống.", 15, 55, "Corsair Vengeance LPX 16GB DDR4 3600", "- Dung lượng 16GB (2x8GB)\r\n- Bus 3600MHz\r\n- Loại DDR4\r\n- Tản nhiệt nhôm cao cấp" },
                    { "SP10", 3200000m, "CT06", "LSP03", "[\"RAM_corsair_1.png\", \"RAM_corsair_2.png\", \"RAM_corsair_3.png\", \"RAM_corsair_4.png\"]", "RAM_corsair_3.png", "Dòng RAM DDR5 cao cấp với dải đèn RGB rực rỡ, tốc độ siêu nhanh 5600MHz, mang lại hiệu năng đỉnh cao cho các dàn máy chơi game hiện đại.", 10, 25, "Corsair Vengeance RGB 32GB DDR5 5600", "- Dung lượng 32GB (2x16GB)\r\n- Bus 5600MHz\r\n- Loại DDR5\r\n- RGB tùy chỉnh" },
                    { "SP11", 1600000m, "CT08", "LSP04", "[\"SSD_KT1T_1.jpg\", \"SSD_KT1T_2.jpg\", \"SSD_KT1T_3.png\", \"SSD_KT1T_4.jpeg\"]", "SSD_KT1T_1.jpg", "SSD Kingston NV2 1TB sử dụng chuẩn NVMe Gen4 tốc độ cao, mang đến khả năng khởi động và tải ứng dụng cực nhanh, tiết kiệm điện năng và bền bỉ.", 25, 70, "Kingston NV2 1TB NVMe SSD", "- Dung lượng 1TB\r\n- Chuẩn NVMe PCIe 4.0\r\n- Đọc: 3500MB/s, Ghi: 2100MB/s\r\n- Hệ số hình dạng M.2 2280" },
                    { "SP12", 2500000m, "CT06", "LSP04", "[\"SSD_CS1T_1.jpg\", \"SSD_CS1T_2.jpg\", \"SSD_CS1T_3.jpg\", \"SSD_CS1T_4.jpg\"]", "SSD_Corsair_MP600_1TB_NVMe_SSD.jpg", "Corsair MP600 1TB là dòng SSD PCIe Gen4 hiệu suất cực cao, đạt tốc độ đọc ghi lên đến 4950MB/s, phù hợp cho dân chơi PC hiệu năng.", 10, 30, "Corsair MP600 1TB NVMe SSD", "- Dung lượng 1TB\r\n- Chuẩn NVMe PCIe 4.0 x4\r\n- Tốc độ đọc: 4950MB/s, ghi: 4250MB/s\r\n- Tản nhiệt nhôm" },
                    { "SP13", 1400000m, "CT05", "LSP04", "[\"HDD_WD2T_1.jpg\", \"HDD_WD2T_2.jpg\", \"HDD_WD2T_3.jpeg\", \"HDD_WD2T_4.jpg\"]", "HDD_WD2T_1.jpg", "Ổ cứng HDD Gigabyte 2TB tốc độ 7200 vòng/phút, cung cấp dung lượng lưu trữ lớn, độ bền cao, thích hợp cho việc lưu trữ dữ liệu lâu dài.", 15, 45, "Gigabyte 2TB HDD 7200rpm", "- Dung lượng 2TB\r\n- Loại HDD 3.5 inch\r\n- Tốc độ quay 7200rpm\r\n- Cache 64MB" },
                    { "SP14", 8700000m, "CT04", "LSP05", "[\"VGA_30602x_1.jpg\", \"VGA_30602x_2.jpg\", \"VGA_30602x_3.jpg\", \"VGA_30602x_4.jpg\"]", "VGA_MSI_RTX_3060_Ventus_2X_12GB.jpg", "MSI RTX 3060 Ventus 2X trang bị 12GB VRAM GDDR6, mang lại khả năng xử lý đồ họa ấn tượng, chơi tốt các tựa game AAA ở độ phân giải Full HD và 2K.", 10, 28, "MSI RTX 3060 Ventus 2X 12GB", "- GPU NVIDIA RTX 3060\r\n- VRAM 12GB GDDR6\r\n- Giao tiếp PCIe 4.0\r\n- Nguồn khuyến nghị 550W" },
                    { "SP15", 11300000m, "CT05", "LSP05", "[\"VGA_30703x_1.jpg\", \"VGA_30703x_2.jpg\", \"VGA_30703x_3.jpg\", \"VGA_30703x_4.jpg\"]", "VGA_Gigabyte_RTX_3070_Gaming_OC_8GB.jpg", "Gigabyte RTX 3070 Gaming OC là GPU mạnh mẽ với hiệu năng cao, hỗ trợ Ray Tracing, DLSS, mang lại trải nghiệm gaming đỉnh cao.", 5, 18, "Gigabyte RTX 3070 Gaming OC 8GB", "- GPU NVIDIA RTX 3070\r\n- VRAM 8GB GDDR6\r\n- Ray Tracing / DLSS 2.0\r\n- Nguồn khuyến nghị 650W" },
                    { "SP16", 16900000m, "CT03", "LSP05", "[\"VGA_30803x_1.jpg\", \"VGA_30803x_2.jpg\", \"VGA_30803x_3.jpg\", \"VGA_30803x_4.jpg\"]", "VGA_ASUS_TUF_Gaming_RTX_3080_10GB.jpg", "ASUS TUF Gaming RTX 3080 10GB có hiệu năng cực mạnh cho gaming 4K, trang bị hệ thống tản nhiệt tối ưu và thiết kế bền bỉ đạt chuẩn quân đội.", 3, 8, "ASUS TUF Gaming RTX 3080 10GB", "- GPU NVIDIA RTX 3080\r\n- VRAM 10GB GDDR6X\r\n- Tản nhiệt 3 quạt TUF\r\n- Nguồn khuyến nghị 750W" },
                    { "SP17", 1300000m, "CT07", "LSP06", "[\"PSU_650w_1.png\", \"PSU_650w_2.png\", \"PSU_650w_3.png\", \"PSU_650w_4.png\"]", "PSU_650w_3.png", "Cooler Master MWE 650W đạt chứng nhận 80+ Bronze, hiệu suất ổn định, hoạt động êm ái và an toàn cho toàn bộ hệ thống.", 15, 40, "Cooler Master MWE 650W 80+ Bronze", "- Công suất 650W\r\n- Chứng nhận 80+ Bronze\r\n- Quạt 120mm yên tĩnh\r\n- Bảo vệ OVP / OCP / SCP" },
                    { "SP18", 2600000m, "CT06", "LSP06", "[\"PSU_Corsair_RM750x_750W_80+_Gold.jpg\", \"PSU_650w_2.png\", \"PSU_Corsair_RM750x_750W_80+_Gold.jpg\", \"PSU_750w_4.png\"]", "PSU_Corsair_RM750x_750W_80+_Gold.jpg", "Corsair RM750x 750W cung cấp công suất ổn định, hiệu suất cao 80+ Gold, dây cáp bọc lưới mềm, hoạt động yên tĩnh với quạt Zero RPM.", 5, 20, "Corsair RM750x 750W 80+ Gold", "- Công suất 750W\r\n- Chứng nhận 80+ Gold\r\n- Dây cáp rời mềm\r\n- Quạt Zero RPM" },
                    { "SP19", 1800000m, "CT07", "LSP07", "[\"CASE_td500_1.jpg\", \"CASE_td500_2.jpg\", \"CASE_td500_3.jpg\", \"CASE_td500_4.jpg\"]", "CASE_Cooler_Master_MasterBox_TD500.jpg", "Thiết kế góc cạnh ấn tượng, mặt kính cường lực và hệ thống đèn RGB nổi bật, MasterBox TD500 mang lại vẻ đẹp hiện đại cho dàn PC.", 10, 30, "Cooler Master MasterBox TD500", "- Hỗ trợ main ATX / mATX / ITX\r\n- 3 quạt RGB đi kèm\r\n- Mặt kính cường lực\r\n- Kích thước: Mid Tower" },
                    { "SP20", 2100000m, "CT03", "LSP07", "[\"CASE_tuf_1.jpg\", \"CASE_tuf_2.jpg\", \"CASE_tuf_3.jpg\", \"CASE_tuf_4.jpg\"]", "CASE_tuf_1.jpg", "ASUS TUF Gaming GT301 có thiết kế bền bỉ, hỗ trợ nhiều quạt làm mát, dễ dàng lắp đặt và phù hợp với các bo mạch chủ ATX, Micro-ATX.", 10, 25, "ASUS TUF Gaming GT301", "- Hỗ trợ main ATX / Micro-ATX\r\n- Lưới tản nhiệt mặt trước\r\n- Kính cường lực bên hông\r\n- 3 quạt LED TUF kèm theo" },
                    { "SP21", 950000m, "CT07", "LSP08", "[\"TNHIET_212_1.png\", \"TNHIET_212_2.png\", \"TNHIET_212_3.png\", \"TNHIET_212_4.png\"]", "TNHIET_Cooler_Master_Hyper_212_Black_Edition.jpg", "Cooler Master Hyper 212 Black Edition là tản khí huyền thoại với khả năng làm mát hiệu quả, hoạt động êm ái, phù hợp cho các CPU tầm trung.", 20, 50, "Cooler Master Hyper 212 Black Edition", "- Loại tản khí\r\n- Quạt 120mm PWM\r\n- Độ ồn thấp\r\n- Hỗ trợ Intel / AMD" },
                    { "SP22", 3200000m, "CT06", "LSP08", "[\"TNHIET_h100i_1.jpg\", \"TNHIET_h100i_2.jpg\", \"TNHIET_h100i_3.jpg\", \"TNHIET_h100i_4.jpg\"]", "TNHIET_Corsair_iCUE_H100i_Elite_Liquid_Cooler.jpg", "Tản nhiệt nước Corsair iCUE H100i Elite với đèn RGB tùy chỉnh, hiệu suất làm mát cao, điều khiển thông minh qua phần mềm iCUE.", 5, 20, "Corsair iCUE H100i Elite Liquid Cooler", "- Loại tản nước AIO 240mm\r\n- 2 quạt 120mm RGB\r\n- Điều khiển qua phần mềm iCUE\r\n- Hỗ trợ Intel & AMD" },
                    { "SP23", 5600000m, "CT03", "LSP09", "[\"MHINH_TUF_1.jpg\", \"MHINH_TUF_2.jpg\", \"MHINH_TUF_3.jpg\", \"MHINH_TUF_4.jpg\"]", "MHinh_MASUS_TUF_Gaming_VG259QM.jpg", "Màn hình ASUS TUF VG259QM với tần số quét 280Hz và thời gian phản hồi 1ms, hiển thị mượt mà cho các game eSports.", 5, 15, "ASUS TUF Gaming VG259QM 24.5 inch, FHD, IPS, 280Hz, 1ms", "- Kích thước 24.5 inch\r\n- Tấm nền IPS\r\n- Tần số quét 280Hz\r\n- Thời gian phản hồi 1ms" },
                    { "SP24", 4700000m, "CT04", "LSP09", "[\"MHINH_MSI_1.png\", \"MHINH_MSI_2.png\", \"MHINH_MSI_3.png\", \"MHINH_MSI_4.png\"]", "MHinh_MSI_G2712FDE.jpg", "MSI G2712FDE mang đến trải nghiệm hình ảnh sống động, tấm nền IPS cao cấp và tần số quét 180Hz cực nhanh.", 10, 35, "MSI G2712FDE (27 inch, Full HD, 180Hz, Rapid IPS, 1ms, Black)", "- Kích thước 27 inch\r\n- Độ phân giải Full HD\r\n- Tần số quét 180Hz\r\n- Tấm nền Rapid IPS" },
                    { "SP25", 3600000m, "CT05", "LSP09", "[\"MHINH_GGB_1.png\", \"MHINH_GGB_2.png\", \"MHINH_GGB_3.png\", \"MHINH_GGB_4.png\"]", "MHinh_Gigabyte_G24F2.jpg", "Màn hình Gigabyte G24F2 có thiết kế mỏng nhẹ, tần số quét 165Hz, hỗ trợ ép xung 180Hz, rất lý tưởng cho game thủ FPS.", 15, 40, "Gigabyte G24F2 (23.8inch, FHD, IPS, 165Hz, 180Hz(OC), 1ms)", "- Kích thước 23.8 inch\r\n- Độ phân giải Full HD\r\n- Tần số quét 165Hz (OC 180Hz)\r\n- Tấm nền IPS / Thời gian phản hồi 1ms" }
                });

            migrationBuilder.InsertData(
                table: "XaPhuong",
                columns: new[] { "idXaPhuong", "idTinh", "tenXaPhuong" },
                values: new object[,]
                {
                    { "XP001", "T001", "Phường Hàng Bạc" },
                    { "XP002", "T001", "Phường Hàng Buồm" },
                    { "XP003", "T001", "Phường Tràng Tiền" },
                    { "XP004", "T001", "Phường Ngọc Hà" },
                    { "XP005", "T001", "Phường Kim Mã" },
                    { "XP006", "T001", "Phường Điện Biên" },
                    { "XP007", "T002", "Phường Bến Nghé" },
                    { "XP008", "T002", "Phường Bến Thành" },
                    { "XP009", "T002", "Phường Nguyễn Thái Bình" },
                    { "XP010", "T002", "Phường 1" },
                    { "XP011", "T002", "Phường 2" },
                    { "XP012", "T002", "Phường 3" },
                    { "XP013", "T003", "Phường Hải Châu I" },
                    { "XP014", "T003", "Phường Hải Châu II" },
                    { "XP015", "T003", "Phường Bình Hiên" },
                    { "XP016", "T003", "Phường An Khê" },
                    { "XP017", "T003", "Phường Chính Gián" },
                    { "XP018", "T003", "Phường Tam Thuận" },
                    { "XP019", "T004", "Phường An Biên" },
                    { "XP020", "T004", "Phường An Dương" },
                    { "XP021", "T004", "Phường Dư Hàng" },
                    { "XP022", "T004", "Phường Hoàng Văn Thụ" },
                    { "XP023", "T004", "Phường Hạ Lý" },
                    { "XP024", "T004", "Phường Quán Toan" },
                    { "XP025", "T005", "Phường An Hòa" },
                    { "XP026", "T005", "Phường An Nghiệp" },
                    { "XP027", "T005", "Phường Tân An" },
                    { "XP028", "T005", "Phường An Thới" },
                    { "XP029", "T005", "Phường Bình Thủy" },
                    { "XP030", "T005", "Phường Trà An" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "hoTen", "idPhuongXa", "soNha" },
                values: new object[] { "USER1", 0, "B3D2E5C5-A1E4-4D71-8B2E-C5D4B3A2C1D2", "minhhuy091@gmail.com", true, false, null, "MINHHUY91@GMAIL.COM", "MINHHUY91@GMAIL.COM", "AQAAAAIAAYagAAAAECostYXDzWMxjeRK8BZV9Y2l5j9jgqJ8h65CSvX0UQnI657xBoFczZpIOGj8p8Fm1Q==", "0987654321", false, "A2C1D2A4-F2D5-4E80-9A1F-A6B3A9B2F2A1", false, "minhhuy91@gmail.com", "Phạm Minh Huy", "XP001", "24 Bắc Đẩu" });

            migrationBuilder.InsertData(
                table: "ChiTietDonHang",
                columns: new[] { "idDonDat", "idSanPham", "donGia", "soluong" },
                values: new object[,]
                {
                    { "DH001", "SP01", 4500000m, 1 },
                    { "DH001", "SP08", 1200000m, 2 },
                    { "DH002", "SP03", 5200000m, 1 },
                    { "DH002", "SP05", 3500000m, 1 },
                    { "DH003", "SP14", 8500000m, 1 },
                    { "DH004", "SP02", 9500000m, 1 },
                    { "DH004", "SP06", 3800000m, 1 },
                    { "DH005", "SP10", 3100000m, 2 },
                    { "DH005", "SP15", 13500000m, 1 },
                    { "DH006", "SP17", 1500000m, 1 },
                    { "DH007", "SP11", 1100000m, 1 },
                    { "DH007", "SP19", 1800000m, 1 },
                    { "DH008", "SP04", 8700000m, 1 },
                    { "DH008", "SP07", 4200000m, 1 },
                    { "DH009", "SP12", 2300000m, 1 },
                    { "DH009", "SP22", 3400000m, 1 },
                    { "DH010", "SP13", 1800000m, 1 },
                    { "DH011", "SP16", 18500000m, 1 },
                    { "DH012", "SP18", 2800000m, 1 },
                    { "DH013", "SP20", 2200000m, 1 },
                    { "DH014", "SP21", 900000m, 1 },
                    { "DH015", "SP09", 1350000m, 2 },
                    { "DH016", "SP01", 4500000m, 1 },
                    { "DH017", "SP05", 3500000m, 1 },
                    { "DH018", "SP14", 8500000m, 1 },
                    { "DH019", "SP11", 1100000m, 1 },
                    { "DH020", "SP03", 5200000m, 1 }
                });

            migrationBuilder.InsertData(
                table: "ChiTietGioHang",
                columns: new[] { "idChiTietGioHang", "idNguoiDung", "idSanPham", "soLuongTrongGio" },
                values: new object[,]
                {
                    { "GH001", "USER1", "SP01", 1 },
                    { "GH002", "USER1", "SP08", 2 },
                    { "GH003", "USER1", "SP03", 1 },
                    { "GH004", "USER1", "SP05", 1 },
                    { "GH005", "USER1", "SP14", 1 },
                    { "GH006", "USER1", "SP19", 1 },
                    { "GH007", "USER1", "SP11", 1 },
                    { "GH008", "USER1", "SP02", 1 },
                    { "GH009", "USER1", "SP22", 1 },
                    { "GH010", "USER1", "SP16", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_idPhuongXa",
                table: "AspNetUsers",
                column: "idPhuongXa");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_idSanPham",
                table: "ChiTietDonHang",
                column: "idSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHang_idNguoiDung",
                table: "ChiTietGioHang",
                column: "idNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHang_idSanPham",
                table: "ChiTietGioHang",
                column: "idSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_idCongTy",
                table: "SanPham",
                column: "idCongTy");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_idLoaiSanPham",
                table: "SanPham",
                column: "idLoaiSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_XaPhuong_idTinh",
                table: "XaPhuong",
                column: "idTinh");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ChiTietDonHang");

            migrationBuilder.DropTable(
                name: "ChiTietGioHang");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DonDatHang");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "XaPhuong");

            migrationBuilder.DropTable(
                name: "CongTy");

            migrationBuilder.DropTable(
                name: "LoaiSanPham");

            migrationBuilder.DropTable(
                name: "Tinh");
        }
    }
}
