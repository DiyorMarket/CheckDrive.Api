using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Application.Interfaces.Review;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Reviews;

[Route("api/reviews/operators/{operatorId:int}")]
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
        [FromRoute] int operatorId,
        [FromBody] CreateOperatorReviewDto review)
    {
        if (review.OperatorId != operatorId)
        {
            return BadRequest($"Route id: {operatorId} does not match with body id: {review.OperatorId}.");
        }

        var createdReview = await _reviewService.CreateAsync(review);

        // TODO: Change to CreatedAtAction method
        return Created("", createdReview);
    }
}
