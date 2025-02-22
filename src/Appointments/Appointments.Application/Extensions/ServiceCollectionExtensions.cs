using Appointments.Application.Features.Appointments.Factories;
using Appointments.Application.Features.Appointments.Factories.Abstractions;
using Appointments.Application.Features.Commands.Appointments.Shared;
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

namespace Appointments.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddSingleton<IFactoryManager, FactoryManager>()
			.AddScoped<IRepositoryManager, RepositoryManager>()
			.AddSingleton<IAppointmentFactory, AppointmentFactory>()
			.AddSingleton<IUserDataFactory, UserDataFactory>()
			.AddSingleton<ICreateAppointmentDTOFactory, CreateAppointmentDTOFactory>()
			.AddTransient<IAppointmentCommandHandlerHelper, AppointmentCommandHandlerHelper>()
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
			.AddMapper(currentAssembly);


		return services;
	}
}
