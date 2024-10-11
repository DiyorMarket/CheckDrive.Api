using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Interfaces;
using CheckDrive.TestDataCreator;
using CheckDrive.TestDataCreator.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CheckDrive.Api.Helpers;

public static class DatabaseSeeder
{

    public static void SeedDatabase(
        ICheckDriveDbContext context,
        UserManager<IdentityUser> userManager,
        DataSeedOptions options)
    {
        CreateCars(context, options);
        CreateRoles(context);
        CreateDrivers(context, userManager, options);
        CreateDoctors(context, userManager, options);
        CreateMechanics(context, userManager, options);
        CreateOperators(context, userManager, options);
        CreateDispatchers(context, userManager, options);

        CreateDefaultOilMarks(context);
        CreateCheckPoints(context, options);
        CreateDoctorReviews(context, options);
        CreateMechanicHandovers(context);
        CreateOperatorReviews(context);
        CreateMechanicAcceptances(context);
        CreateDispatcherReviews(context);
    }

    private static void CreateDefaultOilMarks(ICheckDriveDbContext context)
    {
        if (context.OilMarks.Any()) return;

        var oilMarks = new List<OilMark>
        {
            new OilMark { Name = "AI-80" },
            new OilMark { Name = "AI-92" },
            new OilMark { Name = "AI-95" },
            new OilMark { Name = "AI-100" },
            new OilMark { Name = "Diesel"},
            new OilMark { Name = "Gas" }
        };

        context.OilMarks.AddRange(oilMarks);
        context.SaveChanges();
    }

    private static void CreateCars(ICheckDriveDbContext context, DataSeedOptions options)
    {
        if (context.Cars.Any()) return;

        var uniqueCarsByName = new Dictionary<string, Car>();

        for (int i = 0; i < options.CarsCount; i++)
        {
            var car = FakeDataGenerator.GetCar().Generate();

            if (uniqueCarsByName.TryAdd(car.Model, car))
            {
                context.Cars.Add(car);
            }
        }

        context.SaveChanges();
    }

    private static void CreateRoles(ICheckDriveDbContext context)
    {
        if (context.Roles.Any()) return;

        var driver = new IdentityRole { Name = "Driver" };
        var doctor = new IdentityRole { Name = "Doctor" };
        var mechanic = new IdentityRole { Name = "Mechanic" };
        var @operator = new IdentityRole { Name = "Operator" };
        var dispatcher = new IdentityRole { Name = "Dispatcher" };
        var manager = new IdentityRole { Name = "Manager" };
        var admin = new IdentityRole { Name = "Admin" };

        context.Roles.AddRange(driver, doctor, mechanic, @operator, dispatcher, manager, admin);

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

        for (int i = 0; i < options.DriversCount; i++)
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

        for (int i = 0; i < options.DriversCount; i++)
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

        for (int i = 0; i < options.DriversCount; i++)
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

    private static void CreateCheckPoints(ICheckDriveDbContext context, DataSeedOptions options)
    {
        if (context.CheckPoints.Any()) return;

        var checkPoints = FakeDataGenerator.GetCheckPoints()
            .Generate(options.CheckPointsCount);

        foreach (var checkPoint in checkPoints)
        {
            checkPoint.DoctorReview = null!;
            context.CheckPoints.Add(checkPoint);
        }

        context.SaveChanges();
    }

    private static void CreateDoctorReviews(ICheckDriveDbContext context, DataSeedOptions options)
    {
        if (context.DoctorReviews.Any()) return;

        var checkPointId = context.CheckPoints.Select(x => x.Id).ToList();
        var driverIds = context.Drivers.Select(x => x.Id).ToList();
        var doctorIds = context.Doctors.Select(x => x.Id).ToList();
        var uniqueDoctorReviews = new Dictionary<int,DoctorReview>();

        for (int i = 0; i < options.CheckPointsCount; i++)
        {
            var doctorReview = FakeDataGenerator.GetDoctorReview(doctorIds, driverIds).Generate();

            doctorReview.CheckPointId = checkPointId[i];

            if (uniqueDoctorReviews.TryAdd(doctorReview.CheckPointId, doctorReview))
            {
                context.DoctorReviews.Add(doctorReview);
            }
        }

        context.SaveChanges();
    }

    private static void CreateMechanicHandovers(ICheckDriveDbContext context)
    {
        if (context.MechanicHandovers.Any()) return;

        var checkPoint = context.CheckPoints
            .Include(x => x.DoctorReview)
            .Where(x => x.Stage == CheckPointStage.DoctorReview)
            .Where(x => x.DoctorReview.Status == ReviewStatus.Approved)
            .ToList();

        var mechanicIds = context.Mechanics.Select(x => x.Id).ToList();

        var cars = context.Cars
            .ToList();

        var uniqueMechanicHandovers= new Dictionary<int, MechanicHandover>();

        for (int i = 0; i < checkPoint.Count; i++)
        {
            var mechanicHandover = FakeDataGenerator.GetMechanichandover(mechanicIds, cars).Generate();

            mechanicHandover.CheckPointId = checkPoint[i].Id;
            mechanicHandover.Date = checkPoint[i].StartDate;

            checkPoint[i].Stage = CheckPointStage.MechanicHandover;
            
            if(mechanicHandover.Status == ReviewStatus.RejectedByReviewer)
            {
                checkPoint[i].Status = CheckPointStatus.InterruptedByReviewerRejection;
            }

            if (uniqueMechanicHandovers.TryAdd(mechanicHandover.CheckPointId, mechanicHandover))
            {
                context.MechanicHandovers.Add(mechanicHandover);
            }
        }

        context.SaveChanges();
    }

    private static void CreateOperatorReviews(ICheckDriveDbContext context)
    {
        if (context.OperatorReviews.Any()) return;

        var checkPoint = context.CheckPoints
            .Include(x => x.MechanicHandover)
            .Where(x => x.Stage == CheckPointStage.MechanicHandover)
            .Where(x => x.Status == CheckPointStatus.InProgress)
            .ToList();

        var operatorIds = context.Operators.Select(x => x.Id).ToList();

        var oilMarkIds = context.OilMarks
            .Select(x => x.Id)
            .ToList();

        var uniqueMechanicHandovers = new Dictionary<int, OperatorReview>();

        for (int i = 0; i < checkPoint.Count; i++)
        {
            var operatorReview = FakeDataGenerator.GetOperatorReviews(operatorIds, oilMarkIds).Generate();

            operatorReview.CheckPointId = checkPoint[i].Id;
            operatorReview.Date = checkPoint[i].StartDate;

            checkPoint[i].Stage = CheckPointStage.OperatorReview;

            if (operatorReview.Status == ReviewStatus.RejectedByReviewer)
            {
                checkPoint[i].Status = CheckPointStatus.InterruptedByReviewerRejection;
            }

            if (uniqueMechanicHandovers.TryAdd(operatorReview.CheckPointId, operatorReview))
            {
                context.OperatorReviews.Add(operatorReview);
            }
        }

        context.SaveChanges();
    }

    private static void CreateMechanicAcceptances(ICheckDriveDbContext context)
    {
        if (context.MechanicAcceptances.Any()) return;

        var checkPoint = context.CheckPoints
            .Where(x => x.Stage == CheckPointStage.OperatorReview)
            .Where(x => x.Status == CheckPointStatus.InProgress)
            .ToList();

        var mechanicIds = context.Mechanics.Select(x => x.Id).ToList();

        var uniqueMechanicAcceptance = new Dictionary<int, MechanicAcceptance>();

        for (int i = 0; i < checkPoint.Count; i++)
        {
            var mechanicAcceptance = FakeDataGenerator.GetMechanicAcceptance(mechanicIds).Generate();

            mechanicAcceptance.CheckPointId = checkPoint[i].Id;

            var randomDate = new Random().Next(1, 20);
            mechanicAcceptance.Date = checkPoint[i].StartDate.AddDays(randomDate);

            checkPoint[i].Stage = CheckPointStage.MechanicAcceptance;

            if (mechanicAcceptance.Status == ReviewStatus.RejectedByReviewer)
            {
                checkPoint[i].Status = CheckPointStatus.InterruptedByReviewerRejection;
            }

            if (uniqueMechanicAcceptance.TryAdd(mechanicAcceptance.CheckPointId, mechanicAcceptance))
            {
                context.MechanicAcceptances.Add(mechanicAcceptance);
            }
        }

        context.SaveChanges();
    }

    private static void CreateDispatcherReviews(ICheckDriveDbContext context)
    {
        if (context.DispatcherReviews.Any()) return;

        var checkPoints = context.CheckPoints
            .Include(x => x.MechanicAcceptance)
            .Where(x => x.Stage == CheckPointStage.MechanicAcceptance)
            .Where(x => x.Status == CheckPointStatus.InProgress)
            .ToList();

        var dispatcherIds = context.Dispatchers.Select(x => x.Id).ToList();

        var uniqueDispatcherReviews = new Dictionary<int, DispatcherReview>();

        for (int i = 0; i < checkPoints.Count; i++)
        {
            var dispatcherReview = FakeDataGenerator.GetDispatcherReview(dispatcherIds).Generate();

            dispatcherReview.CheckPointId = checkPoints[i].Id;
            dispatcherReview.Date = checkPoints[i].MechanicAcceptance!.Date;

            checkPoints[i].Stage = CheckPointStage.DispatcherReview;

            if (dispatcherReview.FuelConsumptionAdjustment.HasValue || dispatcherReview.DistanceTravelledAdjustment.HasValue)
            {
                checkPoints[i].Stage = CheckPointStage.ManagerReview;
                checkPoints[i].Status = CheckPointStatus.PendingManagerReview;
            }
            else if (dispatcherReview.Status != ReviewStatus.Approved)
            {
                checkPoints[i].Status = CheckPointStatus.InterruptedByReviewerRejection;
            }
            else
            {
                checkPoints[i].Status = CheckPointStatus.Completed;
            }


            if (uniqueDispatcherReviews.TryAdd(dispatcherReview.CheckPointId, dispatcherReview))
            {
                context.DispatcherReviews.Add(dispatcherReview);
            }
        }

        context.SaveChanges();
    }
}
