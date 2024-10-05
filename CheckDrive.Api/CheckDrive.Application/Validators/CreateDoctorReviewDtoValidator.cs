using CheckDrive.Application.DTOs.DoctorReview;
using FluentValidation;

namespace CheckDrive.Application.Validators;
public class CreateDoctorReviewDtoValidator : AbstractValidator<CreateDoctorReviewDto>
{
    public CreateDoctorReviewDtoValidator()
    {
        RuleFor(x => x.DriverId)
            .GreaterThan(0).WithMessage("Haydovchini tanlashingiz shart.");
    }
}
