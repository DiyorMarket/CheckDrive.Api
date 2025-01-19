using CheckDrive.Application.Interfaces.Jobs;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CheckDrive.Application.Services.Jobs;

internal sealed class ResetCarLimitsService(IServiceProvider serviceProvider) : IResetCarLimitsService
{
    public async Task ExecuteMonthlyResetAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ICheckDriveDbContext>();

        await context.Cars.ExecuteUpdateAsync(
            x => x.SetProperty(x => x.UsageSummary.CurrentMonthDistance, 0));

        await context.Cars.ExecuteUpdateAsync(
            x => x.SetProperty(x => x.UsageSummary.CurrentMonthFuelConsumption, 0));
    }

    public async Task ExecuteYearlyResetAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ICheckDriveDbContext>();

        await context.Cars.ExecuteUpdateAsync(
            x => x.SetProperty(x => x.UsageSummary.CurrentYearDistance, 0));

        await context.Cars.ExecuteUpdateAsync(
            x => x.SetProperty(x => x.UsageSummary.CurrentYearFuelConsumption, 0));
    }
}
