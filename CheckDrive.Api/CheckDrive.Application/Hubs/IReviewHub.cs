using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.Hubs;

public interface IReviewHub
{
    Task NotifyNewReviewAsync(ReviewDtoBase review);
    Task SendReviewConfirmationAsync(ReviewConfirmationDto reviewConfirmation);
}
