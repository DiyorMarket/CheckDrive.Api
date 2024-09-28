using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

internal sealed class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable(nameof(Car));
        builder.HasKey(c => c.Id);

        #region Relationships

        builder
            .HasMany(c => c.Handovers)
            .WithOne(ch => ch.Car)
            .HasForeignKey(ch => ch.CarId);

        #endregion

        #region Properties

        builder.Property(c => c.Model)
               .HasMaxLength(Constants.MAX_STRING_LENGTH)
               .IsRequired();

        builder.Property(c => c.Color)
               .HasMaxLength(Constants.MAX_STRING_LENGTH)
               .IsRequired();

        builder.Property(c => c.Number)
               .HasMaxLength(Constants.CAR_NUMBER_LENGTH)
               .IsRequired();

        builder.Property(c => c.ManufacturedYear)
               .IsRequired();

        builder.Property(c => c.Mileage)
               .IsRequired();

        builder.Property(c => c.YearlyDistanceLimit)
               .IsRequired();

        builder.Property(c => c.AverageFuelConsumption)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(c => c.FuelCapacity)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(c => c.RemainingFuel)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(c => c.Status)
               .HasDefaultValue(CarStatus.Free)
               .IsRequired();

        #endregion
    }
}
