using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddModelFuelTransmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "DailyPrice",
                table: "Models",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "FuelId",
                table: "Models",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransmissionId",
                table: "Models",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Fuels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fuels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transmissions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Fuels",
                columns: new[] { "Id", "CreatedDate", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diesel", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Electric", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DailyPrice", "FuelId", "TransmissionId" },
                values: new object[] { 2000.0, 1, 1 });

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DailyPrice", "FuelId", "TransmissionId" },
                values: new object[] { 3000.0, 1, 2 });

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DailyPrice", "FuelId", "TransmissionId" },
                values: new object[] { 2500.0, 2, 2 });

            migrationBuilder.InsertData(
                table: "Transmissions",
                columns: new[] { "Id", "CreatedDate", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manuel", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Automatic", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Models_FuelId",
                table: "Models",
                column: "FuelId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_TransmissionId",
                table: "Models",
                column: "TransmissionId");

            migrationBuilder.CreateIndex(
                name: "UK_Fuels_Name",
                table: "Fuels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Transmissions_Name",
                table: "Transmissions",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Models_Fuels_FuelId",
                table: "Models",
                column: "FuelId",
                principalTable: "Fuels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Models_Transmissions_TransmissionId",
                table: "Models",
                column: "TransmissionId",
                principalTable: "Transmissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Models_Fuels_FuelId",
                table: "Models");

            migrationBuilder.DropForeignKey(
                name: "FK_Models_Transmissions_TransmissionId",
                table: "Models");

            migrationBuilder.DropTable(
                name: "Fuels");

            migrationBuilder.DropTable(
                name: "Transmissions");

            migrationBuilder.DropIndex(
                name: "IX_Models_FuelId",
                table: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Models_TransmissionId",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "FuelId",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "TransmissionId",
                table: "Models");

            migrationBuilder.AlterColumn<decimal>(
                name: "DailyPrice",
                table: "Models",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 1,
                column: "DailyPrice",
                value: 2000m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 2,
                column: "DailyPrice",
                value: 3000m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 3,
                column: "DailyPrice",
                value: 2500m);
        }
    }
}
