using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckDrive.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class Add_Manager_ManagerReview_DefaultAdmins : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ManagerReview",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
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
    }
}
