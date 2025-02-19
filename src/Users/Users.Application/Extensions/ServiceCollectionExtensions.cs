using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Extensions;
using Users.Application.Auth.Helpers;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.Factories;
using Users.Application.Factories.Interfaces;
using Users.Application.Helpers;
using Users.Application.Managers;
using Users.Application.Managers.Interfaces;
using Users.Infrastructure.DBContexts;

namespace Users.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddTransient<IJwtParser, JwtParser>()
			.AddTransient<IPasswordManager, PasswordManager>()
			.AddTransient<ITokenManager, TokenManager>()
			.AddSingleton<ITokenDTOFactory, TokenDTOFactory>()
			.AddSingleton<IMessageDTOFactory, MessageDTOFactory>()
			.AddSingleton<IUserFactory, UserFactory>()
			.AddSingleton<IUserCreatedEventFactory, UserCreatedEventFactory>()
			.AddSingleton<IEmailVerificationTokenFactory, EmailVerificationTokenFactory>()
			.AddSingleton<IFactoryManager, FactoryManager>()
			.AddScoped<IRepositoryManager, RepositoryManager>()
			.AddSingleton<IEmailVerificationLinkFactory, EmailVerificationLinkFactory>()
			.AddTransient<IUserConfirmEmailEventFactory, UserConfirmEmailEventFactory>()
			.AddTransient<IEmailConfirmationTokenPublisher, EmailConfirmationTokenPublisher>()
			.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

		services
			.AddMediatR(currentAssembly)
			.AddValidatorsFromAssembly(currentAssembly)
			.AddMessageBroker(configuration, currentAssembly, busConfigurator =>
			{
				busConfigurator.AddTransactionalOutbox<UsersDBContext>();
			});



		return services;
	}
}
