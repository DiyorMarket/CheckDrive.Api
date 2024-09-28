using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

internal sealed class OilMarkConfiguration : IEntityTypeConfiguration<OilMark>
{
    public void Configure(EntityTypeBuilder<OilMark> builder)
    {
        builder.ToTable(nameof(OilMark));
        builder.HasKey(x => x.Id);

        #region Relationships

        builder
            .HasMany(o => o.Reviews)
            .WithOne(r => r.OilMark)
            .HasForeignKey(r => r.OilMarkId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        #endregion

        #region Properties

        builder
            .Property(o => o.Name)
            .HasMaxLength(Constants.DEFAULT_STRING_LENGTH)
            .IsRequired();

        #endregion
    }
}
