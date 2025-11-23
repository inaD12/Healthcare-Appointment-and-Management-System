using Doctors.Infrastructure.Features.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Abstractions;

namespace Doctors.Infrastructure.Features.Helpers;

internal class DatabaseInitializer : IDatabaseInitializer
{
	public async Task ApplyMigrationsAsync(IServiceScope scope)
	{
		DoctorsDbContext dBContext =
		   scope.ServiceProvider.GetRequiredService<DoctorsDbContext>();

		var pendingMigrations = await dBContext.Database.GetPendingMigrationsAsync();

		if (pendingMigrations.Any())
			await dBContext.Database.MigrateAsync();
	}
}