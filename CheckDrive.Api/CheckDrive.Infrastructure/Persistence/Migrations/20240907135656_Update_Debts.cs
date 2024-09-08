using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckDrive.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_Debts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Debts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Debts_CarId",
                table: "Debts",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Debts_Car_CarId",
                table: "Debts",
                column: "CarId",
                principalTable: "Car",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Debts_Car_CarId",
                table: "Debts");

            migrationBuilder.DropIndex(
                name: "IX_Debts_CarId",
                table: "Debts");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Debts");
        }
    }
}
