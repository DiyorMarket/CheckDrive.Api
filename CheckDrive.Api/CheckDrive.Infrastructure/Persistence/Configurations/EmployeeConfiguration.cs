﻿using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations;

internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable(nameof(Employee));
        builder.HasKey(e => e.Id);

        #region Relationships

        builder
            .HasOne(e => e.Account)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        #endregion

        #region Properties

        builder
            .HasDiscriminator(x => x.Position)
            .HasValue<Employee>(EmployeePosition.Base)
            .HasValue<Driver>(EmployeePosition.Driver)
            .HasValue<Doctor>(EmployeePosition.Doctor)
            .HasValue<Mechanic>(EmployeePosition.Mechanic)
            .HasValue<Operator>(EmployeePosition.Operator)
            .HasValue<Dispatcher>(EmployeePosition.Dispatcher);

        builder
            .Property(e => e.FirstName)
            .HasMaxLength(Constants.DEFAULT_STRING_LENGTH)
            .IsRequired();

        builder
            .Property(e => e.LastName)
            .HasMaxLength(Constants.DEFAULT_STRING_LENGTH)
            .IsRequired();

        builder
            .Property(e => e.Passport)
            .HasMaxLength(20)
            .IsRequired();

        builder
            .Property(e => e.Address)
            .HasMaxLength(Constants.MAX_STRING_LENGTH)
            .IsRequired();

        builder
            .Property(e => e.Birthdate)
            .IsRequired();

        #endregion
    }
}
