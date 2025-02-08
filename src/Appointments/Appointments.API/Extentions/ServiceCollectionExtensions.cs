﻿using Shared.API.Extensions;

namespace Appointments.API.Extentions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddAPILayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddAuthentication(configuration)
			.AddSwagger()
			.ConfigureCors()
			.AddEndpointsApiExplorer()
			.ConfigureAppSettings(configuration)
			.AddHttpContextAccessor()
			.ConfigureDBs(configuration);

		return services;
	}
}
