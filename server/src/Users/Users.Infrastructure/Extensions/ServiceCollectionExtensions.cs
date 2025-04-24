using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Abstractions;
using Shared.Domain.Enums;
using Shared.Infrastructure.Extensions;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Infrastructure.DBContexts;
using Users.Infrastructure.Features.Helpers;
using Users.Infrastructure.Features.Repositories;
using Users.Domain.Auth.Abstractions;
using Users.Infrastructure.Features.Auth;

namespace Users.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddTransient<IPasswordManager, PasswordManager>()
			.AddTransient<ITokenFactory, TokenFactory>()
			.AddScoped<IUserRepository, UserRepository>()
			.AddTransient<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>()
			.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

		services
			.AddUnitOfWork<UsersDBContext>()
			.AddDatabaseContext<UsersDBContext>(configuration, optionsAction =>
			{
				optionsAction.MapEnum<Roles>("roles");
			});

		return services;
	}
}
