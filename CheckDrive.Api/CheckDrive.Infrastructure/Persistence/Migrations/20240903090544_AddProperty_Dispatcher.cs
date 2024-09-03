using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckDrive.Infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProperty_Dispatcher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ChangedDistanceCovered",
                table: "DispatcherReview",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ChangedFuelSpendede",
                table: "DispatcherReview",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangedDistanceCovered",
                table: "DispatcherReview");

            migrationBuilder.DropColumn(
                name: "ChangedFuelSpendede",
                table: "DispatcherReview");
        }
    }
}
