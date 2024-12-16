using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.DTOs.ManagerReview;

public sealed record CreateManagerReviewDto(
    int CheckPointId,
    int ReviewerId,
    string? Notes,
    int FinalMileage,
    decimal DebtAmount,
    decimal FuelConsumption,
    decimal RemainingFuelAmount)
    : CreateReviewDtoBase(
        ReviewerId: ReviewerId,
        Notes: Notes);
