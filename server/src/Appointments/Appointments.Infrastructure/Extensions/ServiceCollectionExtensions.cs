using Appointments.Domain.Entities.Enums;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Infrastructure.Features.Appointments.Repositories;
using Appointments.Infrastructure.Features.BackgroundJobs;
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
			.AddHostedService<HangfireHostedService>()
			.AddDatabaseContext<AppointmentsDBContext>(configuration, optionsAction =>
			{
				optionsAction.MapEnum<Roles>("roles");

				optionsAction.MapEnum<AppointmentStatus>("appointmentstatus");
			});

		return services;
	}
}
