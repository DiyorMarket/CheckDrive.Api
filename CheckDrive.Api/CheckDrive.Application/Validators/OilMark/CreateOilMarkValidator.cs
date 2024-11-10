using FluentValidation;
using CheckDrive.Application.DTOs.OilMark;

namespace CheckDrive.Application.Validators.OilMark;

public sealed class CreateOilMarkValidator : AbstractValidator<CreateOilMarkDto>
{
    public CreateOilMarkValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Oil mark should be specified.");
    }
}
