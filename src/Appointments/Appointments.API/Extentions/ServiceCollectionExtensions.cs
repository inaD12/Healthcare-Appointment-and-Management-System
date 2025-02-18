using Appointments.Domain.Enums;
using Appointments.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;
using Shared.API.Extensions;
using Shared.Domain.Enums;

namespace Appointments.API.Extentions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddAPILayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddAuthentication(configuration)
			.AddSwagger()
			.ConfigureCors()
			.AddEndpointsApiExplorer()
			.AddHttpContextAccessor()
			.ConfigureDBs(configuration);

		return services;
	}

	public static IServiceCollection ConfigureDBs(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<AppointmentsDBContext>(options =>
		options.UseNpgsql(configuration.GetConnectionString("AppointmentsDBConnection"), o =>
		{
			o.MapEnum<Roles>("roles");

			o.MapEnum<AppointmentStatus>("appointmentstatus");
		}));

		return services;
	}
}
