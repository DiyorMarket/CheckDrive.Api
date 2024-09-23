using CheckDrive.Application.DTOs.Review;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.DoctorReview;

public record CreateDoctorReviewDto(
    Guid DriverId,
    Guid ReviewerId,
    string Notes,
    ReviewStatus Status)
    : CreateReviewDtoBase(
        ReviwerId: ReviewerId,
        Notes: Notes,
        Status: Status);
