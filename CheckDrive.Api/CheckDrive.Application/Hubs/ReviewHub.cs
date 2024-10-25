using CheckDrive.Application.DTOs.Review;
using Microsoft.AspNetCore.SignalR;

namespace CheckDrive.Application.Hubs;

public sealed class ReviewHub : Hub<IReviewHub>
{
    public async Task NotifyNewReviewAsync(ReviewDtoBase review)
    {
        await Clients
            .User(review.DriverId.ToString())
            .NotifyNewReviewAsync(review);
    }

    public override Task OnConnectedAsync()
    {
        var s = Context.UserIdentifier;
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var s = Context.UserIdentifier;
        return base.OnDisconnectedAsync(exception);
    }
}
