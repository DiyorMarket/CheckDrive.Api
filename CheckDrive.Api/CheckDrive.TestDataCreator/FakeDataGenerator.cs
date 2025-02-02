using Bogus;
using Bogus.Extensions.Poland;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.TestDataCreator;

public static class FakeDataGenerator
{
    public static Faker<T> GetEmployee<T>() where T : Employee => new Faker<T>()
        .RuleFor(x => x.FirstName, f => f.Person.FirstName)
        .RuleFor(x => x.LastName, f => f.Person.LastName)
        .RuleFor(x => x.Patronymic, f => f.Person.FirstName)
        .RuleFor(x => x.Passport, f => f.Person.Pesel())
        .RuleFor(x => x.Address, f => f.Address.FullAddress())
        .RuleFor(x => x.Birthdate, f => f.Person.DateOfBirth);

    public static Faker<IdentityUser> GetAccount() => new Faker<IdentityUser>()
        .RuleFor(x => x.UserName, f => f.Person.UserName)
        .RuleFor(x => x.Email, f => f.Person.Email)
        .RuleFor(x => x.EmailConfirmed, _ => true)
        .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber("+998 9# ###-##-##"));

    public static Faker<Car> GetCar(List<int> oilMarks) => new Faker<Car>()
        .RuleFor(x => x.Model, f => f.Vehicle.Model())
        .RuleFor(x => x.Number, f => f.Vehicle.Vin()[..7])
        .RuleFor(x => x.ManufacturedYear, f => f.Date.Between(DateTime.Now.AddYears(-20), DateTime.Now.AddYears(-2)).Year)
        .RuleFor(x => x.Mileage, f => f.Random.Number(0, 500_000))
        .RuleFor(x => x.FuelCapacity, f => f.Random.Number(50, 100))
        .RuleFor(x => x.AverageFuelConsumption, f => f.Random.Number(10, 50))
        .RuleFor(x => x.RemainingFuel, (f, x) => f.Random.Decimal(0, x.FuelCapacity))
        .RuleFor(x => x.Status, _ => CarStatus.Free)
        .RuleFor(x => x.Limits, (_, car) => GetCarLimit(car).Generate())
        .RuleFor(x => x.UsageSummary, _ => new CarUsageSummary())
        .RuleFor(x => x.OilMarkId, f => f.PickRandom(oilMarks));

    public static Faker<CarLimits> GetCarLimit(Car car) => new Faker<CarLimits>()
        .RuleFor(x => x.MonthlyDistanceLimit, f => f.Random.Int(1_000, 5_000))
        .RuleFor(x => x.YearlyDistanceLimit, (_, limits) => limits.MonthlyDistanceLimit * 12)
        .RuleFor(x => x.MonthlyFuelConsumptionLimit, (_, limits) => GetMonthlyLimit(car.AverageFuelConsumption, limits.MonthlyDistanceLimit))
        .RuleFor(x => x.YearlyFuelConsumptionLimit, (_, limits) => limits.MonthlyFuelConsumptionLimit * 12);

    private static decimal GetMonthlyLimit(decimal averageFuelConsumption, decimal monthlyDistanceLimit)
    {
        return (averageFuelConsumption * monthlyDistanceLimit) / 100;
    }
}
