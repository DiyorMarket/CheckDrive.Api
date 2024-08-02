﻿// <auto-generated />
using System;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CheckDrive.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(CheckDriveDbContext))]
    partial class CheckDriveDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CheckDrive.Domain.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Bithdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double>("FuelTankCapacity")
                        .HasColumnType("float");

                    b.Property<int>("ManufacturedYear")
                        .HasColumnType("int");

                    b.Property<double>("MeduimFuelConsumption")
                        .HasColumnType("float");

                    b.Property<int>("Mileage")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("OneYearMediumDistance")
                        .HasColumnType("int");

                    b.Property<double>("RemainingFuel")
                        .HasColumnType("float");

                    b.Property<bool>("isBusy")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Car", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Dispatcher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Dispatcher", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.DispatcherReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DispatcherId")
                        .HasColumnType("int");

                    b.Property<double>("DistanceCovered")
                        .HasColumnType("float");

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<double>("FuelSpended")
                        .HasColumnType("float");

                    b.Property<int>("MechanicAcceptanceId")
                        .HasColumnType("int");

                    b.Property<int>("MechanicHandoverId")
                        .HasColumnType("int");

                    b.Property<int>("MechanicId")
                        .HasColumnType("int");

                    b.Property<int>("OperatorId")
                        .HasColumnType("int");

                    b.Property<int>("OperatorReviewId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("DispatcherId");

                    b.HasIndex("DriverId");

                    b.HasIndex("MechanicAcceptanceId");

                    b.HasIndex("MechanicHandoverId");

                    b.HasIndex("MechanicId");

                    b.HasIndex("OperatorId");

                    b.HasIndex("OperatorReviewId");

                    b.ToTable("DispatcherReview", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Doctor", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.DoctorReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comments")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<bool>("IsHealthy")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("DriverId");

                    b.ToTable("DoctorReview", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Driver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<bool>("isBusy")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Driver", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Mechanic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Mechanic", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.MechanicAcceptance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("Distance")
                        .HasColumnType("float");

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("bit");

                    b.Property<int>("MechanicId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("DriverId");

                    b.HasIndex("MechanicId");

                    b.ToTable("MechanicAcceptance", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.MechanicHandover", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("Distance")
                        .HasColumnType("float");

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<bool>("IsHanded")
                        .HasColumnType("bit");

                    b.Property<int>("MechanicId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("DriverId");

                    b.HasIndex("MechanicId");

                    b.ToTable("MechanicHandover", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.OilMarks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("OilMark")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("OilMarks", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Operator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Operator", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.OperatorReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<bool>("IsGiven")
                        .HasColumnType("bit");

                    b.Property<double>("OilAmount")
                        .HasColumnType("float");

                    b.Property<int>("OilMarkId")
                        .HasColumnType("int");

                    b.Property<int>("OperatorId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("DriverId");

                    b.HasIndex("OilMarkId");

                    b.HasIndex("OperatorId");

                    b.ToTable("OperatorReview", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.UndeliveredMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("ReviewId")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.Property<int>("SendingMessageStatus")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("UndeliveredMessage", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Account", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.Role", "Role")
                        .WithMany("Accounts")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Dispatcher", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.Account", "Account")
                        .WithMany("Dispatchers")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.DispatcherReview", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.Car", "Car")
                        .WithMany("Reviewers")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Dispatcher", "Dispatcher")
                        .WithMany("DispetcherReviews")
                        .HasForeignKey("DispatcherId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Driver", "Driver")
                        .WithMany("DispetcherReviews")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.MechanicAcceptance", "MechanicAcceptance")
                        .WithMany("DispatcherReviews")
                        .HasForeignKey("MechanicAcceptanceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.MechanicHandover", "MechanicHandover")
                        .WithMany("DispatcherReviews")
                        .HasForeignKey("MechanicHandoverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Mechanic", "Mechanic")
                        .WithMany("DispetcherReviews")
                        .HasForeignKey("MechanicId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Operator", "Operator")
                        .WithMany("DispetcherReviews")
                        .HasForeignKey("OperatorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.OperatorReview", "OperatorReview")
                        .WithMany("DispatcherReviews")
                        .HasForeignKey("OperatorReviewId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("Dispatcher");

                    b.Navigation("Driver");

                    b.Navigation("Mechanic");

                    b.Navigation("MechanicAcceptance");

                    b.Navigation("MechanicHandover");

                    b.Navigation("Operator");

                    b.Navigation("OperatorReview");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Doctor", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.Account", "Account")
                        .WithMany("Doctors")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.DoctorReview", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.Doctor", "Doctor")
                        .WithMany("DoctorReviews")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Driver", "Driver")
                        .WithMany("DoctorReviews")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Driver");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Driver", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.Account", "Account")
                        .WithMany("Drivers")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Mechanic", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.Account", "Account")
                        .WithMany("Mechanics")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.MechanicAcceptance", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.Car", "Car")
                        .WithMany("MechanicAcceptance")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Driver", "Driver")
                        .WithMany("MechanicAcceptance")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Mechanic", "Mechanic")
                        .WithMany("MechanicAcceptance")
                        .HasForeignKey("MechanicId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("Driver");

                    b.Navigation("Mechanic");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.MechanicHandover", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.Car", "Car")
                        .WithMany("MechanicHandovers")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Driver", "Driver")
                        .WithMany("MechanicHandovers")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Mechanic", "Mechanic")
                        .WithMany("MechanicHandovers")
                        .HasForeignKey("MechanicId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("Driver");

                    b.Navigation("Mechanic");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Operator", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.Account", "Account")
                        .WithMany("Operators")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.OperatorReview", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.Car", "Car")
                        .WithMany("OperatorReviews")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Driver", "Driver")
                        .WithMany("OperatorReviews")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.OilMarks", "OilMark")
                        .WithMany("OperatorReviews")
                        .HasForeignKey("OilMarkId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Operator", "Operator")
                        .WithMany("OperatorReviews")
                        .HasForeignKey("OperatorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("Driver");

                    b.Navigation("OilMark");

                    b.Navigation("Operator");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Account", b =>
                {
                    b.Navigation("Dispatchers");

                    b.Navigation("Doctors");

                    b.Navigation("Drivers");

                    b.Navigation("Mechanics");

                    b.Navigation("Operators");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Car", b =>
                {
                    b.Navigation("MechanicAcceptance");

                    b.Navigation("MechanicHandovers");

                    b.Navigation("OperatorReviews");

                    b.Navigation("Reviewers");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Dispatcher", b =>
                {
                    b.Navigation("DispetcherReviews");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Doctor", b =>
                {
                    b.Navigation("DoctorReviews");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Driver", b =>
                {
                    b.Navigation("DispetcherReviews");

                    b.Navigation("DoctorReviews");

                    b.Navigation("MechanicAcceptance");

                    b.Navigation("MechanicHandovers");

                    b.Navigation("OperatorReviews");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Mechanic", b =>
                {
                    b.Navigation("DispetcherReviews");

                    b.Navigation("MechanicAcceptance");

                    b.Navigation("MechanicHandovers");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.MechanicAcceptance", b =>
                {
                    b.Navigation("DispatcherReviews");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.MechanicHandover", b =>
                {
                    b.Navigation("DispatcherReviews");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.OilMarks", b =>
                {
                    b.Navigation("OperatorReviews");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Operator", b =>
                {
                    b.Navigation("DispetcherReviews");

                    b.Navigation("OperatorReviews");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.OperatorReview", b =>
                {
                    b.Navigation("DispatcherReviews");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Role", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
