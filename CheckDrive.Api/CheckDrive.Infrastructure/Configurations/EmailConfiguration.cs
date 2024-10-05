using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Infrastructure.Configurations;

public class EmailConfigurations
{
    public const string SectionName = nameof(EmailConfigurations);
    
    [Required(ErrorMessage = "From is required")]
    [EmailAddress]
    public required string From { get; init; }
    
    [Required(ErrorMessage = "Server is required")]
    public required string Server { get; init; }
    
    [Required(ErrorMessage = "Port is required")]
    public required int Port { get; init; }
    
    [Required(ErrorMessage = "Username is required")]
    public required string UserName { get; init; }
    
    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; init; }
}
