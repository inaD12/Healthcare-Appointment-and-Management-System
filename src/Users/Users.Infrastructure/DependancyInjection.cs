using Microsoft.Extensions.DependencyInjection;
using Users.Infrastructure.Repositories;
using Users.Infrastructure.Repositories.Interfaces;

namespace Users.Infrastructure
{
    public static class DependancyInjection
	{
		public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
		{
			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();

			return services;
		}
	}
}
