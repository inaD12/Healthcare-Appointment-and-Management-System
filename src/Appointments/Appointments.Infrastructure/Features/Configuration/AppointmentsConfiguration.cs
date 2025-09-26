using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.Entities;

namespace Appointments.Infrastructure.Features.Configuration;

internal sealed class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasIndex(a => a.Id).IsUnique();
			
        builder.OwnsOne(a => a.Duration, duration =>
        {
            duration.Property(d => d.Start).HasColumnName("ScheduledStartTime");
            duration.Property(d => d.End).HasColumnName("ScheduledEndTime");
        });
    }
}
