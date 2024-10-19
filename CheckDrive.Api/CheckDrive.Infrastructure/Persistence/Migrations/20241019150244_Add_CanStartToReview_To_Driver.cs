using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CheckDrive.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Add_CanStartToReview_To_Driver : Migration
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
            name: "CanStartToReview",
            table: "Employee",
            type: "bit",
            nullable: true);

        migrationBuilder.InsertData(
            table: "Role",
            columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            values: new object[,]
            {
                { "03039f19-2a64-4b66-96f8-20fe0751f39d", null, "Driver", "DRIVER" },
                { "55a92e9e-756b-461b-acc1-bff5a9f11668", null, "Mechanic", "MECHANIC" },
                { "6d59e31b-348a-4212-815c-b8582fa107e6", null, "Administrator", "ADMINISTRATOR" },
                { "964c5a44-d170-4824-b41d-3c7875416c6a", null, "Doctor", "DOCTOR" },
                { "aa9df118-1c6e-4d98-8638-09315613799e", null, "Dispatcher", "DISPATCHER" },
                { "cbb9f901-ee05-419e-8723-e8f7ae86b1ca", null, "Operator", "OPERATOR" },
                { "cfde30c8-db21-4486-9efa-f965e8090e25", null, "Manager", "MANAGER" }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "03039f19-2a64-4b66-96f8-20fe0751f39d");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "55a92e9e-756b-461b-acc1-bff5a9f11668");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "6d59e31b-348a-4212-815c-b8582fa107e6");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "964c5a44-d170-4824-b41d-3c7875416c6a");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "aa9df118-1c6e-4d98-8638-09315613799e");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "cbb9f901-ee05-419e-8723-e8f7ae86b1ca");

        migrationBuilder.DeleteData(
            table: "Role",
            keyColumn: "Id",
            keyValue: "cfde30c8-db21-4486-9efa-f965e8090e25");

        migrationBuilder.DropColumn(
            name: "CanStartToReview",
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
