using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckDrive.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class newFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OilMarks",
                table: "OperatorReview");

            migrationBuilder.AddColumn<int>(
                name: "OilMarkId",
                table: "OperatorReview",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OneYearMediumDistance",
                table: "Car",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OilMarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OilMark = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OilMarks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperatorReview_OilMarkId",
                table: "OperatorReview",
                column: "OilMarkId");

            migrationBuilder.AddForeignKey(
                name: "FK_OperatorReview_OilMarks_OilMarkId",
                table: "OperatorReview",
                column: "OilMarkId",
                principalTable: "OilMarks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperatorReview_OilMarks_OilMarkId",
                table: "OperatorReview");

            migrationBuilder.DropTable(
                name: "OilMarks");

            migrationBuilder.DropIndex(
                name: "IX_OperatorReview_OilMarkId",
                table: "OperatorReview");

            migrationBuilder.DropColumn(
                name: "OilMarkId",
                table: "OperatorReview");

            migrationBuilder.DropColumn(
                name: "OneYearMediumDistance",
                table: "Car");

            migrationBuilder.AddColumn<int>(
                name: "OilMarks",
                table: "OperatorReview",
                type: "int",
                maxLength: 255,
                nullable: false,
                defaultValue: 0);
        }
    }
}
