using CheckDrive.Application.Models;

namespace CheckDrive.Application.Interfaces;

public interface IEmailService
{
    Task SendAsync(EmailMetadata metadata);
}
