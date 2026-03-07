using Shared.API.Extensions;
using Shared.Application.Extensions;

namespace Doctors.API.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddAPILayer(this IServiceCollection serviceCollection, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		serviceCollection
			.AddSwagger()
			.ConfigureCors(configuration)
			.AddMediatR(currentAssembly)
			.AddExceptionHandling()
			.AddEnumConversion()
			.AddEndpointsApiExplorer();

		return serviceCollection;
	}
}
