using CheckDrive.ApiContracts.Driver;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Infrastructure.Persistence;

namespace CheckDrive.Services;

public class DriverReviewService : IReviewService
{
    private readonly CheckDriveDbContext _context;

    public DriverReviewService(CheckDriveDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<DriverReviewDto>> GetReviewsAsync(int driverId)
    {
        throw new NotImplementedException();
    }
}
