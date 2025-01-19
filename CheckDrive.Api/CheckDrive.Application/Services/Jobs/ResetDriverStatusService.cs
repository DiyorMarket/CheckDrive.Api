﻿using CheckDrive.Application.Interfaces.Jobs;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CheckDrive.Application.Services.Jobs;

internal sealed class ResetDriverStatusService(IServiceProvider serviceProvider) : IResetDriverStatusService
{
    public async Task ExecuteDailyResetAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ICheckDriveDbContext>();

        await context.Drivers
            .Where(x => x.Status == DriverStatus.NotHealthy)
            .ExecuteUpdateAsync(
                x => x.SetProperty(d => d.Status, DriverStatus.Available));
    }
}
