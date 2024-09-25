using CheckDrive.Application.DTOs.DispatcherReview;

namespace CheckDrive.Application.Interfaces.Review;

public interface IDispatcherReviewService
{
    Task<DispatcherReviewDto> CreateAsync(CreateDispatcherReviewDto review);
}
