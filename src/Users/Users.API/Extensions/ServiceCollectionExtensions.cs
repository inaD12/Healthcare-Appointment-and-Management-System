using Microsoft.EntityFrameworkCore;
using Shared.API.Extensions;
using Shared.Application.Extensions;
using Shared.Domain.Enums;
using Shared.Domain.Options;
using Users.Infrastructure.DBContexts;

namespace Users.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddAPILayer(this IServiceCollection serviceCollection, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		serviceCollection
			.AddAuthentication(configuration)
			.AddSwagger()
			.ConfigureCors()
			.AddMediatR(currentAssembly)
			.AddEndpointsApiExplorer()
			.AddHttpContextAccessor()
			.ConfigureDBs(configuration)
			.AddFluentEmail(configuration);

		return serviceCollection;
	}

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
}
