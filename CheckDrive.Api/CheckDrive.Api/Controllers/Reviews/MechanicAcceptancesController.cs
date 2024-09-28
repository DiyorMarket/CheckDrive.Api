using CheckDrive.Application.DTOs.MechanicAcceptance;
using CheckDrive.Application.Interfaces.Review;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Reviews;

[Route("api/reviews/mechanics/{mechanicId:int}/acceptance")]
[ApiController]
public class MechanicAcceptancesController : ControllerBase
{
    private readonly IMechanicAcceptanceService _reviewService;

    public MechanicAcceptancesController(IMechanicAcceptanceService reviewService)
    {
        _reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
    }

    [HttpPost]
    public async Task<ActionResult<MechanicAcceptanceReviewDto>> CreateAcceptanceReviewAsync(
        [FromRoute] int mechanicId,
        [FromBody] CreateMechanicAcceptanceReviewDto review)
    {
        if (review.ReviewerId != mechanicId)
        {
            return BadRequest($"Route id: {mechanicId} does not match with body id: {review.ReviewerId}.");
        }

        var createdReview = await _reviewService.CreateAsync(review);

        // TOODO: Change to CreatedAtAction
        return Created("", createdReview);
    }
}
