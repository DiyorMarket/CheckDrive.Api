using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

internal sealed class ManagerReviewConfiguration : IEntityTypeConfiguration<ManagerReview>
{
    public void Configure(EntityTypeBuilder<ManagerReview> builder)
    {
        builder.ToTable(nameof(ManagerReview));
        builder.HasKey(mn => mn.Id);

        #region Relationships

        builder
            .HasOne(ma => ma.CheckPoint)
            .WithOne(cp => cp.ManagerReview)
            .HasForeignKey<ManagerReview>(ma => ma.CheckPointId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(dr => dr.Manager)
            .WithMany(d => d.Reviews)
            .HasForeignKey(dr => dr.ManagerId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        #endregion
    }
}
