using Patients.API.Patients.GraphQL;
using Patients.API.Patients.GraphQL.DataLoaders;
using Patients.Infrastructure.Features.DBContexts;
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
			.RegisterDbContextFactory<PatientsQueryDbContext>()
			.AddType<AppointmentType>()
			.AddType<EncounterType>()
			.AddQueryType<Query>()
			.AddFiltering()
			.AddSorting()
			.AddProjections();
		
		serviceCollection
			.AddDataLoader<AppointmentsByPatientDataLoader>()
			.AddDataLoader<UserNamesDataLoader>()
			.AddDataLoader<NotesByEncounterDataLoader>()
			.AddDataLoader<DiagnosesByEncounterDataLoader>()
			.AddDataLoader<EncountersByAppointmentDataLoader>()
			.AddDataLoader<PrescriptionsByEncounterDataLoader>()
			.AddDataLoader<EncountersByPatientDataLoader>()
			.AddDataLoader<AddendumsByEncounterDataLoader>();

		return serviceCollection;
	}
}
