using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Abstractions.Repositories.Command;
using Patients.Domain.Abstractions.Repositories.Query;
using Patients.Infrastructure.Features.DBContexts;
using Patients.Infrastructure.Features.Helpers;
using Patients.Infrastructure.Features.Repositories;
using Patients.Infrastructure.Features.Repositories.Command;
using Patients.Infrastructure.Features.Repositories.Query;
using Patients.Infrastructure.Features.Services;
using Shared.Domain.Abstractions;
using Shared.Infrastructure.Extensions;

namespace Patients.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddScoped<IPatientQueryRepository, PatientQueryRepository>()
			.AddScoped<IEncounterQueryRepository, EncounterQueryQueryRepository>()
			.AddScoped<IAppointmentQueryRepository, AppointmentQueryRepository>()
			.AddScoped<IPatientCommandRepository, PatientCommandRepository>()
			.AddScoped<IEncounterCommandRepository, EncounterCommandQueryRepository>()
			.AddScoped<IAppointmentCommandRepository, AppointmentCommandRepository>()
			.AddTransient<IBatchNamesService, BatchNamesService>()
			.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

		services
			.AddUnitOfWork<PatientsCommandDbContext>()
			.AddMessageBroker(configuration, currentAssembly)
			.AddAuth(configuration)
			.AddPermissionService()
			.AddDatabaseContextFactory<PatientsQueryDbContext>(configuration, optionsAction =>
			{
				optionsAction.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
			})
			.AddDatabaseContext<PatientsCommandDbContext>(configuration);

		return services;
	}
}
