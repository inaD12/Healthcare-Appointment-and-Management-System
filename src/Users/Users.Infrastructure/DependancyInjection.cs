using Microsoft.Extensions.DependencyInjection;
using Users.Application.Factories;
using Users.Infrastructure.DBContexts;
using Users.Infrastructure.Repositories;

namespace Users.Infrastructure
{
	public static class DependancyInjection
	{
		public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
		{
			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
			services.AddTransient<IRepositoryFactory, RepositoryFactory>();
			services.AddScoped<IDBContext, DBContext>();

			return services;
		}
	}
}
