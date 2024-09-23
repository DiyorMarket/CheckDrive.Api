using CheckDrive.Application.DTOs.Review;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.DoctorReview;

public record CreateDoctorReviewDto(
    Guid DriverId,
    string Notes,
    DateTime Date,
    ReviewStatus Status)
    : ReviewDtoBase(
        Notes: Notes,
        Date: Date,
        Status: Status);
