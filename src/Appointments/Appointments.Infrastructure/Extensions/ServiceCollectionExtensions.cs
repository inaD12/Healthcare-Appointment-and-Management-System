using Appointments.Domain.Entities.Enums;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Infrastructure.Features.BackgroundJobs;
using Appointments.Infrastructure.Features.DBContexts;
using Appointments.Infrastructure.Features.Helpers;
using Appointments.Infrastructure.Features.Repositories;
using Appointments.Infrastructure.Features.Services;
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
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;
		
		services
			.AddTransient<IAppointmentRepository, AppointmentRepository>()
			.AddTransient<IRolesService, RolesService>()
			.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

		services
			.AddUnitOfWork<AppointmentsDBContext>()
			.AddHostedService<HangfireHostedService>()
			.AddMessageBroker(configuration, currentAssembly)
			.AddAuth(configuration)
			.AddPermissionService()
			.AddDatabaseContext<AppointmentsDBContext>(configuration, optionsAction =>
			{
				optionsAction.MapEnum<AppointmentStatus>("appointmentstatus");
				optionsAction.MapEnum<Roles>("roles");
			});

		return services;
	}
}
