using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Application.DTOs.Identity;

public record LoginDto(
    string Email,
    string Password
);
