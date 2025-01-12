using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Employee;

public record class UpdateEmployeeDto(
    int Id,
    string AccountId,
    string FirstName,
    string LastName,
    string Patronymic,
    string Passport,
    string Username,
    string? Address,
    string? PhoneNumber,
    string? Email,
    DateTime? Birthdate,
    EmployeePosition Position,
    string? PositionDescription);
