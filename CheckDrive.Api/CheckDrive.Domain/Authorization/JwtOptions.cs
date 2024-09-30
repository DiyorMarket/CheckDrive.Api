using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Domain.Authorization;
public class JwtOptions
{
    public const string SectionName = nameof(JwtOptions);

    [Required(ErrorMessage = "Secret Key is required")]
    public required string SecretKey { get; init; }
    public int ExpiresHours { get; init; }
}
