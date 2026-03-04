using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Domain.Abstractions;

namespace Shared.API.Extensions;

public static class HostExtensions
{
	public static async Task SetUpDatabaseAsync(this IHost host)
	{
		using var scope = host.Services.CreateScope();

		var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();

		await databaseInitializer.ApplyMigrationsAsync(scope);
	}
}
