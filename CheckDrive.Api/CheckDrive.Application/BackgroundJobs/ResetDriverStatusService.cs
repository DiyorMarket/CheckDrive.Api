using CheckDrive.Application.Constants;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CheckDrive.Application.BackgroundJobs;

internal sealed class ResetDriverStatusService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ResetDriverStatusService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow.AddHours(TimeConstants.TashkentTimeUtc);

            if (now.Hour == 7)
            {
                await PerformDailyResetAsync();
            }
            else // In case if server was down at 7 we should not wait until next day
            {
                await TryResetDriversWithReviewMoreThanOneDay();
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task PerformDailyResetAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ICheckDriveDbContext>();

        var driversToReset = await context.Drivers
            .Where(x => x.Status == DriverStatus.NotHealthy)
            .AsTracking()
            .ToListAsync();

        foreach (var driver in driversToReset)
        {
            driver.Status = DriverStatus.Available;
        }

        await context.SaveChangesAsync();
    }

    private async Task TryResetDriversWithReviewMoreThanOneDay()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ICheckDriveDbContext>();

        var driversToReset = await context.DoctorReviews
            .Include(x => x.Driver)
            .Where(x => x.IsHealthy)
            .Where(x => x.Date <= DateTime.UtcNow.AddDays(-1))
            .Select(x => x.Driver)
            .ToListAsync();

        foreach (var driver in driversToReset)
        {
            driver.Status = DriverStatus.Available;
        }

        await context.SaveChangesAsync();
    }
}
