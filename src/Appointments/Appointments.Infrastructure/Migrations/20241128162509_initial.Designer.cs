﻿// <auto-generated />
using Appointments.Domain.Enums;
using Appointments.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Shared.Domain.Enums;

#nullable disable

namespace Appointments.Infrastructure.Migrations
{
	[DbContext(typeof(AppointmentsDBContext))]
    [Migration("20241128162509_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "appointmentstatus", new[] { "cancelled", "completed", "rescheduled", "scheduled" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "roles", new[] { "admin", "doctor", "patient" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Appointments.Domain.Entities.Appointment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("DoctorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ScheduledEndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ScheduledStartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<AppointmentStatus>("Status")
                        .HasColumnType("appointmentstatus");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("Appointments.Domain.Entities.UserData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Roles>("Role")
                        .HasColumnType("roles");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("UserData");
                });
#pragma warning restore 612, 618
        }
    }
}
