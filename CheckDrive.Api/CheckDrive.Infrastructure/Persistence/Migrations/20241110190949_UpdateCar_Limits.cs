using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CheckDrive.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCar_Limits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentMonthFuelConsumption",
                table: "Car",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentYearFuelConsumption",
                table: "Car",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CurrentYearMileage",
                table: "Car",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MonthlyDistanceLimit",
                table: "Car",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyFuelConsumptionLimit",
                table: "Car",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "YearlyFuelConsumptionLimit",
                table: "Car",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentMonthFuelConsumption",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "CurrentYearFuelConsumption",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "CurrentYearMileage",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "MonthlyDistanceLimit",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "MonthlyFuelConsumptionLimit",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "YearlyFuelConsumptionLimit",
                table: "Car");
        }
    }
}
