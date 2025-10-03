using MassTransit;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Infrastructure.Features.Configurations;

namespace Users.Infrastructure.Features.DBContexts;

public class UsersDbContext : DbContext
{
	public DbSet<User> Users { get; set; }
	public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }

	public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.AddInboxStateEntity();
		modelBuilder.AddOutboxMessageEntity();
		modelBuilder.AddOutboxStateEntity();

		modelBuilder.ApplyConfiguration(new PermissionConfiguration());
		modelBuilder.ApplyConfiguration(new RoleConfiguration());
		modelBuilder.ApplyConfiguration(new EmailVerificationTokenConfiguration());
		modelBuilder.ApplyConfiguration(new UserConfiguration());
	}
}
