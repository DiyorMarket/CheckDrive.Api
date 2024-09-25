using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/doctors")]
[ApiController]
public class DoctorsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public DoctorsController(IReviewService reviewService)
    {
        _reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
    }

    [HttpPost("{doctorId:guid}/reviews")]
    public async Task<ActionResult<DoctorReviewDto>> CreateReview(Guid doctorId, CreateDoctorReviewDto review)
    {
        if (doctorId != review.ReviewerId)
        {
            return BadRequest($"Route id: {doctorId} does not match with body id: {review.ReviewerId}.");
        }

        var craetedReview = await _reviewService.CreateAsync(review);

        // TODO: change it to CreatedAtAction
        return Created("", craetedReview);
    }
}
