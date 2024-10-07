using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CheckDrive.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Add_Manager_ManagerReview : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "46a46741-5b52-444e-a5f9-169556a44c5b");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "52e9693c-ccc2-453b-bb73-6f1987c60664");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "87ed0262-1d37-4824-ae87-f1a8119c9d50");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "8da8eab2-4243-44e1-bfae-0d7a86135bea");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "bcaf852e-2688-44c1-aa77-4b2d65231832");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "d864ade1-ac59-4ab3-ba59-a6c12f3250e3");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "d907a396-369a-4fde-8f7d-3918e185f554");

        migrationBuilder.AddColumn<int>(
            name: "ManagerReviewId",
            table: "Debt",
            type: "int",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "ManagerReview",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                DebtAmountAdjusment = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                FuelConsumptionAdjustment = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                CheckPointId = table.Column<int>(type: "int", nullable: false),
                ManagerId = table.Column<int>(type: "int", nullable: false),
                DebtId = table.Column<int>(type: "int", nullable: true),
                Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ManagerReview", x => x.Id);
                table.ForeignKey(
                    name: "FK_ManagerReview_CheckPoint_CheckPointId",
                    column: x => x.CheckPointId,
                    principalTable: "CheckPoint",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_ManagerReview_Debt_ManagerId",
                    column: x => x.ManagerId,
                    principalTable: "Debt",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_ManagerReview_Employee_ManagerId",
                    column: x => x.ManagerId,
                    principalTable: "Employee",
                    principalColumn: "Id");
            });

        migrationBuilder.InsertData(
            table: "Role",
            columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            values: new object[,]
            {
                { "5bd07b96-507a-4b1a-b9ff-b421cfe1d205", null, "Driver", "DRIVER" },
                { "7fbd87ba-5311-4855-b18f-54b7e711eb82", null, "Mechanic", "MECHANIC" },
                { "8b08e145-6325-4046-b55c-77b9906966b5", null, "Doctor", "DOCTOR" },
                { "c9734b82-c84a-43b2-b453-c4ff6eb8c1cf", null, "Operator", "OPERATOR" },
                { "d6c0f8c6-b894-44ff-80f9-eb65961d8002", null, "Administrator", "ADMINISTRATOR" },
                { "ed4099e0-601c-414d-b763-f2151a29040d", null, "Dispatcher", "DISPATCHER" },
                { "ee9613ef-226d-4fba-9ab3-9a07d902018b", null, "Manager", "MANAGER" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_ManagerReview_CheckPointId",
            table: "ManagerReview",
            column: "CheckPointId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ManagerReview_ManagerId",
            table: "ManagerReview",
            column: "ManagerId",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ManagerReview");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "5bd07b96-507a-4b1a-b9ff-b421cfe1d205");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "7fbd87ba-5311-4855-b18f-54b7e711eb82");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "8b08e145-6325-4046-b55c-77b9906966b5");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "c9734b82-c84a-43b2-b453-c4ff6eb8c1cf");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "d6c0f8c6-b894-44ff-80f9-eb65961d8002");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "ed4099e0-601c-414d-b763-f2151a29040d");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "ee9613ef-226d-4fba-9ab3-9a07d902018b");

        migrationBuilder.DropColumn(
            name: "ManagerReviewId",
            table: "Debt");

        migrationBuilder.InsertData(
            table: "Role",
            columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            values: new object[,]
            {
                { "46a46741-5b52-444e-a5f9-169556a44c5b", null, "Administrator", "ADMINISTRATOR" },
                { "52e9693c-ccc2-453b-bb73-6f1987c60664", null, "Driver", "DRIVER" },
                { "87ed0262-1d37-4824-ae87-f1a8119c9d50", null, "Manager", "MANAGER" },
                { "8da8eab2-4243-44e1-bfae-0d7a86135bea", null, "Dispatcher", "DISPATCHER" },
                { "bcaf852e-2688-44c1-aa77-4b2d65231832", null, "Mechanic", "MECHANIC" },
                { "d864ade1-ac59-4ab3-ba59-a6c12f3250e3", null, "Doctor", "DOCTOR" },
                { "d907a396-369a-4fde-8f7d-3918e185f554", null, "Operator", "OPERATOR" }
            });
    }
}
