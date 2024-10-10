using Bogus;
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

    public static Faker<CheckPoint> GetCheckPoints() => new Faker<CheckPoint>()
        .RuleFor(x => x.StartDate, f => f.Date.Past(5))
        .RuleFor(x => x.Stage, CheckPointStage.DoctorReview)
        .RuleFor(x => x.Status, f => f.Random.Bool(0.75f) ? CheckPointStatus.InProgress : CheckPointStatus.InterruptedByReviewerRejection);

    public static Faker<DoctorReview> GetDoctorReview(List<int> doctorIds,
        List<int> driverIds) => new Faker<DoctorReview>()
        .RuleFor(x => x.DoctorId, f => f.PickRandom(doctorIds))
        .RuleFor(x => x.DriverId, f => f.PickRandom(driverIds))
        .RuleFor(x => x.Date, f => f.Date.Past(3))
        .RuleFor(x => x.Notes, f => f.Lorem.Sentence(2, 4))
        .RuleFor(x => x.Status, f => f.Random.Bool(0.85f) ? ReviewStatus.Approved : ReviewStatus.RejectedByReviewer);

    public static Faker<MechanicHandover> GetMechanichandover(List<int> mechanicIds,
        List<Car> cars) => new Faker<MechanicHandover>()
      .RuleFor(x => x.MechanicId, f => f.PickRandom(mechanicIds))
      .RuleFor(x => x.CarId, f => f.PickRandom(cars).Id)
      .RuleFor(x => x.InitialMileage, f => f.PickRandom(cars).Mileage + f.Random.Int(1,10))
      .RuleFor(x => x.Notes, f => f.Lorem.Sentence(2, 4))
      .RuleFor(x => x.Status, f => f.Random.Bool(0.85f) ? ReviewStatus.PendingDriverApproval : ReviewStatus.RejectedByReviewer);

    public static Faker<OperatorReview> GetOperatorReviews(List<int> operatorIds,
        List<int> oilMarkIds) => new Faker<OperatorReview>()
      .RuleFor(x => x.OperatorId, f => f.PickRandom(operatorIds))
      .RuleFor(x => x.OilMarkId, f => f.PickRandom(oilMarkIds))
      .RuleFor(x => x.InitialOilAmount, f => f.Random.Int(10,60))
      .RuleFor(x => x.OilRefillAmount, f => f.Random.Int(0,60))
      .RuleFor(x => x.Notes, f => f.Lorem.Sentence(2, 4))
      .RuleFor(x => x.Status, f => f.Random.Bool(0.85f) ? ReviewStatus.PendingDriverApproval : ReviewStatus.RejectedByReviewer);
}

