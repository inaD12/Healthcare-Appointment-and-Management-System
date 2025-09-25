using MassTransit;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;

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

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.HasIndex(e => e.Email).IsUnique();
			entity.HasIndex(e => e.IdentityId).IsUnique();
		});

		modelBuilder.Entity<EmailVerificationToken>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.UserId).IsRequired();

			entity.HasOne(e => e.User)
				.WithMany()
				.HasForeignKey(e => e.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		});
	}
}
