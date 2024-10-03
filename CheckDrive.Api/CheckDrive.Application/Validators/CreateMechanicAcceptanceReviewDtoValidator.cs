using CheckDrive.Application.DTOs.MechanicAcceptance;
using FluentValidation;

public class CreateMechanicAcceptanceReviewDtoValidator : AbstractValidator<CreateMechanicAcceptanceReviewDto>
{
    public CreateMechanicAcceptanceReviewDtoValidator()
    {
        RuleFor(x => x.DriverId)
            .GreaterThan(0).WithMessage("Haydovchini tanlashingiz shart.");

        RuleFor(x => x.FinalMileage)
            .GreaterThan(0).WithMessage("Spidometrdagi masofani kiritishingiz shart.");

        RuleFor(x => x.RemainingFuelAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Yoqilgi qoldigi miqdorini kiritishingiz shart va manfiy qiymat qabul qilinmaydi.");

        RuleFor(x => x.DebtAmountFromDriver)
            .GreaterThanOrEqualTo(0).WithMessage("Qarzdorlik miqdori manfiy bo'lmasligi kerak.");
    }
}
