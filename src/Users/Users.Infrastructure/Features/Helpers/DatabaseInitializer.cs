﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Abstractions;
using Users.Infrastructure.DBContexts;

namespace Users.Infrastructure.Features.Helpers;

internal class DatabaseInitializer : IDatabaseInitializer
{
	public async Task ApplyMigrationsAsync(IServiceScope scope)
	{
		UsersDBContext dBContext =
		   scope.ServiceProvider.GetRequiredService<UsersDBContext>();

		var pendingMigrations = await dBContext.Database.GetPendingMigrationsAsync();

		if (pendingMigrations.Any())
			await dBContext.Database.MigrateAsync();
	}
}
