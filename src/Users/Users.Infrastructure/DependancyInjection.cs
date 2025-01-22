using Microsoft.Extensions.DependencyInjection;
using Users.Infrastructure.MessageBroker;
using Users.Infrastructure.Repositories;
using Users.Infrastructure.Repositories.Interfaces;

namespace Users.Infrastructure
{
    public static class DependancyInjection
	{
		public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
		{
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddTransient<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
			services.AddScoped<IEventBus, EventBus>();

			return services;
		}
	}
}
