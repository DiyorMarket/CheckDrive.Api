using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDrive.Infrastructure.Configurations;

public class EmailConfigurations
{
    public const string SectionName = nameof(EmailConfigurations);

    [Required]
    [EmailAddress]
    public required string From { get; set; }

    [Required]
    public required string Server { get; set; }

    [Required]
    public required int Port { get; set; }

    [Required]
    public required string UserName { get; set; }

    [Required]
    public required string Password { get; set; }
}
