using AutoMapper;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.DTOs.MechanicHandover;
using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Application.DTOs.Review;
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

    public async Task<ReviewDto> CreateAsync(CreateDoctorReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var doctor = await GetAndValidateDoctorAsync(review.ReviewerId);
        var driver = await GetAndValidateDriverAsync(review.DriverId);

        var checkPoint = new CheckPoint
        {
            StartDate = DateTime.UtcNow,
            Status = review.IsApprovedByReviewer ? CheckPointStatus.InProgress : CheckPointStatus.InterruptedByReviewerRejection,
            Stage = CheckPointStage.DoctorReview,
            DriverId = review.DriverId
        };
        var createdCheckPoint = _context.CheckPoints.Add(checkPoint).Entity;

        var reviewEntity = new DoctorReview
        {
            CheckPoint = checkPoint,
            Doctor = doctor,
            Notes = review.Notes,
            Status = review.IsApprovedByReviewer ? ReviewStatus.Completed : ReviewStatus.RejectedByReviewer
        };
        var createdReview = _context.DoctorReviews.Add(reviewEntity).Entity;

        await _context.SaveChangesAsync();

        var dto = new ReviewDto();
        dto.CheckPointId = createdCheckPoint.Id;
        dto.ReviewerId = doctor.Id;
        dto.ReviewerName = doctor.FirstName + " " + doctor.LastName;
        dto.Date = createdReview.Date;
        dto.Notes = createdReview.Notes;
        dto.Status = reviewEntity.Status;
        dto.Type = GetType(createdCheckPoint.Stage);
        dto.DriverId = driver.Id;
        dto.DriverName = driver.FirstName + " " + driver.LastName;

        return dto;
    }

    public async Task<MechanicHandoverReviewDto> CreateAsync(CreateMechanicHandoverDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var mechanic = await GetAndValidateMechanicAsync(review.ReviewerId);
        var driver = await GetAndValidateDriverAsync(review.DriverId);
        var checkPoint = await GetInProgressCheckPointAsync(review.DriverId);

        var reviewEntity = new MechanicHandover
        {
            CheckPoint = checkPoint,
            Mechanic = mechanic,
            Notes = review.Notes,
            Date = DateTime.UtcNow,
            InitialMileage = review.InitialMileage,
            Status = review.IsApprovedByReviewer ? ReviewStatus.Completed : ReviewStatus.RejectedByReviewer,
        };
        checkPoint.Stage = CheckPointStage.MechanicHandover;
        checkPoint.Status = review.IsApprovedByReviewer ? CheckPointStatus.InProgress : CheckPointStatus.InterruptedByReviewerRejection;

        var createdReview = _context.MechanicHandovers.Add(reviewEntity).Entity;
        await _context.SaveChangesAsync();

        var dto = new MechanicHandoverReviewDto();
        dto.CheckPointId = checkPoint.Id;
        dto.ReviewerId = mechanic.Id;
        dto.ReviewerName = mechanic.FirstName + " " + mechanic.LastName;
        dto.Date = createdReview.Date;
        dto.Notes = createdReview.Notes;
        dto.Status = createdReview.Status;
        dto.Type = ReviewType.MechanicHandover;
        dto.DriverId = driver.Id;
        dto.DriverName = driver.FirstName + " " + driver.LastName;

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

    private async Task<CheckPoint> GetInProgressCheckPointAsync(Guid driverId)
    {
        var checkPoint = await _context.CheckPoints
            .AsTracking()
            .FirstOrDefaultAsync(x => x.DriverId == driverId
                && x.Status == CheckPointStatus.InProgress
                && x.Stage == CheckPointStage.DoctorReview);

        if (checkPoint == null)
        {
            throw new InvalidOperationException($"Cannot start mechanic review without doctor's review present.");
        }

        return checkPoint;
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

    private static ReviewType GetType(CheckPointStage stage)
    {
        switch (stage)
        {
            case CheckPointStage.DoctorReview:
                return ReviewType.Doctor;
            case CheckPointStage.MechanicHandover:
                return ReviewType.MechanicHandover;
            default:
                throw new ArgumentOutOfRangeException(nameof(stage));
        }
    }
}
