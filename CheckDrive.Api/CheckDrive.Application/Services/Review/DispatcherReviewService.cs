using AutoMapper;
using CheckDrive.Application.DTOs.DispatcherReview;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services.Review;

internal sealed class DispatcherReviewService : IDispatcherReviewService
{
    private readonly ICheckDriveDbContext _context;
    private readonly IMapper _mapper;

    public DispatcherReviewService(ICheckDriveDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<DispatcherReviewDto> CreateAsync(CreateDispatcherReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var checkPoint = await GetAndValidateCheckPointAsync(review.CheckPointId);
        var dispatcher = await GetAndValidateDispatcherAsync(review.ReviewerId);

        UpdateCheckPoint(checkPoint, review);

        var reviewEntity = CreateReviewEntity(checkPoint, dispatcher, review);

        _context.DispatcherReviews.Add(reviewEntity);
        await _context.SaveChangesAsync();

        var dto = _mapper.Map<DispatcherReviewDto>(reviewEntity);

        return dto;
    }

    private async Task<CheckPoint> GetAndValidateCheckPointAsync(int checkPointId)
    {
        var checkPoint = await _context.CheckPoints
            .Include(x => x.MechanicAcceptance)
            .Include(x => x.MechanicHandover)
            .ThenInclude(mh => mh!.Car)
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
        var dispatcher = await _context.Dispatchers
            .FirstOrDefaultAsync(x => x.Id == dispatcherId);

        if (dispatcher is null)
        {
            throw new EntityNotFoundException($"Dispatcher with id: {dispatcherId} is not found.");
        }

        return dispatcher;
    }

    private static DispatcherReview CreateReviewEntity(CheckPoint checkPoint, Dispatcher dispatcher, CreateDispatcherReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(checkPoint);

        var entity = new DispatcherReview
        {
            CheckPoint = checkPoint,
            Dispatcher = dispatcher,
            Notes = review.Notes,
            Date = DateTime.UtcNow,
            Status = review.IsApprovedByReviewer ? ReviewStatus.Approved : ReviewStatus.RejectedByReviewer,
            FuelConsumptionAdjustment = review.FuelConsumptionAdjustment,
            DistanceTravelledAdjustment = review.DistanceTravelledAdjustment,
        };

        return entity;
    }

    private static void UpdateCheckPoint(CheckPoint checkPoint, CreateDispatcherReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(checkPoint);

        checkPoint.Stage = CheckPointStage.DispatcherReview;

        if (review.FuelConsumptionAdjustment.HasValue || review.DistanceTravelledAdjustment.HasValue)
        {
            checkPoint.Stage = CheckPointStage.ManagerReview;
            return;
        }

        if (!review.IsApprovedByReviewer)
        {
            checkPoint.Status = CheckPointStatus.InterruptedByReviewerRejection;
            return;
        }

        checkPoint.Status = CheckPointStatus.Completed;
    }
}
