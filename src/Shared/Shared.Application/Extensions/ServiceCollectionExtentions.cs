using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.PipelineBehaviors;
using Shared.Application.Settings;
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

	public static IServiceCollection AddMassTransit<TDbContext>(
	   this IServiceCollection services,
	   Action<IBusRegistrationConfigurator>? configureConsumers = null
	) where TDbContext : DbContext
	{
		services.AddMassTransit(busConfigurator =>
		{
			busConfigurator.AddEntityFrameworkOutbox<TDbContext>(o =>
			{
				o.QueryDelay = TimeSpan.FromSeconds(1);
				o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
				o.UsePostgres().UseBusOutbox();
			});

			busConfigurator.SetKebabCaseEndpointNameFormatter();

			configureConsumers?.Invoke(busConfigurator);

			busConfigurator.UsingRabbitMq((context, configurator) =>
			{
				var settings = context.GetRequiredService<MessageBrokerSettings>();

				configurator.Host(new Uri(settings.Host), h =>
				{
					h.Username(settings.Username);
					h.Password(settings.Password);
				});

				configurator.ConfigureEndpoints(context);
			});
		});

		return services;
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
