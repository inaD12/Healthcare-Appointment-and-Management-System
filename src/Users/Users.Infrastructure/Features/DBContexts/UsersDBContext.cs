﻿using MassTransit;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;

namespace Users.Infrastructure.DBContexts;

public class UsersDBContext : DbContext
{
	public DbSet<User> Users { get; set; }
	public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }

	public UsersDBContext(DbContextOptions<UsersDBContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.AddInboxStateEntity();
		modelBuilder.AddOutboxMessageEntity();
		modelBuilder.AddOutboxStateEntity();

		modelBuilder.Entity<User>()
			.HasIndex(u => u.Email)
			.IsUnique();

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
