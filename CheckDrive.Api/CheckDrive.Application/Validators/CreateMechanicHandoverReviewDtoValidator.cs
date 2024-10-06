using CheckDrive.Application.DTOs.MechanicHandover;
using FluentValidation;

namespace CheckDrive.Application.Validators;
public class CreateMechanicHandoverReviewDtoValidator : AbstractValidator<CreateMechanicHandoverReviewDto>
{
    public CreateMechanicHandoverReviewDtoValidator()
    {
        RuleFor(x => x.DriverId)
            .GreaterThan(0).WithMessage("Haydovchini tanlashingiz shart.");

        RuleFor(x => x.CarId)
            .GreaterThan(0).WithMessage("Moshinani tanlashingiz shart.");

        RuleFor(x => x.InitialMileage)
            .GreaterThan(0).WithMessage("Spidometrdagi masofani kiritishingiz shart.");
    }
}
