using CheckDrive.Application.DTOs.DispatcherReview;
using FluentValidation;

namespace CheckDrive.Application.Validators;
public class CreateDispatcherReviewDtoValidator : AbstractValidator<CreateDispatcherReviewDto>
{
    public CreateDispatcherReviewDtoValidator()
    {
        RuleFor(x => x.DistanceTravelledAdjustment)
            .GreaterThanOrEqualTo(0).When(x => x.DistanceTravelledAdjustment.HasValue)
            .WithMessage("Masofa o'zgartirilishi manfiy qiymat bo'lishi mumkin emas.");

        RuleFor(x => x.FuelConsumptionAdjustment)
            .GreaterThanOrEqualTo(0).When(x => x.FuelConsumptionAdjustment.HasValue)
            .WithMessage("Yoqilgi sarfi o'zgartirilishi manfiy qiymat bo'lishi mumkin emas.");
    }
}
