using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Account;

public record class AccountDto(
    string Id,
    string AccountId,
    string Username,
    string PhoneNumber,
    string? Email,
    string FirstName,
    string LastName,
    string Passport,
    string Address,
    DateTime Birthdate,
    EmployeePosition Position);
