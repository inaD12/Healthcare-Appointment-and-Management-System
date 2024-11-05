using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.DBContexts
{
	public class UserDataDBContext : DbContext
	{
		public DbSet<UserData> UserData { get; set; }

		public UserDataDBContext(DbContextOptions<UserDataDBContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<UserData>()
				.HasIndex(u => u.Id)
				.IsUnique();
		}
	}
}
