using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Account;

public record class UpdateAccountDto(
    string Id,
    string Username,
    string Password,
    string PasswordConfirm,
    string PhoneNumber,
    string? Email,
    string FirstName,
    string LastName,
    string Address,
    string Passport,
    DateTime Birthdate,
    EmployeePosition Position);
