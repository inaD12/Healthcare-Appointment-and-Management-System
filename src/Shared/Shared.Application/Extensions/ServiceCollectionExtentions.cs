using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Abstractions;
using Shared.Application.Helpers;
using Shared.Application.PipelineBehaviors;
using System.Reflection;

namespace Shared.Application.Extensions;

public static class ServiceCollectionExtentions
{
	public static IServiceCollection AddMediatR(this IServiceCollection serviceCollection, Assembly assembly)
	{
		serviceCollection.AddMediatR(
			cf =>
			{
				cf.RegisterServicesFromAssembly(assembly);

				cf.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
			});

		return serviceCollection;
	}

	public static IServiceCollection AddMapper(this IServiceCollection serviceCollection, Assembly assembly)
	{
		serviceCollection.AddAutoMapper(assembly);

		serviceCollection.AddScoped<IHAMSMapper, HAMSMapper>();

		return serviceCollection;
	}

	public static IServiceCollection AddHangFire(this IServiceCollection services, string connectionString)
	{
		services.AddHangfire(config =>
		{
			config.UseSimpleAssemblyNameTypeSerializer()
				  .UseRecommendedSerializerSettings()
				  .UsePostgreSqlStorage(options =>
					  options.UseNpgsqlConnection(connectionString));
		});

		services.AddHangfireServer();

		return services;
	}
}
