using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.DTOs.ManagerReview;

public sealed record CreateManagerReviewDto(
    int CheckPointId,
    int ReviewerId,
    string? Notes,
    int InitialMileage,
    int FinalMileage,
    decimal FuelConsumption,
    decimal RemainingFuelAmount,
    decimal DebtAmount)
    : CreateReviewDtoBase(
        ReviewerId: ReviewerId,
        Notes: Notes);
