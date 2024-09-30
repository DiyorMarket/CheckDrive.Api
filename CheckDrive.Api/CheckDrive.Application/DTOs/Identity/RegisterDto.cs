using CheckDrive.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Application.DTOs.Identity;

public record RegisterDto(
    string FirstName,
    string LastName,
    string Passport,
    string Address,
    string Email,
    string Password,
    DateTime Birthdate,
    EmployeePosition Position
);
