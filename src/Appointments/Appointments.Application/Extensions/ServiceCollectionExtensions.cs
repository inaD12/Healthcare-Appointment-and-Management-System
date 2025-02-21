using Appointments.Application.Commands.Appointments.Shared;
using Appointments.Application.Factories;
using Appointments.Application.Helpers;
using Appointments.Application.Jobs;
using Appointments.Application.Managers;
using Appointments.Application.Managers.Interfaces;
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
			.AddTransient<IJwtParser, JwtParser>()
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
			.AddMessageBroker(configuration, currentAssembly);


		return services;
	}
}
