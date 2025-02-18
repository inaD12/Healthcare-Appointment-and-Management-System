using Shared.API.Extensions;
using Shared.Application.Extensions;

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
			.AddFluentEmail(configuration);

		return serviceCollection;
	}

	public static IServiceCollection AddFluentEmail(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddFluentEmail(configuration["Email:SenderEmail"], configuration["Email:Sender"])
				.AddSmtpSender(configuration["Email:Host"], configuration.GetValue<int>("Email:Port"));

		return services;
	}
}
