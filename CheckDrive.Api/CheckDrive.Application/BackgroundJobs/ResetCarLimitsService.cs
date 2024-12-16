using CheckDrive.Application.Constants;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CheckDrive.Application.BackgroundJobs;

internal sealed class ResetCarLimitsService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ResetCarLimitsService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            DateTime now = DateTime.UtcNow.AddHours(TimeConstants.TashkentTimeUtc);

            if (now.Day == 1)
            {
                await ResetMonthlyLimits(stoppingToken);
            }

            if (now.Month == 1)
            {
                await ResetYearlyLimits(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }

    private async Task ResetMonthlyLimits(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ICheckDriveDbContext>();

        await context.Cars.ExecuteUpdateAsync(
            x => x.SetProperty(x => x.UsageSummary.CurrentMonthDistance, 0),
            stoppingToken);

        await context.Cars.ExecuteUpdateAsync(
            x => x.SetProperty(x => x.UsageSummary.CurrentMonthFuelConsumption, 0),
            stoppingToken);
    }

    private async Task ResetYearlyLimits(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ICheckDriveDbContext>();

        await context.Cars.ExecuteUpdateAsync(
            x => x.SetProperty(x => x.UsageSummary.CurrentYearDistance, 0),
            stoppingToken);

        await context.Cars.ExecuteUpdateAsync(
            x => x.SetProperty(x => x.UsageSummary.CurrentYearFuelConsumption, 0),
            stoppingToken);
    }
}
