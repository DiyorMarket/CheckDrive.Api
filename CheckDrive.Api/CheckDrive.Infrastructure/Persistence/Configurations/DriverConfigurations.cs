using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

internal sealed class DriverConfigurations : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.Property(x => x.Status)
            .HasDefaultValue(DriverStatus.Available)
            .IsRequired();
    }
}
