using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

internal sealed class DispatcherReviewConfiguration : IEntityTypeConfiguration<DispatcherReview>
{
    public void Configure(EntityTypeBuilder<DispatcherReview> builder)
    {
        builder.ToTable(nameof(DispatcherReview));
        builder.HasKey(dr => dr.Id);

        #region Relationships

        builder
            .HasOne(dr => dr.CheckPoint)
            .WithOne(cp => cp.DispatcherReview)
            .HasForeignKey<DispatcherReview>(dr => dr.CheckPointId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(dr => dr.Dispatcher)
            .WithMany(d => d.Reviews)
            .HasForeignKey(dr => dr.DispatcherId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        #endregion

        #region Properties

        builder
            .Property(dr => dr.FinalMileage)
            .IsRequired();

        builder
            .Property(dr => dr.FuelConsumptionAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder
            .Property(dr => dr.RemainingFuelAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        #endregion
    }
}
