using CheckDrive.Application.DTOs.MechanicAcceptance;
using CheckDrive.Application.Interfaces.Review;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Reviews;

[Route("api/reviews/mechanics/{mechanicId:int}/acceptance")]
[ApiController]
public class MechanicAcceptancesController(IMechanicAcceptanceService reviewService) : ControllerBase
{
    [HttpGet("{reviewId}", Name = nameof(GetMechanicAcceptanceReviewByIdAsync))]
    public async Task<ActionResult<MechanicAcceptanceReviewDto>> GetMechanicAcceptanceReviewByIdAsync(int reviewId)
    {
        var review = await reviewService.GetByIdAsync(reviewId);

        return Ok(review);
    }

    [HttpPost]
    public async Task<ActionResult<MechanicAcceptanceReviewDto>> CreateAcceptanceReviewAsync(
        [FromRoute] int mechanicId,
        [FromBody] CreateMechanicAcceptanceReviewDto review)
    {
        if (review.MechanicId != mechanicId)
        {
            return BadRequest($"Route id: {mechanicId} does not match with body id: {review.MechanicId}.");
        }

        var createdReview = await reviewService.CreateAsync(review);

        return CreatedAtAction(
            nameof(GetMechanicAcceptanceReviewByIdAsync),
            new { mechanicId = createdReview.MechanicId, reviewId = createdReview.Id },
            createdReview);
    }
}
