using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Abstractions;
using Shared.Domain.Enums;
using Shared.Infrastructure.Extensions;
using Users.Domain.Abstractions.Repositories;
using Users.Infrastructure.DBContexts;
using Users.Infrastructure.Features.Helpers;
using Users.Infrastructure.Features.Repositories;

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
			.AddDatabaseContext<UsersDBContext>(configuration, optionsAction =>
			{
				optionsAction.MapEnum<Roles>("roles");
			});

		return services;
	}
}
