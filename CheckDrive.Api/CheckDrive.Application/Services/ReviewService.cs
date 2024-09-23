using AutoMapper;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.DTOs.MechanicHandover;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services;

internal sealed class ReviewService : IReviewService
{
    private readonly ICheckDriveDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public async Task Create(CreateDoctorReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var doctor = _currentUserService.GetCurrentUser();

        if (doctor is null || doctor.Position != EmployeePosition.Doctor)
        {
            throw new ArgumentException("Only doctor can create doctor's review.");
        }

        var driver = await _context.Users.FirstOrDefaultAsync(x => x.Id == review.DriverId && x.Position == EmployeePosition.Driver);

        if (driver is null)
        {
            throw new EntityNotFoundException($"Driver with id: {review.DriverId} is not found.");
        }

        var reviewEntity = _mapper.Map<DoctorReview>(review);
        reviewEntity.DoctorId = doctor.Id;

        var createdReview = _context.DoctorReviews.Add(reviewEntity).Entity;

        var checkPoint = new CheckPoint
        {
            StartDate = DateTime.UtcNow,
            Stage = CheckPointStage.DoctorReview,
            Status = GetStatus(review.Status),
            DriverId = review.DriverId,
            DoctorReview = createdReview,
            Driver = driver,
        };

        _context.CheckPoints.Add(checkPoint);
        await _context.SaveChangesAsync();
    }

    public async Task Create(CreateMechanicHandoverDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var mechanic = _currentUserService.GetCurrentUser();

        if (mechanic is null || mechanic.Position != EmployeePosition.Mechanic)
        {
            throw new InvalidOperationException($"Only mechanic can create handover review.");
        }

        var checkPoint = await _context.CheckPoints
            .AsTracking()
            .FirstOrDefaultAsync(x => x.DriverId == review.DriverId
                && x.Status == CheckPointStatus.InProgress
                && x.Stage == CheckPointStage.DoctorReview);

        if (checkPoint == null)
        {
            throw new InvalidOperationException($"Cannot start mechanic review without doctor's review present.");
        }

        var reviewEntity = new MechanicHandover
        {
            CheckPoint = checkPoint,
            Mechanic = mechanic,
            InitialMileage = review.InitialMileage,
            Notes = review.Notes,
            Status = review.Status,
            Date = DateTime.UtcNow,
        };
        checkPoint.Stage = CheckPointStage.MechanicHandover;
        checkPoint.Status = GetStatus(review.Status);

        _context.MechanicHandovers.Add(reviewEntity);
        await _context.SaveChangesAsync();
    }

    private static CheckPointStatus GetStatus(ReviewStatus status)
    {
        switch (status)
        {
            case ReviewStatus.Accepted:
                return CheckPointStatus.Completed;
            case ReviewStatus.Pending:
                return CheckPointStatus.InProgress;
            default:
                return CheckPointStatus.PendingManagerReview;
        }
    }
}
