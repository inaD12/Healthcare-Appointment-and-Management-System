using Microsoft.Extensions.DependencyInjection;

namespace Users.Infrastructure
{
	public static class DependancyInjection
	{
		public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
		{
			//services.AddTransient<ISqlConnectionFactory, SqlConnectionFactory>();

			return services;
		}
	}
}
