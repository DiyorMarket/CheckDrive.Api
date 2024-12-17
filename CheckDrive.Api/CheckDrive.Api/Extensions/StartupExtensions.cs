using CheckDrive.Api.Middlewares;
using CheckDrive.Domain.Interfaces;
using CheckDrive.TestDataCreator.Configurations;
using CheckDrive.TestDataCreator.Interfaces;
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

    public static bool IsTesting(this IHostEnvironment environment)
        => environment.IsEnvironment("Testing");
}
