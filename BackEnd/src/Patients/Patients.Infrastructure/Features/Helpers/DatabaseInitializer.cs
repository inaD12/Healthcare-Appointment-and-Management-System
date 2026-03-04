using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Patients.Infrastructure.Features.DBContexts;
using Shared.Domain.Abstractions;

namespace Patients.Infrastructure.Features.Helpers;

internal class DatabaseInitializer : IDatabaseInitializer
{
	public async Task ApplyMigrationsAsync(IServiceScope scope)
	{
		PatientsDbContext dBContext =
		   scope.ServiceProvider.GetRequiredService<PatientsDbContext>();

		var pendingMigrations = await dBContext.Database.GetPendingMigrationsAsync();

		if (pendingMigrations.Any())
			await dBContext.Database.MigrateAsync();
	}
}