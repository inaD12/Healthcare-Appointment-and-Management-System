using Doctors.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctors.Infrastructure.Features.Configurations;

internal class SpecialityConfiguration : IEntityTypeConfiguration<Speciality>
{
	public void Configure(EntityTypeBuilder<Speciality> builder)
	{
        builder
	        .HasIndex(s => s.Name).IsUnique();
      
        builder
	        .HasIndex(s => s.Name)
	        .IsUnique();

        builder
	        .Property(s => s.Embedding)
	        .HasColumnType("vector(1024)");

        builder
	        .HasIndex(s => s.Embedding)
	        .HasMethod("hnsw")
	        .HasOperators("vector_l2_ops")
	        .HasStorageParameter("m", 16)
	        .HasStorageParameter("ef_construction", 64);
	}
}
