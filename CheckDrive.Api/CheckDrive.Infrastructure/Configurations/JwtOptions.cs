using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Infrastructure.Configurations;

public class JwtOptions
{
    public const string SectionName = nameof(JwtOptions);

    [Required(ErrorMessage = "Secret Key is required.")]
    public required string SecretKey { get; init; }

    [Required(ErrorMessage = "Expiration time is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Expiration time must be greater than 1 hour.")]
    public int ExpiresInHours { get; init; }
}
