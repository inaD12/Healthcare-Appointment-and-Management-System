using Appointments.Domain.Entities;
using Appointments.Infrastructure.Features.Configuration;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Features.DBContexts;

public class AppointmentsDBContext : DbContext
{
	public DbSet<Appointment> Appointments { get; set; }
	public DbSet<UserData> UserData { get; set; }

	public AppointmentsDBContext(DbContextOptions<AppointmentsDBContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.AddInboxStateEntity();
		modelBuilder.AddOutboxMessageEntity();
		modelBuilder.AddOutboxStateEntity();

		modelBuilder.ApplyConfiguration(new PermissionConfiguration());
		modelBuilder.ApplyConfiguration(new RoleConfiguration());
		modelBuilder.ApplyConfiguration(new AppointmentConfiguration());
		modelBuilder.ApplyConfiguration(new UserDataConfiguration());
	}

}
