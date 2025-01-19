using CheckDrive.Api.Filters;
using CheckDrive.Api.Middlewares;
using CheckDrive.Application.Configurations;
using CheckDrive.Application.Interfaces.Jobs;
using CheckDrive.Domain.Interfaces;
using CheckDrive.TestDataCreator.Configurations;
using CheckDrive.TestDataCreator.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CheckDrive.Api.Extensions;

public static class StartupExtensions
{
    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();

        return app;
    }

    public static IApplicationBuilder UseDatabaseSeeder(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ICheckDriveDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var options = scope.ServiceProvider.GetRequiredService<IOptions<DataSeedOptions>>();

        var seederFactory = scope.ServiceProvider.GetRequiredService<IDatabaseSeederFactory>();
        var seeder = seederFactory.CreateSeeder(app.Environment.EnvironmentName);

        seeder.SeedDatabase(context, userManager, options.Value);

        return app;
    }

    public static IApplicationBuilder UseHangfire(this WebApplication app)
    {
        var hangfire = app.Services.GetRequiredService<IOptions<HangfireSettings>>()?.Value
            ?? throw new InvalidOperationException("Cannot setup Hangfire without configuration values.");

        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization =
            [
                new HangfireDashboardAuthorizationFilter(hangfire.UserName, hangfire.Password)
            ]
        });

        return app;
    }

    public static IApplicationBuilder UseBackgroundJobs(this WebApplication app)
    {
        RecurringJob.AddOrUpdate<IResetCarLimitsService>(
            "monthly-car-limits-reset",
            service => service.ExecuteMonthlyResetAsync(),
            "0 0 1 * *"); // Cron expression for the first day of the month at midnight

        RecurringJob.AddOrUpdate<IResetCarLimitsService>(
            "yearly-car-limits-reset",
            service => service.ExecuteYearlyResetAsync(),
            "0 0 1 1 *"); // Cron expression for the first day of the year at midnight

        RecurringJob.AddOrUpdate<IResetDriverStatusService>(
            "daily-driver-reset",
            service => service.ExecuteDailyResetAsync(),
            "0 2 * * *"); // Cron expression for scheduling a job every day at 7:00 AM UTC+5

        return app;
    }

    public static bool IsTesting(this IHostEnvironment environment)
        => environment.IsEnvironment("Testing");
}
