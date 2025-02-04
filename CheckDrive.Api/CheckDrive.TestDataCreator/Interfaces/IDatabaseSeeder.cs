﻿using CheckDrive.Domain.Interfaces;
using CheckDrive.TestDataCreator.Configurations;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.TestDataCreator.Interfaces;

public interface IDatabaseSeeder
{
    Task SeedDatabaseAsync(
        ICheckDriveDbContext context,
        UserManager<IdentityUser> userManager,
        DataSeedOptions options);
}
