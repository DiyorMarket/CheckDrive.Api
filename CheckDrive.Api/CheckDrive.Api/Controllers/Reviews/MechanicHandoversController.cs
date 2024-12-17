using CheckDrive.Application.DTOs.MechanicHandover;
using CheckDrive.Application.Interfaces.Review;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Reviews;

[Route("api/reviews/mechanics/{mechanicId:int}/handover")]
[ApiController]
public class MechanicHandoversController : ControllerBase
{
    private readonly IMechanicHandoverService _reviewService;

    public MechanicHandoversController(IMechanicHandoverService reviewService)
    {
        _reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
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

        var createdReview = await _reviewService.CreateAsync(review);

        // TODO: Change to CreatedAtAction method
        return Created("", createdReview);
    }
}
