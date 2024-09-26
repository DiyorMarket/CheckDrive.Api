

using CheckDrive.Api.Helpers;
using CheckDrive.Api.Middlewares;
using CheckDrive.Application.Interfaces;

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
        var services = scope.ServiceProvider.GetRequiredService<ICheckDriveDbContext>();

        DatabaseSeeder.SeedDatabase(services);

        return app;
    }
}
