using AutoMapper;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.Hubs;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services.Review;

internal sealed class DoctorReviewService : IDoctorReviewService
{
    private readonly ICheckDriveDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<ReviewHub, IReviewHub> _reviewHub;

    public DoctorReviewService(ICheckDriveDbContext context, IMapper mapper, IHubContext<ReviewHub, IReviewHub> reviewHub)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _reviewHub = reviewHub ?? throw new ArgumentNullException(nameof(reviewHub));
    }

    public async Task<DoctorReviewDto> CreateAsync(CreateDoctorReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var doctor = await GetAndValidateDoctorAsync(review.ReviewerId);
        var driver = await GetAndValidateDriverAsync(review.DriverId);

        var checkPoint = CreateCheckPoint(review);
        var reviewEntity = CreateReviewEntity(review, checkPoint, doctor, driver);

        _context.DoctorReviews.Add(reviewEntity);
        // await _context.SaveChangesAsync();

        var dto = _mapper.Map<DoctorReviewDto>(reviewEntity);

        await _reviewHub.Clients.User(dto.DriverId.ToString()).NotifyNewReviewAsync(dto);

        return dto;
    }

    private async Task<Doctor> GetAndValidateDoctorAsync(int doctorId)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(x => x.Id == doctorId);

        if (doctor is null)
        {
            throw new EntityNotFoundException($"Doctor with id: {doctorId} is not found.");
        }

        return doctor;
    }

    private async Task<Driver> GetAndValidateDriverAsync(int driverId)
    {
        var driver = await _context.Drivers
            .FirstOrDefaultAsync(x => x.Id == driverId);

        if (driver is null)
        {
            throw new EntityNotFoundException($"Driver with id: {driverId} is not found.");
        }

        return driver;
    }

    private static CheckPoint CreateCheckPoint(CreateDoctorReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var checkPoint = new CheckPoint
        {
            StartDate = DateTime.UtcNow,
            Stage = CheckPointStage.DoctorReview,
            Status = review.IsApprovedByReviewer ? CheckPointStatus.InProgress : CheckPointStatus.InterruptedByReviewerRejection,
            DoctorReview = null!,
        };

        return checkPoint;
    }

    private static DoctorReview CreateReviewEntity(CreateDoctorReviewDto review, CheckPoint checkPoint, Doctor doctor, Driver driver)
    {
        ArgumentNullException.ThrowIfNull(review);
        ArgumentNullException.ThrowIfNull(checkPoint);

        var doctorReview = new DoctorReview
        {
            CheckPoint = checkPoint,
            Doctor = doctor,
            Driver = driver,
            Date = DateTime.UtcNow,
            Notes = review.Notes,
            Status = review.IsApprovedByReviewer ? ReviewStatus.Approved : ReviewStatus.RejectedByReviewer
        };

        return doctorReview;
    }
}
