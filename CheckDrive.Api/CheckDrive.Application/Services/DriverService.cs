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

    public async Task<List<DriverDto>> GetAvailableDriversAsync(CheckPointStage? stage)
    {
        var query = _context.Drivers
            .AsNoTracking()
            .Include(x => x.Account)
            .AsQueryable();

        query = FilterByCheckPointStage(query, stage);

        var drivers = await query
            .Include(x => x.Reviews)
            .Select(x => new
            {
                x.Id,
                x.AccountId,
                FullName = x.FirstName + " " + x.LastName,
                x.Reviews
            })
            .ToListAsync();

        var driversWithCheckPoints = drivers.Select(x =>
        {
            int? checkPointId = x.Reviews.FirstOrDefault(x => x.Date.Date == DateTime.UtcNow.Date)?.CheckPointId;
            return new DriverDto(x.Id, x.AccountId, x.FullName, checkPointId);
        }).ToList();

        return driversWithCheckPoints;
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

        await _context.SaveChangesAsync();
    }

    private async Task<CheckPoint> GetAndValidateCheckPointAsync(int checkPointId)
    {
        var checkPoint = await _context.CheckPoints
            .Include(x => x.MechanicHandover)
            .Include(x => x.MechanicAcceptance)
            .Include(x => x.OperatorReview)
            .FirstOrDefaultAsync(x => x.Id == checkPointId);

        if (checkPoint is null)
        {
            throw new EntityNotFoundException($"Check Point with id: {checkPointId} is not found.");
        }

        return checkPoint;
    }

    private async Task AcceptMechanicHandoverAsync(CheckPoint checkPoint)
    {
        if (checkPoint.MechanicHandover is null)
        {
            throw new InvalidOperationException($"Cannot update car for Check Point without Mechanic Handover Review.");
        }

        var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == checkPoint.MechanicHandover.CarId);

        if (car is null)
        {
            throw new EntityNotFoundException($"Car with id: {checkPoint.MechanicHandover.CarId}");
        }

        car.Mileage = checkPoint.MechanicHandover.InitialMileage;
        checkPoint.MechanicHandover.Status = ReviewStatus.Approved;
        checkPoint.Stage = CheckPointStage.OperatorReview;
    }

    private async Task AcceptOperatorReviewAsync(CheckPoint checkPoint)
    {
        if (checkPoint.OperatorReview is null || checkPoint.MechanicHandover is null)
        {
            throw new InvalidOperationException($"Cannot update car for Check Point without Operator Review.");
        }

        var car = await GetAndValidateCarAsync(checkPoint.MechanicHandover.CarId);
        car.RemainingFuel += checkPoint.OperatorReview.OilRefillAmount;

        checkPoint.Stage = CheckPointStage.MechanicAcceptance;
        checkPoint.OperatorReview.Status = ReviewStatus.Approved;
    }

    private async Task AcceptMechanicAcceptanceAsync(CheckPoint checkPoint)
    {
        if (checkPoint.MechanicHandover is null || checkPoint.MechanicAcceptance is null)
        {
            throw new InvalidOperationException("Cannot perform Mechanic Acceptance without Mechanic Handover Review.");
        }

        var car = await GetAndValidateCarAsync(checkPoint.MechanicHandover.CarId);
        car.Mileage = checkPoint.MechanicAcceptance.FinalMileage;
        car.RemainingFuel = checkPoint.MechanicAcceptance.RemainingFuelAmount;

        checkPoint.Stage = CheckPointStage.DispatcherReview;
        checkPoint.MechanicAcceptance.Status = ReviewStatus.Approved;
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

    private static IQueryable<Driver> FilterByCheckPointStage(IQueryable<Driver> query, CheckPointStage? stage)
    {
        switch (stage)
        {
            case null:
                return query.Where(x => !x.Reviews.Any(x => x.Date.Date == DateTime.UtcNow.Date));
            case CheckPointStage.DoctorReview:
                return query.Where(x => x.Reviews.Any(r => r.Date.Date == DateTime.UtcNow.Date && r.Status == ReviewStatus.Approved));
            default:
                return query;
        }
    }
}
