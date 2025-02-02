using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.Interfaces.Review;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Reviews;

[Route("api/reviews/doctors/{doctorId:int}")]
[ApiController]
public class DoctorReviewsController(IDoctorReviewService reviewService) : ControllerBase
{
    [HttpGet("{reviewId}", Name = nameof(GetReviewByIdAsync))]
    public async Task<ActionResult<DoctorReviewDto>> GetReviewByIdAsync(int reviewId)
    {
        var checkPoint = await reviewService.GetByIdAsync(reviewId);

        return Ok(checkPoint);
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

        var createdReview = await reviewService.CreateAsync(review);

        return CreatedAtAction(
            nameof(GetReviewByIdAsync),
            new { doctorId = createdReview.DoctorId, reviewId = createdReview.Id },
            createdReview);
    }
}
