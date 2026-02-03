using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ratings.Infrastructure.Features.DBContexts;
using Shared.Domain.Abstractions;

namespace Ratings.Infrastructure.Features.Helpers;

internal class DatabaseInitializer : IDatabaseInitializer
{
	public async Task ApplyMigrationsAsync(IServiceScope scope)
	{
		RatingsDbContext dBContext =
		   scope.ServiceProvider.GetRequiredService<RatingsDbContext>();

		var pendingMigrations = await dBContext.Database.GetPendingMigrationsAsync();

		if (pendingMigrations.Any())
			await dBContext.Database.MigrateAsync();
	}
}