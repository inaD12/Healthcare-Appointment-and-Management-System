using Appointments.Domain.Abstractions.Repository;
using Appointments.Domain.Repositories;
using Appointments.Infrastructure.Helpers;
using Appointments.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Abstractions;

namespace Appointments.Infrastructure.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddTransient<IAppointmentRepository, AppointmentRepository>()
				.AddScoped<IUserDataRepository, UserDataRepository>()
				.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

			return services;
		}
	}
}
