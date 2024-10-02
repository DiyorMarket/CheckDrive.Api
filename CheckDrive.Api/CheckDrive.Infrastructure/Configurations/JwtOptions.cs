using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Infrastructure.Configurations;

public class JwtOptions
{
    public const string SectionName = nameof(JwtOptions);

    [Required(ErrorMessage = "Secret Key is required")]
    public required string SecretKey { get; init; }
    public int ExpiresHours { get; init; }
}
