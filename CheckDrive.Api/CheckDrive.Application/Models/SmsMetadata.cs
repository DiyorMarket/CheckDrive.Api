namespace CheckDrive.Application.Models;

public class SmsMetadata(string userName, string phoneNumber, string message)
{
    public string UserName { get; init; } = userName;
    public string PhoneNumber { get; init; } = phoneNumber;
    public string Message { get; init; } = message;
}
