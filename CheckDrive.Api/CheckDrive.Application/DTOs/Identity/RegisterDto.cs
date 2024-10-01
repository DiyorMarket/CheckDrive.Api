using CheckDrive.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Application.DTOs.Identity;

public record RegisterDto(
    string Email,
    string Password
);
