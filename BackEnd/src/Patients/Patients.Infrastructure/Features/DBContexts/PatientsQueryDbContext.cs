using Microsoft.EntityFrameworkCore;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.Configurations;

namespace Patients.Infrastructure.Features.DBContexts;

public sealed class PatientsQueryDbContext(DbContextOptions<PatientsQueryDbContext> options) : DbContext(options)
{
	public DbSet<Patient> Patients => Set<Patient>();
	public DbSet<Encounter> Encounters => Set<Encounter>();
	public DbSet<AppointmentProjection> AppointmentProjections => Set<AppointmentProjection>();


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new PatientConfiguration());
		modelBuilder.ApplyConfiguration(new EncounterConfiguration());
		modelBuilder.ApplyConfiguration(new AppointmentProjectionConfiguration());
	}
}
