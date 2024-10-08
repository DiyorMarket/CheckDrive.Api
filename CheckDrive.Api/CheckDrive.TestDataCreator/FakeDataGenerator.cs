﻿using Bogus;
using Bogus.Extensions.Poland;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.TestDataCreator;

public static class FakeDataGenerator
{
    private static readonly List<string> _oilMarks = [
        "80", "90", "95", "98", "100", "110"
    ];

    public static Faker<T> GetEmployee<T>() where T : Employee => new Faker<T>()
        .RuleFor(x => x.FirstName, f => f.Person.FirstName)
        .RuleFor(x => x.LastName, f => f.Person.LastName)
        .RuleFor(x => x.Passport, f => f.Person.Pesel())
        .RuleFor(x => x.Address, f => f.Address.FullAddress())
        .RuleFor(x => x.Birthdate, f => f.Person.DateOfBirth);

    public static Faker<IdentityUser> GetAccount() => new Faker<IdentityUser>()
        .RuleFor(x => x.UserName, f => f.Person.UserName)
        .RuleFor(x => x.Email, f => f.Person.Email)
        .RuleFor(x => x.EmailConfirmed, _ => true)
        .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber("+998 9# ###-##-##"));

    public static Faker<Car> GetCar() => new Faker<Car>()
        .RuleFor(x => x.Model, f => f.Vehicle.Model())
        .RuleFor(x => x.Color, f => f.Commerce.Color())
        .RuleFor(x => x.Number, f => f.Vehicle.Vin().Substring(0, 10))
        .RuleFor(x => x.ManufacturedYear, f => f.Date.Between(DateTime.Now.AddYears(-20), DateTime.Now.AddYears(-2)).Year)
        .RuleFor(x => x.Mileage, f => f.Random.Number(0, 100_000))
        .RuleFor(x => x.YearlyDistanceLimit, f => f.Random.Number(1_000, 5_000))
        .RuleFor(x => x.FuelCapacity, f => f.Random.Number(50, 70))
        .RuleFor(x => x.AverageFuelConsumption, f => f.Random.Number(10, 20))
        .RuleFor(x => x.RemainingFuel, (f, x) => f.Random.Decimal(0, x.FuelCapacity))
        .RuleFor(x => x.Status, _ => CarStatus.Free);

    public static Faker<OilMark> GetOilMark() => new Faker<OilMark>()
        .RuleFor(x => x.Name, f => f.PickRandom(_oilMarks));
}
