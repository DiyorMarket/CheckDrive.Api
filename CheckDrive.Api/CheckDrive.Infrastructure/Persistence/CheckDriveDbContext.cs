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
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Driver> Drivers { get; set; }
    public virtual DbSet<Doctor> Doctors { get; set; }
    public virtual DbSet<Mechanic> Mechanics { get; set; }
    public virtual DbSet<Operator> Operators { get; set; }
    public virtual DbSet<Dispatcher> Dispatchers { get; set; }
    public virtual DbSet<Manager> Managers { get; set; }
    public virtual DbSet<Car> Cars { get; set; }
    public virtual DbSet<CheckPoint> CheckPoints { get; set; }
    public virtual DbSet<DoctorReview> DoctorReviews { get; set; }
    public virtual DbSet<MechanicHandover> MechanicHandovers { get; set; }
    public virtual DbSet<OperatorReview> OperatorReviews { get; set; }
    public virtual DbSet<MechanicAcceptance> MechanicAcceptances { get; set; }
    public virtual DbSet<DispatcherReview> DispatcherReviews { get; set; }
    public virtual DbSet<ManagerReview> ManagerReviews { get; set; }
    public virtual DbSet<Debt> Debts { get; set; }
    public virtual DbSet<OilMark> OilMarks { get; set; }

    public CheckDriveDbContext(DbContextOptions<CheckDriveDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
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
                Id = Guid.NewGuid().ToString(),
                Name = Application.Constants.Roles.Administrator,
                NormalizedName = Application.Constants.Roles.Administrator.ToUpper(),
            },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Application.Constants.Roles.Driver,
                NormalizedName = Application.Constants.Roles.Driver.ToUpper(),
            },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Application.Constants.Roles.Doctor,
                NormalizedName = Application.Constants.Roles.Doctor.ToUpper(),
            },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Application.Constants.Roles.Dispatcher,
                NormalizedName = Application.Constants.Roles.Dispatcher.ToUpper(),
            },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Application.Constants.Roles.Manager,
                NormalizedName = Application.Constants.Roles.Manager.ToUpper(),
            },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Application.Constants.Roles.Mechanic,
                NormalizedName = Application.Constants.Roles.Mechanic.ToUpper(),
            },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Application.Constants.Roles.Operator,
                NormalizedName = Application.Constants.Roles.Operator.ToUpper(),
            }
        );

        #endregion
    }

    public IDbContextTransaction BeginTransaction()
        => Database.BeginTransaction();
}
