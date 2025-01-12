using CheckDrive.Application.DTOs.Employee;
using FluentValidation;

namespace CheckDrive.Application.Validators.Employee;

public sealed class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeDto>
{
    public UpdateEmployeeValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(1)
            .WithMessage(x => $"Invalid employee id: {x.Id}");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(ValidatorConstants.DEFAULT_STRING_LENGTH)
            .WithMessage($"First name must have maximum {ValidatorConstants.DEFAULT_STRING_LENGTH} characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(ValidatorConstants.DEFAULT_STRING_LENGTH)
            .WithMessage($"Last name must have maximum {ValidatorConstants.DEFAULT_STRING_LENGTH} characters");

        RuleFor(x => x.Patronymic)
            .NotEmpty()
            .WithMessage("Patronymic name is required")
            .MaximumLength(ValidatorConstants.DEFAULT_STRING_LENGTH)
            .WithMessage($"Patronymic name must have maximum {ValidatorConstants.DEFAULT_STRING_LENGTH} characters");

        RuleFor(x => x.Passport)
            .NotEmpty()
            .WithMessage("Passport is required")
            .MaximumLength(ValidatorConstants.PASSPORT_LENGTH)
            .WithMessage($"Passport must have maximum {ValidatorConstants.PASSPORT_LENGTH} characters");

        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required")
            .MaximumLength(ValidatorConstants.DEFAULT_STRING_LENGTH)
            .WithMessage($"Username must have maximum {ValidatorConstants.DEFAULT_STRING_LENGTH} characters");

        RuleFor(x => x.Address)
            .MaximumLength(ValidatorConstants.MAX_STRING_LENGTH)
            .WithMessage($"Address must have maximum {ValidatorConstants.MAX_STRING_LENGTH} characters");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(ValidatorConstants.PHONE_NUMBER_LENGTH)
            .WithMessage($"Phone number must have maximum {ValidatorConstants.PHONE_NUMBER_LENGTH} characters");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email address");

        RuleFor(x => x.PositionDescription)
           .MaximumLength(ValidatorConstants.DEFAULT_STRING_LENGTH)
           .WithMessage($"Position description must have maximum {ValidatorConstants.DEFAULT_STRING_LENGTH} characters");
    }
}
