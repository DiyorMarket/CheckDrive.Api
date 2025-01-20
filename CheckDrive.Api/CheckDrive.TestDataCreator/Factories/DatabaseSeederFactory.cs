using CheckDrive.TestDataCreator.Interfaces;
using CheckDrive.TestDataCreator.Seeders;

namespace CheckDrive.TestDataCreator.Factories;

internal sealed class DatabaseSeederFactory : IDatabaseSeederFactory
{
    public IDatabaseSeeder CreateSeeder(string environment)
    {
        environment = environment.Trim().ToLower();

        return environment switch
        {
            "development" => new DevelopmentDatabaseSeeder(),
            "testing" => new TestingDatabaseSeeder(),
            "staging" => new StagingDatabaseSeeder(),
            "production" => new ProductionDatabaseSeeder(),
            _ => throw new ArgumentOutOfRangeException($"Could not resolve environment: {environment}."),
        };
    }
}
