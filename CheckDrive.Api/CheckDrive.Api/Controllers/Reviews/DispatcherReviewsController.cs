using CheckDrive.Application.DTOs.DispatcherReview;
using CheckDrive.Application.Interfaces.Review;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Reviews;

[Route("api/reviews/dispatchers/{dispatcherId:int}")]
[ApiController]
public class DispatcherReviewsController : ControllerBase
{
    private readonly IDispatcherReviewService _reviewService;

    public DispatcherReviewsController(IDispatcherReviewService reviewService)
    {
        _reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
    }

    [HttpPost]
    public async Task<ActionResult<DispatcherReviewDto>> CreateAsync(
        [FromRoute] int dispatcherId,
        [FromBody] CreateDispatcherReviewDto review)
    {
        if (review.DispatcherId != dispatcherId)
        {
            return BadRequest($"Route id: {dispatcherId} does not match with body id: {review.DispatcherId}.");
        }

        var createdReview = await _reviewService.CreateAsync(review);

        // TODO: Change to CreatedAtAction method
        return Created();
    }
}
