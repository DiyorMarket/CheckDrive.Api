using CheckDrive.Application.DTOs.ManagerReview;

namespace CheckDrive.Application.Interfaces.Review;

public interface IManagerReviewService
{
    Task<ManagerReviewDto> CreateAsync(CreateManagerReviewDto review);
}
