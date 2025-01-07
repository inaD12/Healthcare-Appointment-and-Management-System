using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.EmailVerification;
using Users.Application.Factories;
using Users.Application.Factories.Interfaces;
using Users.Application.Helpers;
using Users.Application.Helpers.Interfaces;
using Users.Application.Managers;
using Users.Application.Managers.Interfaces;
using Users.Application.Services;
using Users.Application.Services.Interfaces;
using Users.Application.Settings;
using Users.Application.Validators;
using Users.Domain.DTOs.Requests;

namespace Users.Application
{
    public static class DependancyInjection
	{
		public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration )
		{
			services.AddTransient<IJwtParser, JwtParser>();
			services.AddTransient<IPasswordManager, PasswordManager>();
			services.AddTransient<ITokenManager, TokenManager>();
			services.AddSingleton<ITokenDTOFactory, TokenDTOFactory>();
			services.AddSingleton<IMessageDTOFactory, MessageDTOFactory>();
			services.AddTransient<IValidator<LoginReqDTO>, LoginReqDTOValidator>();
			services.AddTransient<IValidator<RegisterReqDTO>, RegisterReqDTOValidator>();
			services.AddTransient<IValidator<UpdateUserReqDTO>, UpdateUserReqDTOValidator>();
			services.AddTransient<IEmailService, EmailService>();
			services.AddSingleton<IUserFactory, UserFactory>();
			services.AddSingleton<IUserCreatedEventFactory, UserCreatedEventFactory>();
			services.AddSingleton<IEmailVerificationTokenFactory, EmailVerificationTokenFactory>();
			services.AddTransient<IEmailVerificationSender, EmailVerificationSender>();
			services.AddSingleton<IFactoryManager, FactoryManager>();
			services.AddScoped<IRepositoryManager, RepositoryManager>();
			services.AddSingleton<IEmailVerificationLinkFactory, EmailVerificationLinkFactory>();
			services.AddSingleton(sp =>
				sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			return services;
		}
	}
}
