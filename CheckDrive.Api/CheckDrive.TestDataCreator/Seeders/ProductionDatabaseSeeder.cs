using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces;
using CheckDrive.TestDataCreator.Configurations;
using CheckDrive.TestDataCreator.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.TestDataCreator.Seeders;

// TODO: Update with real data: read from file or enter manually etc...
internal sealed class ProductionDatabaseSeeder : IDatabaseSeeder
{
    private static readonly List<string> _oilMarks =
        [
            "80", "90", "95", "98", "100", "110"
        ];

    public async Task SeedDatabaseAsync(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        CreateOilMarks(context);
        CreateCars(context, options);
        await CreateDrivers(context, userManager, options);
        await CreateDoctors(context, userManager, options);
        await CreateMechanics(context, userManager, options);
        await CreateOperators(context, userManager, options);
        await CreateDispatchers(context, userManager, options);
        await CreateManagers(context, userManager, options);
    }

    private static void CreateOilMarks(ICheckDriveDbContext context)
    {
        if (context.OilMarks.Any())
        {
            return;
        }

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

    private static void CreateCars(ICheckDriveDbContext context, DataSeedOptions options)
    {
        if (context.Cars.Any())
        {
            return;
        }

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

    private static async Task CreateDrivers(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Drivers.Any())
        {
            return;
        }

        var role = context.Roles.First(x => x.Name == "driver");
        var account = new IdentityUser
        {
            UserName = "driver",
            Email = "driver@checkdrive.uz",
            EmailConfirmed = true,
            PhoneNumber = "+998901010011"
        };
        var driver = FakeDataGenerator.GetEmployee<Driver>().Generate();
        var result = await userManager.CreateAsync(account, $"Qwerty-123");

        driver.Account = account;
        context.Drivers.Add(driver);
        context.SaveChanges();

        var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = driver.AccountId };
        context.UserRoles.Add(userRole);
        context.SaveChanges();
    }

    private static async Task CreateDoctors(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Doctors.Any())
        {
            return;
        }

        var role = context.Roles.First(x => x.Name == "doctor");
        var account = new IdentityUser
        {
            UserName = "doctor",
            Email = "doctor@checkdrive.uz",
            EmailConfirmed = true,
            PhoneNumber = "+998901010011"
        };
        var doctor = FakeDataGenerator.GetEmployee<Doctor>().Generate();
        var result = await userManager.CreateAsync(account, $"Qwerty-123");

        doctor.Account = account;
        context.Doctors.Add(doctor);
        context.SaveChanges();

        var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = doctor.AccountId };
        context.UserRoles.Add(userRole);
        context.SaveChanges();
    }

    private static async Task CreateMechanics(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Mechanics.Any())
        {
            return;
        }

        var role = context.Roles.First(x => x.Name == "mechanic");
        var account = new IdentityUser
        {
            UserName = "mechanic",
            Email = "mechanic@checkdrive.uz",
            EmailConfirmed = true,
            PhoneNumber = "+998901010011"
        };
        var mechanic = FakeDataGenerator.GetEmployee<Mechanic>().Generate();
        var result = await userManager.CreateAsync(account, $"Qwerty-123");

        mechanic.Account = account;
        context.Mechanics.Add(mechanic);
        context.SaveChanges();

        var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = mechanic.AccountId };
        context.UserRoles.Add(userRole);
        context.SaveChanges();
    }

    private static async Task CreateOperators(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Operators.Any())
        {
            return;
        }

        var role = context.Roles.First(x => x.Name == "operator");
        var account = new IdentityUser
        {
            UserName = "operator",
            Email = "operator@checkdrive.uz",
            EmailConfirmed = true,
            PhoneNumber = "+998901010011"
        };
        var @operator = FakeDataGenerator.GetEmployee<Operator>().Generate();
        var result = await userManager.CreateAsync(account, $"Qwerty-123");

        @operator.Account = account;
        context.Operators.Add(@operator);
        context.SaveChanges();

        var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = @operator.AccountId };
        context.UserRoles.Add(userRole);
        context.SaveChanges();
    }

    private static async Task CreateDispatchers(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Dispatchers.Any())
        {
            return;
        }

        var role = context.Roles.First(x => x.Name == "dispatcher");
        var account = new IdentityUser
        {
            UserName = "dispatcher",
            Email = "dispatcher@checkdrive.uz",
            EmailConfirmed = true,
            PhoneNumber = "+998901010011"
        };
        var dispatcher = FakeDataGenerator.GetEmployee<Dispatcher>().Generate();
        var result = await userManager.CreateAsync(account, $"Qwerty-123");

        dispatcher.Account = account;
        context.Dispatchers.Add(dispatcher);
        context.SaveChanges();

        var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = dispatcher.AccountId };
        context.UserRoles.Add(userRole);
        context.SaveChanges();
    }

    private static async Task CreateManagers(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Managers.Any())
        {
            return;
        }

        var role = context.Roles.First(x => x.Name == "manager");
        var account = new IdentityUser
        {
            UserName = "manager",
            Email = "manager@checkdrive.uz",
            EmailConfirmed = true,
            PhoneNumber = "+998901010011"
        };
        var manager = FakeDataGenerator.GetEmployee<Manager>().Generate();
        var result = await userManager.CreateAsync(account, $"Qwerty-123");

        manager.Account = account;
        context.Managers.Add(manager);
        context.SaveChanges();

        var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = manager.AccountId };
        context.UserRoles.Add(userRole);
        context.SaveChanges();
    }
}
