using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Abstractions;
using Users.Infrastructure.Features.DBContexts;

namespace Users.Infrastructure.Features.Helpers;

internal class DatabaseInitializer : IDatabaseInitializer
{
	public async Task ApplyMigrationsAsync(IServiceScope scope)
	{
		UsersDbContext dBContext =
		   scope.ServiceProvider.GetRequiredService<UsersDbContext>();

		var pendingMigrations = await dBContext.Database.GetPendingMigrationsAsync();

		if (pendingMigrations.Any())
			await dBContext.Database.MigrateAsync();
	}
}
