using CheckDrive.Application.Interfaces.Jobs;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CheckDrive.Application.Services.Jobs;

internal sealed class ResetCarLimitsService(
    IServiceProvider serviceProvider,
    ILogger<ResetCarLimitsService> logger) : IResetCarLimitsService
{
    public async Task ExecuteMonthlyResetAsync()
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ICheckDriveDbContext>();

            await context.Cars.ExecuteUpdateAsync(
                x => x.SetProperty(x => x.UsageSummary.CurrentMonthDistance, 0));

            await context.Cars.ExecuteUpdateAsync(
                x => x.SetProperty(x => x.UsageSummary.CurrentMonthFuelConsumption, 0));
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error occurred while executing 'Montly Car Limits Reset'. {Message}",
                ex.Message);
        }
    }

    public async Task ExecuteYearlyResetAsync()
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ICheckDriveDbContext>();

            await context.Cars.ExecuteUpdateAsync(
                x => x.SetProperty(x => x.UsageSummary.CurrentYearDistance, 0));

            await context.Cars.ExecuteUpdateAsync(
                x => x.SetProperty(x => x.UsageSummary.CurrentYearFuelConsumption, 0));
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error occurred while executing 'Yearly Car Limits Reset'. {Message}",
                ex.Message);
        }
    }
}
