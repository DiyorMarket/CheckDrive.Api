using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckDrive.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoPropertiesToMechanicAccepatnceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "OilAmount",
                table: "MechanicAcceptance",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RemainingFuel",
                table: "MechanicAcceptance",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OilAmount",
                table: "MechanicAcceptance");

            migrationBuilder.DropColumn(
                name: "RemainingFuel",
                table: "MechanicAcceptance");
        }
    }
}
