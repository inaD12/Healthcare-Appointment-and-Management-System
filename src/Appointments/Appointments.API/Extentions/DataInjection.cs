using Appointments.Domain.Enums;
using Appointments.Infrastructure.DBContexts;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Settings;
using Shared.Domain.Enums;

namespace Appointments.API.Extentions
{
	public static class DataInjection
	{
		public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<MessageBrokerSettings>(
				configuration.GetSection("MessageBroker"));

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
}
