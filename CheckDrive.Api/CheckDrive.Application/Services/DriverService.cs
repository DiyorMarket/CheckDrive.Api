using CheckDrive.Application.DTOs.Driver;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services;

internal sealed class DriverService : IDriverService
{
    private readonly ICheckDriveDbContext _context;

    public DriverService(ICheckDriveDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<DriverDto>> GetAvailableDriversAsync()
    {
        var drivers = await _context.Drivers
            .Where(x => !x.Reviews.Any(x => x.Date.Date == DateTime.UtcNow.Date))
            .Select(x => new DriverDto(x.Id, x.AccountId, x.FirstName + " " + x.LastName))
            .ToListAsync();

        return drivers;
    }

    public async Task CreateReviewConfirmation(DriverReviewConfirmationDto confirmation)
    {
        ArgumentNullException.ThrowIfNull(confirmation);

        var checkPoint = await GetAndValidateCheckPointAsync(confirmation.CheckPointId);

        if (!confirmation.IsAcceptedByDriver)
        {
            checkPoint.Status = CheckPointStatus.InterruptedByDriverRejection;
            await _context.SaveChangesAsync();

            return;
        }

        await CreateReviewAsync(checkPoint, confirmation);
        await _context.SaveChangesAsync();
    }

    private async Task CreateReviewAsync(CheckPoint checkPoint, DriverReviewConfirmationDto confirmation)
    {
        if (confirmation.ReviewType == ReviewType.MechanicHandover)
        {
            await AcceptMechanicHandoverAsync(checkPoint);
        }

        if (confirmation.ReviewType == ReviewType.OperatorReview)
        {
            await AcceptOperatorReviewAsync(checkPoint);
        }

        if (confirmation.ReviewType == ReviewType.MechanicAcceptance)
        {
            await AcceptMechanicAcceptanceAsync(checkPoint);
        }
    }

    private async Task AcceptMechanicHandoverAsync(CheckPoint checkPoint)
    {
        if (checkPoint.MechanicHandover is null)
        {
            throw new InvalidOperationException($"Cannot update car for Check Point without Mechanic Handover Review.");
        }

        var car = await GetAndValidateCarAsync(checkPoint.MechanicHandover.CarId);

        car.Mileage = checkPoint.MechanicHandover.InitialMileage;
        car.Status = CarStatus.Busy;
        checkPoint.Stage = CheckPointStage.MechanicHandover;
        checkPoint.MechanicHandover.Status = ReviewStatus.Approved;
    }

    private async Task AcceptOperatorReviewAsync(CheckPoint checkPoint)
    {
        if (checkPoint.OperatorReview is null || checkPoint.MechanicHandover is null)
        {
            throw new InvalidOperationException($"Cannot update car for Check Point without Operator Review.");
        }

        var car = await GetAndValidateCarAsync(checkPoint.MechanicHandover.CarId);

        car.RemainingFuel = checkPoint.OperatorReview.InitialOilAmount + checkPoint.OperatorReview.OilRefillAmount;
        checkPoint.Stage = CheckPointStage.OperatorReview;
        checkPoint.OperatorReview.Status = ReviewStatus.Approved;
    }

    private async Task AcceptMechanicAcceptanceAsync(CheckPoint checkPoint)
    {
        if (checkPoint.MechanicHandover is null || checkPoint.MechanicAcceptance is null)
        {
            throw new InvalidOperationException("Cannot perform Mechanic Acceptance without Mechanic Handover Review.");
        }

        var car = await GetAndValidateCarAsync(checkPoint.MechanicHandover.CarId);
        var fuelConsumption = CalculateFuelConsumption(
            checkPoint.MechanicHandover.InitialMileage,
            checkPoint.MechanicAcceptance.FinalMileage,
            car.AverageFuelConsumption);

        if (fuelConsumption > car.RemainingFuel)
        {
            var debt = CreateDebt(
                checkPoint,
                fuelConsumption,
                car.RemainingFuel,
                checkPoint.MechanicAcceptance.RemainingFuelAmount);
            _context.Debts.Add(debt);
        }

        car.Mileage = checkPoint.MechanicAcceptance.FinalMileage;
        car.RemainingFuel = checkPoint.MechanicAcceptance.RemainingFuelAmount;
        car.CurrentMonthMileage += checkPoint.MechanicHandover.InitialMileage - checkPoint.MechanicAcceptance.FinalMileage;
        checkPoint.Stage = CheckPointStage.MechanicAcceptance;
        checkPoint.MechanicAcceptance.Status = ReviewStatus.Approved;
    }

    private async Task<CheckPoint> GetAndValidateCheckPointAsync(int checkPointId)
    {
        var checkPoint = await _context.CheckPoints
            .Include(x => x.MechanicHandover)
            .Include(x => x.OperatorReview)
            .Include(x => x.MechanicAcceptance)
            .FirstOrDefaultAsync(x => x.Id == checkPointId);

        if (checkPoint is null)
        {
            throw new EntityNotFoundException($"Check Point with id: {checkPointId} is not found.");
        }

        if (checkPoint.Status != CheckPointStatus.InProgress)
        {
            throw new InvalidOperationException(
                $"Driver can perform review confirmation only for In Progress Check Point. Current check point status: {checkPoint.Status}");
        }

        return checkPoint;
    }

    private async Task<Car> GetAndValidateCarAsync(int carId)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == carId);

        if (car is null)
        {
            throw new EntityNotFoundException($"Car with id: {carId} is not found.");
        }

        return car;
    }

    private static decimal CalculateFuelConsumption(
        int initialMileage,
        int finalMileage,
        decimal averageFuelConsumption)
    {
        var mileageAmount = finalMileage - initialMileage;
        var fuelSpent = averageFuelConsumption * mileageAmount;

        return fuelSpent;
    }

    private static Debt CreateDebt(CheckPoint checkPoint, decimal fuelSpent, decimal fuelInitialAmount, decimal fuelFinalAmount)
    {
        var debt = new Debt
        {
            CheckPoint = checkPoint,
            FuelAmount = fuelSpent - fuelInitialAmount - fuelFinalAmount,
            PaidAmount = 0,
            Status = DebtStatus.Unpaid
        };

        return debt;
    }
}
