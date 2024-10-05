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
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CheckDrive.Domain.Entities.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AverageFuelConsumption")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<decimal>("FuelCapacity")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ManufacturedYear")
                        .HasColumnType("int");

                    b.Property<int>("Mileage")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<decimal>("RemainingFuel")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("YearlyDistanceLimit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Car", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.CheckPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Notes")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("Stage")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.ToTable("CheckPoint", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Debt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CheckPointId")
                        .HasColumnType("int");

                    b.Property<decimal>("FuelAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PaidAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("Id");

                    b.HasIndex("CheckPointId")
                        .IsUnique();

                    b.ToTable("Debt", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.DispatcherReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CheckPointId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DispatcherId")
                        .HasColumnType("int");

                    b.Property<decimal?>("DistanceTravelledAdjustment")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("FuelConsumptionAdjustment")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CheckPointId")
                        .IsUnique();

                    b.HasIndex("DispatcherId");

                    b.ToTable("DispatcherReview", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.DoctorReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CheckPointId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CheckPointId")
                        .IsUnique();

                    b.HasIndex("DoctorId");

                    b.HasIndex("DriverId");

                    b.ToTable("DoctorReview", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Passport")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("Employee", (string)null);

                    b.HasDiscriminator<int>("Position").HasValue(0);

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.ManagerReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CheckPointId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("ManagerId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CheckPointId")
                        .IsUnique();

                    b.HasIndex("ManagerId");

                    b.ToTable("ManagerReview", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.MechanicAcceptance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CheckPointId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("FinalMileage")
                        .HasColumnType("int");

                    b.Property<int>("MechanicId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("RemainingFuelAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CheckPointId")
                        .IsUnique();

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

                    b.Property<int>("CheckPointId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("InitialMileage")
                        .HasColumnType("int");

                    b.Property<int>("MechanicId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("CheckPointId")
                        .IsUnique();

                    b.HasIndex("MechanicId");

                    b.ToTable("MechanicHandover", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.OilMark", b =>
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

                    b.ToTable("OilMark", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.OperatorReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CheckPointId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("InitialOilAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OilMarkId")
                        .HasColumnType("int");

                    b.Property<decimal>("OilRefillAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("OperatorId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CheckPointId")
                        .IsUnique();

                    b.HasIndex("OilMarkId");

                    b.HasIndex("OperatorId");

                    b.ToTable("OperatorReview", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Role", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "1eef2d65-63aa-4bd3-ad11-97b05465411a",
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR"
                        },
                        new
                        {
                            Id = "50732e08-b22e-4d0c-8196-13a14fda4edb",
                            Name = "Driver",
                            NormalizedName = "DRIVER"
                        },
                        new
                        {
                            Id = "95e28700-5bea-4855-bda5-4fe4660dbaaa",
                            Name = "Doctor",
                            NormalizedName = "DOCTOR"
                        },
                        new
                        {
                            Id = "b8854b49-d887-4ac9-9ffe-bc391909380c",
                            Name = "Dispatcher",
                            NormalizedName = "DISPATCHER"
                        },
                        new
                        {
                            Id = "099bbfb1-a22a-48f8-a707-bd954851b749",
                            Name = "Manager",
                            NormalizedName = "MANAGER"
                        },
                        new
                        {
                            Id = "78333ddd-03ba-4274-ae24-321c3563584a",
                            Name = "Mechanic",
                            NormalizedName = "MECHANIC"
                        },
                        new
                        {
                            Id = "2f72a51b-f05d-49b5-98fc-c2c1b32219e1",
                            Name = "Operator",
                            NormalizedName = "OPERATOR"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaim", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaim", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogin", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserToken", (string)null);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Dispatcher", b =>
                {
                    b.HasBaseType("CheckDrive.Domain.Entities.Employee");

                    b.HasDiscriminator().HasValue(5);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Doctor", b =>
                {
                    b.HasBaseType("CheckDrive.Domain.Entities.Employee");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Driver", b =>
                {
                    b.HasBaseType("CheckDrive.Domain.Entities.Employee");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Manager", b =>
                {
                    b.HasBaseType("CheckDrive.Domain.Entities.Employee");

                    b.HasDiscriminator().HasValue(6);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Mechanic", b =>
                {
                    b.HasBaseType("CheckDrive.Domain.Entities.Employee");

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Operator", b =>
                {
                    b.HasBaseType("CheckDrive.Domain.Entities.Employee");

                    b.HasDiscriminator().HasValue(4);
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Debt", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.CheckPoint", "CheckPoint")
                        .WithOne("Debt")
                        .HasForeignKey("CheckDrive.Domain.Entities.Debt", "CheckPointId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CheckPoint");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.DispatcherReview", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.CheckPoint", "CheckPoint")
                        .WithOne("DispatcherReview")
                        .HasForeignKey("CheckDrive.Domain.Entities.DispatcherReview", "CheckPointId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Dispatcher", "Dispatcher")
                        .WithMany("Reviews")
                        .HasForeignKey("DispatcherId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CheckPoint");

                    b.Navigation("Dispatcher");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.DoctorReview", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.CheckPoint", "CheckPoint")
                        .WithOne("DoctorReview")
                        .HasForeignKey("CheckDrive.Domain.Entities.DoctorReview", "CheckPointId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Doctor", "Doctor")
                        .WithMany("Reviews")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Driver", "Driver")
                        .WithMany("Reviews")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CheckPoint");

                    b.Navigation("Doctor");

                    b.Navigation("Driver");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Employee", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "Account")
                        .WithOne()
                        .HasForeignKey("CheckDrive.Domain.Entities.Employee", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.ManagerReview", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.CheckPoint", "CheckPoint")
                        .WithOne("ManagerReview")
                        .HasForeignKey("CheckDrive.Domain.Entities.ManagerReview", "CheckPointId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Manager", "Manager")
                        .WithMany("Reviews")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CheckPoint");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.MechanicAcceptance", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.CheckPoint", "CheckPoint")
                        .WithOne("MechanicAcceptance")
                        .HasForeignKey("CheckDrive.Domain.Entities.MechanicAcceptance", "CheckPointId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Mechanic", "Mechanic")
                        .WithMany("Acceptances")
                        .HasForeignKey("MechanicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CheckPoint");

                    b.Navigation("Mechanic");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.MechanicHandover", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.Car", "Car")
                        .WithMany("Handovers")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.CheckPoint", "CheckPoint")
                        .WithOne("MechanicHandover")
                        .HasForeignKey("CheckDrive.Domain.Entities.MechanicHandover", "CheckPointId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Mechanic", "Mechanic")
                        .WithMany("Handovers")
                        .HasForeignKey("MechanicId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("CheckPoint");

                    b.Navigation("Mechanic");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.OperatorReview", b =>
                {
                    b.HasOne("CheckDrive.Domain.Entities.CheckPoint", "CheckPoint")
                        .WithOne("OperatorReview")
                        .HasForeignKey("CheckDrive.Domain.Entities.OperatorReview", "CheckPointId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.OilMark", "OilMark")
                        .WithMany("Reviews")
                        .HasForeignKey("OilMarkId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CheckDrive.Domain.Entities.Operator", "Operator")
                        .WithMany("Reviews")
                        .HasForeignKey("OperatorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CheckPoint");

                    b.Navigation("OilMark");

                    b.Navigation("Operator");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Car", b =>
                {
                    b.Navigation("Handovers");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.CheckPoint", b =>
                {
                    b.Navigation("Debt");

                    b.Navigation("DispatcherReview");

                    b.Navigation("DoctorReview")
                        .IsRequired();

                    b.Navigation("ManagerReview");

                    b.Navigation("MechanicAcceptance");

                    b.Navigation("MechanicHandover");

                    b.Navigation("OperatorReview");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.OilMark", b =>
                {
                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Dispatcher", b =>
                {
                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Doctor", b =>
                {
                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Driver", b =>
                {
                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Manager", b =>
                {
                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Mechanic", b =>
                {
                    b.Navigation("Acceptances");

                    b.Navigation("Handovers");
                });

            modelBuilder.Entity("CheckDrive.Domain.Entities.Operator", b =>
                {
                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
