using Microsoft.Extensions.DependencyInjection;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.Helpers;
using Users.Application.Services;
using Users.Infrastructure.Repositories;

namespace Users.Application
{
	public static class DependancyInjection
	{
		public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
		{
			services.AddTransient<IUserService, UserService>();
			services.AddTransient<IJwtParser, JwtParser>();
			services.AddTransient<IPasswordManager, PasswordManager>();
			services.AddTransient<ITokenManager, TokenManager>();

			return services;
		}
	}
}
