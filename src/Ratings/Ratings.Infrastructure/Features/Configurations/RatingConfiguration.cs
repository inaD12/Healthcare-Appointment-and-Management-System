using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ratings.Domain.Entities;

namespace Ratings.Infrastructure.Features.Configurations;

internal sealed class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
	public void Configure(EntityTypeBuilder<Rating> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(x => x.Score)
			.IsRequired();

		builder.Property(x => x.Comment)
			.HasMaxLength(1000);

		builder.HasIndex(x => x.AppointmentId)
			.IsUnique();

		builder.HasIndex(x => x.DoctorId);
		builder.HasIndex(x => x.PatientId);
	}
}

