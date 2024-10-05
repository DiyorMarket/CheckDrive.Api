using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Infrastructure.Configurations;

public class SmsConfigurations
{
    public const string SectionName = nameof(SmsConfigurations);

    [Required(ErrorMessage = "Sms token is required")]
    [MinLength(10, ErrorMessage = "Possibly invalid token")]
    public required string Token { get; init; }
    [Required(ErrorMessage = "From is required")]
    public required string From { get; init; } = "ATP Garaj";
    [Required(ErrorMessage = "CallbackUrl is required")]
    public required string CallbackUrl { get; init; } = "http://0000.uz/test.php";
    [Required(ErrorMessage = "Token type is required")]
    public required string Token_Type { get; init;}
    [Required(ErrorMessage = "Sms API url is required")]
    public required string ApiUrl { get; init; }
}
