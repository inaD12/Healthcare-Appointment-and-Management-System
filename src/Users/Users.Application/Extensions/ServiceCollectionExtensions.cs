using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Authorization;
using Shared.Application.Extensions;
using Shared.Infrastructure.Extensions;
using Users.Application.Features.Email.Helpers;
using Users.Application.Features.Email.Helpers.Abstractions;
using Users.Application.Features.Users.Consumers;
using Users.Application.Features.Users.Services;
using Users.Infrastructure.Features.DBContexts;

namespace Users.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddSingleton<IEmailVerificationLinkFactory, EmailVerificationLinkFactory>()
			.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
			.AddScoped<IPermissionService, PermissionService>();

		services
			.AddMediatR(currentAssembly)
			.AddValidatorsFromAssembly(currentAssembly)
			.AddMapper(currentAssembly)
			.AddDateTimeProvider()
			.AddMessageBroker(configuration, currentAssembly, busConfigurator =>
			{
				busConfigurator.AddTransactionalOutbox<UsersDbContext>();
				
				ConfigureConsumers(busConfigurator, instanceId: "users-service");
			});
		
		return services;
	}

	private static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator, string instanceId)
	{
		registrationConfigurator.AddConsumer<GetUserPermissionsRequestConsumer>()
			.Endpoint(c => c.InstanceId = instanceId);
	}
}
