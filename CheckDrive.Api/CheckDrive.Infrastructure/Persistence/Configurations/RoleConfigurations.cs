using CheckDrive.Application.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

class RoleConfigurations : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Roles.Administrator,
                NormalizedName = Roles.Administrator.ToUpper(),
            },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Roles.Driver,
                NormalizedName = Roles.Driver.ToUpper(),
            },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Roles.Doctor,
                NormalizedName = Roles.Doctor.ToUpper(),
            },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Roles.Dispatcher,
                NormalizedName = Roles.Dispatcher.ToUpper(),
            },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Roles.Manager,
                NormalizedName = Roles.Manager.ToUpper(),
            },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Roles.Mechanic,
                NormalizedName = Roles.Mechanic.ToUpper(),
            },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Roles.Operator,
                NormalizedName = Roles.Operator.ToUpper(),
            }
        );
    }
}
