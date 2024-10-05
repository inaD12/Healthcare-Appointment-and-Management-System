using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;

namespace Users.Infrastructure.UsersDBContexts
{
	public class UsersDBContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		public UsersDBContext(DbContextOptions<UsersDBContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>()
				.HasIndex(u => u.Email)
				.IsUnique();
		}
	}
}
