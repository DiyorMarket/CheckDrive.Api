using CheckDrive.Infrastructure.Models;
using FluentEmail.Core.Models;
using System.Net;

namespace CheckDrive.Infrastructure.Email.Factories;

public class EmailMetadataFactory : IEmailMetadataFactory
{
    private readonly Dictionary<EmailType, string> emailSubjects = new()
    {
        { EmailType.EmailConfirmation, "Email Confirmation" },
        { EmailType.ForgotPassword, "Password Reset" }
    };

    public EmailMetadata Create(
        EmailType emailType,
        string to,
        UserInfo? userInfo = null,
        string? callbackUri = null,
        List<string>? cc = null)
    {
        var subject = emailSubjects[emailType] ?? string.Empty;
        var addresses = cc is not null
            ? cc.Select(x => new Address(x)).ToList()
            : [];

        return new EmailMetadata
        {
            To = to,
            Subject = subject,
            EmailType = emailType,
            ClientUri = callbackUri,
            UserInfo = userInfo,
            CC = addresses
        };
    }
}
