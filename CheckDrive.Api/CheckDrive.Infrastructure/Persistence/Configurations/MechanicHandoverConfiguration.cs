using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

internal sealed class MechanicHandoverConfiguration : IEntityTypeConfiguration<MechanicHandover>
{
    public void Configure(EntityTypeBuilder<MechanicHandover> builder)
    {
        builder.ToTable(nameof(MechanicHandover));
        builder.HasKey(mh => mh.Id);

        #region Relationships

        builder
            .HasOne(mh => mh.CheckPoint)
            .WithOne(cp => cp.MechanicHandover)
            .HasForeignKey<MechanicHandover>(mh => mh.CheckPointId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(mh => mh.Car)
            .WithMany(c => c.Handovers)
            .HasForeignKey(mh => mh.CarId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(mh => mh.Mechanic)
            .WithMany(m => m.Handovers)
            .HasForeignKey(mh => mh.MechanicId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        #endregion

        #region Properties

        builder
            .Property(mh => mh.InitialMileage)
            .IsRequired();

        #endregion
    }
}
