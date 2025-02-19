using Shared.API.Extensions;

namespace Emails.API.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddAPILayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddSwagger()
			.ConfigureCors(configuration)
			.AddEndpointsApiExplorer();

		return services;
	}
}
