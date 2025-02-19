using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.PipelineBehaviors;
using Shared.Domain.Options;
using Shared.Infrastructure.MessageBroker;
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

	public static IServiceCollection AddMessageBroker(
	   this IServiceCollection services,
	   IConfiguration configuration,
		Assembly assembly,
	   Action<IBusRegistrationConfigurator>? configure = null
	)
	{
		services
			.AddOptions<MessageBrokerOptions>()
			.BindConfiguration(nameof(MessageBrokerOptions))
			.ValidateDataAnnotations()
			.ValidateOnStart();

		var settings = configuration
			.GetSection(nameof(MessageBrokerOptions))
			.Get<MessageBrokerOptions>()!;

		services.AddMassTransit(busConfigurator =>
		{
			busConfigurator.SetKebabCaseEndpointNameFormatter();

			busConfigurator.AddConsumers(assembly);

			configure?.Invoke(busConfigurator);

			busConfigurator.UsingRabbitMq((context, configurator) =>
			{
				configurator.Host(new Uri(settings.Host), h =>
				{
					h.Username(settings.Username);
					h.Password(settings.Password);
				});

				configurator.ConfigureEndpoints(context);
			});
		});

		services
		   .AddScoped<IEventBus, EventBus>();

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
