using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Application.Configurations;

public sealed class HangfireSettings
{
    public const string SectionName = nameof(HangfireSettings);

    [Required(ErrorMessage = "Hangfire username is required")]
    public required string UserName { get; set; }

    [Required(ErrorMessage = "Hangfire password is required")]
    public required string Password { get; set; }
}
