using AutoMapper;
using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services.Review;

internal sealed class OperatorReviewService : IOperatorReviewService
{
    private readonly ICheckDriveDbContext _context;
    private readonly IMapper _mapper;

    public OperatorReviewService(ICheckDriveDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<OperatorReviewDto> CreateAsync(CreateOperatorReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var checkPoint = await GetAndValidateCheckPointAsync(review.CheckPointId);
        var @operator = await GetAndValidateOperatorAsync(review.ReviewerId);
        var oilMark = await GetAndValidateOilMarkAsync(review.OilMarkId);

        RefillCar(checkPoint.MechanicHandover!.Car, review);
        UpdateCheckPoint(checkPoint, review);

        var reviewEntity = CreateReviewEntity(checkPoint, oilMark, @operator, review);

        var createdReview = _context.OperatorReviews.Add(reviewEntity).Entity;
        await _context.SaveChangesAsync();

        var dto = _mapper.Map<OperatorReviewDto>(createdReview);

        return dto;
    }

    private async Task<CheckPoint> GetAndValidateCheckPointAsync(int checkPointId)
    {
        var checkPoint = await _context.CheckPoints
            .Where(x => x.Id == checkPointId)
            .Where(x => x.MechanicHandover != null)
            .Include(x => x.MechanicHandover)
            .ThenInclude(mh => mh!.Car)
            .FirstOrDefaultAsync();

        if (checkPoint is null)
        {
            throw new EntityNotFoundException($"Check point with id: {checkPointId} is not found.");
        }

        if (checkPoint.MechanicHandover!.Status != ReviewStatus.Approved)
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

    private static void RefillCar(Car car, CreateOperatorReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(car);

        var total = review.InitialOilAmount + review.OilRefillAmount;

        if (car.FuelCapacity < total)
        {
            throw new FuelAmountExceedsCarCapacityException($"{total} exceeds car fuel capacity: {car.FuelCapacity}.");
        }

        car.RemainingFuel = total;
    }

    private static void UpdateCheckPoint(CheckPoint checkPoint, CreateOperatorReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(checkPoint);
        ArgumentNullException.ThrowIfNull(review);

        checkPoint.Stage = CheckPointStage.OperatorReview;

        if (!review.IsApprovedByReviewer)
        {
            checkPoint.Status = CheckPointStatus.InterruptedByReviewerRejection;
        }
    }

    private static OperatorReview CreateReviewEntity(
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
            Date = DateTime.Now,
            Status = review.IsApprovedByReviewer ? ReviewStatus.PendingDriverApproval : ReviewStatus.RejectedByReviewer
        };

        return entity;
    }
}
