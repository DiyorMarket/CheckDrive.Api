using CheckDrive.Application.DTOs.OperatorReview;
using FluentValidation;

namespace CheckDrive.Application.Validators;
public class CreateOperatorReviewDtoValidator : AbstractValidator<CreateOperatorReviewDto>
{
    public CreateOperatorReviewDtoValidator()
    {
        RuleFor(x => x.DriverId)
            .GreaterThan(0).WithMessage("Haydovchini tanlashingiz shart.");

        RuleFor(x => x.OilRefillAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Yoqilgi miqdorini kiritishingiz shart va manfiy qiymat bo'lishi mumkin emas.");

        RuleFor(x => x.InitialOilAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Yoqilgi qoldigi miqdorini kiritishingiz shart va manfiy qiymat bo'lishi mumkin emas.");
    }
}
