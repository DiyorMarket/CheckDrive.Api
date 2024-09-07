using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations
{
    internal class DebtsEntityConfiguration : IEntityTypeConfiguration<Debts>
    {
        public void Configure(EntityTypeBuilder<Debts> builder)
        {
            builder.ToTable(nameof(Debts));

            builder.HasKey(d => d.Id);

            builder.Property(x => x.OilAmount)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            builder.HasOne(a => a.Driver)
                .WithMany(x => x.Debts)
                .HasForeignKey(a => a.DriverId);

            builder.HasOne(a => a.Car)
                .WithMany(x => x.Debts)
                .HasForeignKey(a => a.CarId);
        }
    }
}
