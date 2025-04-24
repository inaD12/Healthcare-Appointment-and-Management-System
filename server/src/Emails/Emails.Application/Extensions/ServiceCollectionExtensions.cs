using Emails.Application.Features.Emails.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Extensions;

namespace Emails.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddEmailService(configuration);

		services
			.AddMessageBroker(configuration, currentAssembly);

		return services;
	}
}