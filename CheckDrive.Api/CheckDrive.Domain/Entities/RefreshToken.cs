using CheckDrive.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Domain.Entities;

public class RefreshToken : EntityBase
{
    public required string Token { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public bool IsRevoked { get; set; }

    public required string UserId { get; set; }
    public required IdentityUser User { get; set; }
}
