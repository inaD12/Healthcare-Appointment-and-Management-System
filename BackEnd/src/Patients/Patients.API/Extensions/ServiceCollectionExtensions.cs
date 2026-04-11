using Patients.API.Patients.GraphQL;
using Shared.API.Extensions;
using Shared.Application.Extensions;

namespace Patients.API.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApiLayer(this IServiceCollection serviceCollection, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		serviceCollection
			.AddSwagger()
			.ConfigureCors(configuration)
			.AddMediatR(currentAssembly)
			.AddExceptionHandling()
			.AddEndpointsApiExplorer();

		serviceCollection
			.AddGraphQLServer()
			.AddType<AppointmentType>()
			.AddType<EncounterType>()
			.AddQueryType<Query>()
			.AddFiltering()
			.AddSorting()
			.AddProjections();

		return serviceCollection;
	}
}
