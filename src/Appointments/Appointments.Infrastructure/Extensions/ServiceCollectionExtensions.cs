using Appointments.Domain.Abstractions.Repository;
using Appointments.Domain.Enums;
using Appointments.Infrastructure.Features.Appointments.Repositories;
using Appointments.Infrastructure.Features.DBContexts;
using Appointments.Infrastructure.Features.Helpers;
using Appointments.Infrastructure.Features.UsersData.Repositories;
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
			.AddUnitOfWork<AppointmentsDBContext>()
			.AddDatabaseContext<AppointmentsDBContext>(configuration, optionsAction =>
			{
				optionsAction.MapEnum<Roles>("roles");

				optionsAction.MapEnum<AppointmentStatus>("appointmentstatus");
			});

		return services;
	}
}
