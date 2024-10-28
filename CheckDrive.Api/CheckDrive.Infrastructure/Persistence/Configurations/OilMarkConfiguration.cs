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

        #region Default Data

        builder.HasData(
            new OilMark
            {
                Id = 1,
                Name = "80"
            },
            new OilMark
            {
                Id = 2,
                Name = "85"
            },
            new OilMark
            {
                Id = 3,
                Name = "90"
            },
            new OilMark
            {
                Id = 4,
                Name = "95"
            },
            new OilMark
            {
                Id = 5,
                Name = "100"
            });

        #endregion
    }
}
