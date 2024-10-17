using Microsoft.Extensions.DependencyInjection;
using Users.Infrastructure.Repositories;

namespace Users.Infrastructure
{
	public static class DependancyInjection
	{
		public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
		{
			services.AddTransient<IUserRepository, UserRepository>();

			return services;
		}
	}
}
