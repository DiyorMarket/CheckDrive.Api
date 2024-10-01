using CheckDrive.Application.Models;

namespace CheckDrive.Application.Interfaces;

public interface ISmsService
{
    Task SendAsync(SmsMetadata metadata);
}
