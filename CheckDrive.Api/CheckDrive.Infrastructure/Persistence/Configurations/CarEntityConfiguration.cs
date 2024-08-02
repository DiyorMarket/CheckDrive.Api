﻿using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations
{
    internal class CarEntityConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.ToTable("Car");
            builder.HasKey(c => c.Id);

            builder.Property(x => x.isBusy)
                .IsRequired();

            builder.Property(x => x.Model)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Color)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Number)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.Mileage)
                .IsRequired();

            builder.Property(x => x.OneYearMediumDistance)
                .IsRequired();

            builder.Property(x => x.ManufacturedYear)
                .IsRequired();

            builder.Property(x => x.MeduimFuelConsumption)
                .IsRequired();

            builder.Property(x => x.FuelTankCapacity)
                .IsRequired();

            builder.Property(x => x.RemainingFuel)
                .IsRequired();

            builder.HasMany(c => c.MechanicHandovers)
                .WithOne(m => m.Car)
                .HasForeignKey(m => m.CarId);
        }
    }
}
