using CheckDrive.Application.DTOs.ManagerReview;
using CheckDrive.Application.Interfaces.Review;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Reviews;

[ApiController]
[Route("api/reviews/managers/{managerId:int}")]
public class ManagerReviewsController(IManagerReviewService reviewService) : ControllerBase
{
    [HttpGet("{reviewId}", Name = nameof(GetManagerReviewByIdAsync))]
    public async Task<ActionResult<ManagerReviewDto>> GetManagerReviewByIdAsync(int reviewId)
    {
        var review = await reviewService.GetByIdAsync(reviewId);

        return Ok(review);
    }

    [HttpPost]
    public async Task<ActionResult<ManagerReviewDto>> CreateReview([FromBody] CreateManagerReviewDto review)
    {
        var createdReview = await reviewService.CreateAsync(review);

        return CreatedAtAction(
            nameof(GetManagerReviewByIdAsync),
            new { managerId = createdReview.ReviewerId, reviewId = createdReview.Id },
            createdReview);
    }
}
