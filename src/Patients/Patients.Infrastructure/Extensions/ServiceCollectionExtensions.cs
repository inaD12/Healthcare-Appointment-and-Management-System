using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patients.Domain.Abstractions.Repositories;
using Patients.Infrastructure.Features.DBContexts;
using Patients.Infrastructure.Features.Helpers;
using Patients.Infrastructure.Features.ReadModels.Abstractions;
using Patients.Infrastructure.Features.Repositories;
using Shared.Domain.Abstractions;
using Shared.Infrastructure.Extensions;

namespace Patients.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddScoped<IPatientRepository, PatientRepository>()
			.AddScoped<IEncounterRepository, EncounterRepository>()
			.AddScoped<IAppointmentReadRepository, AppointmentReadRepository>()
			.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
		
		services
			.AddUnitOfWork<PatientsDbContext>()
			.AddMessageBroker(configuration, currentAssembly)
			.AddAuth(configuration)
			.AddPermissionService()
			.AddDatabaseContext<PatientsDbContext>(configuration)
			.AddDatabaseContext<PatientsReadDbContext>(configuration);

		return services;
	}
}
