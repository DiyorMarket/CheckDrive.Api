using Bogus;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces;
using CheckDrive.TestDataCreator.Configurations;
using CheckDrive.TestDataCreator.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.TestDataCreator.Seeders;

internal sealed class DevelopmentDatabaseSeeder : IDatabaseSeeder
{
    private static readonly Faker _faker;

    static DevelopmentDatabaseSeeder()
    {
        _faker = new Faker();
    }

    private static readonly List<string> _oilMarks = [
        "80", "90", "95", "98", "100", "110", "Diesel"
    ];

    public async Task SeedDatabaseAsync(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        await CreateOilMarksAsync(context);
        await CreateCarsAsync(context, options);
        await CreateDriversAsync(context, userManager, options);
        await CreateDoctorsAsync(context, userManager, options);
        await CreateDoctorsAsync(context, userManager, options);
        await CreateDoctorsAsync(context, userManager, options);
        await CreateDoctorsAsync(context, userManager, options);
        await CreateDoctorsAsync(context, userManager, options);
    }

    private static async Task CreateOilMarksAsync(ICheckDriveDbContext context)
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

        await context.SaveChangesAsync();
    }

    private static async Task CreateCarsAsync(ICheckDriveDbContext context, DataSeedOptions options)
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

        await context.SaveChangesAsync();
    }

    private static async Task CreateDriversAsync(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Drivers.Any())
        {
            return;
        }

        var uniqueDriversByName = new Dictionary<string, Driver>();
        var carIds = context.Cars.Select(x => x.Id).ToList();

        for (int i = 0; i < options.DriversCount; i++)
        {
            var account = FakeDataGenerator.GetAccount().Generate();
            var driver = FakeDataGenerator.GetEmployee<Driver>().Generate();
            driver.AssignedCarId = carIds[i];

            if (uniqueDriversByName.TryAdd(driver.FirstName + driver.LastName, driver))
            {
                var result = userManager.CreateAsync(account, $"Qwerty-123");

                if (!result.Result.Succeeded)
                {
                    continue;
                }

                driver.Account = account;
                context.Drivers.Add(driver);
            }
        }

        await context.SaveChangesAsync();

        var drivers = context.Drivers.ToArray();
        await AssignToRole(context, drivers, "driver");
    }

    private static async Task CreateDoctorsAsync(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Doctors.Any())
        {
            return;
        }

        var uniqueDoctorsByName = new Dictionary<string, Doctor>();

        for (int i = 0; i < options.DoctorsCount; i++)
        {
            var account = FakeDataGenerator.GetAccount().Generate();
            var doctor = FakeDataGenerator.GetEmployee<Doctor>().Generate();

            if (uniqueDoctorsByName.TryAdd(doctor.FirstName + doctor.LastName, doctor))
            {
                var result = userManager.CreateAsync(account, $"Qwerty-123");

                if (!result.Result.Succeeded)
                {
                    continue;
                }

                doctor.Account = account;
                context.Doctors.Add(doctor);
            }
        }

        await context.SaveChangesAsync();

        var doctors = context.Doctors.ToArray();
        await AssignToRole(context, doctors, "doctor");
    }

    private static async Task CreateMechanicsAsync(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Mechanics.Any())
        {
            return;
        }

        var uniqueMechanicsByName = new Dictionary<string, Mechanic>();

        for (int i = 0; i < options.MechanicsCount; i++)
        {
            var account = FakeDataGenerator.GetAccount().Generate();
            var mechanic = FakeDataGenerator.GetEmployee<Mechanic>().Generate();

            if (uniqueMechanicsByName.TryAdd(mechanic.FirstName + mechanic.LastName, mechanic))
            {
                var result = userManager.CreateAsync(account, $"Qwerty-123");

                if (!result.Result.Succeeded)
                {
                    continue;
                }

                mechanic.Account = account;
                context.Mechanics.Add(mechanic);
            }
        }

        await context.SaveChangesAsync();

        var mechanics = context.Mechanics.ToArray();
        await AssignToRole(context, mechanics, "mechanic");
    }

    private static async Task CreateOperatorsAsync(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Operators.Any())
        {
            return;
        }

        var uniqueOperatorsByName = new Dictionary<string, Operator>();

        for (int i = 0; i < options.OperatorsCount; i++)
        {
            var account = FakeDataGenerator.GetAccount().Generate();
            var @operator = FakeDataGenerator.GetEmployee<Operator>().Generate();

            if (uniqueOperatorsByName.TryAdd(@operator.FirstName + @operator.LastName, @operator))
            {
                var result = userManager.CreateAsync(account, $"Qwerty-123");

                if (!result.Result.Succeeded)
                {
                    continue;
                }

                @operator.Account = account;
                context.Operators.Add(@operator);
            }
        }

        await context.SaveChangesAsync();

        var operators = context.Operators.ToArray();
        await AssignToRole(context, operators, "operator");
    }

    private static async Task CreateDispatchersAsync(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Dispatchers.Any())
        {
            return;
        }

        var uniqueDispatchersByName = new Dictionary<string, Dispatcher>();

        for (int i = 0; i < options.DispatchersCount; i++)
        {
            var account = FakeDataGenerator.GetAccount().Generate();
            var dispatcher = FakeDataGenerator.GetEmployee<Dispatcher>().Generate();

            if (uniqueDispatchersByName.TryAdd(dispatcher.FirstName + dispatcher.LastName, dispatcher))
            {
                var result = userManager.CreateAsync(account, $"Qwerty-123");

                if (!result.Result.Succeeded)
                {
                    continue;
                }

                dispatcher.Account = account;
                context.Dispatchers.Add(dispatcher);
            }
        }

        await context.SaveChangesAsync();

        var dispatchers = context.Dispatchers.ToArray();
        await AssignToRole(context, dispatchers, "dispatcher");
    }

    private static async Task CreateManagers(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    {
        if (context.Managers.Any())
        {
            return;
        }

        var uniqueManagersByName = new Dictionary<string, Manager>();

        for (int i = 0; i < options.ManagersCount; i++)
        {
            var account = FakeDataGenerator.GetAccount().Generate();
            var manager = FakeDataGenerator.GetEmployee<Manager>().Generate();

            if (uniqueManagersByName.TryAdd(manager.FirstName + manager.LastName, manager))
            {
                var result = userManager.CreateAsync(account, $"Qwerty-123");

                if (!result.Result.Succeeded)
                {
                    continue;
                }

                manager.Account = account;
                context.Managers.Add(manager);
            }
        }

        await context.SaveChangesAsync();

        var managers = context.Managers.ToArray();
        await AssignToRole(context, managers, "manager");
    }

    private static void CreateCheckPoints(ICheckDriveDbContext context)
    {
        if (context.CheckPoints.Any())
        {
            return;
        }

        var startDate = DateTime.Now.AddYears(-2);
        var drivers = context.Drivers.ToArray();

        while (startDate != DateTime.Now)
        {
            if (startDate.DayOfWeek == DayOfWeek.Sunday)
            {
                continue;
            }

            foreach (var driver in drivers)
            {
                CreateCheckPoint(context, driver, startDate);
            }

            var daysToAdd = new Random().Next(1, 3);
            startDate = startDate.AddDays(daysToAdd);
        }
    }

    private static async Task AssignToRole(
        ICheckDriveDbContext context,
        Employee[] employees,
        string roleTitle)
    {
        var role = context.Roles.First(x => x.Name == roleTitle);

        if (role is null)
        {
            return;
        }

        foreach (var employee in employees)
        {
            var userRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = employee.AccountId };
            context.UserRoles.Add(userRole);
        }

        await context.SaveChangesAsync();
    }

    private static void CreateCheckPoint(ICheckDriveDbContext context, Driver driver, DateTime dateTime)
    {
        var startDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 8, 15, 0);
        var operators = context.Operators.ToArray();
        var mechanics = context.Mechanics.ToArray();
        var dispatchers = context.Dispatchers.ToArray();
        var managers = context.Managers.ToArray();

        var doctorReview = CreateDoctorReview(context, driver, startDate);

        if (!doctorReview.IsHealthy)
        {
            var checkPoint = new CheckPoint
            {
                DoctorReview = doctorReview,
                StartDate = startDate,
                Stage = Domain.Enums.CheckPointStage.DoctorReview,
                Status = Domain.Enums.CheckPointStatus.Completed,
            };
            context.CheckPoints.Add(checkPoint);
            context.SaveChanges();

            return;
        }
    }

    private static DoctorReview CreateDoctorReview(ICheckDriveDbContext context, Driver driver, DateTime dateTime)
    {
        var doctors = context.Doctors.ToArray();

        var review = new DoctorReview
        {
            Doctor = _faker.PickRandom(doctors),
            Driver = driver,
            CheckPoint = null!,
            Date = _faker.Date.Between(dateTime, dateTime),
            IsHealthy = _faker.Random.Bool(),
        };

        return review;
    }
}
