using CheckDrive.Application.DTOs.MechanicAcceptance;

namespace CheckDrive.Application.Interfaces.Review;

public interface IMechanicAcceptanceService
{
    Task<MechanicAcceptanceReviewDto> CreateAsync(CreateMechanicAcceptanceReviewDto review);
}
