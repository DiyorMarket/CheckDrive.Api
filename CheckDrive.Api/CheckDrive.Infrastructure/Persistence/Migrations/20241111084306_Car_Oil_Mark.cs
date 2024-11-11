using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CheckDrive.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Car_Oil_Mark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OilMarkId",
                table: "Car",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Car_OilMarkId",
                table: "Car",
                column: "OilMarkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_OilMark_OilMarkId",
                table: "Car",
                column: "OilMarkId",
                principalTable: "OilMark",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_OilMark_OilMarkId",
                table: "Car");

            migrationBuilder.DropIndex(
                name: "IX_Car_OilMarkId",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "OilMarkId",
                table: "Car");
        }
    }
}
