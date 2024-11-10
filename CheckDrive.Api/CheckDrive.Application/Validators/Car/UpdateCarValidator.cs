using CheckDrive.Application.DTOs.Car;
using FluentValidation;

namespace CheckDrive.Application.Validators.Car;

public sealed class UpdateCarValidator : AbstractValidator<UpdateCarDto>
{
    public UpdateCarValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(1)
            .WithMessage(x => $"Invalid car id: {x.Id}.");

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("Car model must be specified.");

        RuleFor(x => x.Number)
            .NotEmpty()
            .WithMessage("Car number must be specified.");

        RuleFor(x => x.ManufacturedYear)
            .GreaterThan(1900)
            .LessThan(2026)
            .WithMessage(x => $"Invalid manufactured year: {x.ManufacturedYear}.");

        RuleFor(x => x.Mileage)
            .GreaterThanOrEqualTo(0)
            .WithMessage(x => $"Invalid mileage valule {x.Mileage}.");

        RuleFor(x => x.CurrentMonthMileage)
            .GreaterThanOrEqualTo(0)
            .WithMessage(x => $"Invalid current month mileage: {x.CurrentMonthMileage}.");

        RuleFor(x => x.CurrentYearMileage)
            .GreaterThanOrEqualTo(0)
            .WithMessage(x => $"Invalid current year mileage: {x.CurrentYearMileage}.");

        RuleFor(x => x.MonthlyDistanceLimit)
            .GreaterThan(0)
            .WithMessage(x => $"Invalid monthly distance limit: {x.MonthlyDistanceLimit}.");

        RuleFor(x => x.YearlyDistanceLimit)
            .GreaterThan(0)
            .WithMessage(x => $"Invalid yearly distance limit: {x.YearlyDistanceLimit}.");

        RuleFor(x => x.CurrentMonthFuelConsumption)
            .GreaterThanOrEqualTo(0)
            .WithMessage(x => $"Invalid current month fuel consumption: {x.CurrentMonthFuelConsumption}.");

        RuleFor(x => x.CurrentYearMileage)
            .GreaterThanOrEqualTo(0)
            .WithMessage(x => $"Invalid current year fuel consumption: {x.CurrentYearFuelConsumption}.");

        RuleFor(x => x.MonthlyFuelConsumptionLimit)
            .GreaterThanOrEqualTo(0)
            .WithMessage(x => $"Invalid monthly fuel consumption limit: {x.MonthlyFuelConsumptionLimit}.");

        RuleFor(x => x.YearlyFuelConsumptionLimit)
            .GreaterThanOrEqualTo(0)
            .WithMessage(x => $"Invalid yearly fuel consumption limit: {x.YearlyFuelConsumptionLimit}.");

        RuleFor(x => x.AverageFuelConsumption)
            .GreaterThan(0)
            .WithMessage(x => $"Invalid average fuel consumption value: {x.AverageFuelConsumption}.");

        RuleFor(x => x.FuelCapacity)
            .GreaterThan(0)
            .WithMessage(x => $"Invalid fuel capacity value: {x.FuelCapacity}.");

        RuleFor(x => x.RemainingFuel)
            .GreaterThanOrEqualTo(0)
            .WithMessage(x => $"Invalid remaining fuel value: {x.RemainingFuel}.");
    }
}
