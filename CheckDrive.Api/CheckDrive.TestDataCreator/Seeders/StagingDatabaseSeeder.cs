using CheckDrive.Domain.Interfaces;
using CheckDrive.TestDataCreator.Configurations;
using CheckDrive.TestDataCreator.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.TestDataCreator.Seeders;

internal sealed class StagingDatabaseSeeder : IDatabaseSeeder
{
    public Task SeedDatabaseAsync(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        throw new NotImplementedException();
    }
}
