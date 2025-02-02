using AutoMapper;
using CheckDrive.Application.DTOs.DispatcherReview;
using CheckDrive.Application.Hubs;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services.Review;

internal sealed class DispatcherReviewService(
    ICheckDriveDbContext context,
    IMapper mapper,
    IHubContext<ReviewHub, IReviewHub> hubContext)
    : IDispatcherReviewService
{
    public async Task<DispatcherReviewDto> GetByIdAsync(int reviewId)
    {
        var review = await GetAndValidateReviewAsync(reviewId);

        var dto = mapper.Map<DispatcherReviewDto>(review);

        return dto;
    }

    public async Task<DispatcherReviewDto> CreateAsync(CreateDispatcherReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var checkPoint = await GetAndValidateCheckPointAsync(review.CheckPointId);
        var dispatcher = await GetAndValidateDispatcherAsync(review.DispatcherId);

        checkPoint.Stage = CheckPointStage.DispatcherReview;

        var reviewEntity = CreateReview(checkPoint, dispatcher, review);

        context.DispatcherReviews.Add(reviewEntity);
        await context.SaveChangesAsync();

        var dto = mapper.Map<DispatcherReviewDto>(reviewEntity);

        await hubContext.Clients
            .User(checkPoint.DoctorReview.DriverId.ToString())
            .CheckPointProgressUpdated(checkPoint.Id);

        return dto;
    }

    private async Task<DispatcherReview> GetAndValidateReviewAsync(int reviewId)
    {
        var review = await context.DispatcherReviews
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == reviewId);

        return review is null ? throw new EntityNotFoundException($"Dispatcher Review with id: {reviewId} is not found.") : review;
    }

    private async Task<CheckPoint> GetAndValidateCheckPointAsync(int checkPointId)
    {
        var checkPoint = await context.CheckPoints
            .Include(x => x.DoctorReview)
            .FirstOrDefaultAsync(x => x.Id == checkPointId);

        if (checkPoint is null)
        {
            throw new EntityNotFoundException($"Check Point with id: {checkPointId} is not found.");
        }

        if (checkPoint.Stage != CheckPointStage.MechanicAcceptance)
        {
            throw new InvalidCheckPointStageException(
                $"Check Point should be in Mechanic Acceptance stage in order to start Dispatcher Review.");
        }

        if (checkPoint.Status != CheckPointStatus.InProgress)
        {
            throw new InvalidReviewStatusException(
                $"Mechanic Acceptance Review should be in 'Approved' status in order to start Dispatcher Review.");
        }

        return checkPoint;
    }

    private async Task<Dispatcher> GetAndValidateDispatcherAsync(int dispatcherId)
    {
        var dispatcher = await context.Dispatchers
            .FirstOrDefaultAsync(x => x.Id == dispatcherId);

        return dispatcher is null ? throw new EntityNotFoundException($"Dispatcher with id: {dispatcherId} is not found.") : dispatcher;
    }

    private static DispatcherReview CreateReview(CheckPoint checkPoint, Dispatcher dispatcher, CreateDispatcherReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(checkPoint);
        ArgumentNullException.ThrowIfNull(dispatcher);

        var entity = new DispatcherReview
        {
            Notes = review.Notes,
            Date = DateTime.UtcNow,
            FinalMileage = review.FinalMileage,
            FuelConsumptionAmount = review.FuelConsumptionAmount,
            RemainingFuelAmount = review.RemainingFuelAmount,
            CheckPoint = checkPoint,
            Dispatcher = dispatcher,
        };

        return entity;
    }
}
