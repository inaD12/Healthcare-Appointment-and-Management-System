using Shared.API.Extensions;
using Shared.Application.Extensions;

namespace Appointments.API.Extentions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddAPILayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddAuthentication(configuration)
			.AddSwagger()
			.ConfigureCors(configuration)
			.AddEndpointsApiExplorer()
			.AddHttpContextAccessor()
			.AddMapper(currentAssembly);

		return services;
	}
}
