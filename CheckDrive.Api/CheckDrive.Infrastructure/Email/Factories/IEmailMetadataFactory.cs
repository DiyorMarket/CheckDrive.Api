using CheckDrive.Application.Models;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Infrastructure.Email.Factories;

internal interface IEmailMetadataFactory
{
    EmailMetadata Create(
        EmailType emailType,
        string to,
        UserInfo? userInfo = null,
        string? callbackUri = null,
        List<string>? cc = null);
}
