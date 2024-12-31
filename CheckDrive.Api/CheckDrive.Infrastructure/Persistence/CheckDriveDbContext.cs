using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;

namespace CheckDrive.Infrastructure.Persistence;

public class CheckDriveDbContext : IdentityDbContext, ICheckDriveDbContext
{
    public virtual DbSet<Car> Cars { get; set; }
    public virtual DbSet<OilMark> OilMarks { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Driver> Drivers { get; set; }
    public virtual DbSet<Doctor> Doctors { get; set; }
    public virtual DbSet<Mechanic> Mechanics { get; set; }
    public virtual DbSet<Operator> Operators { get; set; }
    public virtual DbSet<Dispatcher> Dispatchers { get; set; }
    public virtual DbSet<Manager> Managers { get; set; }
    public virtual DbSet<CheckPoint> CheckPoints { get; set; }
    public virtual DbSet<DoctorReview> DoctorReviews { get; set; }
    public virtual DbSet<MechanicHandover> MechanicHandovers { get; set; }
    public virtual DbSet<OperatorReview> OperatorReviews { get; set; }
    public virtual DbSet<MechanicAcceptance> MechanicAcceptances { get; set; }
    public virtual DbSet<DispatcherReview> DispatcherReviews { get; set; }
    public virtual DbSet<ManagerReview> ManagerReviews { get; set; }
    public virtual DbSet<Debt> Debts { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public CheckDriveDbContext(DbContextOptions<CheckDriveDbContext> options)
        : base(options)
    {
        // Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);

        #region Identity

        builder.Entity<IdentityUser>(e =>
        {
            e.ToTable("User");
        });

        builder.Entity<IdentityRole>(e =>
        {
            e.ToTable("Role");
        });

        builder.Entity<IdentityUserClaim<string>>(e =>
        {
            e.ToTable("UserClaim");
        });

        builder.Entity<IdentityUserLogin<string>>(e =>
        {
            e.ToTable("UserLogin");
        });

        builder.Entity<IdentityUserToken<string>>(e =>
        {
            e.ToTable("UserToken");
        });

        builder.Entity<IdentityRoleClaim<string>>(e =>
        {
            e.ToTable("RoleClaim");
        });

        builder.Entity<IdentityUserRole<string>>(e =>
        {
            e.ToTable("UserRole");
        });

        #endregion

        #region Default Roles

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole()
            {
                Id = "2bdbb3ad-886f-49a0-a5f0-2023c975f93c",
                Name = Application.Constants.Roles.Administrator,
                NormalizedName = Application.Constants.Roles.Administrator.ToUpper(),
            },
            new IdentityRole()
            {
                Id = "ed2af201-9f95-4b05-a7db-ba18d139279d",
                Name = Application.Constants.Roles.Driver,
                NormalizedName = Application.Constants.Roles.Driver.ToUpper(),
            },
            new IdentityRole()
            {
                Id = "c559df5f-57dc-494b-a14e-5c9d2a3816ba",
                Name = Application.Constants.Roles.Doctor,
                NormalizedName = Application.Constants.Roles.Doctor.ToUpper(),
            },
            new IdentityRole()
            {
                Id = "70583108-618b-4308-b004-519d83379f6c",
                Name = Application.Constants.Roles.Dispatcher,
                NormalizedName = Application.Constants.Roles.Dispatcher.ToUpper(),
            },
            new IdentityRole()
            {
                Id = "e4b3ca5f-f8d1-4fae-9683-a49a423e1f1b",
                Name = Application.Constants.Roles.Manager,
                NormalizedName = Application.Constants.Roles.Manager.ToUpper(),
            },
            new IdentityRole()
            {
                Id = "f40933b8-3822-46b4-b6e4-9c674c03a6eb",
                Name = Application.Constants.Roles.Mechanic,
                NormalizedName = Application.Constants.Roles.Mechanic.ToUpper(),
            },
            new IdentityRole()
            {
                Id = "49e83980-05d8-4be4-a74d-abc2e11e6aed",
                Name = Application.Constants.Roles.Operator,
                NormalizedName = Application.Constants.Roles.Operator.ToUpper(),
            }
        );

        #endregion
    }

    public IDbContextTransaction BeginTransaction()
        => Database.BeginTransaction();
}
