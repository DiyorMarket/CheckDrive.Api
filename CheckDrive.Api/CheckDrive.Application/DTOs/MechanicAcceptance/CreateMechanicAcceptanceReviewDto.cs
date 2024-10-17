using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.DTOs.MechanicAcceptance;

public sealed record CreateMechanicAcceptanceReviewDto(
    int CheckPointId,
    int DriverId,
    int ReviewerId,
    string? Notes,
    bool IsApprovedByReviewer,
    decimal DebtAmountFromDriver,
    int FinalMileage,
    decimal RemainingFuelAmount)
    : CreateReviewDtoBase(
        ReviewerId: ReviewerId,
        Notes: Notes,
        IsApprovedByReviewer: IsApprovedByReviewer);
