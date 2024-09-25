using AutoMapper;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.DTOs.MechanicHandover;
using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Entities.Identity;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services;

internal sealed class ReviewService : IReviewService
{
    private readonly ICheckDriveDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public ReviewService(
        ICheckDriveDbContext context,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<DoctorReviewDto> CreateAsync(CreateDoctorReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var doctor = await GetAndValidateDoctorAsync(review.ReviewerId);
        var driver = await GetAndValidateDriverAsync(review.DriverId);

        var checkPoint = _mapper.Map<CheckPoint>(review);
        checkPoint.Driver = driver;

        var reviewEntity = _mapper.Map<DoctorReview>(review);
        reviewEntity.CheckPoint = checkPoint;
        reviewEntity.Doctor = doctor;

        var createdReview = _context.DoctorReviews.Add(reviewEntity).Entity;

        await _context.SaveChangesAsync();

        var dto = _mapper.Map<DoctorReviewDto>(createdReview);

        return dto;
    }

    public async Task<MechanicHandoverReviewDto> CreateAsync(CreateMechanicHandoverReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var mechanic = await GetAndValidateMechanicAsync(review.ReviewerId);
        var checkPoint = await GetAndValidateCheckPointAsync(review.CheckPointId, CheckPointStage.DoctorReview);
        var car = await GetAndValidateCarAsync(review.CarId);

        var reviewEntity = _mapper.Map<MechanicHandover>(review);
        reviewEntity.Mechanic = mechanic;
        reviewEntity.CheckPoint = checkPoint;
        reviewEntity.Car = car;

        checkPoint.Stage = CheckPointStage.MechanicHandover;

        if (!review.IsApprovedByReviewer)
        {
            checkPoint.Status = CheckPointStatus.InterruptedByReviewerRejection;
        }

        var createdReview = _context.MechanicHandovers.Add(reviewEntity).Entity;
        await _context.SaveChangesAsync();

        var dto = _mapper.Map<MechanicHandoverReviewDto>(createdReview);

        return dto;
    }

    public async Task<OperatorReviewDto> CreateAsync(CreateOperatorReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var operatorEntity = await GetAndValidateOperatorAsync(review.ReviewerId);
        var driver = await GetAndValidateDriverAsync(review.DriverId);
        var checkPoint = await GetInProgressCheckPointAsync(review.DriverId);
        var oilMark = await GetAndValidateOilMarkAsync(review.OilMarkId);

        var reviewEntity = new OperatorReview
        {
            CheckPoint = checkPoint,
            Operator = operatorEntity,
            OilMark = oilMark,
            Notes = review.Notes,
            Date = DateTime.UtcNow,
            Status = review.IsApprovedByReviewer ? ReviewStatus.Pending : ReviewStatus.RejectedByReviewer,
            InitialOilAmount = review.InitialOilAmount,
            OilRefillAmount = review.OilRefillAmount,
        };
        checkPoint.Stage = CheckPointStage.OperatorReview;
        checkPoint.Status = review.IsApprovedByReviewer ? CheckPointStatus.InProgress : CheckPointStatus.InterruptedByReviewerRejection;

        var createdReview = _context.OperatorReviews.Add(reviewEntity).Entity;
        await _context.SaveChangesAsync();

        var dto = new OperatorReviewDto();
        dto.CheckPointId = checkPoint.Id;
        dto.ReviewerId = operatorEntity.Id;
        dto.ReviewerName = operatorEntity.FirstName + " " + operatorEntity.LastName;
        dto.Date = createdReview.Date;
        dto.Notes = createdReview.Notes;
        dto.Status = createdReview.Status;
        dto.Type = ReviewType.Operator;
        dto.DriverId = driver.Id;
        dto.DriverName = driver.FirstName + " " + driver.LastName;

        return dto;
    }

    private async Task<User> GetAndValidateDoctorAsync(Guid doctorId)
    {
        var doctor = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == doctorId && x.Position == EmployeePosition.Doctor);

        if (doctor is null)
        {
            throw new EntityNotFoundException($"Doctor with id: {doctorId} is not found.");
        }

        var currentUserId = _currentUserService.GetCurrentUserId();

        if (doctor.Id != currentUserId)
        {
            throw new InvalidOperationException($"Only account owner can perform review.");
        }

        return doctor;
    }

    private async Task<User> GetAndValidateMechanicAsync(Guid mechanicId)
    {
        var mechanic = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == mechanicId && x.Position == EmployeePosition.Mechanic);

        if (mechanic is null)
        {
            throw new EntityNotFoundException($"Mechanic with id: {mechanicId} is not found.");
        }

        var currentUserId = _currentUserService.GetCurrentUserId();

        if (mechanic.Id != currentUserId)
        {
            throw new InvalidOperationException($"Only account owner can perform review.");
        }

        return mechanic;
    }

    private async Task<User> GetAndValidateOperatorAsync(Guid operatorId)
    {
        var operatorEntity = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == operatorId && x.Position == EmployeePosition.Operator);

        if (operatorEntity is null)
        {
            throw new EntityNotFoundException($"Operator with id: {operatorId} is not found.");
        }

        var currentUserId = _currentUserService.GetCurrentUserId();

        if (operatorEntity.Id != currentUserId)
        {
            throw new InvalidOperationException($"Only account owner can perform review.");
        }

        return operatorEntity;
    }

    private async Task<User> GetAndValidateDriverAsync(Guid driverId)
    {
        var driver = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == driverId && x.Position == EmployeePosition.Driver);

        if (driver is null)
        {
            throw new EntityNotFoundException($"Driver with id: {driverId} is not found.");
        }

        return driver;
    }

    private async Task<CheckPoint> GetAndValidateCheckPointAsync(int checkPointId, CheckPointStage stage)
    {
        var checkPoint = await _context.CheckPoints
            .Where(x => x.Id == checkPointId)
            .Where(x => x.Stage == stage)
            .Where(x => x.Status == CheckPointStatus.InProgress)
            .FirstOrDefaultAsync();

        if (checkPoint == null)
        {
            throw new InvalidOperationException($"Cannot start mechanic review without doctor's review present.");
        }

        return checkPoint;
    }

    private async Task<Car> GetAndValidateCarAsync(int carId)
    {
        var car = await _context.Cars
            .FirstOrDefaultAsync(x => x.Id == carId);

        if (car is null)
        {
            throw new EntityNotFoundException($"Car with id: {carId} is not found.");
        }

        if (car.Status != CarStatus.Free)
        {
            throw new UnavailableCarException($"Car with id: {carId} is not available for handover.");
        }

        return car;
    }

    private async Task<OilMark> GetAndValidateOilMarkAsync(int oilMarkId)
    {
        var oilMark = await _context.OilMarks.FirstOrDefaultAsync(x => x.Id == oilMarkId);

        if (oilMark is null)
        {
            throw new EntityNotFoundException($"Oil mark with id: {oilMarkId} is not found.");
        }

        return oilMark;
    }
}
