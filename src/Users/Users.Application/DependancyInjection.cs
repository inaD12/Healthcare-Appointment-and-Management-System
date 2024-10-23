using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.Factories;
using Users.Application.Helpers;
using Users.Application.Services;
using Users.Application.Validators;
using Users.Domain.DTOs.Requests;

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
			services.AddTransient<IValidator<LoginReqDTO>, LoginReqDTOValidator>();
			services.AddTransient<IValidator<RegisterReqDTO>, RegisterReqDTOValidator>();
			services.AddTransient<IValidator<UpdateUserReqDTO>, UpdateUserReqDTOValidator>();



			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			return services;
		}
	}
}
