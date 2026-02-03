using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Shared.Application.Extensions;

namespace Ratings.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddMediatR(currentAssembly)
			.AddValidatorsFromAssembly(currentAssembly);

		return services;
	}
}
