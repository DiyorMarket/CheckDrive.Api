using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.Models;

public class SmsMetadata()
{
    public string UserName { get; init; }
    public string PhoneNumber { get; init; }
    public string? Subject { get; init; }
    public string? Code { get; init; }
    public SmsType SmsType { get; init; }
    public string Message { get; init; }
}
