using Appointments.Domain.Abstractions.Repository;
using Appointments.Domain.Enums;
using Appointments.Infrastructure.DBContexts;
using Appointments.Infrastructure.Helpers;
using Appointments.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Abstractions;
using Shared.Domain.Enums;
using Shared.Infrastructure.Extensions;

namespace Appointments.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddTransient<IAppointmentRepository, AppointmentRepository>()
			.AddScoped<IUserDataRepository, UserDataRepository>()
			.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

		services
			.AddDatabaseContext<AppointmentsDBContext>(configuration, optionsAction =>
			{
				optionsAction.MapEnum<Roles>("roles");

				optionsAction.MapEnum<AppointmentStatus>("appointmentstatus");
			});

		return services;
	}
}
