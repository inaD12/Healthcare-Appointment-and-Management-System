using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Application.Extensions;
using Shared.Application.Settings;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.Consumers;
using Users.Application.Factories;
using Users.Application.Factories.Interfaces;
using Users.Application.Helpers;
using Users.Application.Helpers.Interfaces;
using Users.Application.Managers;
using Users.Application.Managers.Interfaces;
using Users.Infrastructure.UsersDBContexts;

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
			.AddSingleton(sp =>
				sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

		services
			.AddMediatR(currentAssembly)
			.AddValidatorsFromAssembly(currentAssembly)
			.AddMassTransit<UsersDBContext>(busConfigurator =>
			{
				busConfigurator.AddConsumer<UserCreatedConsumer>();
			});



		return services;
	}
}
