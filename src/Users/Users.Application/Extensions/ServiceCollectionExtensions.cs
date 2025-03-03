﻿using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Extensions;
using Users.Application.Features.Auth;
using Users.Application.Features.Auth.Abstractions;
using Users.Application.Features.Email.Helpers;
using Users.Application.Features.Email.Helpers.Abstractions;
using Users.Application.Features.Managers;
using Users.Application.Features.Managers.Interfaces;
using Users.Infrastructure.DBContexts;

namespace Users.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddTransient<IPasswordManager, PasswordManager>()
			.AddTransient<ITokenFactory, TokenFactory>()
			.AddSingleton<IEmailVerificationTokenFactory, EmailVerificationTokenFactory>()
			.AddSingleton<IFactoryManager, FactoryManager>()
			.AddScoped<IRepositoryManager, RepositoryManager>()
			.AddSingleton<IEmailVerificationLinkFactory, EmailVerificationLinkFactory>()
			.AddTransient<IEmailConfirmationTokenPublisher, EmailConfirmationTokenPublisher>()
			.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

		services
			.AddMediatR(currentAssembly)
			.AddValidatorsFromAssembly(currentAssembly)
			.AddMapper(currentAssembly)
			.AddMessageBroker(configuration, currentAssembly, busConfigurator =>
			{
				busConfigurator.AddTransactionalOutbox<UsersDBContext>();
			});



		return services;
	}
}
