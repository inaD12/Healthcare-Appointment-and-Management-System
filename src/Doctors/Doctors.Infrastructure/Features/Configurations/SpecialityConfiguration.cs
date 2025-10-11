using Doctors.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctors.Infrastructure.Features.Configurations;

internal class SpecialityConfiguration : IEntityTypeConfiguration<Speciality>
{
	public void Configure(EntityTypeBuilder<Speciality> builder)
	{
        builder.HasIndex(s => s.Name).IsUnique();
	}
}
