using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ratings.Domain.Entities;

namespace Ratings.Infrastructure.Features.Configurations;

internal sealed class RateableAppointmentConfiguration : IEntityTypeConfiguration<RateableAppointment>
{
	public void Configure(EntityTypeBuilder<RateableAppointment> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.IsRequired();

		builder.Property(x => x.DoctorId)
			.IsRequired();

		builder.Property(x => x.PatientId)
			.IsRequired();

		builder.HasIndex(x => x.Id)
			.IsUnique();
	}
}

