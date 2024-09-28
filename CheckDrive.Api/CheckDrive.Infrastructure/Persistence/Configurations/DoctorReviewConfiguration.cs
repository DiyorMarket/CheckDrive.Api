using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

internal sealed class DoctorReviewConfiguration : IEntityTypeConfiguration<DoctorReview>
{
    public void Configure(EntityTypeBuilder<DoctorReview> builder)
    {
        builder.ToTable(nameof(DoctorReview));
        builder.HasKey(dr => dr.Id);

        #region Relationships

        builder
            .HasOne(dr => dr.CheckPoint)
            .WithOne(cp => cp.DoctorReview)
            .HasForeignKey<DoctorReview>(dr => dr.CheckPointId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(dr => dr.Driver)
            .WithMany(d => d.Reviews)
            .HasForeignKey(dr => dr.DriverId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(dr => dr.Doctor)
            .WithMany(d => d.Reviews)
            .HasForeignKey(dr => dr.DoctorId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        #endregion


    }
}
