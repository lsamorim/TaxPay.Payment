﻿// <auto-generated />
using System;
using Infrastructure.SqlServer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.SqlServer.Migrations
{
    [DbContext(typeof(PaymentContext))]
    partial class PaymentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.ScheduleAggregate.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("ScheduleDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("Domain.ScheduleAggregate.Schedule", b =>
                {
                    b.OwnsOne("Domain.ScheduleAggregate.ValueObjects.BankAccount", "BankAccount", b1 =>
                        {
                            b1.Property<int>("ScheduleId")
                                .HasColumnType("int");

                            b1.Property<string>("AccountNumber")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("AccountNumber");

                            b1.Property<string>("BranchNumber")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("BranchNumber");

                            b1.Property<string>("InstitutionNumber")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("InstitutionNumber");

                            b1.HasKey("ScheduleId");

                            b1.ToTable("Schedules");

                            b1.WithOwner()
                                .HasForeignKey("ScheduleId");
                        });

                    b.Navigation("BankAccount")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
