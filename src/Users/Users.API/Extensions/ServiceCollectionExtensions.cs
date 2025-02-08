using Shared.API.Extensions;
using Shared.Application.Extensions;
using UsersAPI.Extentions;

namespace Users.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddAPILayer(this IServiceCollection serviceCollection, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		serviceCollection
			.AddAuthentication(configuration)
			.AddSwagger()
			.ConfigureCors()
			.AddMediatR(currentAssembly)
			.AddEndpointsApiExplorer()
			.ConfigureAppSettings(configuration)
			.AddHttpContextAccessor()
			.ConfigureDBs(configuration)
			.AddFluentEmail(configuration);

		return serviceCollection;
	}
}
