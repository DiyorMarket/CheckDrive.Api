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
                { "153b479b-2c52-4729-94e0-513fdec3f66e", null, "Doctor", "DOCTOR" },
                { "18c5b372-b34e-4194-838c-cdc1c64682f2", null, "Mechanic", "MECHANIC" },
                { "2d7e1223-bd95-4880-9805-54a68f84d74f", null, "Operator", "OPERATOR" },
                { "43e6d885-ec20-4a5a-a94a-8477c8a1fa60", null, "Dispatcher", "DISPATCHER" },
                { "5089a8a9-4e4f-4c6d-848c-23f2b3a3899c", null, "Administrator", "ADMINISTRATOR" },
                { "bdff891f-f7de-4ad0-9267-a5dda2cc22b9", null, "Manager", "MANAGER" },
                { "f481030e-c1b1-41fe-a889-119bf9efbe33", null, "Driver", "DRIVER" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_ManagerReview_CheckPointId",
            table: "ManagerReview",
            column: "CheckPointId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ManagerReview_ManagerId",
            table: "ManagerReview",
            column: "ManagerId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ManagerReview");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "153b479b-2c52-4729-94e0-513fdec3f66e");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "18c5b372-b34e-4194-838c-cdc1c64682f2");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "2d7e1223-bd95-4880-9805-54a68f84d74f");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "43e6d885-ec20-4a5a-a94a-8477c8a1fa60");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "5089a8a9-4e4f-4c6d-848c-23f2b3a3899c");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "bdff891f-f7de-4ad0-9267-a5dda2cc22b9");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "f481030e-c1b1-41fe-a889-119bf9efbe33");

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
