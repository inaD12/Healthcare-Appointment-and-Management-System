using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Settings;
using Shared.Domain.Enums;
using Users.Application.Settings;
using Users.Infrastructure.UsersDBContexts;

namespace UsersAPI.Extentions
{
	public static class DataInjection
	{
		public static IServiceCollection ConfigureDBs(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<UsersDBContext>(options =>
				options.UseNpgsql(configuration.GetConnectionString("UsersDBConnection"), o => o.MapEnum<Roles>("roles")));

			return services;
		}

		public static IServiceCollection AddFluentEmail(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddFluentEmail(configuration["Email:SenderEmail"], configuration["Email:Sender"])
					.AddSmtpSender(configuration["Email:Host"], configuration.GetValue<int>("Email:Port"));

			return services;
		}
		public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<AuthValues>(
				configuration.GetSection("Auth"));
			services.Configure<ConnectionStrings>(
				configuration.GetSection("ConnectionStrings"));
			services.Configure<MessageBrokerSettings>(
				configuration.GetSection("MessageBroker"));

			return services;
		}
	}
}
