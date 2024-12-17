using CheckDrive.TestDataCreator.Factories;
using CheckDrive.TestDataCreator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CheckDrive.TestDataCreator.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection RegisterTestDataCreator(this IServiceCollection services)
    {
        services.AddSingleton<IDatabaseSeederFactory, DatabaseSeederFactory>();

        return services;
    }
}
