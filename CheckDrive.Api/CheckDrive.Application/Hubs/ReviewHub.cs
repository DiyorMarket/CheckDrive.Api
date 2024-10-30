using CheckDrive.Application.DTOs.Review;
using Microsoft.AspNetCore.SignalR;

namespace CheckDrive.Application.Hubs;

public sealed class ReviewHub : Hub<IReviewHub>
{
    public async Task NotifyNewReviewAsync(ReviewDtoBase review)
    {
        await Clients
            .User(review.DriverId.ToString())
            .NotifyDoctorReview(review);
    }
}
