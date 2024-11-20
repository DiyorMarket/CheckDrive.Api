using CheckDrive.Domain.Interfaces;
using CheckDrive.TestDataCreator.Configurations;
using CheckDrive.TestDataCreator.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.TestDataCreator.Seeders;

internal sealed class TestingDatabaseSeeder : IDatabaseSeeder
{
    public void SeedDatabase(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        throw new NotImplementedException();
    }
}
