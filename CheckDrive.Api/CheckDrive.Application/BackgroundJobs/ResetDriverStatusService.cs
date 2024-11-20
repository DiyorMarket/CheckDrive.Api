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
            var now = DateTime.UtcNow.AddHours(+5);

            if (now.Hour == 7)
            {
                await ResetDriversAsync();
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task ResetDriversAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ICheckDriveDbContext>();

        var driversToReset = await context.Drivers
            .Where(x => !x.IsDriverAvailableForReview)
            .Where(x => !x.Reviews.Any(r => r.CheckPoint.Status == CheckPointStatus.InProgress))
            .AsTracking()
            .ToListAsync();

        foreach (var driver in driversToReset)
        {
            driver.IsDriverAvailableForReview = true;
        }

        await context.SaveChangesAsync();
    }
}
