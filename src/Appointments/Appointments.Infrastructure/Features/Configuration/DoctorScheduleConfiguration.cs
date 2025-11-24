using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointments.Infrastructure.Features.Configuration;


internal class DoctorScheduleConfiguration : IEntityTypeConfiguration<DoctorSchedule>
{
    public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
    {
        builder.ToTable("DoctorSchedules");
        builder.HasKey(d => d.DoctorId);

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
                    wt.Property(t => t.Start).IsRequired();
                    wt.Property(t => t.End).IsRequired();
                });

            });
        });

        builder.OwnsMany(d => d.AvailabilityExceptions, a =>
        {
            a.ToTable("DoctorAvailabilityExceptions");
            a.WithOwner().HasForeignKey("DoctorId");

            a.Property(x => x.Start).IsRequired();
            a.Property(x => x.End).IsRequired();

            a.Property(x => x.Reason).HasMaxLength(300);
            a.Property(x => x.Type).IsRequired();
        });
    }
}
