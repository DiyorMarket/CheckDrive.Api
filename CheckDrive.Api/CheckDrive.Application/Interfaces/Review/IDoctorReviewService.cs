using CheckDrive.Application.DTOs.DoctorReview;

namespace CheckDrive.Application.Interfaces.Review;

public interface IDoctorReviewService
{
    Task<DoctorReviewDto> CreateAsync(CreateDoctorReviewDto review);
}
