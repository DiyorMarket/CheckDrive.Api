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

internal sealed class DoctorReviewService(
    ICheckDriveDbContext context,
    IMapper mapper,
    IHubContext<ReviewHub, IReviewHub> reviewHub)
    : IDoctorReviewService
{
    public async Task<DoctorReviewDto> GetByIdAsync(int id)
    {
        var review = await GetAndValidateReviewAsync(id);

        var dto = mapper.Map<DoctorReviewDto>(review);

        return dto;
    }

    public async Task<DoctorReviewDto> CreateAsync(CreateDoctorReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var doctor = await GetAndValidateDoctorAsync(review.DoctorId);
        var driver = await GetAndValidateDriverAsync(review.DriverId);

        var reviewEntity = CreateReview(review, doctor, driver);
        var checkPoint = CreateCheckPoint(reviewEntity);
        driver.Status = review.IsHealthy ? DriverStatus.OnRide : DriverStatus.NotHealthy;

        context.CheckPoints.Add(checkPoint);
        await context.SaveChangesAsync();

        var dto = mapper.Map<DoctorReviewDto>(reviewEntity);

        await reviewHub.Clients.All
                .CheckPointProgressUpdated(checkPoint.Id);

        return dto;
    }

    private async Task<DoctorReview> GetAndValidateReviewAsync(int reviewId)
    {
        var review = await context.DoctorReviews
            .FirstOrDefaultAsync(x => x.Id == reviewId);

        if (review is null)
        {
            throw new EntityNotFoundException($"Review with id: {reviewId} is not found.");
        }

        return review;
    }

    private async Task<Driver> GetAndValidateDriverAsync(int driverId)
    {
        var driver = await context.Drivers
            .FirstOrDefaultAsync(x => x.Id == driverId);

        if (driver is null)
        {
            throw new EntityNotFoundException($"Driver with id: {driverId} is not found.");
        }

        if (driver.Status != DriverStatus.Available)
        {
            throw new InvalidOperationException($"Cannot start new Check Point for Driver with active Check Point.");
        }

        return driver;
    }

    private async Task<Doctor> GetAndValidateDoctorAsync(int doctorId)
    {
        var doctor = await context.Doctors
            .FirstOrDefaultAsync(x => x.Id == doctorId);

        if (doctor is null)
        {
            throw new EntityNotFoundException($"Doctor with id: {doctorId} is not found.");
        }

        return doctor;
    }

    private DoctorReview CreateReview(CreateDoctorReviewDto review, Doctor doctor, Driver driver)
    {
        var entity = mapper.Map<DoctorReview>(review);
        entity.Doctor = doctor;
        entity.Driver = driver;

        return entity;
    }

    private static CheckPoint CreateCheckPoint(DoctorReview review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var checkPoint = new CheckPoint
        {
            StartDate = DateTime.UtcNow,
            Stage = CheckPointStage.DoctorReview,
            Status = review.IsHealthy ? CheckPointStatus.InProgress : CheckPointStatus.Interrupted,
            DoctorReview = review,
        };

        return checkPoint;
    }
}
