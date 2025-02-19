using Shared.API.Extensions;
using Shared.Application.Extensions;

namespace Users.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddAPILayer(this IServiceCollection serviceCollection, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		serviceCollection
			.AddAuthentication(configuration)
			.AddSwagger()
			.ConfigureCors(configuration)
			.AddMediatR(currentAssembly)
			.AddEndpointsApiExplorer()
			.AddHttpContextAccessor();

		return serviceCollection;
	}
}
