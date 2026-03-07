using Appointments.Application.Features.Appointments.Requirements.ModifyAppointment;
using Microsoft.AspNetCore.Authorization;
using Shared.API.Extensions;
using Shared.Application.Extensions;

namespace Appointments.API.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApiLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddSwagger()
			.ConfigureCors(configuration)
			.AddEndpointsApiExplorer()
			.AddEnumConversion()
			.AddExceptionHandling();
		
		services
			.AddScoped<IAuthorizationHandler, ModifyAppointmentHandler>();


		return services;
	}
}
