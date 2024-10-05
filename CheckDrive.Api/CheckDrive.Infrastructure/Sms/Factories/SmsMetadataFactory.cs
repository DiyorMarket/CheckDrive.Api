using CheckDrive.Application.Models;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Infrastructure.Sms.Factories;
internal class SmsMetadataFactory : ISmsMetadataFactory
{
    private readonly Dictionary<SmsType, string> smsSubjects = new()
    {
        { SmsType.MessageType, "Message" },
        { SmsType.ForgotPassword, "Password Reset" }
    };
    public SmsMetadata Create(
        SmsType smsType,
        string userName,
        string phoneNumber,
        string message)
    {
        var subject = smsSubjects[smsType] ?? string.Empty;

        return new SmsMetadata
        {
            UserName = userName,
            PhoneNumber = phoneNumber,
            Subject = subject,
            SmsType = smsType,
            Message = message
        };
    }
}
