using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces;
using CheckDrive.TestDataCreator.Configurations;
using CheckDrive.TestDataCreator.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.TestDataCreator.Seeders;

internal sealed class StagingDatabaseSeeder : IDatabaseSeeder
{
    public void SeedDatabase(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        CreateCars(context, options);
        CreateDrivers(context, userManager, options);
        CreateDoctors(context, userManager, options);
        CreateMechanics(context, userManager, options);
        CreateOperators(context, userManager, options);
        CreateDispatchers(context, userManager, options);
        CreateManagers(context, userManager, options);
    }

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
        var uniqueDriversByName = new Dictionary<string, Driver>();

        for (int i = 0; i < options.DriversCount; i++)
        {
            var account = FakeDataGenerator.GetAccount().Generate();
            var driver = FakeDataGenerator.GetEmployee<Driver>().Generate();

            if (uniqueDriversByName.TryAdd(driver.FirstName + driver.LastName, driver))
            {
                var result = userManager.CreateAsync(account, $"Qwerty-{i}");

                if (!result.Result.Succeeded)
                {
                    continue;
                }

                driver.Account = account;
                context.Drivers.Add(driver);
            }
        }

        context.SaveChanges();
        var drivers = context.Drivers.ToArray();

        foreach (var driver in drivers)
        {
            var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = driver.AccountId };
            context.UserRoles.Add(userRole);
        }

        context.SaveChanges();
    }

    private static void CreateDoctors(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Doctors.Any()) return;

        var role = context.Roles.First(x => x.Name == "doctor");
        var uniqueDoctorsByName = new Dictionary<string, Doctor>();

        for (int i = 0; i < options.DoctorsCount; i++)
        {
            var account = FakeDataGenerator.GetAccount().Generate();
            var doctor = FakeDataGenerator.GetEmployee<Doctor>().Generate();

            if (uniqueDoctorsByName.TryAdd(doctor.FirstName + doctor.LastName, doctor))
            {
                var result = userManager.CreateAsync(account, $"Qwerty-{i}");

                if (!result.Result.Succeeded)
                {
                    continue;
                }

                doctor.Account = account;
                context.Doctors.Add(doctor);
            }
        }

        context.SaveChanges();
        var doctors = context.Doctors.ToArray();

        foreach (var doctor in doctors)
        {
            var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = doctor.AccountId };
            context.UserRoles.Add(userRole);
        }

        context.SaveChanges();
    }

    private static void CreateMechanics(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Mechanics.Any()) return;

        var role = context.Roles.First(x => x.Name == "mechanic");
        var uniqueMechanicsByName = new Dictionary<string, Mechanic>();

        for (int i = 0; i < options.MechanicsCount; i++)
        {
            var account = FakeDataGenerator.GetAccount().Generate();
            var mechanic = FakeDataGenerator.GetEmployee<Mechanic>().Generate();

            if (uniqueMechanicsByName.TryAdd(mechanic.FirstName + mechanic.LastName, mechanic))
            {
                var result = userManager.CreateAsync(account, $"Qwerty-{i}");

                if (!result.Result.Succeeded)
                {
                    continue;
                }

                mechanic.Account = account;
                context.Mechanics.Add(mechanic);
            }
        }

        context.SaveChanges();
        var mechanics = context.Mechanics.ToArray();

        foreach (var mechanic in mechanics)
        {
            var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = mechanic.AccountId };
            context.UserRoles.Add(userRole);
        }

        context.SaveChanges();
    }

    private static void CreateOperators(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Operators.Any()) return;

        var role = context.Roles.First(x => x.Name == "operator");
        var uniqueOperatorsByName = new Dictionary<string, Operator>();

        for (int i = 0; i < options.OperatorsCount; i++)
        {
            var account = FakeDataGenerator.GetAccount().Generate();
            var @operator = FakeDataGenerator.GetEmployee<Operator>().Generate();

            if (uniqueOperatorsByName.TryAdd(@operator.FirstName + @operator.LastName, @operator))
            {
                var result = userManager.CreateAsync(account, $"Qwerty-{i}");

                if (!result.Result.Succeeded)
                {
                    continue;
                }

                @operator.Account = account;
                context.Operators.Add(@operator);
            }
        }

        context.SaveChanges();
        var operators = context.Operators.ToArray();

        foreach (var @operator in operators)
        {
            var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = @operator.AccountId };
            context.UserRoles.Add(userRole);
        }

        context.SaveChanges();
    }

    private static void CreateDispatchers(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Dispatchers.Any()) return;

        var role = context.Roles.First(x => x.Name == "dispatcher");
        var uniqueDispatchersByName = new Dictionary<string, Dispatcher>();

        for (int i = 0; i < options.DispatchersCount; i++)
        {
            var account = FakeDataGenerator.GetAccount().Generate();
            var dispatcher = FakeDataGenerator.GetEmployee<Dispatcher>().Generate();

            if (uniqueDispatchersByName.TryAdd(dispatcher.FirstName + dispatcher.LastName, dispatcher))
            {
                var result = userManager.CreateAsync(account, $"Qwerty-{i}");

                if (!result.Result.Succeeded)
                {
                    continue;
                }

                dispatcher.Account = account;
                context.Dispatchers.Add(dispatcher);
            }
        }

        context.SaveChanges();
        var dispatchers = context.Dispatchers.ToArray();

        foreach (var dispatcher in dispatchers)
        {
            var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = dispatcher.AccountId };
            context.UserRoles.Add(userRole);
        }

        context.SaveChanges();
    }

    private static void CreateManagers(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Managers.Any()) return;

        var role = context.Roles.First(x => x.Name == "manager");
        var uniqueManagersByName = new Dictionary<string, Manager>();

        for (int i = 0; i < options.ManagersCount; i++)
        {
            var account = FakeDataGenerator.GetAccount().Generate();
            var manager = FakeDataGenerator.GetEmployee<Manager>().Generate();

            if (uniqueManagersByName.TryAdd(manager.FirstName + manager.LastName, manager))
            {
                var result = userManager.CreateAsync(account, $"Qwerty-{i}");

                if (!result.Result.Succeeded)
                {
                    continue;
                }

                manager.Account = account;
                context.Managers.Add(manager);
            }
        }

        context.SaveChanges();
        var managers = context.Managers.ToArray();

        foreach (var manager in managers)
        {
            var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = manager.AccountId };
            context.UserRoles.Add(userRole);
        }

        context.SaveChanges();
    }
}
