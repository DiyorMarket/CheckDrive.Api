using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

internal sealed class MechanicAcceptanceConfiguration : IEntityTypeConfiguration<MechanicAcceptance>
{
    public void Configure(EntityTypeBuilder<MechanicAcceptance> builder)
    {
        builder.ToTable(nameof(MechanicAcceptance));
        builder.HasKey(x => x.Id);

        #region Relationships

        builder
            .HasOne(ma => ma.CheckPoint)
            .WithOne(cp => cp.MechanicAcceptance)
            .HasForeignKey<MechanicAcceptance>(ma => ma.CheckPointId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(ma => ma.Mechanic)
            .WithMany(m => m.Acceptances)
            .HasForeignKey(ma => ma.MechanicId)
            .IsRequired();

        #endregion

        #region Properties

        builder
            .Property(ma => ma.FinalMileage)
            .IsRequired();

        builder
            .Property(ma => ma.RemainingFuelAmount)
            .HasPrecision(18, 2);

        #endregion
    }
}
