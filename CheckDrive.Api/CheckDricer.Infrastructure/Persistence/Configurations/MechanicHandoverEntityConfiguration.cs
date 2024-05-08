﻿using CheckDriver.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDricer.Infrastructure.Persistence.Configurations
{
    internal class MechanicHandoverEntityConfiguration : IEntityTypeConfiguration<MechanicHandover>
    {
        public void Configure(EntityTypeBuilder<MechanicHandover> builder)
        {
            builder.ToTable(nameof(MechanicHandover));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Comments)
                .HasMaxLength(255);

            builder.Property(x => x.Status)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasOne(m => m.Mechanic)
                .WithMany(x => x.MechanicHandovers)
                .HasForeignKey(m => m.MechanicId)
                .OnDelete(DeleteBehavior.NoAction); 

            builder.HasOne(m => m.Car)
                .WithMany(x => x.MechanicHandovers)
                .HasForeignKey(m => m.CarId)
                .OnDelete(DeleteBehavior.NoAction); 

            builder.HasOne(m => m.Driver)
                .WithMany(x => x.MechanicHandovers)
                .HasForeignKey(m => m.DriverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(m => m.MechanicAcceptances)
                .WithOne(c => c.MechanicHandover)
                .HasForeignKey(c => c.MechanicHandoverId);
        }
    }

}
