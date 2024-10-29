using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CheckDrive.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Add_AddCanStartCheckPointProcess_To_Driver : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
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

        migrationBuilder.AddColumn<bool>(
            name: "CanStartCheckPointProcess",
            table: "Employee",
            type: "bit",
            nullable: true);

        migrationBuilder.InsertData(
            table: "Role",
            columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            values: new object[,]
            {
                { "2545b628-cfb5-4357-b29b-ea8041104321", null, "Doctor", "DOCTOR" },
                { "3e5f618d-326c-40ec-9738-bc8a1e56f285", null, "Administrator", "ADMINISTRATOR" },
                { "9b1805b6-9bb9-4773-972d-b863ff786219", null, "Driver", "DRIVER" },
                { "a2be3212-944b-4083-9a12-35ae76e982ee", null, "Mechanic", "MECHANIC" },
                { "adedde3b-d787-4d3d-892d-95da8e334397", null, "Dispatcher", "DISPATCHER" },
                { "e8555128-ad21-4719-a289-089f30174e74", null, "Manager", "MANAGER" },
                { "f761f7c2-cd71-4c06-b869-c66778127bc6", null, "Operator", "OPERATOR" }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "2545b628-cfb5-4357-b29b-ea8041104321");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "3e5f618d-326c-40ec-9738-bc8a1e56f285");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "9b1805b6-9bb9-4773-972d-b863ff786219");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "a2be3212-944b-4083-9a12-35ae76e982ee");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "adedde3b-d787-4d3d-892d-95da8e334397");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "e8555128-ad21-4719-a289-089f30174e74");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "f761f7c2-cd71-4c06-b869-c66778127bc6");

        migrationBuilder.DropColumn(
            name: "CanStartCheckPointProcess",
            table: "Employee");

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
    }
}
