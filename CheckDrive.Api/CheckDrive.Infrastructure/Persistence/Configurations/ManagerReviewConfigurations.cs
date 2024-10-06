using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

internal class ManagerReviewConfigurations : IEntityTypeConfiguration<ManagerReview>
{
    public void Configure(EntityTypeBuilder<ManagerReview> builder)
    {
        builder.ToTable(nameof(ManagerReview));
        builder.HasKey(mr => mr.Id);

        #region Relationships

        builder
            .HasOne(mr => mr.CheckPoint)
            .WithOne(cp => cp.ManagerReview)
            .HasForeignKey<ManagerReview>(mr => mr.CheckPointId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(mr => mr.Manager)
            .WithMany(m => m.Reviews)
            .HasForeignKey(mr => mr.ManagerId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(mr => mr.Debt)
            .WithOne(d => d.ManagerReview)
            .HasForeignKey<ManagerReview>(mr => mr.ManagerId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        #endregion

        #region Properties

        builder
            .Property(mr => mr.DebtAmountAdjusment)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder
            .Property(mr => mr.FuelConsumptionAdjustment)
            .HasPrecision(18, 2)
            .IsRequired(false);

        #endregion
    }
}
