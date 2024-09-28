using CheckDrive.Application.DTOs.OperatorReview;

namespace CheckDrive.Application.Interfaces.Review;

public interface IOperatorReviewService
{
    Task<OperatorReviewDto> CreateAsync(CreateOperatorReviewDto review);
}
