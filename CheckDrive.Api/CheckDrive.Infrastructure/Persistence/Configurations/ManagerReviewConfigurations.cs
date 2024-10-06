using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

internal class ManagerReviewConfigurations : IEntityTypeConfiguration<ManagerReview>
{
    public void Configure(EntityTypeBuilder<ManagerReview> builder)
    {
        builder.ToTable(nameof(ManagerReview));
        builder.HasKey(mn => mn.Id);

        #region Relationships

        builder
            .HasOne(mn => mn.CheckPoint)
            .WithOne(cp => cp.ManagerReview)
            .HasForeignKey<ManagerReview>(mn => mn.CheckPointId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(mn => mn.Manager)
            .WithMany(d => d.Reviews)
            .HasForeignKey(mn => mn.ManagerId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        #endregion

        #region Properties

        builder
            .Property(mn => mn.DebtAmountAdjusment)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder
            .Property(mn => mn.FuelConsumptionAdjustment)
            .HasPrecision(18, 2)
            .IsRequired(false);

        #endregion
    }
}
