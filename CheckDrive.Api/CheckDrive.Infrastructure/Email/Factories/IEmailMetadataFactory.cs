using CheckDrive.Infrastructure.Models;

namespace CheckDrive.Infrastructure.Email.Factories;

public interface IEmailMetadataFactory
{
    EmailMetadata Create(
        EmailType emailType,
        string to,
        UserInfo? userInfo = null,
        string? callbackUri = null,
        List<string>? cc = null );
}
