using CheckDrive.Infrastructure.Persistence;
using CheckDrive.Tests.Api.ResponseValidators.Review;

namespace CheckDrive.Tests.Api.ResponseValidators;

public class ResponseValidator
{
    private readonly DoctorReviewValidator _doctorReview;
    public DoctorReviewValidator DoctorReview => _doctorReview;

    public ResponseValidator(CheckDriveDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _doctorReview = new DoctorReviewValidator(context);
    }
}
