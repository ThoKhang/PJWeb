using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBNC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addChatModelsToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatSessions",
                columns: table => new
                {
                    idSession = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatSessions", x => x.idSession);
                    table.ForeignKey(
                        name: "FK_ChatSessions_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    idMessage = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idSession = table.Column<int>(type: "int", nullable: false),
                    role = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.idMessage);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatSessions_idSession",
                        column: x => x.idSession,
                        principalTable: "ChatSessions",
                        principalColumn: "idSession",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_idSession",
                table: "ChatMessages",
                column: "idSession");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_userId",
                table: "ChatSessions",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ChatSessions");
        }
    }
}
