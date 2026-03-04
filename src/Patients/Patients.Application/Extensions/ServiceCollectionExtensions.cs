using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patients.Application.Features.AppointmentProjections.Queries.DataLoaders;
using Patients.Application.Features.Encounters.Queries.DataLoaders;
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
		
		services
			.AddDataLoader<AppointmentsByPatientDataLoader>()
			.AddDataLoader<NotesByEncounterDataLoader>()
			.AddDataLoader<DiagnosesByEncounterDataLoader>()
			.AddDataLoader<PrescriptionsByEncounterDataLoader>()
			.AddDataLoader<EncountersByPatientDataLoader>()
			.AddDataLoader<AddendumsByEncounterDataLoader>();

		return services;
	}
}
