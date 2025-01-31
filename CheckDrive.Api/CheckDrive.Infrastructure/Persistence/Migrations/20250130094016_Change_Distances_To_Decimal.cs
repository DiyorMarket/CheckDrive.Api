using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckDrive.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Changed_distances_to_decimal : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<decimal>(
            name: "UsageSummary_CurrentYearDistance",
            table: "Car",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<decimal>(
            name: "UsageSummary_CurrentMonthDistance",
            table: "Car",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<decimal>(
            name: "Mileage",
            table: "Car",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<decimal>(
            name: "Limits_YearlyDistanceLimit",
            table: "Car",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<decimal>(
            name: "Limits_MonthlyDistanceLimit",
            table: "Car",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "UsageSummary_CurrentYearDistance",
            table: "Car",
            type: "int",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");

        migrationBuilder.AlterColumn<int>(
            name: "UsageSummary_CurrentMonthDistance",
            table: "Car",
            type: "int",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");

        migrationBuilder.AlterColumn<int>(
            name: "Mileage",
            table: "Car",
            type: "int",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");

        migrationBuilder.AlterColumn<int>(
            name: "Limits_YearlyDistanceLimit",
            table: "Car",
            type: "int",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");

        migrationBuilder.AlterColumn<int>(
            name: "Limits_MonthlyDistanceLimit",
            table: "Car",
            type: "int",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");
    }
}
