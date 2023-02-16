using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserOtpAuthenticator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserEmailAuthenticators_UserId",
                table: "UserEmailAuthenticators");

            migrationBuilder.CreateTable(
                name: "UserOtpAuthenticators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SecretKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOtpAuthenticators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOtpAuthenticators_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEmailAuthenticators_UserId",
                table: "UserEmailAuthenticators",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserOtpAuthenticators_UserId",
                table: "UserOtpAuthenticators",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOtpAuthenticators");

            migrationBuilder.DropIndex(
                name: "IX_UserEmailAuthenticators_UserId",
                table: "UserEmailAuthenticators");

            migrationBuilder.CreateIndex(
                name: "IX_UserEmailAuthenticators_UserId",
                table: "UserEmailAuthenticators",
                column: "UserId");
        }
    }
}
