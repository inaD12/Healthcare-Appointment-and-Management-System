using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patients.Domain.Abstractions.Repositories;
using Patients.Infrastructure.Features.Helpers;
using Patients.Infrastructure.Features.Repositories;
using Shared.Domain.Abstractions;

namespace Patients.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddScoped<IPatientRepository, PatientRepository>()
			.AddScoped<IEncounterRepository, EncounterRepository>()
			.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

		return services;
	}
}
