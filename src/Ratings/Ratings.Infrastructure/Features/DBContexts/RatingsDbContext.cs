using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ratings.Domain.Entities;
using Ratings.Infrastructure.Features.Configurations;

namespace Ratings.Infrastructure.Features.DBContexts;

public sealed class RatingsDbContext(DbContextOptions<RatingsDbContext> options) : DbContext(options)
{
	public DbSet<Rating> Ratings => Set<Rating>();
	public DbSet<DoctorRatingStats> DoctorRatingStats => Set<DoctorRatingStats>();
	public DbSet<RateableAppointment> RateableAppointments => Set<RateableAppointment>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.AddInboxStateEntity();
		modelBuilder.AddOutboxMessageEntity();
		modelBuilder.AddOutboxStateEntity();
		
		modelBuilder.ApplyConfiguration(new RatingConfiguration());
		modelBuilder.ApplyConfiguration(new DoctorRatingStatsConfiguration());
		modelBuilder.ApplyConfiguration(new RateableAppointmentConfiguration());
	}
}
