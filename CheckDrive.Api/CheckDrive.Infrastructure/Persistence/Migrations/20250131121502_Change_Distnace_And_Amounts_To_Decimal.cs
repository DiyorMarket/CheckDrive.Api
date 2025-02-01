using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckDrive.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Change_Distnace_And_Amounts_To_Decimal : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<decimal>(
            name: "InitialMileage",
            table: "MechanicHandover",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<decimal>(
            name: "FinalMileage",
            table: "MechanicAcceptance",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<decimal>(
            name: "InitialMileage",
            table: "ManagerReview",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<decimal>(
            name: "FinalMileage",
            table: "ManagerReview",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<decimal>(
            name: "FinalMileage",
            table: "DispatcherReview",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "InitialMileage",
            table: "MechanicHandover",
            type: "int",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");

        migrationBuilder.AlterColumn<int>(
            name: "FinalMileage",
            table: "MechanicAcceptance",
            type: "int",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");

        migrationBuilder.AlterColumn<int>(
            name: "InitialMileage",
            table: "ManagerReview",
            type: "int",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");

        migrationBuilder.AlterColumn<int>(
            name: "FinalMileage",
            table: "ManagerReview",
            type: "int",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");

        migrationBuilder.AlterColumn<int>(
            name: "FinalMileage",
            table: "DispatcherReview",
            type: "int",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");
    }
}
