using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Account;

public record class AccountDto(
    Guid Id,
    string Username,
    string PhoneNumber,
    string? Email,
    string FirstName,
    string LastName,
    string Passport,
    string Address,
    DateTime Birthdate,
    EmployeePosition Position);
