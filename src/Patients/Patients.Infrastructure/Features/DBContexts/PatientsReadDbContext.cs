using Microsoft.EntityFrameworkCore;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.Configurations;
using Patients.Infrastructure.Features.ReadModels;

namespace Patients.Infrastructure.Features.DBContexts;

public sealed class PatientsReadDbContext(DbContextOptions<PatientsReadDbContext> options) : DbContext(options)
{
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Encounter> Encounters => Set<Encounter>();
    public DbSet<AppointmentProjection> AppointmentProjections => Set<AppointmentProjection>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PatientConfiguration());
        modelBuilder.ApplyConfiguration(new EncounterConfiguration());
        modelBuilder.ApplyConfiguration(new AppointmentProjectionConfiguration());
    }
}
