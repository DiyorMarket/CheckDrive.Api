using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

internal sealed class CheckPointConfiguration : IEntityTypeConfiguration<CheckPoint>
{
    public void Configure(EntityTypeBuilder<CheckPoint> builder)
    {
        builder.ToTable(nameof(CheckPoint));
        builder.HasKey(x => x.Id);

        #region Relationships

        builder
            .HasOne(cp => cp.DoctorReview)
            .WithOne(dr => dr.CheckPoint)
            .HasForeignKey<DoctorReview>(dr => dr.CheckPointId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(cp => cp.MechanicHandover)
            .WithOne(mh => mh.CheckPoint)
            .HasForeignKey<MechanicHandover>(mh => mh.CheckPointId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(cp => cp.OperatorReview)
            .WithOne(or => or.CheckPoint)
            .HasForeignKey<OperatorReview>(or => or.CheckPointId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(cp => cp.MechanicAcceptance)
            .WithOne(ma => ma.CheckPoint)
            .HasForeignKey<MechanicAcceptance>(ma => ma.CheckPointId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(cp => cp.DispatcherReview)
            .WithOne(mr => mr.CheckPoint)
            .HasForeignKey<DispatcherReview>(mr => mr.CheckPointId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(cp => cp.Debt)
            .WithOne(d => d.CheckPoint)
            .HasForeignKey<Debt>(d => d.CheckPointId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        #endregion

        #region Properties

        builder
            .Property(cp => cp.Status)
            .HasDefaultValue(CheckPointStatus.InProgress)
            .IsRequired();

        builder
            .Property(cp => cp.Stage)
            .HasDefaultValue(CheckPointStage.DoctorReview)
            .IsRequired();

        #endregion
    }
}
