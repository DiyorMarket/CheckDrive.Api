using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Employee;

public record class UpdateEmployeeDto(
    string Id,
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
