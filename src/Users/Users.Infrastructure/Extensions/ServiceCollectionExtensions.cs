using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Domain.Abstractions;
using Shared.Domain.Enums;
using Shared.Infrastructure.Extensions;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Infrastructure.Features.Helpers;
using Users.Infrastructure.Features.Repositories;
using Users.Domain.Auth.Abstractions;
using Users.Domain.Auth.Options;
using Users.Infrastructure.Features.DBContexts;
using Users.Infrastructure.Features.Identity;

namespace Users.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddScoped<IUserRepository, UserRepository>()
			.AddTransient<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>()
			.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

		services
			.AddUnitOfWork<UsersDbContext>()
			.AddDatabaseContext<UsersDbContext>(configuration, optionsAction =>
			{
				optionsAction.MapEnum<Roles>("roles");
			});
		
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
}
