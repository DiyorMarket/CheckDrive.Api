using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CheckDrive.Application.BackgroundJobs;

internal sealed class CarMileageResetService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public CarMileageResetService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
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

                // Check if it's the first month of the year
                if (now.Month == 1)
                {
                    await ResetCarYearlyMileage(stoppingToken);
                }
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

        await context.Cars.ExecuteUpdateAsync(
            x => x.SetProperty(x => x.CurrentMonthFuelConsumption, 0),
            stoppingToken);
    }

    private async Task ResetCarYearlyMileage(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ICheckDriveDbContext>();

        await context.Cars.ExecuteUpdateAsync(
            x => x.SetProperty(x => x.CurrentYearMileage, 0),
            stoppingToken);

        await context.Cars.ExecuteUpdateAsync(
            x => x.SetProperty(x => x.CurrentYearFuelConsumption, 0),
            stoppingToken);
    }
}
