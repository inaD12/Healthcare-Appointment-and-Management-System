using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Abstractions;
using Shared.Application.Authorization;
using Shared.Application.Extensions;
using Shared.Infrastructure.Extensions;
using Users.Application.Features.Users.Services;

namespace Users.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
			.AddScoped<IPermissionService, PermissionService>()
			.AddScoped<IRolesService, RolesService>()
			.AddTransient<INamesService, NamesService>()
			.AddTransient<IBatchNamesService, NamesService>();

		services
			.AddMediatR(currentAssembly)
			.AddValidatorsFromAssembly(currentAssembly)
			.AddDateTimeProvider();
		
		return services;
	}
}
