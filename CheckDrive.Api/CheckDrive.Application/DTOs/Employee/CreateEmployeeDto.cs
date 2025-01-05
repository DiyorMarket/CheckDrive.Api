using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Employee;

public record CreateEmployeeDto(
    string FirstName,
    string LastName,
    string Patronymic,
    string Passport,
    string Username,
    string Password,
    string? Address,
    string? PhoneNumber,
    string? Email,
    DateTime? Birthdate,
    EmployeePosition Position,
    string? PositionDescription);
