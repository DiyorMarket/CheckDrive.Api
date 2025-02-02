using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Application.Interfaces.Review;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Reviews;

[Route("api/reviews/operators/{operatorId:int}")]
[ApiController]
public class OperatorReviewsController(IOperatorReviewService reviewService) : ControllerBase
{
    [HttpGet("{reviewId}", Name = nameof(GetOperatorReviewByIdAsync))]
    public async Task<ActionResult<OperatorReviewDto>> GetOperatorReviewByIdAsync(int reviewId)
    {
        var review = await reviewService.GetByIdAsync(reviewId);

        return Ok(review);
    }

    [HttpPost]
    public async Task<ActionResult<OperatorReviewDto>> CreateAsync(
        [FromRoute] int operatorId,
        [FromBody] CreateOperatorReviewDto review)
    {
        if (review.OperatorId != operatorId)
        {
            return BadRequest($"Route id: {operatorId} does not match with body id: {review.OperatorId}.");
        }

        var createdReview = await reviewService.CreateAsync(review);

        return CreatedAtAction(
            nameof(GetOperatorReviewByIdAsync),
            new { operatorId = createdReview.OperatorId, reviewId = createdReview.Id },
            createdReview);
    }
}
