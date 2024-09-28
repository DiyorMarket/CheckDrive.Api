using FluentEmail.Core.Models;

namespace CheckDrive.Infrastructure.Models;

public class EmailMetadata
{
    public required string To { get; init; }
    public string? Subject { get; init; }
    public List<Address> CC { get; init; }
    public EmailType EmailType { get; init; }
    public UserInfo? UserInfo { get; init; }
    public string? ClientUri { get; init; }
    public EmailMetadata()
    {
        CC = [];
    }
}
