using CheckDrive.Application.DTOs.DispatcherReview;

namespace CheckDrive.Application.Interfaces.Review;

public interface IDispatcherReviewService
{
    Task<DispatcherReviewDto> GetByIdAsync(int reviewId);
    Task<DispatcherReviewDto> CreateAsync(CreateDispatcherReviewDto review);
}
