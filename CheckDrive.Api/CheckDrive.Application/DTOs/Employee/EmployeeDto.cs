using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Employee;

public record class EmployeeDto(
    int Id,
    string AccountId,
    string Username,
    string PhoneNumber,
    string? Email,
    string FirstName,
    string LastName,
    string Patronymic,
    string Passport,
    string Address,
    DateTime Birthdate,
    EmployeePosition Position);
