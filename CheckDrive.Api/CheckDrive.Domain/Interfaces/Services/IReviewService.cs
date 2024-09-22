using CheckDrive.ApiContracts.Driver;

namespace CheckDrive.Domain.Interfaces.Services;

public interface IReviewService
{
    Task<List<DriverReviewDto>> GetReviewsAsync(int driverId);
}
