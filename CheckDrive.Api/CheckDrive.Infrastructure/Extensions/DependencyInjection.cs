using CheckDrive.Domain.Authorization;
using CheckDrive.Domain.Interfaces;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CheckDrive.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ICheckDriveDbContext, CheckDriveDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CheckDriveConnection")));

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<CheckDriveDbContext>()
            .AddDefaultTokenProviders();

        AddConfigurations(services, configuration);

        return services;
    }

    private static void AddConfigurations(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
