using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.DBContexts
{
	public class AppointmentsDBContext : DbContext
	{
		public DbSet<Appointment> Appointments { get; set; }

		public AppointmentsDBContext(DbContextOptions<AppointmentsDBContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Appointment>()
				.HasIndex(u => u.Id)
				.IsUnique();
		}
	}
}
