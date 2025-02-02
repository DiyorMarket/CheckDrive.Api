using CheckDrive.Application.DTOs.MechanicHandover;

namespace CheckDrive.Application.Interfaces.Review;

public interface IMechanicHandoverService
{
    Task<MechanicHandoverReviewDto> GetByIdAsync(int reviewId);
    Task<MechanicHandoverReviewDto> CreateAsync(CreateMechanicHandoverReviewDto review);
}
