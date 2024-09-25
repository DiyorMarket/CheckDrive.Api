using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Application.Interfaces.Review;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Reviews;

[Route("api/reviews/operators/{operatorId:guid}")]
[ApiController]
public class OperatorReviewsController : ControllerBase
{
    private readonly IOperatorReviewService _reviewService;

    public OperatorReviewsController(IOperatorReviewService reviewService)
    {
        _reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
    }

    [HttpPost]
    public async Task<ActionResult<OperatorReviewDto>> CreateAsync(
        [FromRoute] Guid operatorId,
        [FromBody] CreateOperatorReviewDto review)
    {
        if (review.ReviewerId != operatorId)
        {
            return BadRequest($"Route id: {operatorId} does not match with body id: {review.ReviewerId}.");
        }

        var createdReview = await _reviewService.CreateAsync(review);

        // TODO: Change to CreatedAtAction method
        return Created("", createdReview);
    }
}
