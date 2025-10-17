using Doctors.Domain.Entities;
using Doctors.Domain.Infrastructure.Abstractions.Repositories;
using Doctors.Infrastructure.Features.DBContexts;
using Doctors.Infrastructure.Features.Helpers;
using Doctors.Infrastructure.Features.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Abstractions;
using Shared.Infrastructure.Extensions;

namespace Doctors.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;
		
		services
			.AddScoped<IDoctorRepository, DoctorRepository>()
			.AddScoped<ISpecialityRepository, SpecialityRepository>()
			.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

		services
			.AddUnitOfWork<DoctorsDbContext>()
			.AddMessageBroker(configuration, currentAssembly, busConfigurator =>
			{
				busConfigurator.AddTransactionalOutbox<DoctorsDbContext>();
			})
			.AddDatabaseContext<DoctorsDbContext>(configuration, optionsAction =>
			{
				optionsAction.MapEnum<DayOfWeek>("dayofweek");
				optionsAction.MapEnum<AvailabilityExceptionType>("availabilityexceptiontype");
			});

		return services;
	}
}
