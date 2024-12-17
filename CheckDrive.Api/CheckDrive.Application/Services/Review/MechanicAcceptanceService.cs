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

internal sealed class MechanicAcceptanceService : IMechanicAcceptanceService
{
    private readonly ICheckDriveDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<ReviewHub, IReviewHub> _hubContext;

    public MechanicAcceptanceService(
        ICheckDriveDbContext context,
        IMapper mapper,
        IHubContext<ReviewHub, IReviewHub> hubContext)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }

    public async Task<MechanicAcceptanceReviewDto> CreateAsync(CreateMechanicAcceptanceReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var checkPoint = await GetAndValidateCheckPointAsync(review.CheckPointId);
        var mechanic = await GetAndValidateMechanicAsync(review.ReviewerId);

        if (!review.IsApprovedByReviewer)
        {
            checkPoint.Stage = CheckPointStage.MechanicAcceptance;
            checkPoint.Status = CheckPointStatus.InterruptedByReviewerRejection;
        }

        var reviewEntity = CreateReview(checkPoint, mechanic, review);

        _context.MechanicAcceptances.Add(reviewEntity);
        await _context.SaveChangesAsync();

        var dto = _mapper.Map<MechanicAcceptanceReviewDto>(reviewEntity);

        await _hubContext.Clients
            .User(dto.DriverId.ToString())
            .MechanicAcceptanceConfirmation(dto);

        return dto;
    }

    private async Task<CheckPoint> GetAndValidateCheckPointAsync(int checkPointId)
    {
        var checkPoint = await _context.CheckPoints
            .Include(x => x.DoctorReview)
            .Include(x => x.MechanicHandover)
            .ThenInclude(x => x.Car)
            .Include(x => x.OperatorReview)
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
        var mechanic = await _context.Mechanics
            .FirstOrDefaultAsync(x => x.Id == mechanicId);

        if (mechanic is null)
        {
            throw new EntityNotFoundException($"Mechanic with id: {mechanicId} is not found.");
        }

        return mechanic;
    }

    private static MechanicAcceptance CreateReview(
        CheckPoint checkPoint,
        Mechanic mechanic,
        CreateMechanicAcceptanceReviewDto review)
    {
        var entity = new MechanicAcceptance
        {
            CheckPoint = checkPoint,
            Mechanic = mechanic,
            FinalMileage = review.FinalMileage,
            RemainingFuelAmount = review.RemainingFuelAmount,
            Notes = review.Notes,
            Date = DateTime.UtcNow,
            Status = review.IsApprovedByReviewer ? ReviewStatus.PendingDriverApproval : ReviewStatus.RejectedByReviewer,
        };

        return entity;
    }
}
