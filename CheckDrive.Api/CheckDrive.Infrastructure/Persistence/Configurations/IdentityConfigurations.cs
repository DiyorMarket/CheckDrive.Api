using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

class IdentityConfigurations(UserManager<IdentityUser>userManager) : IEntityTypeConfiguration<IdentityUser>
{
    private readonly UserManager<IdentityUser> _userManager = userManager
        ?? throw new ArgumentNullException(nameof(userManager));

    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        //Adding defualt admins to register new admins and employees
        builder.HasData(new[]
        {
            CreateUser("CheckDriveDefaultAdmin1@gmail.com","$9Wj2&yQ!k6^Tc1H"),
            CreateUser("CheckDriveDefaultAdmin2@gmail.com","M7@p4#rD2^z3Vq!K"),
            CreateUser("CheckDriveDefaultAdmin3@gmail.com","J8*rF3%hZ1!sQ4+V")
        });
    }
    private async Task<IdentityUser> CreateUser(string email,string password)
    {
        var user = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var createUserResult = await _userManager.CreateAsync(user, password);
        if (!createUserResult.Succeeded)
        {
            throw new RegistrationFailedException((string.Join(", ", createUserResult.Errors.Select(e => e.Description))));
        }

        return user;
    }
}
