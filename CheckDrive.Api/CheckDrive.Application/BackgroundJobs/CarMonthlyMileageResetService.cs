using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CheckDrive.Application.BackgroundJobs;

internal sealed class CarMonthlyMileageResetService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public CarMonthlyMileageResetService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            DateTime now = DateTime.UtcNow;

            // Check if it's the first day of the month at midnight UTC
            if (now.Day == 1 && now.Hour == 0)
            {
                await ResetCarMonthlyMileage(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task ResetCarMonthlyMileage(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ICheckDriveDbContext>();

        await context.Cars.ExecuteUpdateAsync(
            x => x.SetProperty(x => x.CurrentMonthMileage, 0),
            stoppingToken);
    }
}
