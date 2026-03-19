using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Domain.Abstractions;
using Shared.Infrastructure.Extensions;
using Users.Domain.Abstractions.Repositories;
using Users.Domain.Auth.Abstractions;
using Users.Domain.Auth.Options;
using Users.Infrastructure.Features.Consumers;
using Users.Infrastructure.Features.Helpers;
using Users.Infrastructure.Features.Repositories;
using Users.Infrastructure.Features.DBContexts;
using Users.Infrastructure.Features.Helpers.Abstractions;
using Users.Infrastructure.Features.Identity;

namespace Users.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;
		
		services
			.AddScoped<IUserRepository, UserRepository>()
			.AddTransient<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>()
			.AddSingleton<IEmailVerificationLinkFactory, EmailVerificationLinkFactory>()
			.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

		services
			.AddMessageBroker(configuration, currentAssembly, busConfigurator =>
			{
				busConfigurator.AddTransactionalOutbox<UsersDbContext>();
				
				ConfigureConsumers(busConfigurator, instanceId: "users-service");
			})
			.AddDatabaseContext<UsersDbContext>(configuration)
			.AddUnitOfWork<UsersDbContext>()
			.AddAuth(configuration);
		
		services.Configure<KeyCloakOptions>(configuration.GetSection("KeyCloak"));

		services.AddTransient<KeyCloakAuthDelegatingHandler>();

		services
			.AddHttpClient<KeyCloakClient>((serviceProvider, httpClient) =>
			{
				KeyCloakOptions keycloakOptions = serviceProvider
					.GetRequiredService<IOptions<KeyCloakOptions>>().Value;

				httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
			})
			.AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>();

		services.AddTransient<IIdentityProviderService, IdentityProviderService>();

		return services;
	}
	
	private static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator, string instanceId)
	{
		registrationConfigurator.AddConsumer<GetUserPermissionsRequestConsumer>()
			.Endpoint(c => c.InstanceId = instanceId);
		
		registrationConfigurator.AddConsumer<GetUserNamesRequestConsumer>()
			.Endpoint(c => c.InstanceId = instanceId);
		
		registrationConfigurator.AddConsumer<GetUserRolesRequestConsumer>()
			.Endpoint(c => c.InstanceId = instanceId);
	}
}
