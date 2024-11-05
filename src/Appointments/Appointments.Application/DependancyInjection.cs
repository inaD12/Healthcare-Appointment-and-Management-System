using Appointments.Application.Managers;
using Appointments.Application.Managers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Application.DependancyInjection
{
	public static class DependancyInjection
	{
		public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<IFactoryManager, FactoryManager>();
			services.AddSingleton<IRepositoryManager, RepositoryManager>();

			return services;
		}
	}
}
