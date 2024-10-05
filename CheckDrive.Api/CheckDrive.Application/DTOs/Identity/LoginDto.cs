using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Application.DTOs.Identity;

public sealed record LoginDto(
    string UserName,
    string Password);
