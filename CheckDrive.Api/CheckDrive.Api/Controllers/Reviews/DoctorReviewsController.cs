using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.Interfaces.Review;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Reviews;

[Route("api/reviews/doctors/{doctorId:int}")]
[ApiController]
public class DoctorReviewsController : ControllerBase
{
    private readonly IDoctorReviewService _reviewService;

    public DoctorReviewsController(IDoctorReviewService reviewService)
    {
        _reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
    }

    [HttpPost]
    public async Task<ActionResult<DoctorReviewDto>> CreateReview(
        [FromRoute] int doctorId,
        [FromBody] CreateDoctorReviewDto review)
    {
        if (doctorId != review.DoctorId)
        {
            return BadRequest($"Route id: {doctorId} does not match with body id: {review.DoctorId}.");
        }

        var createdReview = await _reviewService.CreateAsync(review);

        // TODO: change it to CreatedAtAction
        return Created();
    }
}
