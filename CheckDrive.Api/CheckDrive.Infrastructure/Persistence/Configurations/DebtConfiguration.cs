using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

internal sealed class DebtConfiguration : IEntityTypeConfiguration<Debt>
{
    public void Configure(EntityTypeBuilder<Debt> builder)
    {
        builder.ToTable(nameof(Debt));
        builder.HasKey(d => d.Id);

        #region Relationships

        builder
            .HasOne(d => d.CheckPoint)
            .WithOne(cp => cp.Debt)
            .HasForeignKey<Debt>(d => d.CheckPointId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        #endregion

        #region Properties

        builder
            .Property(d => d.FuelAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder
            .Property(d => d.PaidAmount)
            .HasPrecision(18, 2)
            .HasDefaultValue(0)
            .IsRequired();

        builder
            .Property(d => d.Status)
            .HasDefaultValue(DebtStatus.Unpaid)
            .IsRequired();

        #endregion
    }
}
