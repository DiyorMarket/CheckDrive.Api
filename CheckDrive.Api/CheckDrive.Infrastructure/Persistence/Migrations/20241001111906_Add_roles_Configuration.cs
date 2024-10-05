using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CheckDrive.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Add_roles_Configuration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
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

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
