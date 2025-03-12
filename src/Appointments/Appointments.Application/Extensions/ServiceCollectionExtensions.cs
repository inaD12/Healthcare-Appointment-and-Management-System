using Appointments.Application.Features.Jobs;
using Appointments.Domain.Infrastructure.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Extensions;
using Shared.Domain.Options;
using Shared.Infrastructure.Extensions;

namespace Appointments.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddScoped<ICompleteAppointmentsJob, CompleteAppointmentsJob>()
			.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

		var dbOptions = configuration
			.GetSection(nameof(DatabaseOptions))
			.Get<DatabaseOptions>()!;

		services
			.AddMediatR(currentAssembly)
			.AddValidatorsFromAssembly(currentAssembly)
			.AddHangFire(dbOptions.ConnectionString)
			.AddMessageBroker(configuration, currentAssembly)
			.AddMapper(currentAssembly)
			.AddDateTimeProvider();


		return services;
	}
}
