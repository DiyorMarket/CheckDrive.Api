using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces;
using CheckDrive.TestDataCreator;
using CheckDrive.TestDataCreator.Configurations;
using Microsoft.AspNetCore.Identity;

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
        CreateManagers(context, userManager, options);
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

    //private static void CreateEmployees(ICheckDriveDbContext context, UserManager<IdentityUser> userManager, DataSeedOptions options)
    //{
    //    if (context.Users.Any())
    //    {
    //        return;
    //    }

    //    var roles = context.Roles.Where(x => x.Name != "Admin").Select(x => x.Id).ToArray();
    //    var uniqueEmployeesByName = new Dictionary<string, Employee>();

    //    for (int i = 0; i < options.UsersCount; i++)
    //    {
    //        var employee = FakeDataGenerator.GetEmployee().Generate();
    //        var account = FakeDataGenerator.GetAccount().Generate();
    //        var role = _faker.PickRandom(roles);

    //        if (uniqueEmployeesByName.TryAdd(employee.FirstName + employee.LastName, employee))
    //        {
    //            var result = userManager.CreateAsync(account, $"Qwerty-{i}#");
    //            var userRole = new IdentityUserRole<string> { RoleId = role, UserId = account.Id };

    //            if (!result.Result.Succeeded)
    //            {
    //                continue;
    //            }

    //            context.UserRoles.Add(userRole);
    //            employee.Account = account;
    //            context.Employees.Add(employee);
    //        }
    //    }

    //    context.SaveChanges();
    //}

    //private static void CreateRoles(CheckDriveDbContext context)
    //{
    //    if (context.Roles.Any()) return;

    //    List<Role> roles = new()
    //    {
    //        new Role()
    //        {
    //            Name = "Manager"
    //        },
    //        new Role()
    //        {
    //            Name = "Haydovchi"
    //        },
    //        new Role()
    //        {
    //            Name = "Shifokor"
    //        },
    //        new Role()
    //        {
    //            Name = "Operator"
    //        },
    //        new Role()
    //        {
    //            Name = "Dispetcher"
    //        },
    //        new Role()
    //        {
    //            Name = "Mexanik"
    //        }
    //    };

    //    context.Roles.AddRange(roles);
    //    context.SaveChanges();
    //}

    //private static void CreateOilMarks(CheckDriveDbContext context)
    //{
    //    if (context.OilMarks.Any()) return;

    //    List<OilMark> oilMarks = new()
    //    {
    //        new OilMark()
    //        {
    //            OilMark = "A80"
    //        },
    //        new OilMark()
    //        {
    //            OilMark = "A91"
    //        },
    //        new OilMark()
    //        {
    //            OilMark = "A92"
    //        },
    //        new OilMark()
    //        {
    //            OilMark = "A95"
    //        },
    //        new OilMark()
    //        {
    //            OilMark = "Diesel"
    //        },
    //    };

    //    context.OilMarks.AddRange(oilMarks);
    //    context.SaveChanges();
    //}

    //private static void CreateAccounts(CheckDriveDbContext context)
    //{
    //    if (context.Accounts.Any()) return;
    //    List<Account> accounts = new()
    //    {
    //        new Account()
    //        {
    //            Bithdate = DateTime.Now,
    //            FirstName = "Botir",
    //            LastName = "Qodirov",
    //            Login = "manager",
    //            Password = "1234",
    //            PhoneNumber = "+998946152254",
    //            Address = "Sergeli",
    //            Position = "Menedjer",
    //            Passport = "AA-0000000",
    //            RoleId = 1,
    //        },
    //        new Account()
    //        {
    //            Bithdate = new DateTime(1980, 7, 23),
    //            FirstName = "Razzakov",
    //            LastName = "Doniyor",
    //            Login = "Razzakov",
    //            Password = "1234",
    //            PhoneNumber = "+998991204020",
    //            Address = "Olmazor tumani, Qorasaroy tor ko`chasi, Qumloq 7 a uy",
    //            Position = "Toshkent shahar hokimi haydovchisi",
    //            Passport = "AA-9861688",
    //            RoleId = 2,
    //        },
    //        new Account()
    //        {
    //            Bithdate = new DateTime(1990, 5, 15),
    //            FirstName = "Fayzullayev",
    //            LastName = "Dilmurod",
    //            Login = "Fayzullayev",
    //            Password = "1234",
    //            PhoneNumber = "+998909903939",
    //            Address = "Shayhantoxur tumani, Tarona 1-tor ko`chasi, 9 uy",
    //            Position = "Toshkent shahar hokimi haydovchisi",
    //            Passport = "AA-9460873",
    //            RoleId = 2,
    //        },
    //        new Account()
    //        {
    //            Bithdate = new DateTime(1967, 12, 12),
    //            FirstName = "Xashimov",
    //            LastName = "Abdukarim",
    //            Login = "Xashimov",
    //            Password = "1234",
    //            PhoneNumber = "+998997966474",
    //            Address = "Sergeli tumani, Quruvchi ko'chasi,13-a uy, 13-xonadon",
    //            Position = "Toshkent shahar hokimining moliya-iqtisodiyot va kambag'allikni qisqartirish masalalari bo'yicha hirinchi o'rinbosari-shahar iqtisodiy taraqqiyotva kambag'allikni qisqartirish bosh boshqarmasi boshlig'ining haydovchisi",
    //            Passport = "AA-0645617",
    //            RoleId = 2,
    //        },
    //        new Account()
    //        {
    //            Bithdate = new DateTime(1986, 7, 5),
    //            FirstName = "Kamilov",
    //            LastName = "Davron",
    //            Login = "Kamilov",
    //            Password = "1234",
    //            PhoneNumber = "+998998118180",
    //            Address = "Shayxontoxur tumani, Kolxoznaya ko'chasi, 85a-uy",
    //            Position = "Toshkent shahar hokimining qurilish masalalari bo'yicha maslahatchisi haydovchisi",
    //            Passport = "AA-4246023",
    //            RoleId = 2,
    //        },
    //        new Account()
    //        {
    //            Bithdate = new DateTime(1971, 4, 8),
    //            FirstName = "Abduraxmanov",
    //            LastName = "Abdusalom",
    //            Login = "Abduraxmanov",
    //            Password = "1234",
    //            PhoneNumber = "+998935901754",
    //            Address = "Qibray tumani, O'nqo'rg'on QFY, A.Latifo'jayev 38 uy",
    //            Position = "Toshkent shahar hokimining qurilish, kommunikatsiyalarni rivojlantirish, ekologiya va ko'kalamzorlashtirish masalalari bo'yicha birinchi o'rinbosari haydovchisi",
    //            Passport = "AV-4067546",
    //            RoleId = 2,
    //        },
    //        new Account()
    //        {
    //            Bithdate = new DateTime(1978, 12, 17),
    //            FirstName = "Magrubdjanov",
    //            LastName = "Abdusamat",
    //            Login = "Magrubdjanov",
    //            Password = "1234",
    //            PhoneNumber = "+998974442214",
    //            Address = "Yunusobod tumani, Xotira 3-tor ko'chasi, 4-uy",
    //            Position = "Toshkent shahar hokimining o'rinbosari, Investitsiyalar va tashqi savdo boshqarmasi haydovchisi",
    //            Passport = "AA-8593883",
    //            RoleId = 2,
    //        },
    //        new Account()
    //        {
    //            Bithdate = new DateTime(1970, 1, 1),
    //            FirstName = "Xoshimov",
    //            LastName = "Shuxrat",
    //            Login = "Xoshimov",
    //            Password = "1234",
    //            PhoneNumber = "+998977857757",
    //            Address = "Toshkent shaxar, Olmazor tumani, Тошмухамеда 1-тор кўчаси 4-уй",
    //            Position = "Toshkent shahar hokimining Jamoat va diniy tashkilotlar bilan aloqalar bo'yicha o'rinbosari haydovchisi",
    //            Passport = "AB-2174559",
    //            RoleId = 2,
    //        },
    //        new Account()
    //        {
    //            Bithdate = new DateTime(1986, 6, 20),
    //            FirstName = "Begmatov",
    //            LastName = "Sulton",
    //            Login = "Begmatov",
    //            Password = "1234",
    //            PhoneNumber = "+998998180949",
    //            Address = "Mirzo Ulug'bek tumani, Feruza mavesi, 63 uy, 42 xonadon",
    //            Position = "Toshkent shahar hokimining o'rinbosari oila va xotin-qizlar boshqarmasi boshlig'ining xaydovchisi",
    //            Passport = "AV-6449253",
    //            RoleId = 2,
    //        },
    //        new Account()
    //        {
    //            Bithdate = new DateTime(1974, 2, 16),
    //            FirstName = "Davrukov",
    //            LastName = "Faxriddin",
    //            Login = "Davrukov",
    //            Password = "1234",
    //            PhoneNumber = "+998900000000",
    //            Address = "Mirzo Ulug'bek tumani, Muxitdinova ko'chasi, 3b-uy 23-xonadon",
    //            Position = "Toshkent shahar hokimining xokimining turizm va sport bo'yicha o'rinbosari",
    //            Passport = "AV-7430746",
    //            RoleId = 2,
    //        },
    //        new Account()
    //        {
    //            Bithdate = new DateTime(1958, 7, 20),
    //            FirstName = "Pirmetov",
    //            LastName = "Shuxrat",
    //            Login = "Pirmetov",
    //            Password = "1234",
    //            PhoneNumber = "+998909506381",
    //            Address = "Mirzo Ulug'bek tumani, TTZ-2 mavzesi 64-uy, 22-xonadon",
    //            Position = "Toshkent shahar hokimligi haydovchisi",
    //            Passport = "AA-4590867",
    //            RoleId = 2,
    //        },
    //        new Account()
    //        {
    //            Bithdate = new DateTime(1970, 5, 10),
    //            FirstName = "Aripov",
    //            LastName = "Maxmudjon",
    //            Login = "Aripov",
    //            Password = "1234",
    //            PhoneNumber = "+998935810606",
    //            Address = "Yakkasaroy tumani, 5- tor A.Qaxxor ko'chasi 1-uy",
    //            Position = "Toshkent shahar hokimligi haydovchisi",
    //            Passport = "AA-4380462",
    //            RoleId = 2,
    //        },
    //        new Account()
    //        {
    //            Bithdate = new DateTime(1971, 9, 21),
    //            FirstName = "Tairov",
    //            LastName = "Sulton",
    //            Login = "Tairov",
    //            Password = "1234",
    //            PhoneNumber = "+998900000000",
    //            Address = "Toshkent shahar, Yunusobod tumani, 9-mavze, 10-uy. 60-xonadon",
    //            Position = "Toshkent shahar hokimligi ishlar boshqarmasi boshlig'i",
    //            Passport = "AA-4732149",
    //            RoleId = 2,
    //        },
    //    };

    //    context.Accounts.AddRange(accounts);
    //    context.SaveChanges();


    //}

    //private static void CreateCars(CheckDriveDbContext context)
    //{
    //    if (context.Cars.Any()) return;

    //    List<Car> cars = new()
    //    {
    //        new Car()
    //        {
    //            Model = "Mercedes-Benz S450",
    //            Color = "Qora",
    //            Number = "PAA 240",
    //            Mileage = 0,
    //            MeduimFuelConsumption = 18,
    //            FuelTankCapacity = 76,
    //            ManufacturedYear = 2019,
    //            RemainingFuel = 0,
    //            CarStatus = CarStatus.Free,
    //            OneYearMediumDistance = 43200
    //        },
    //        new Car()
    //        {
    //            Model = "Mercedes-Benz S500",
    //            Color = "Qora",
    //            Number = "01/010 DAV",
    //            Mileage = 0,
    //            MeduimFuelConsumption = 18,
    //            FuelTankCapacity = 76,
    //            ManufacturedYear = 2021,
    //            RemainingFuel = 0,
    //            CarStatus = CarStatus.Free,
    //            OneYearMediumDistance = 43200
    //        },
    //        new Car()
    //        {
    //            Model = "Tayota Prado",
    //            Color = "Qora",
    //            Number = "01/005 DAV",
    //            Mileage = 0,                   
    //            MeduimFuelConsumption = 15.1,
    //            FuelTankCapacity = 93,
    //            ManufacturedYear = 2017,
    //            RemainingFuel = 0,
    //            CarStatus = CarStatus.Free,
    //            OneYearMediumDistance = 36036
    //        },
    //        new Car()
    //        {
    //            Model = "Chevrolet Captiva",
    //            Color = "Qora",
    //            Number = "01/012 DAV",
    //            Mileage = 0,
    //            MeduimFuelConsumption = 12,
    //            FuelTankCapacity = 65,
    //            ManufacturedYear = 2018,
    //            RemainingFuel = 0,
    //            CarStatus = CarStatus.Free,
    //            OneYearMediumDistance = 36000
    //        },
    //        new Car()
    //        {
    //            Model = "Chevrolet Malibu-2",
    //            Color = "Qora",
    //            Number = "01/003 DAV",
    //            Mileage = 0,
    //            MeduimFuelConsumption = 11.5,
    //            FuelTankCapacity = 60,
    //            ManufacturedYear = 2021,
    //            RemainingFuel = 0,
    //            CarStatus = CarStatus.Free,
    //            OneYearMediumDistance = 36000
    //        },
    //        new Car()
    //        {
    //            Model = "Chevrolet Malibu-2",
    //            Color = "Qora",
    //            Number = "01/004 DAV",
    //            Mileage = 0,
    //            MeduimFuelConsumption = 11.5,
    //            FuelTankCapacity = 60,
    //            ManufacturedYear = 2020,
    //            RemainingFuel = 0,
    //            CarStatus = CarStatus.Free,
    //            OneYearMediumDistance = 36000
    //        },
    //        new Car()
    //        {
    //            Model = "Chevrolet Malibu-2",
    //            Color = "Qora",
    //            Number = "01/009 DAV",
    //            Mileage = 0,
    //            MeduimFuelConsumption = 11.5,
    //            FuelTankCapacity = 60,
    //            ManufacturedYear = 2020,
    //            RemainingFuel = 0,
    //            CarStatus = CarStatus.Free,
    //            OneYearMediumDistance = 36000
    //        },
    //        new Car()
    //        {
    //            Model = "Chevrolet Malibu-2",
    //            Color = "Qora",
    //            Number = "01/011 DAV",
    //            Mileage = 0,
    //            MeduimFuelConsumption = 11.5,
    //            FuelTankCapacity = 60,
    //            ManufacturedYear = 2018,
    //            RemainingFuel = 0,
    //            CarStatus = CarStatus.Free,
    //            OneYearMediumDistance = 36000
    //        },
    //        new Car()
    //        {
    //            Model = "Chevrolet Malibu-2",
    //            Color = "Qora",
    //            Number = "01/013 DAV",
    //            Mileage = 0,
    //            MeduimFuelConsumption = 11.5,
    //            FuelTankCapacity = 60,
    //            ManufacturedYear = 2019,
    //            RemainingFuel = 0,
    //            CarStatus = CarStatus.Free,
    //            OneYearMediumDistance = 36000
    //        },
    //        new Car()
    //        {
    //            Model = "Chevrolet Malibu-2",
    //            Color = "Qora",
    //            Number = "01/014 DAV",
    //            Mileage = 0,
    //            MeduimFuelConsumption = 11.5,
    //            FuelTankCapacity = 60,
    //            ManufacturedYear = 2020,
    //            RemainingFuel = 0,
    //            CarStatus = CarStatus.Free,
    //            OneYearMediumDistance = 36000
    //        },
    //        new Car()
    //        {
    //            Model = "Chevrolet Lacetti",
    //            Color = "Qora",
    //            Number = "01/234 THA",
    //            Mileage = 0,
    //            MeduimFuelConsumption = 10,
    //            FuelTankCapacity = 60,
    //            ManufacturedYear = 2020,
    //            RemainingFuel = 0,
    //            CarStatus = CarStatus.Free,
    //            OneYearMediumDistance = 27720
    //        },
    //        new Car()
    //        {
    //            Model = "Chevrolet Lacetti",
    //            Color = "Qora",
    //            Number = "01/213 THA",
    //            Mileage = 0,
    //            MeduimFuelConsumption = 10,
    //            FuelTankCapacity = 60,
    //            ManufacturedYear = 2020,
    //            RemainingFuel = 0,
    //            CarStatus = CarStatus.Free,
    //            OneYearMediumDistance = 27720
    //        },
    //        new Car()
    //        {
    //            Model = "Chevrolet Lacetti",
    //            Color = "Qora",
    //            Number = "01/219 THA",
    //            Mileage = 0,
    //            MeduimFuelConsumption = 10,
    //            FuelTankCapacity = 60,
    //            ManufacturedYear = 2017,
    //            RemainingFuel = 0,
    //            CarStatus = CarStatus.Free,
    //            OneYearMediumDistance = 27720
    //        },
    //        new Car()
    //        {
    //            Model = "Mercedes-Benz Sprinter",
    //            Color = "Qora",
    //            Number = "01/200 THA",
    //            Mileage = 28,
    //            MeduimFuelConsumption = 18.6,
    //            FuelTankCapacity = 75,
    //            ManufacturedYear = 2020,
    //            RemainingFuel = 0,
    //            CarStatus = CarStatus.Free,
    //            OneYearMediumDistance = 16128
    //        },
    //        new Car()
    //        {
    //            Model = "Mercedes-Benz Sprinter",
    //            Color = "Qora",
    //            Number = "01/300 THA",
    //            Mileage = 28,
    //            MeduimFuelConsumption = 18.6,
    //            FuelTankCapacity = 75,
    //            ManufacturedYear = 1996,
    //            RemainingFuel = 0,
    //            CarStatus = CarStatus.Free,
    //            OneYearMediumDistance = 16128
    //        }
    //    };

    //    context.Cars.AddRange(cars);
    //    context.SaveChanges();
    //}

    //private static void CreateDrivers(CheckDriveDbContext context)
    //{
    //    if (context.Drivers.Any()) return;

    //    var accounts = context.Accounts.ToList();
    //    var roles = context.Roles.ToList();
    //    List<Driver> drivers = new();

    //    foreach (var account in accounts)
    //    {
    //        var driverRole = roles.FirstOrDefault(r => r.Name == "Haydovchi");
    //        if (driverRole != null && account.RoleId == driverRole.Id)
    //        {
    //            drivers.Add(new Driver()
    //            {
    //                AccountId = account.Id,
    //            });
    //        }
    //    }

    //    context.Drivers.AddRange(drivers);
    //    context.SaveChanges();
    //}

    //private static void CreateDoctors(CheckDriveDbContext context)
    //{
    //    if (context.Doctors.Any()) return;

    //    var accounts = context.Accounts.ToList();
    //    var roles = context.Roles.ToList();
    //    List<Doctor> doctors = new();

    //    foreach (var account in accounts)
    //    {
    //        var doctorRole = roles.FirstOrDefault(r => r.Name == "Shifokor");
    //        if (doctorRole != null && account.RoleId == doctorRole.Id)
    //        {
    //            doctors.Add(new Doctor()
    //            {
    //                AccountId = account.Id,
    //            });
    //        }
    //    }

    //    context.Doctors.AddRange(doctors);
    //    context.SaveChanges();
    //}

    //private static void CreateOperators(CheckDriveDbContext context)
    //{
    //    if (context.Operators.Any()) return;

    //    var accounts = context.Accounts.ToList();
    //    var roles = context.Roles.ToList();
    //    List<Operator> operators = new();

    //    foreach (var account in accounts)
    //    {
    //        var operatorRole = roles.FirstOrDefault(r => r.Name == "Operator");
    //        if (operatorRole != null && account.RoleId == operatorRole.Id)
    //        {
    //            operators.Add(new Operator()
    //            {
    //                AccountId = account.Id,
    //            });
    //        }
    //    }

    //    context.Operators.AddRange(operators);
    //    context.SaveChanges();
    //}

    //private static void CreateDispatchers(CheckDriveDbContext context)
    //{
    //    if (context.Dispatchers.Any()) return;

    //    var accounts = context.Accounts.ToList();
    //    var roles = context.Roles.ToList();
    //    List<Dispatcher> dispatchers = new();

    //    foreach (var account in accounts)
    //    {
    //        var dispatcherRole = roles.FirstOrDefault(r => r.Name == "Dispetcher");
    //        if (dispatcherRole != null && account.RoleId == dispatcherRole.Id)
    //        {
    //            dispatchers.Add(new Dispatcher()
    //            {
    //                AccountId = account.Id,
    //            });
    //        }
    //    }

    //    context.Dispatchers.AddRange(dispatchers);
    //    context.SaveChanges();
    //}

    //private static void CreateMechanics(CheckDriveDbContext context)
    //{
    //    if (context.Mechanics.Any()) return;

    //    var accounts = context.Accounts.ToList();
    //    var roles = context.Roles.ToList();
    //    List<Mechanic> mechanics = new();

    //    foreach (var account in accounts)
    //    {
    //        var mechanicRole = roles.FirstOrDefault(r => r.Name == "Mexanik");
    //        if (mechanicRole != null && account.RoleId == mechanicRole.Id)
    //        {
    //            mechanics.Add(new Mechanic()
    //            {
    //                AccountId = account.Id,
    //            });
    //        }
    //    }

    //    context.Mechanics.AddRange(mechanics);
    //    context.SaveChanges();
    //}

    //private static void CreateDoctorReviews(CheckDriveDbContext context)
    //{
    //    if (context.DoctorReviews.Any()) return;

    //    var drivers = context.Drivers.ToList();
    //    var doctors = context.Doctors.ToList();
    //    List<DoctorReview> doctorReviews = new();

    //    foreach (var doctor in doctors)
    //    {
    //        int doctorReviewsCount = new Random().Next(5, 10);

    //        for (int i = 0; i < doctorReviewsCount; i++)
    //        {
    //            var randomDriver = _faker.PickRandom(drivers);
    //            var isHealthy = _faker.Random.Bool();
    //            var comments = isHealthy ? "" : _faker.Lorem.Sentence();

    //            doctorReviews.Add(new DoctorReview()
    //            {
    //                IsHealthy = isHealthy,
    //                Date = _faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now),
    //                Comments = comments,
    //                DoctorId = doctor.Id,
    //                DriverId = randomDriver.Id,
    //            });
    //        }
    //    }

    //    context.DoctorReviews.AddRange(doctorReviews);
    //    context.SaveChanges();
    //}

    //private static void CreateMechanicHandovers(CheckDriveDbContext context)
    //{
    //    if (context.MechanicsHandovers.Any()) return;

    //    var cars = context.Cars.ToList();
    //    var drivers = context.Drivers.ToList();
    //    var mechanics = context.Mechanics.ToList();
    //    List<MechanicHandover> mechanicHandovers = new();

    //    foreach (var mechanic in mechanics)
    //    {
    //        var mechanicHandoversCount = new Random().Next(5, 10);
    //        for (int i = 0; i < mechanicHandoversCount; i++)
    //        {
    //            var randomDriver = _faker.PickRandom(drivers);
    //            var randomCar = _faker.PickRandom(cars);
    //            var isHanded = _faker.Random.Bool();
    //            var comments = isHanded ? "" : _faker.Lorem.Sentence();
    //            var status = _faker.Random.Enum<Status>();

    //            mechanicHandovers.Add(new MechanicHandover()
    //            {
    //                IsHanded = isHanded,
    //                Comments = comments,
    //                Date = _faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now),
    //                Status = status,
    //                Distance = _faker.Random.Int(50, 100),
    //                MechanicId = mechanic.Id,
    //                DriverId = randomDriver.Id,
    //                CarId = randomCar.Id,
    //            });
    //        }
    //    }

    //    context.MechanicsHandovers.AddRange(mechanicHandovers);
    //    context.SaveChanges();
    //}

    //private static void CreateOperatorReviews(CheckDriveDbContext context)
    //{
    //    if (context.OperatorReviews.Any()) return;

    //    var operators = context.Operators.ToList();
    //    var drivers = context.Drivers.ToList();
    //    var cars = context.Cars.ToList();
    //    var oilMark = context.OilMarks.ToList();
    //    List<OperatorReview> operatorReviews = new();

    //    foreach (var operatorr in operators)
    //    {
    //        var operatorReviewsCount = new Random().Next(5, 10);
    //        for (int i = 0; i < operatorReviewsCount; i++)
    //        {
    //            var randomDriver = _faker.PickRandom(drivers);
    //            var randomCar = _faker.PickRandom(cars);
    //            var randomOilMark = _faker.PickRandom(oilMark);
    //            var status = _faker.Random.Enum<Status>();
    //            var isGiven = _faker.Random.Bool();
    //            var comments = isGiven ? "" : _faker.Lorem.Sentence();

    //            operatorReviews.Add(new OperatorReview()
    //            {
    //                OilAmount = _faker.Random.Double(10, 20),
    //                Date = _faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now),
    //                Status = status,
    //                IsGiven = isGiven,
    //                Comments = comments,
    //                OperatorId = operatorr.Id,
    //                DriverId = randomDriver.Id,
    //                CarId = randomCar.Id,
    //                OilMarkId = randomOilMark.Id,
    //            });
    //        }
    //    }

    //    context.OperatorReviews.AddRange(operatorReviews);
    //    context.SaveChanges();
    //}
    //private static void CreateMechanicAcceptance(CheckDriveDbContext context)
    //{
    //    if (context.MechanicsAcceptances.Any()) return;

    //    var cars = context.Cars.ToList();
    //    var drivers = context.Drivers.ToList();
    //    var mechanics = context.Mechanics.ToList();
    //    List<MechanicAcceptance> mechanicAcceptances = new();

    //    foreach (var mechanic in mechanics)
    //    {
    //        var mechanicAcceptancesCount = new Random().Next(5, 10);
    //        for (int i = 0; i < mechanicAcceptancesCount; i++)
    //        {
    //            var randomDriver = _faker.PickRandom(drivers);
    //            var randomCar = _faker.PickRandom(cars);
    //            var isAccepted = _faker.Random.Bool();
    //            var comments = isAccepted ? "" : _faker.Lorem.Sentence();
    //            var status = _faker.Random.Enum<Status>();

    //            mechanicAcceptances.Add(new MechanicAcceptance()
    //            {
    //                IsAccepted = isAccepted,
    //                Comments = comments,
    //                Date = _faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now),
    //                Status = status,
    //                Distance = _faker.Random.Int(50, 100),
    //                MechanicId = mechanic.Id,
    //                DriverId = randomDriver.Id,
    //                CarId = randomCar.Id,
    //            });
    //        }
    //    }

    //    context.MechanicsAcceptances.AddRange(mechanicAcceptances);
    //    context.SaveChanges();
    //}

    //private static void CreateDispatcherReviews(CheckDriveDbContext context)
    //{
    //    if (context.DispatchersReviews.Any()) return;

    //    var dispatchers = context.Dispatchers.ToList();
    //    var mechanics = context.Mechanics.ToList();
    //    var drivers = context.Drivers.ToList();
    //    var operators = context.Operators.ToList();
    //    var cars = context.Cars.ToList();
    //    var mechanicHandovers = context.MechanicsHandovers.ToList();
    //    var mechanicAcceptances = context.MechanicsAcceptances.ToList();
    //    var operatorReviews = context.OperatorReviews.ToList();
    //    List<DispatcherReview> dispatcherReviews = new();

    //    foreach (var dispatcher in dispatchers)
    //    {
    //        var dispatcherReviewsCount = new Random().Next(5, 10);
    //        for (int i = 0; i < dispatcherReviewsCount; i++)
    //        {
    //            var randomDriver = _faker.PickRandom(drivers);
    //            var randomOperator = _faker.PickRandom(operators);
    //            var randomMechanics = _faker.PickRandom(mechanics);
    //            var randomCar = _faker.PickRandom(cars);
    //            var randomMechanicHandover = _faker.PickRandom(mechanicHandovers);
    //            var randomMechanicAcceptance = _faker.PickRandom(mechanicAcceptances);
    //            var randomOperatorReview = _faker.PickRandom(operatorReviews);

    //            dispatcherReviews.Add(new DispatcherReview()
    //            {
    //                Date = _faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now),
    //                FuelSpended = _faker.Random.Double(10, 20),
    //                DistanceCovered = _faker.Random.Int(50, 100),
    //                DispatcherId = dispatcher.Id,
    //                DriverId = randomDriver.Id,
    //                OperatorId = randomOperator.Id,
    //                MechanicId = randomMechanics.Id,
    //                CarId = randomCar.Id,
    //                MechanicAcceptanceId = randomMechanicAcceptance.Id,
    //                MechanicHandoverId = randomMechanicHandover.Id,
    //                OperatorReviewId = randomOperatorReview.Id,
    //            });
    //        }
    //    }

    //    context.DispatchersReviews.AddRange(dispatcherReviews);
    //    context.SaveChanges();
    //}
}
