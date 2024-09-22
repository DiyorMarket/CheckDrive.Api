using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CheckDrive.Infrastructure.Persistence;

public class CheckDriveDbContext : DbContext
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Car> Cars { get; set; }
    public virtual DbSet<CheckPoint> CheckPoints { get; set; }
    public virtual DbSet<DoctorReview> DoctorReviews { get; set; }
    public virtual DbSet<MechanicHandover> MechanicHandovers { get; set; }
    public virtual DbSet<OperatorReview> OperatorReviews { get; set; }
    public virtual DbSet<MechanicAcceptance> MechanicAcceptances { get; set; }
    public virtual DbSet<DispatcherReview> DispatcherReviews { get; set; }
    public virtual DbSet<Debt> Debts { get; set; }
    public virtual DbSet<OilMark> OilMarks { get; set; }

    public CheckDriveDbContext(DbContextOptions<CheckDriveDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
        Database.Migrate();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
