using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations
{
    public class OilMarkConfiguration : IEntityTypeConfiguration<OilMarks>
    {
        public void Configure(EntityTypeBuilder<OilMarks> builder)
        {
            builder.ToTable(nameof(Role));
            builder.HasKey(x => x.Id);


            builder.Property(x => x.OilMark)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasMany(r => r.OperatorReviews)
                .WithOne(a => a.OilMark);
        }
    }
}
