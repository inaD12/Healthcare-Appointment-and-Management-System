using System.Reflection;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Shared.Application.Authorization;
using Shared.Application.Data;
using Shared.Domain.Abstractions;
using Shared.Domain.Options;
using Shared.Infrastructure.Authorization;
using Shared.Infrastructure.Clock;
using Shared.Infrastructure.Data;
using Shared.Infrastructure.MessageBroker;

namespace Shared.Infrastructure.Extensions;

public static class ServiceCollectionExtentions
{
	public static IServiceCollection AddDateTimeProvider(this IServiceCollection services)
	{
		services.AddTransient<IDateTimeProvider, DateTimeProvider>();

		return services;
	}

	public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
		where TContext : DbContext
	{
		services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();

		return services;
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

				configurator.UseNewtonsoftJsonSerializer();
				configurator.UseNewtonsoftJsonDeserializer();
			});
		});

		services
			.AddScoped<IEventBus, EventBus>();

		return services;
	}

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
		
		NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseOptions.ConnectionString).Build();
		services.AddSingleton<NpgsqlDataSource>(npgsqlDataSource);
		services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

		return services;
	}

	public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
			.BindConfiguration(nameof(JwtBearerOptions))
			.ValidateDataAnnotations()
			.ValidateOnStart();

		services
			.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer();

		services
			.AddAuthorization()
			.AddHttpContextAccessor();

		services
			.AddAuthorizationInternal();

		return services;
	}

	public static IServiceCollection AddPermissionService(this IServiceCollection services)
	{
		services.AddScoped<IPermissionService, PermissionService>();
		
		return services;
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
}
