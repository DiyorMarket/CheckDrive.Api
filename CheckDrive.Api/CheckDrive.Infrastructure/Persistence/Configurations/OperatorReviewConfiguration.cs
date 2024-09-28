using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

internal sealed class OperatorReviewConfiguration : IEntityTypeConfiguration<OperatorReview>
{
    public void Configure(EntityTypeBuilder<OperatorReview> builder)
    {
        builder.ToTable(nameof(OperatorReview));
        builder.HasKey(or => or.Id);

        #region Relationships

        builder
            .HasOne(or => or.CheckPoint)
            .WithOne(cp => cp.OperatorReview)
            .HasForeignKey<OperatorReview>(or => or.CheckPointId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(or => or.OilMark)
            .WithMany(om => om.Reviews)
            .HasForeignKey(or => or.OilMarkId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(or => or.Operator)
            .WithMany(o => o.Reviews)
            .HasForeignKey(or => or.OperatorId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        #endregion

        #region Properties

        builder
            .Property(or => or.InitialOilAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder
            .Property(or => or.OilRefillAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        #endregion
    }
}
