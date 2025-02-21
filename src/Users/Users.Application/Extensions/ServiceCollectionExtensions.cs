using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Extensions;
using Users.Application.Features.Auth;
using Users.Application.Features.Auth.Abstractions;
using Users.Application.Features.Email.Factories;
using Users.Application.Features.Email.Factories.Abstractions;
using Users.Application.Features.Email.Helpers;
using Users.Application.Features.Email.Helpers.Abstractions;
using Users.Application.Features.Managers;
using Users.Application.Features.Managers.Interfaces;
using Users.Application.Features.Users.Factories;
using Users.Application.Features.Users.Factories.Abstractions;
using Users.Infrastructure.DBContexts;

namespace Users.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
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
