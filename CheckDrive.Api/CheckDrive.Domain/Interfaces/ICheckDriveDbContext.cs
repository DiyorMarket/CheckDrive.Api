using CheckDrive.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CheckDrive.Domain.Interfaces;

public interface ICheckDriveDbContext
{
    DbSet<Driver> Drivers { get; set; }
    DbSet<Doctor> Doctors { get; set; }
    DbSet<Mechanic> Managers { get; set; }
    DbSet<Mechanic> Mechanics { get; set; }
    DbSet<Operator> Operators { get; set; }
    DbSet<Dispatcher> Dispatchers { get; set; }
    DbSet<Car> Cars { get; set; }
    DbSet<CheckPoint> CheckPoints { get; set; }
    DbSet<DoctorReview> DoctorReviews { get; set; }
    DbSet<MechanicHandover> MechanicHandovers { get; set; }
    DbSet<MechanicHandover> ManagerReviews { get; set; }
    DbSet<OperatorReview> OperatorReviews { get; set; }
    DbSet<MechanicAcceptance> MechanicAcceptances { get; set; }
    DbSet<DispatcherReview> DispatcherReviews { get; set; }
    DbSet<Debt> Debts { get; set; }
    DbSet<OilMark> OilMarks { get; set; }

    // Identity
    DbSet<IdentityUser> Users { get; set; }
    DbSet<IdentityRole> Roles { get; set; }
    DbSet<IdentityUserRole<string>> UserRoles { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    IDbContextTransaction BeginTransaction();
}
