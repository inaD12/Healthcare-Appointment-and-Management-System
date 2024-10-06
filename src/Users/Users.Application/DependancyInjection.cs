using Microsoft.Extensions.DependencyInjection;

namespace Users.Application
{
	public static class DependancyInjection
	{
		public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
		{
			//services.AddTransient<ISqlConnectionFactory, SqlConnectionFactory>();

			return services;
		}
	}
}
