using AutoMapper;
using CheckDrive.Application.DTOs.MechanicAcceptance;
using CheckDrive.Application.Hubs;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services.Review;

internal sealed class MechanicAcceptanceService(
    ICheckDriveDbContext context,
    IMapper mapper,
    IHubContext<ReviewHub, IReviewHub> hubContext)
    : IMechanicAcceptanceService
{
    public async Task<MechanicAcceptanceReviewDto> GetByIdAsync(int reviewId)
    {
        var review = await GetAndValidateReviewAsync(reviewId);

        var dto = mapper.Map<MechanicAcceptanceReviewDto>(review);

        return dto;
    }

    public async Task<MechanicAcceptanceReviewDto> CreateAsync(CreateMechanicAcceptanceReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var checkPoint = await GetAndValidateCheckPointAsync(review.CheckPointId);
        var mechanic = await GetAndValidateMechanicAsync(review.MechanicId);

        ValidateMileage(checkPoint, review);
        var reviewEntity = await CreateReviewAsync(checkPoint, mechanic, review);

        context.MechanicAcceptances.Update(reviewEntity);
        await context.SaveChangesAsync();

        var dto = mapper.Map<MechanicAcceptanceReviewDto>(reviewEntity);

        await hubContext.Clients
            .User(checkPoint.DoctorReview.DriverId.ToString())
            .CheckPointProgressUpdated(checkPoint.Id);

        return dto;
    }

    private async Task<MechanicAcceptance> GetAndValidateReviewAsync(int reviewId)
    {
        var review = await context.MechanicAcceptances
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == reviewId);

        if (review is null)
        {
            throw new EntityNotFoundException($"Mechanic Acceptance Review with id: {reviewId} is not found.");
        }

        if (review.Status is not ReviewStatus.Pending)
        {
            throw new InvalidOperationException($"Review's status is invalid to perform Mechanic Acceptance.");
        }

        return review;
    }

    private async Task<CheckPoint> GetAndValidateCheckPointAsync(int checkPointId)
    {
        var checkPoint = await context.CheckPoints
            .Include(x => x.DoctorReview)
            .Include(x => x.MechanicHandover)
            .ThenInclude(x => x!.Car)
            .Include(x => x.OperatorReview)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == checkPointId);

        if (checkPoint is null)
        {
            throw new EntityNotFoundException($"Check point with id: {checkPointId} is not found.");
        }

        if (checkPoint.Stage != CheckPointStage.OperatorReview)
        {
            throw new InvalidOperationException(
                $"Check Point Stage should be in Operator Review to start Mechanic Acceptance Review. Check Point Stage: {checkPoint.Stage}.");
        }

        if (checkPoint.Status != CheckPointStatus.InProgress)
        {
            throw new InvalidOperationException(
                $"Operator Review should be approved before starting Mechanic Acceptance Review. " +
                $"Current check point status: {checkPoint.Status}");
        }

        return checkPoint;
    }

    private async Task<Mechanic> GetAndValidateMechanicAsync(int mechanicId)
    {
        var mechanic = await context.Mechanics
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == mechanicId);

        return mechanic is null ? throw new EntityNotFoundException($"Mechanic with id: {mechanicId} is not found.") : mechanic;
    }

    private async Task<MechanicAcceptance> CreateReviewAsync(
        CheckPoint checkPoint,
        Mechanic mechanic,
        CreateMechanicAcceptanceReviewDto review)
    {

        var entity = new MechanicAcceptance
        {
            Notes = review.Notes,
            Date = DateTime.UtcNow,
            FinalMileage = review.FinalMileage,
            IsCarInGoodCondition = review.IsCarInGoodCondition,
            Status = ReviewStatus.Pending,
            CheckPoint = checkPoint,
            Mechanic = mechanic
        };

        var existingReview = await context.MechanicAcceptances
            .FirstOrDefaultAsync(x => x.CheckPointId == review.CheckPointId);
        if (existingReview is not null)
        {
            entity.Id = existingReview.Id;
        }

        return entity;
    }

    private static void ValidateMileage(CheckPoint checkPoint, CreateMechanicAcceptanceReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(checkPoint);
        ArgumentNullException.ThrowIfNull(review);

        if (checkPoint.MechanicHandover is null)
        {
            throw new InvalidOperationException($"Cannot validate mileage for Check Point without Mechanic Handover");
        }

        var car = checkPoint.MechanicHandover.Car;

        if (review.FinalMileage < car.Mileage)
        {
            throw new InvalidMileageException($"Final mileage ({review.FinalMileage}) cannot be less than car's current mileage ({car.Mileage})");
        }
    }
}
