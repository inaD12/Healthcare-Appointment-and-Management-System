using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Authorization;
using Shared.Application.Extensions;
using Shared.Domain.Abstractions;
using Shared.Infrastructure.Extensions;
using Users.Application.Features.Email.Helpers;
using Users.Application.Features.Email.Helpers.Abstractions;
using Users.Application.Features.Users.Services;

namespace Users.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddSingleton<IEmailVerificationLinkFactory, EmailVerificationLinkFactory>()
			.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
			.AddScoped<IPermissionService, PermissionService>()
			.AddTransient<INamesService, NamesService>();

		services
			.AddMediatR(currentAssembly)
			.AddValidatorsFromAssembly(currentAssembly)
			.AddDateTimeProvider();
		
		return services;
	}
}
