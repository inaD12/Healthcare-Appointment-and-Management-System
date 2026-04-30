using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Extensions;
using Shared.Infrastructure.Extensions;

namespace Patients.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddMediatR(currentAssembly)
			.AddDateTimeProvider()
			.AddValidatorsFromAssembly(currentAssembly);

		return services;
	}
}
