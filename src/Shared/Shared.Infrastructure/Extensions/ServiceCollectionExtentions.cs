using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Shared.Domain.Options;

namespace Shared.Infrastructure.Extensions;

public static class ServiceCollectionExtentions
{
	public static IServiceCollection AddDatabaseContext<TContext>(
	   this IServiceCollection services,
	   IConfiguration configuration,
	   Action<NpgsqlDbContextOptionsBuilder>? optionsAction = null)
   where TContext : DbContext
	{
		services
			.AddOptions<DatabaseOptions>()
			.BindConfiguration(nameof(DatabaseOptions))
			.ValidateDataAnnotations()
			.ValidateOnStart();

		var databaseOptions = configuration
					.GetSection(nameof(DatabaseOptions))
					.Get<DatabaseOptions>()!;

		services.AddDbContext<TContext>(options =>
		{
			options.UseNpgsql(
					databaseOptions.ConnectionString,
			npgsqlOptions =>
			{
				npgsqlOptions.EnableRetryOnFailure();
				optionsAction?.Invoke(npgsqlOptions);
			});
		});

		services
			.AddHealthChecks()
			.AddDbContextCheck<TContext>();

		return services;
	}
}
