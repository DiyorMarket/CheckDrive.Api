using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Account;

public record CreateAccountDto(
    string Username,
    string Password,
    string PasswordConfirm,
    string PhoneNumber,
    string? Email,
    string FirstName,
    string LastName,
    string Patronymic,
    string Address,
    string Passport,
    DateTime Birthdate,
    EmployeePosition Position);
