using AutoMapper;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Entities.Identity;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services.Review;

internal sealed class DoctorReviewService : IDoctorReviewService
{
    private readonly ICheckDriveDbContext _context;
    private readonly IMapper _mapper;

    public DoctorReviewService(ICheckDriveDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<DoctorReviewDto> CreateAsync(CreateDoctorReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var doctor = await GetAndValidateDoctorAsync(review.ReviewerId);
        var driver = await GetAndValidateDriverAsync(review.DriverId);

        var checkPoint = CreateCheckPoint(review, driver);
        var reviewEntity = CreateReviewEntity(review, checkPoint, doctor);

        var createdReview = _context.DoctorReviews.Add(reviewEntity).Entity;
        await _context.SaveChangesAsync();

        var dto = _mapper.Map<DoctorReviewDto>(createdReview);

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

        if (doctor.Position != EmployeePosition.Doctor)
        {
            throw new InvalidOperationException("Only doctor can perform Doctor Review.");
        }

        return doctor;
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

    private static CheckPoint CreateCheckPoint(CreateDoctorReviewDto review, User driver)
    {
        ArgumentNullException.ThrowIfNull(review);

        var checkPoint = new CheckPoint
        {
            Notes = string.Empty,
            StartDate = DateTime.UtcNow,
            Stage = CheckPointStage.DoctorReview,
            Status = review.IsApprovedByReviewer ? CheckPointStatus.InProgress : CheckPointStatus.InterruptedByReviewerRejection,
            DriverId = driver.Id
        };

        return checkPoint;
    }

    private static DoctorReview CreateReviewEntity(CreateDoctorReviewDto review, CheckPoint checkPoint, User doctor)
    {
        ArgumentNullException.ThrowIfNull(review);
        ArgumentNullException.ThrowIfNull(checkPoint);

        var doctorReview = new DoctorReview
        {
            CheckPoint = checkPoint,
            Doctor = doctor,
            Date = DateTime.UtcNow,
            Notes = review.Notes,
            Status = review.IsApprovedByReviewer ? ReviewStatus.Approved : ReviewStatus.RejectedByReviewer
        };

        return doctorReview;
    }
}
