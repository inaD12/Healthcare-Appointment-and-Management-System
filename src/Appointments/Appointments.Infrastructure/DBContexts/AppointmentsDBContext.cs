﻿using Appointments.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.DBContexts
{
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

			modelBuilder.Entity<Appointment>(entity =>
			{
				entity.HasIndex(a => a.Id).IsUnique();

			});

			modelBuilder.Entity<UserData>(entity =>
			{
				entity.HasIndex(u => u.Id).IsUnique();
			});
		}

	}
}
