using Appointments.Application.Features.Helpers;
using Appointments.Application.Features.Jobs;
using Appointments.Application.Features.Jobs.Managers;
using Appointments.Application.Features.Jobs.Managers.Interfaces;
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
			.AddScoped<IRepositoryManager, RepositoryManager>()
			.AddScoped<CompleteAppointmentsJob>()
			.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

		var dbOptions = configuration
			.GetSection(nameof(DatabaseOptions))
			.Get<DatabaseOptions>()!;

		services
			.AddMediatR(currentAssembly)
			.AddValidatorsFromAssembly(currentAssembly)
			.AddHangFire(dbOptions.ConnectionString)
			.AddHostedService<HangfireHostedService>()
			.AddMessageBroker(configuration, currentAssembly)
			.AddMapper(currentAssembly)
			.AddDateTimeProvider();


		return services;
	}
}
