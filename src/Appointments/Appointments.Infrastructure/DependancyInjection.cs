using Appointments.Domain.Repositories;
using Appointments.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Infrastructure.DependancyInjection
{
	public static class DependancyInjection
	{
		public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddTransient<IAppointmentRepository, AppointmentRepository>();
			services.AddScoped<IUserDataRepository, UserDataRepository>();

			return services;
		}
	}
}
