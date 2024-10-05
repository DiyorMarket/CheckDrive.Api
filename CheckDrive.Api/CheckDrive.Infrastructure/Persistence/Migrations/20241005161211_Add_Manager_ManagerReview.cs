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
            keyValue: "17222f04-4d61-48c7-b50b-16287b8f1d70");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "2ff29e6d-a8df-4dcc-af3b-7dfd59a13510");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "6f6835f9-ea44-483e-aa69-97d452868bff");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "89887e6f-24d2-4fc5-a874-af4b4258d7b1");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "b8182f0e-79e3-4c31-b5a2-5c74a8598990");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "c58e9004-eef9-4568-9205-41d84054d234");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "f7d4b7c5-83e1-41ef-aec8-15722f67532a");

        migrationBuilder.InsertData(
            table: "Role",
            columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            values: new object[,]
            {
                { "099bbfb1-a22a-48f8-a707-bd954851b749", null, "Manager", "MANAGER" },
                { "1eef2d65-63aa-4bd3-ad11-97b05465411a", null, "Administrator", "ADMINISTRATOR" },
                { "2f72a51b-f05d-49b5-98fc-c2c1b32219e1", null, "Operator", "OPERATOR" },
                { "50732e08-b22e-4d0c-8196-13a14fda4edb", null, "Driver", "DRIVER" },
                { "78333ddd-03ba-4274-ae24-321c3563584a", null, "Mechanic", "MECHANIC" },
                { "95e28700-5bea-4855-bda5-4fe4660dbaaa", null, "Doctor", "DOCTOR" },
                { "b8854b49-d887-4ac9-9ffe-bc391909380c", null, "Dispatcher", "DISPATCHER" }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "099bbfb1-a22a-48f8-a707-bd954851b749");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "1eef2d65-63aa-4bd3-ad11-97b05465411a");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "2f72a51b-f05d-49b5-98fc-c2c1b32219e1");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "50732e08-b22e-4d0c-8196-13a14fda4edb");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "78333ddd-03ba-4274-ae24-321c3563584a");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "95e28700-5bea-4855-bda5-4fe4660dbaaa");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "b8854b49-d887-4ac9-9ffe-bc391909380c");

        migrationBuilder.InsertData(
            table: "Role",
            columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            values: new object[,]
            {
                { "17222f04-4d61-48c7-b50b-16287b8f1d70", null, "Administrator", "ADMINISTRATOR" },
                { "2ff29e6d-a8df-4dcc-af3b-7dfd59a13510", null, "Manager", "MANAGER" },
                { "6f6835f9-ea44-483e-aa69-97d452868bff", null, "Operator", "OPERATOR" },
                { "89887e6f-24d2-4fc5-a874-af4b4258d7b1", null, "Doctor", "DOCTOR" },
                { "b8182f0e-79e3-4c31-b5a2-5c74a8598990", null, "Driver", "DRIVER" },
                { "c58e9004-eef9-4568-9205-41d84054d234", null, "Dispatcher", "DISPATCHER" },
                { "f7d4b7c5-83e1-41ef-aec8-15722f67532a", null, "Mechanic", "MECHANIC" }
            });
    }
}
