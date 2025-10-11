using Doctors.Domain.Entities;
using Doctors.Infrastructure.Features.Configurations;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Doctors.Infrastructure.Features.DBContexts;

public class DoctorsDbContext : DbContext
{
	public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Speciality> Specialities { get; set; }

	public DoctorsDbContext(DbContextOptions<DoctorsDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.AddInboxStateEntity();
		modelBuilder.AddOutboxMessageEntity();
		modelBuilder.AddOutboxStateEntity();
         
		modelBuilder.ApplyConfiguration(new DoctorConfiguration());
		modelBuilder.ApplyConfiguration(new SpecialityConfiguration());
	}
}
