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

    /// <summary>
    /// Creates new Operator review. If the review already exists, then existing one is updated.
    /// </summary>
    /// <param name="review">Review to create or update if it already exists.</param>
    /// <returns>Newly created or updated review.</returns>
    public async Task<OperatorReviewDto> CreateAsync(CreateOperatorReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var checkPoint = await GetAndValidateCheckPointAsync(review.CheckPointId);
        var @operator = await GetAndValidateOperatorAsync(review.OperatorId);
        var oilMark = await GetAndValidateOilMarkAsync(review.OilMarkId);

        ValidateOilAmount(checkPoint, review);
        var reviewEntity = await CreateReviewAsync(checkPoint, oilMark, @operator, review);

        _context.OperatorReviews.Update(reviewEntity);
        await _context.SaveChangesAsync();

        var dto = _mapper.Map<OperatorReviewDto>(reviewEntity);

        await _hubContext.Clients
            .User(checkPoint.DoctorReview.DriverId.ToString())
            .CheckPointProgressUpdated(checkPoint.Id);

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

        if (checkPoint.Stage != CheckPointStage.MechanicHandover || checkPoint.Status != CheckPointStatus.InProgress)
        {
            throw new InvalidOperationException($"Check Point's stage or status is invalid to perform Operator review.");
        }

        return checkPoint;
    }

    private async Task<Operator> GetAndValidateOperatorAsync(int operatorId)
    {
        var @operator = await _context.Operators
            .AsNoTracking()
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
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == oilMarkId);

        if (oilMark is null)
        {
            throw new EntityNotFoundException($"Oil mark with id: {oilMarkId} is not found.");
        }

        return oilMark;
    }

    private async Task<OperatorReview> CreateReviewAsync(
        CheckPoint checkPoint,
        OilMark oilMark,
        Operator @operator,
        CreateOperatorReviewDto review)
    {
        checkPoint.Stage = CheckPointStage.OperatorReview;

        var entity = new OperatorReview()
        {
            Notes = review.Notes,
            Date = DateTime.UtcNow,
            InitialOilAmount = review.InitialOilAmount,
            OilRefillAmount = review.OilRefillAmount,
            Status = ReviewStatus.Pending,
            CheckPoint = checkPoint,
            OilMark = oilMark,
            Operator = @operator
        };

        var existingReview = await _context.OperatorReviews
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CheckPointId == review.CheckPointId);

        if (existingReview is not null)
        {
            entity.Id = existingReview.Id;
        }

        return entity;
    }

    private static void ValidateOilAmount(CheckPoint checkPoint, CreateOperatorReviewDto review)
    {
        if (checkPoint.MechanicHandover is null || checkPoint.MechanicHandover.Car is null)
        {
            throw new InvalidOperationException("Cannot validate oil amount without Mechanic Handover.");
        }

        var car = checkPoint.MechanicHandover.Car;
        var totalFuelAmount = review.InitialOilAmount + review.OilRefillAmount;

        if (car.FuelCapacity < totalFuelAmount)
        {
            throw new InvalidOperationException(
                $"Oil refill amount ({review.InitialOilAmount + review.OilRefillAmount}) exceeds Car's fuel capacity ({car.FuelCapacity}).");
        }
    }
}
