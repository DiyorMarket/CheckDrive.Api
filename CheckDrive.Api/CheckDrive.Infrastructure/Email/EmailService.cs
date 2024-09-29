using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Models;
using CheckDrive.Domain.Enums;
using FluentEmail.Core;

namespace CheckDrive.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    private readonly Dictionary<EmailType, string> templateNames = new()
    {
       { EmailType.EmailConfirmation, "EmailConfirmationTemplate.cshtml" },
       { EmailType.ForgotPassword, "ForgotPasswordTemplate.cshtml" }
    };

    private readonly IFluentEmail _fluentEmail;

    public EmailService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail ?? throw new ArgumentNullException(nameof(fluentEmail));
    }

    public async Task SendAsync(EmailMetadata metadata)
    {
        var template = GetTemplate(metadata.EmailType);

        await _fluentEmail
            .To(metadata.To)
            .Subject(metadata.Subject)
            .UsingTemplateFromFile(template, metadata)
            .SendAsync();
    }

    private string GetTemplate(EmailType emailType)
    {
        string templateName = templateNames[emailType]
            ?? throw new ArgumentOutOfRangeException(nameof(emailType));

        return Path.Combine(AppContext.BaseDirectory, "Email/Templates", templateName);
    }
}
