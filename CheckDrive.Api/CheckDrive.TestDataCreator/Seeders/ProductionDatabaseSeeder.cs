using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces;
using CheckDrive.TestDataCreator.Configurations;
using CheckDrive.TestDataCreator.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.TestDataCreator.Seeders;

internal sealed class ProductionDatabaseSeeder : IDatabaseSeeder
{
    private static readonly List<string> _oilMarks =
        [
            "80", "90", "95", "98", "100", "110"
        ];

    public void SeedDatabase(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        CreateOilMarks(context);
        CreateCars(context, options);
        CreateDrivers(context, userManager, options);
        CreateDoctors(context, userManager, options);
        CreateMechanics(context, userManager, options);
        CreateOperators(context, userManager, options);
        CreateDispatchers(context, userManager, options);
        CreateManagers(context, userManager, options);
    }

    private static void CreateOilMarks(ICheckDriveDbContext context)
    {
        if (context.OilMarks.Any()) return;

        foreach (var oilMarkName in _oilMarks)
        {
            var oilMark = new OilMark
            {
                Name = oilMarkName
            };
            context.OilMarks.Add(oilMark);
        }

        context.SaveChanges();
    }

    // TODO: Update with real data, read from file or enter manually etc...
    private static void CreateCars(ICheckDriveDbContext context, DataSeedOptions options)
    {
        if (context.Cars.Any()) return;

        var uniqueCarsByName = new Dictionary<string, Car>();
        var oilMarks = context.OilMarks.Select(x => x.Id).ToList();

        for (int i = 0; i < options.CarsCount; i++)
        {
            var car = FakeDataGenerator.GetCar(oilMarks).Generate();

            if (uniqueCarsByName.TryAdd(car.Model, car))
            {
                context.Cars.Add(car);
            }
        }

        context.SaveChanges();
    }

    private static void CreateDrivers(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Drivers.Any()) return;

        var role = context.Roles.First(x => x.Name == "driver");
        var account = new IdentityUser
        {
            UserName = "driver",
            Email = "driver@checkdrive.uz",
            EmailConfirmed = true,
            PhoneNumber = "+998901010011"
        };
        var manager = FakeDataGenerator.GetEmployee<Manager>().Generate();

        var result = userManager.CreateAsync(account, $"Qwerty-123");

        manager.Account = account;
        context.Managers.Add(manager);

        context.SaveChanges();
        var managers = context.Managers.ToArray();

        var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = manager.AccountId };
        context.UserRoles.Add(userRole);

        context.SaveChanges();
    }

    private static void CreateDoctors(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Doctors.Any()) return;

        var role = context.Roles.First(x => x.Name == "doctor");
        var account = new IdentityUser
        {
            UserName = "doctor",
            Email = "doctor@checkdrive.uz",
            EmailConfirmed = true,
            PhoneNumber = "+998901010011"
        };
        var manager = FakeDataGenerator.GetEmployee<Manager>().Generate();

        var result = userManager.CreateAsync(account, $"Qwerty-123");

        manager.Account = account;
        context.Managers.Add(manager);

        context.SaveChanges();
        var managers = context.Managers.ToArray();

        var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = manager.AccountId };
        context.UserRoles.Add(userRole);

        context.SaveChanges();
    }

    private static void CreateMechanics(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Mechanics.Any()) return;

        var role = context.Roles.First(x => x.Name == "mechanic");
        var account = new IdentityUser
        {
            UserName = "mechanic",
            Email = "mechanic@checkdrive.uz",
            EmailConfirmed = true,
            PhoneNumber = "+998901010011"
        };
        var manager = FakeDataGenerator.GetEmployee<Manager>().Generate();

        var result = userManager.CreateAsync(account, $"Qwerty-123");

        manager.Account = account;
        context.Managers.Add(manager);

        context.SaveChanges();
        var managers = context.Managers.ToArray();

        var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = manager.AccountId };
        context.UserRoles.Add(userRole);

        context.SaveChanges();
    }

    private static void CreateOperators(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Operators.Any()) return;

        var role = context.Roles.First(x => x.Name == "operator");
        var account = new IdentityUser
        {
            UserName = "operator",
            Email = "operator@checkdrive.uz",
            EmailConfirmed = true,
            PhoneNumber = "+998901010011"
        };
        var manager = FakeDataGenerator.GetEmployee<Manager>().Generate();

        var result = userManager.CreateAsync(account, $"Qwerty-123");

        manager.Account = account;
        context.Managers.Add(manager);

        context.SaveChanges();
        var managers = context.Managers.ToArray();

        var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = manager.AccountId };
        context.UserRoles.Add(userRole);

        context.SaveChanges();
    }

    private static void CreateDispatchers(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Dispatchers.Any()) return;

        var role = context.Roles.First(x => x.Name == "dispatcher");
        var account = new IdentityUser
        {
            UserName = "dispatcher",
            Email = "dispatcher@checkdrive.uz",
            EmailConfirmed = true,
            PhoneNumber = "+998901010011"
        };
        var manager = FakeDataGenerator.GetEmployee<Manager>().Generate();

        var result = userManager.CreateAsync(account, $"Qwerty-123");

        manager.Account = account;
        context.Managers.Add(manager);

        context.SaveChanges();
        var managers = context.Managers.ToArray();

        var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = manager.AccountId };
        context.UserRoles.Add(userRole);

        context.SaveChanges();
    }

    private static void CreateManagers(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Managers.Any()) return;

        var role = context.Roles.First(x => x.Name == "manager");
        var account = new IdentityUser
        {
            UserName = "manager",
            Email = "manager@checkdrive.uz",
            EmailConfirmed = true,
            PhoneNumber = "+998901010011"
        };
        var manager = FakeDataGenerator.GetEmployee<Manager>().Generate();

        var result = userManager.CreateAsync(account, $"Qwerty-123");

        manager.Account = account;
        context.Managers.Add(manager);

        context.SaveChanges();
        var managers = context.Managers.ToArray();

        var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = manager.AccountId };
        context.UserRoles.Add(userRole);

        context.SaveChanges();
    }
}
