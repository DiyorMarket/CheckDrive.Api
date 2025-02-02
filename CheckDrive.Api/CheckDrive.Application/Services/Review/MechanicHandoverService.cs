using AutoMapper;
using CheckDrive.Application.DTOs.MechanicHandover;
using CheckDrive.Application.Hubs;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services.Review;

internal sealed class MechanicHandoverService(
    ICheckDriveDbContext context,
    IMapper mapper,
    IHubContext<ReviewHub, IReviewHub> hubContext)
    : IMechanicHandoverService
{
    public async Task<MechanicHandoverReviewDto> GetByIdAsync(int reviewId)
    {
        var review = await GetAndValidateReviewAsync(reviewId);

        var dto = mapper.Map<MechanicHandoverReviewDto>(review);

        return dto;
    }
    /// <summary>
    /// Creates new Mechanic Handover review. If review already exists, then existing one is updated.
    /// </summary>
    /// <param name="review">Review to create or update if it already exists.</param>
    /// <returns>Newly created or updated review.</returns>
    public async Task<MechanicHandoverReviewDto> CreateAsync(CreateMechanicHandoverReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var checkPoint = await GetAndValidateCheckPointAsync(review.CheckPointId);
        var mechanic = await GetAndValidateMechanicAsync(review.MechanicId);
        var car = await GetAndValidateCarAsync(review.CarId);

        ValidateMileage(car, review);
        var reviewEntity = await CreateReviewAsync(review, checkPoint, mechanic, car);

        context.MechanicHandovers.Update(reviewEntity);
        await context.SaveChangesAsync();

        var dto = mapper.Map<MechanicHandoverReviewDto>(reviewEntity);

        await hubContext.Clients
            .User(checkPoint.DoctorReview.DriverId.ToString())
            .CheckPointProgressUpdated(checkPoint.Id);

        return dto;
    }

    private async Task<MechanicHandover> GetAndValidateReviewAsync(int reviewId)
    {
        var review = await context.MechanicHandovers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == reviewId);

        if (review is null)
        {
            throw new EntityNotFoundException($"Mechanic Handover Review with id: {reviewId} is not found.");
        }

        if (review.Status is not ReviewStatus.Pending)
        {
            throw new InvalidOperationException($"Review's status is invalid to perform Mechanic Handover.");
        }

        return review;
    }

    private async Task<CheckPoint> GetAndValidateCheckPointAsync(int checkPointId)
    {
        var checkPoint = await context.CheckPoints
            .Include(x => x.DoctorReview)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == checkPointId);

        if (checkPoint == null)
        {
            throw new EntityNotFoundException($"Check point with id: {checkPointId} is not found.");
        }

        if (checkPoint.Stage != CheckPointStage.DoctorReview || checkPoint.Status != CheckPointStatus.InProgress)
        {
            throw new InvalidOperationException($"Check Point's stage or status is invalid to perform Mechanic Handover.");
        }

        return checkPoint;
    }

    private async Task<Mechanic> GetAndValidateMechanicAsync(int mechanicId)
    {
        var mechanic = await context.Mechanics
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == mechanicId);

        return mechanic
            is null ? throw new EntityNotFoundException($"Mechanic with id: {mechanicId} is not found.")
            : mechanic;
    }

    private async Task<Car> GetAndValidateCarAsync(int carId)
    {
        var car = await context.Cars
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == carId);

        if (car is null)
        {
            throw new EntityNotFoundException($"Car with id: {carId} is not found.");
        }

        if (car.Status != CarStatus.Free)
        {
            throw new CarUnavailableException($"Car with id: {carId} is not available for handover.");
        }

        return car;
    }

    private async Task<MechanicHandover> CreateReviewAsync(
        CreateMechanicHandoverReviewDto review,
        CheckPoint checkPoint,
        Mechanic mechanic,
        Car car)
    {
        ArgumentNullException.ThrowIfNull(review);
        ArgumentNullException.ThrowIfNull(checkPoint);

        var entity = new MechanicHandover()
        {
            Notes = review.Notes,
            Date = DateTime.UtcNow,
            InitialMileage = review.InitialMileage,
            Status = ReviewStatus.Pending,
            CheckPoint = checkPoint,
            Car = car,
            Mechanic = mechanic,
        };

        var existingReview = await context.MechanicHandovers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CheckPointId == review.CheckPointId);
        if (existingReview is not null)
        {
            entity.Id = existingReview.Id;
        }

        return entity;
    }

    private static void ValidateMileage(Car car, CreateMechanicHandoverReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(car);
        ArgumentNullException.ThrowIfNull(review);

        if (review.InitialMileage < car.Mileage)
        {
            throw new InvalidMileageException($"Initial mileage ({review.InitialMileage}) cannot be less than car's current mileage ({car.Mileage})");
        }
    }
}
