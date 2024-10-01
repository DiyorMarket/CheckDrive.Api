using CheckDrive.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Application.DTOs.Identity;

public record RegisterDto(
    string Username,
    string Password,
    string PasswordConfirm,
    string PhoneNumber,
    string? Email,
    string FirstName,
    string LastName,
    string Address,
    string Passport,
    DateTime Birthdate);
