using AutoMapper;
using CheckDrive.Application.DTOs.ManagerReview;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services.Review;

internal sealed class ManagerReviewService(
    ICheckDriveDbContext context,
    IMapper mapper)
    : IManagerReviewService
{
    public async Task<ManagerReviewDto> GetByIdAsync(int reviewId)
    {
        var review = await GetAndValidateReviewAsync(reviewId);

        var dto = mapper.Map<ManagerReviewDto>(review);

        return dto;
    }

    public async Task<ManagerReviewDto> CreateAsync(CreateManagerReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var checkPoint = await GetAndValidateCheckPointAsync(review.CheckPointId);
        var manager = await GetAndValidateManagerAsync(review.ReviewerId);

        CreateDebt(review, checkPoint);
        UpdateDriver(checkPoint);
        UpdateCar(review, checkPoint);
        var reviewEntity = CreateReview(review, checkPoint, manager);

        context.ManagerReviews.Add(reviewEntity);
        await context.SaveChangesAsync();

        var dto = mapper.Map<ManagerReviewDto>(reviewEntity);

        return dto;
    }

    private async Task<CheckPoint> GetAndValidateCheckPointAsync(int checkPointId)
    {
        var checkPoint = await context.CheckPoints
            .Include(x => x.DoctorReview)
            .ThenInclude(x => x.Driver)
            .Include(x => x.MechanicHandover)
            .ThenInclude(x => x.Car)
            .Include(x => x.MechanicAcceptance)
            .FirstOrDefaultAsync(x => x.Id == checkPointId);

        if (checkPoint is null)
        {
            throw new EntityNotFoundException($"Check Point with id: {checkPoint} is not found.");
        }

        if (checkPoint.Stage != CheckPointStage.DispatcherReview || checkPoint.Status != CheckPointStatus.InProgress)
        {
            throw new InvalidOperationException($"Check Point stage or status is invalid to perform Manager review.");
        }

        return checkPoint;
    }

    private async Task<ManagerReview> GetAndValidateReviewAsync(int reviewId)
    {
        var review = await context.ManagerReviews
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == reviewId);

        if (review is null)
        {
            throw new EntityNotFoundException($"Manager Review with id: {reviewId} is not found.");
        }

        return review;
    }

    private async Task<Manager> GetAndValidateManagerAsync(int managerId)
    {
        var manager = await context.Managers
            .FirstOrDefaultAsync(x => x.Id == managerId);

        if (manager is null)
        {
            throw new EntityNotFoundException($"Manager with id: {managerId} is not found.");
        }

        return manager;
    }

    private void CreateDebt(CreateManagerReviewDto review, CheckPoint checkPoint)
    {
        ArgumentNullException.ThrowIfNull(review);
        ArgumentNullException.ThrowIfNull(checkPoint);

        if (review.DebtAmount <= 0)
        {
            return;
        }

        var debt = new Debt
        {
            FuelAmount = review.DebtAmount,
            CheckPoint = checkPoint
        };

        context.Debts.Add(debt);
    }

    private static void UpdateDriver(CheckPoint checkPoint)
    {
        ArgumentNullException.ThrowIfNull(checkPoint);

        var driver = checkPoint.DoctorReview.Driver;
        driver.Status = DriverStatus.Available;
    }

    private static void UpdateCar(CreateManagerReviewDto review, CheckPoint checkPoint)
    {
        ArgumentNullException.ThrowIfNull(checkPoint);

        if (checkPoint.MechanicHandover is null || checkPoint.MechanicAcceptance is null)
        {
            throw new InvalidOperationException($"Cannot update car for Check Point without Mechanic reviews.");
        }

        var car = checkPoint.MechanicHandover.Car;
        var travelledDistance = review.FinalMileage - car.Mileage;

        car.UsageSummary.CurrentMonthFuelConsumption += review.FuelConsumption;
        car.UsageSummary.CurrentYearFuelConsumption += review.FuelConsumption;
        car.UsageSummary.CurrentMonthDistance += travelledDistance;
        car.UsageSummary.CurrentYearDistance += travelledDistance;
        car.Mileage = review.FinalMileage;
        car.RemainingFuel = review.RemainingFuelAmount;

        UpdateCarStatus(car, checkPoint.MechanicAcceptance.IsCarInGoodCondition);
    }

    private static ManagerReview CreateReview(
        CreateManagerReviewDto review,
        CheckPoint checkPoint,
        Manager manager)
    {
        ArgumentNullException.ThrowIfNull(review);
        ArgumentNullException.ThrowIfNull(checkPoint);

        var entity = new ManagerReview
        {
            Notes = review.Notes,
            Date = DateTime.UtcNow,
            InitialMileage = review.InitialMileage,
            FinalMileage = review.FinalMileage,
            FuelConsumptionAmount = review.FuelConsumption,
            RemainingFuelAmount = review.RemainingFuelAmount,
            DebtAmount = review.DebtAmount,
            CheckPoint = checkPoint,
            Manager = manager
        };

        return entity;
    }

    private static void UpdateCarStatus(Car car, bool isCarInGoodCondition)
    {
        if (!isCarInGoodCondition)
        {
            car.Status = CarStatus.OutOfService;
            return;
        }

        if (HasCarReachedLimits(car))
        {
            car.Status = CarStatus.LimitReached;
            return;
        }

        car.Status = CarStatus.Free;
    }

    private static bool HasCarReachedLimits(Car car)
    {
        if (car.UsageSummary.CurrentMonthDistance >= car.Limits.MonthlyDistanceLimit)
        {
            return true;
        }

        if (car.UsageSummary.CurrentMonthFuelConsumption >= car.Limits.MonthlyFuelConsumptionLimit)
        {
            return true;
        }

        if (car.UsageSummary.CurrentYearDistance >= car.Limits.YearlyDistanceLimit)
        {
            return true;
        }

        if (car.UsageSummary.CurrentYearFuelConsumption >= car.Limits.YearlyFuelConsumptionLimit)
        {
            return true;
        }

        return false;
    }
}
