using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Abstractions;
using Users.Domain.Abstractions.Repositories;
using Users.Infrastructure.Helpers;
using Users.Infrastructure.MessageBroker;
using Users.Infrastructure.Repositories;

namespace Users.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
	{
		services
			.AddScoped<IUserRepository, UserRepository>()
			.AddTransient<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>()
			.AddScoped<IEventBus, EventBus>()
			.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

		return services;
	}
}
