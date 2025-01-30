using CheckDrive.Application.DTOs.ManagerReview;
using CheckDrive.Application.Interfaces.Review;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Reviews;

[ApiController]
[Route("api/reviews/managers/{managerId:int}")]
public class ManagerReviewsController : ControllerBase
{
    private readonly IManagerReviewService _reviewService;

    public ManagerReviewsController(IManagerReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpPost]
    public async Task<ActionResult<ManagerReviewDto>> CreateReview([FromBody] CreateManagerReviewDto review)
    {
        var result = await _reviewService.CreateAsync(review);

        return Created();
    }
}
