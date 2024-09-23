using CheckDrive.Domain.Entities.Identity;

namespace CheckDrive.Application.Interfaces;

public interface ICurrentUserService
{
    User GetCurrentUser();
    Guid GetCurrentUserId();
}
