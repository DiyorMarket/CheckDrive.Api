using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

internal sealed class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable(nameof(Car));
    }
}
