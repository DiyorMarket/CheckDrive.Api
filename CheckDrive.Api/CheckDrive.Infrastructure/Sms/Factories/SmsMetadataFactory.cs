using CheckDrive.Application.Models;
using CheckDrive.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDrive.Infrastructure.Sms.Factories;

internal class SmsMetadataFactory : ISmsMetadataFactory
{
    private readonly Dictionary<SmsType, string> smsSubjects = new()
    {
        { SmsType.NotificationMessage, "Notification" },
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
