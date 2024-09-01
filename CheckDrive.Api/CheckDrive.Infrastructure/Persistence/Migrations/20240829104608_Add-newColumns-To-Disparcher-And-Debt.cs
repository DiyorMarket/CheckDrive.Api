using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckDrive.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddnewColumnsToDisparcherAndDebt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "DispatcherReview",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "DispatcherReview",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Account",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Passport",
                table: "Account",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Account",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Debts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OilAmount = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Debts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Debts_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Driver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Debts_DriverId",
                table: "Debts",
                column: "DriverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Debts");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "DispatcherReview");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DispatcherReview");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Passport",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Account");
        }
    }
}
