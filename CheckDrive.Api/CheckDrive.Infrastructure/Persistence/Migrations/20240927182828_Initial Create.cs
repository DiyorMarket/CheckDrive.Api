using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckDrive.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Car",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Model = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Color = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                ManufacturedYear = table.Column<int>(type: "int", nullable: false),
                Mileage = table.Column<int>(type: "int", nullable: false),
                YearlyDistanceLimit = table.Column<int>(type: "int", nullable: false),
                AverageFuelConsumption = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                FuelCapacity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                RemainingFuel = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Car", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CheckPoint",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                Stage = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CheckPoint", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "OilMark",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OilMark", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Role",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Role", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "User",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_User", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Debt",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FuelAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                PaidAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                CheckPointId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Debt", x => x.Id);
                table.ForeignKey(
                    name: "FK_Debt_CheckPoint_CheckPointId",
                    column: x => x.CheckPointId,
                    principalTable: "CheckPoint",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "RoleClaim",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleClaim", x => x.Id);
                table.ForeignKey(
                    name: "FK_RoleClaim_Role_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Role",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Employee",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Passport = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Position = table.Column<int>(type: "int", nullable: false),
                AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Employee", x => x.Id);
                table.ForeignKey(
                    name: "FK_Employee_User_AccountId",
                    column: x => x.AccountId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserClaim",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserClaim", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserClaim_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserLogin",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserLogin", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_UserLogin_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserRole",
            columns: table => new
            {
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_UserRole_Role_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Role",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserRole_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserToken",
            columns: table => new
            {
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_UserToken_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "DispatcherReview",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FuelConsumptionAdjustment = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                DistanceTravelledAdjustment = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                CheckPointId = table.Column<int>(type: "int", nullable: false),
                DispatcherId = table.Column<int>(type: "int", nullable: false),
                Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DispatcherReview", x => x.Id);
                table.ForeignKey(
                    name: "FK_DispatcherReview_CheckPoint_CheckPointId",
                    column: x => x.CheckPointId,
                    principalTable: "CheckPoint",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_DispatcherReview_Employee_DispatcherId",
                    column: x => x.DispatcherId,
                    principalTable: "Employee",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "DoctorReview",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CheckPointId = table.Column<int>(type: "int", nullable: false),
                DriverId = table.Column<int>(type: "int", nullable: false),
                DoctorId = table.Column<int>(type: "int", nullable: false),
                Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DoctorReview", x => x.Id);
                table.ForeignKey(
                    name: "FK_DoctorReview_CheckPoint_CheckPointId",
                    column: x => x.CheckPointId,
                    principalTable: "CheckPoint",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_DoctorReview_Employee_DoctorId",
                    column: x => x.DoctorId,
                    principalTable: "Employee",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_DoctorReview_Employee_DriverId",
                    column: x => x.DriverId,
                    principalTable: "Employee",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "MechanicAcceptance",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FinalMileage = table.Column<int>(type: "int", nullable: false),
                RemainingFuelAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                CheckPointId = table.Column<int>(type: "int", nullable: false),
                MechanicId = table.Column<int>(type: "int", nullable: false),
                Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MechanicAcceptance", x => x.Id);
                table.ForeignKey(
                    name: "FK_MechanicAcceptance_CheckPoint_CheckPointId",
                    column: x => x.CheckPointId,
                    principalTable: "CheckPoint",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_MechanicAcceptance_Employee_MechanicId",
                    column: x => x.MechanicId,
                    principalTable: "Employee",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "MechanicHandover",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                InitialMileage = table.Column<int>(type: "int", nullable: false),
                CheckPointId = table.Column<int>(type: "int", nullable: false),
                CarId = table.Column<int>(type: "int", nullable: false),
                MechanicId = table.Column<int>(type: "int", nullable: false),
                Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MechanicHandover", x => x.Id);
                table.ForeignKey(
                    name: "FK_MechanicHandover_Car_CarId",
                    column: x => x.CarId,
                    principalTable: "Car",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_MechanicHandover_CheckPoint_CheckPointId",
                    column: x => x.CheckPointId,
                    principalTable: "CheckPoint",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_MechanicHandover_Employee_MechanicId",
                    column: x => x.MechanicId,
                    principalTable: "Employee",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "OperatorReview",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                InitialOilAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                OilRefillAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                CheckPointId = table.Column<int>(type: "int", nullable: false),
                OilMarkId = table.Column<int>(type: "int", nullable: false),
                OperatorId = table.Column<int>(type: "int", nullable: false),
                Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OperatorReview", x => x.Id);
                table.ForeignKey(
                    name: "FK_OperatorReview_CheckPoint_CheckPointId",
                    column: x => x.CheckPointId,
                    principalTable: "CheckPoint",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_OperatorReview_Employee_OperatorId",
                    column: x => x.OperatorId,
                    principalTable: "Employee",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_OperatorReview_OilMark_OilMarkId",
                    column: x => x.OilMarkId,
                    principalTable: "OilMark",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Debt_CheckPointId",
            table: "Debt",
            column: "CheckPointId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_DispatcherReview_CheckPointId",
            table: "DispatcherReview",
            column: "CheckPointId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_DispatcherReview_DispatcherId",
            table: "DispatcherReview",
            column: "DispatcherId");

        migrationBuilder.CreateIndex(
            name: "IX_DoctorReview_CheckPointId",
            table: "DoctorReview",
            column: "CheckPointId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_DoctorReview_DoctorId",
            table: "DoctorReview",
            column: "DoctorId");

        migrationBuilder.CreateIndex(
            name: "IX_DoctorReview_DriverId",
            table: "DoctorReview",
            column: "DriverId");

        migrationBuilder.CreateIndex(
            name: "IX_Employee_AccountId",
            table: "Employee",
            column: "AccountId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_MechanicAcceptance_CheckPointId",
            table: "MechanicAcceptance",
            column: "CheckPointId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_MechanicAcceptance_MechanicId",
            table: "MechanicAcceptance",
            column: "MechanicId");

        migrationBuilder.CreateIndex(
            name: "IX_MechanicHandover_CarId",
            table: "MechanicHandover",
            column: "CarId");

        migrationBuilder.CreateIndex(
            name: "IX_MechanicHandover_CheckPointId",
            table: "MechanicHandover",
            column: "CheckPointId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_MechanicHandover_MechanicId",
            table: "MechanicHandover",
            column: "MechanicId");

        migrationBuilder.CreateIndex(
            name: "IX_OperatorReview_CheckPointId",
            table: "OperatorReview",
            column: "CheckPointId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_OperatorReview_OilMarkId",
            table: "OperatorReview",
            column: "OilMarkId");

        migrationBuilder.CreateIndex(
            name: "IX_OperatorReview_OperatorId",
            table: "OperatorReview",
            column: "OperatorId");

        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            table: "Role",
            column: "NormalizedName",
            unique: true,
            filter: "[NormalizedName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_RoleClaim_RoleId",
            table: "RoleClaim",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "User",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            table: "User",
            column: "NormalizedUserName",
            unique: true,
            filter: "[NormalizedUserName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_UserClaim_UserId",
            table: "UserClaim",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserLogin_UserId",
            table: "UserLogin",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserRole_RoleId",
            table: "UserRole",
            column: "RoleId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Debt");

        migrationBuilder.DropTable(
            name: "DispatcherReview");

        migrationBuilder.DropTable(
            name: "DoctorReview");

        migrationBuilder.DropTable(
            name: "MechanicAcceptance");

        migrationBuilder.DropTable(
            name: "MechanicHandover");

        migrationBuilder.DropTable(
            name: "OperatorReview");

        migrationBuilder.DropTable(
            name: "RoleClaim");

        migrationBuilder.DropTable(
            name: "UserClaim");

        migrationBuilder.DropTable(
            name: "UserLogin");

        migrationBuilder.DropTable(
            name: "UserRole");

        migrationBuilder.DropTable(
            name: "UserToken");

        migrationBuilder.DropTable(
            name: "Car");

        migrationBuilder.DropTable(
            name: "CheckPoint");

        migrationBuilder.DropTable(
            name: "Employee");

        migrationBuilder.DropTable(
            name: "OilMark");

        migrationBuilder.DropTable(
            name: "Role");

        migrationBuilder.DropTable(
            name: "User");
    }
}
