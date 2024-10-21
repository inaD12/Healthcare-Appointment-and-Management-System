using Microsoft.Extensions.DependencyInjection;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.Factories;
using Users.Application.Helpers;
using Users.Application.Services;

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
			services.AddTransient<ITokenDTOFactory, TokenDTOFactory>();
			services.AddTransient<IMessageDTOFactory, MessageDTOFactory>();

			return services;
		}
	}
}
