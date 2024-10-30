using AutoMapper;
using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Application.Hubs;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services.Review;

internal sealed class OperatorReviewService : IOperatorReviewService
{
    private readonly ICheckDriveDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<ReviewHub, IReviewHub> _hubContext;

    public OperatorReviewService(
        ICheckDriveDbContext context,
        IMapper mapper,
        IHubContext<ReviewHub, IReviewHub> hubContext)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }

    public async Task<OperatorReviewDto> CreateAsync(CreateOperatorReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var checkPoint = await GetAndValidateCheckPointAsync(review.CheckPointId);
        var @operator = await GetAndValidateOperatorAsync(review.ReviewerId);
        var oilMark = await GetAndValidateOilMarkAsync(review.OilMarkId);
        var car = checkPoint.MechanicHandover!.Car;

        if (car.FuelCapacity < review.InitialOilAmount + review.OilRefillAmount)
        {
            throw new InvalidOperationException(
                $"Oil refill amount ({review.InitialOilAmount + review.OilRefillAmount}) exceeds Car's fuel capacity ({car.FuelCapacity}).");
        }

        if (!review.IsApprovedByReviewer)
        {
            checkPoint.Stage = CheckPointStage.OperatorReview;
            checkPoint.Status = CheckPointStatus.InterruptedByReviewerRejection;
        }

        var reviewEntity = CreateReview(checkPoint, oilMark, @operator, review);

        _context.OperatorReviews.Add(reviewEntity);
        await _context.SaveChangesAsync();

        var dto = _mapper.Map<OperatorReviewDto>(reviewEntity);

        await _hubContext.Clients
            .User(dto.DriverId.ToString())
            .OperatorReviewConfirmation(dto);

        return dto;
    }

    private async Task<CheckPoint> GetAndValidateCheckPointAsync(int checkPointId)
    {
        var checkPoint = await _context.CheckPoints
            .Include(x => x.DoctorReview)
            .ThenInclude(x => x.Driver)
            .Include(x => x.MechanicHandover)
            .ThenInclude(x => x!.Car)
            .FirstOrDefaultAsync(x => x.Id == checkPointId);

        if (checkPoint is null)
        {
            throw new EntityNotFoundException($"Check point with id: {checkPointId} is not found.");
        }

        if (checkPoint.Stage != CheckPointStage.MechanicHandover)
        {
            throw new InvalidOperationException($"Cannot start car operator review when check point stage is not Mechanic Handover Review");
        }

        if (checkPoint.Status != CheckPointStatus.InProgress)
        {
            throw new InvalidOperationException($"Cannot start Operator Review while Mechanic Review is not completed.");
        }

        return checkPoint;
    }

    private async Task<Operator> GetAndValidateOperatorAsync(int operatorId)
    {
        var @operator = await _context.Operators
            .FirstOrDefaultAsync(x => x.Id == operatorId);

        if (@operator is null)
        {
            throw new EntityNotFoundException($"Operator with id: {operatorId} is not found.");
        }

        return @operator;
    }

    private async Task<OilMark> GetAndValidateOilMarkAsync(int oilMarkId)
    {
        var oilMark = await _context.OilMarks
            .FirstOrDefaultAsync(x => x.Id == oilMarkId);

        if (oilMark is null)
        {
            throw new EntityNotFoundException($"Oil mark with id: {oilMarkId} is not found.");
        }

        return oilMark;
    }

    private static OperatorReview CreateReview(
        CheckPoint checkPoint,
        OilMark oilMark,
        Operator @operator,
        CreateOperatorReviewDto review)
    {
        var entity = new OperatorReview()
        {
            CheckPoint = checkPoint,
            OilMark = oilMark,
            Operator = @operator,
            InitialOilAmount = review.InitialOilAmount,
            OilRefillAmount = review.OilRefillAmount,
            Notes = review.Notes,
            Date = DateTime.UtcNow,
            Status = review.IsApprovedByReviewer ? ReviewStatus.PendingDriverApproval : ReviewStatus.RejectedByReviewer
        };

        return entity;
    }
}
