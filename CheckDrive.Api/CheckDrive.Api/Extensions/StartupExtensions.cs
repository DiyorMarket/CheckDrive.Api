using CheckDrive.Api.Helpers;
using CheckDrive.Api.Middlewares;
using CheckDrive.Application.Constants;
using CheckDrive.Domain.Interfaces;
using CheckDrive.TestDataCreator.Configurations;
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

        DatabaseSeeder.SeedDatabase(context, userManager, options.Value);

        return app;
    }
}
