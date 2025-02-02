using CheckDrive.Application.DTOs.MechanicHandover;
using CheckDrive.Application.Interfaces.Review;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Reviews;

[Route("api/reviews/mechanics/{mechanicId:int}/handover")]
[ApiController]
public class MechanicHandoversController(IMechanicHandoverService reviewService) : ControllerBase
{
    [HttpGet("{reviewId}", Name = nameof(GetMechanicHandoverReviewByIdAsync))]
    public async Task<ActionResult<MechanicHandoverReviewDto>> GetMechanicHandoverReviewByIdAsync(int reviewId)
    {
        var review = await reviewService.GetByIdAsync(reviewId);

        return Ok(review);
    }

    [HttpPost]
    public async Task<ActionResult<MechanicHandoverReviewDto>> CreateHandoverReviewAsync(
        [FromRoute] int mechanicId,
        [FromBody] CreateMechanicHandoverReviewDto review)
    {
        if (review.MechanicId != mechanicId)
        {
            return BadRequest($"Route id: {mechanicId} does not match with body id: {review.MechanicId}.");
        }

        var createdReview = await reviewService.CreateAsync(review);

        return CreatedAtAction(
            nameof(GetMechanicHandoverReviewByIdAsync),
            new { mechanicId = createdReview.MechanicId, reviewId = createdReview.CheckPointId },
            createdReview);
    }
}
