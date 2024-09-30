using CheckDrive.Api.Helpers;
using CheckDrive.Api.Middlewares;
using CheckDrive.Application.DTOs.Identity;
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
    public static async Task<IApplicationBuilder> UseEnsureRolesCreatedAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] 
            {
                Roles.Administrator,
                Roles.Dispatcher,
                Roles.Driver,
                Roles.Doctor,
                Roles.Mechanic,
                Roles.Manager,
                Roles.Operator
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        return app;
    }
}
