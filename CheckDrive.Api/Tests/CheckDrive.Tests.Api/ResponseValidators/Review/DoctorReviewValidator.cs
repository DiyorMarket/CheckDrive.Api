using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Infrastructure.Persistence;
using CheckDrive.Tests.Api.Extensions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Tests.Api.ResponseValidators.Review;

public class DoctorReviewValidator
{
    private readonly CheckDriveDbContext _context;

    public DoctorReviewValidator(CheckDriveDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task ValidateCreateAsync(CreateDoctorReviewDto request, DoctorReviewDto response)
    {
        response.Should().NotBeNull();

        var createdReview = await _context.DoctorReviews
            .Include(x => x.Doctor)
            .Include(x => x.Driver)
            .Include(x => x.CheckPoint)
            .SingleAsync(x => x.CheckPointId == response.CheckPointId);

        ValidateEntity(createdReview, request);
        ValidateDto(response, createdReview);
        ValidateCheckPoint(createdReview);
    }

    private static void ValidateEntity(DoctorReview entity, CreateDoctorReviewDto dto)
    {
        Assert.NotNull(dto);
        Assert.NotNull(entity);

        entity.Notes.Should().Be(dto.Notes);
        entity.Date.Should().BeCloseToUtcNow();
        entity.Status.Should().Be(dto.IsApprovedByReviewer ? ReviewStatus.Approved : ReviewStatus.RejectedByReviewer);
        entity.DriverId.Should().Be(dto.DriverId);
        entity.DoctorId.Should().Be(dto.ReviewerId);
    }

    private static void ValidateDto(DoctorReviewDto dto, DoctorReview entity)
    {
        Assert.NotNull(entity);
        Assert.NotNull(dto);

        dto.Notes.Should().Be(entity.Notes);
        dto.Date.Should().Be(entity.Date);
        dto.Status.Should().Be(entity.Status);
        dto.ReviewerId.Should().Be(entity.DoctorId);
        dto.ReviewerName.Should().Be(entity.Doctor.FirstName + " " + entity.Doctor.LastName);
        dto.DriverId.Should().Be(entity.DriverId);
        dto.DriverName.Should().Be(entity.Driver.FirstName + " " + entity.Driver.LastName);
    }

    private static void ValidateCheckPoint(DoctorReview review)
    {
        Assert.NotNull(review);

        var checkPoint = review.CheckPoint;
        var expectedStatus = review.Status == ReviewStatus.Approved
            ? CheckPointStatus.InProgress
            : CheckPointStatus.InterruptedByReviewerRejection;

        checkPoint.StartDate.Should().BeCloseToUtcNow();
        checkPoint.Stage.Should().Be(CheckPointStage.DoctorReview);
        checkPoint.Status.Should().Be(expectedStatus);
    }
}
