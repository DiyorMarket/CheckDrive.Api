using FluentValidation;
using CheckDrive.Application.DTOs.OilMark;

namespace CheckDrive.Application.Validators.OilMark;

public sealed class UpdateOilMarkValidator : AbstractValidator<UpdateOilMarkDto>
{
    public UpdateOilMarkValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(1)
            .WithMessage(x => $"Invalid oil mark id: {x.Id}.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Oil mark name should be specified.");
    }
}
