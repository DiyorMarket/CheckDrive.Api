using CheckDrive.Domain.Entities.Identity;
using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Interfaces;

public interface ICheckDriveDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<Car> Cars { get; set; }
    DbSet<CheckPoint> CheckPoints { get; set; }
    DbSet<DoctorReview> DoctorReviews { get; set; }
    DbSet<MechanicHandover> MechanicHandovers { get; set; }
    DbSet<OperatorReview> OperatorReviews { get; set; }
    DbSet<MechanicAcceptance> MechanicAcceptances { get; set; }
    DbSet<DispatcherReview> DispatcherReviews { get; set; }
    DbSet<Debt> Debts { get; set; }
    DbSet<OilMark> OilMarks { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
