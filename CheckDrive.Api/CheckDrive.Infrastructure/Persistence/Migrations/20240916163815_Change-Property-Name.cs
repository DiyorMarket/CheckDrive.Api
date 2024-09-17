using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckDrive.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangePropertyName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemainingFuel",
                table: "MechanicAcceptance",
                newName: "RemainingFuelInCar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemainingFuelInCar",
                table: "MechanicAcceptance",
                newName: "RemainingFuel");
        }
    }
}
