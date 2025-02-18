using Appointments.Application.Commands.Appointments.Shared;
using Appointments.Application.Consumers;
using Appointments.Application.Factories;
using Appointments.Application.Helpers;
using Appointments.Application.Jobs;
using Appointments.Application.Managers;
using Appointments.Application.Managers.Interfaces;
using Appointments.Infrastructure.DBContexts;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Application.Extensions;
using Shared.Application.Settings;

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
			.AddTransient<IJWTUserExtractor, JWTUserExtractor>()
			.AddTransient<IAppointmentCommandHandlerHelper, AppointmentCommandHandlerHelper>()
			.AddScoped<CompleteAppointmentsJob>()
			.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

		services
			.AddMediatR(currentAssembly)
			.AddValidatorsFromAssembly(currentAssembly)
			.AddHangFire(configuration.GetConnectionString("AppointmentsDBConnection"))
			.AddHostedService<HangfireHostedService>()
			.AddMessageBroker(configuration, busConfigurator =>
			{
				busConfigurator.AddConsumer<UserCreatedConsumer>();
			});


		return services;
	}
}
