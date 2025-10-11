using Doctors.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctors.Infrastructure.Features.Configurations;

internal class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
	public void Configure(EntityTypeBuilder<Doctor> builder)
	{
        builder.ToTable("Doctors");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.UserId)
            .IsRequired();

        builder.Property(d => d.TimeZoneId)
            .IsRequired();
                
        builder.HasMany(d => d.Specialities)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "DoctorSpecialities",
                right => right.HasOne<Speciality>()
                    .WithMany()
                    .HasForeignKey("SpecialityId"),
                left => left.HasOne<Doctor>()
                    .WithMany()
                    .HasForeignKey("DoctorId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("DoctorSpecialities");
                    join.HasKey("DoctorId", "SpecialityId");
                    
                    join.HasIndex("SpecialityId");
                });


        builder.OwnsOne(d => d.WeeklySchedule, schedule =>
        {
            schedule.ToTable("WeeklySchedules");
            schedule.OwnsMany(w => w.WorkDays, wd =>
            {
                wd.ToTable("WorkDays");
                wd.WithOwner().HasForeignKey("WeeklyScheduleId");

                wd.Property(w => w.DayOfWeek)
                    .IsRequired();

                wd.OwnsMany(w => w.WorkTimes, wt =>
                {
                    wt.ToTable("WorkTimeRanges");
                    wt.WithOwner().HasForeignKey("WorkDayId");
                    wt.Property(p => p.Start)
                        .IsRequired();
                    wt.Property(p => p.End)
                        .IsRequired();
                });
            });
        });

        builder.OwnsMany(d => d.AvailabilityExceptions, a =>
        {
            a.ToTable("DoctorAvailabilityExceptions");
            a.WithOwner().HasForeignKey("DoctorId");

            a.Property(x => x.Reason)
                .HasMaxLength(300);

            a.Property(x => x.Type)
                .IsRequired();

            a.OwnsOne(x => x.Range, range =>
            {
                range.Property(r => r.Start)
                    .HasColumnName("Start")
                    .IsRequired();
                range.Property(r => r.End)
                    .HasColumnName("End")
                    .IsRequired();
            });
        });
	}
}
