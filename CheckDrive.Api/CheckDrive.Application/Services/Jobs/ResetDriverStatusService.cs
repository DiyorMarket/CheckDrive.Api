using CheckDrive.Application.Interfaces.Jobs;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CheckDrive.Application.Services.Jobs;

internal sealed class ResetDriverStatusService(
    IServiceProvider serviceProvider,
    ILogger<ResetDriverStatusService> logger) : IResetDriverStatusService
{
    public async Task ExecuteDailyResetAsync()
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ICheckDriveDbContext>();

            await context.Drivers
                .Where(x => x.Status == DriverStatus.NotHealthy)
                .ExecuteUpdateAsync(
                    x => x.SetProperty(d => d.Status, DriverStatus.Available));
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error occured while executing daily Driver Status Reset. {Message}",
                ex.Message);
        }
    }
}
