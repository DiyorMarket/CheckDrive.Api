using CheckDrive.Application.DTOs.DispatcherReview;
using CheckDrive.Application.Interfaces.Review;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Reviews;

[Route("api/reviews/dispatchers/{dispatcherId:int}")]
[ApiController]
public class DispatcherReviewsController(IDispatcherReviewService reviewService) : ControllerBase
{
    [HttpGet("{reviewId}", Name = nameof(GetDispatcherReviewByIdAsync))]
    public async Task<ActionResult<DispatcherReviewDto>> GetDispatcherReviewByIdAsync(int reviewId)
    {
        var review = await reviewService.GetByIdAsync(reviewId);

        return Ok(review);
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

        var createdReview = await reviewService.CreateAsync(review);

        return CreatedAtAction(
            nameof(GetDispatcherReviewByIdAsync),
            new { dispatcherId = createdReview.DispatcherId, reviewId = createdReview.Id },
            createdReview);
    }
}
