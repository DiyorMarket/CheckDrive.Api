using CheckDrive.Application.Models;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Infrastructure.Sms.Factories;
internal interface ISmsMetadataFactory
{
    SmsMetadata Create(
        SmsType smsType,
        string userName,
        string phoneNumber,
        string message);
}
