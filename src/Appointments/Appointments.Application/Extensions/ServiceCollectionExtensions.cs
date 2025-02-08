using Appointments.Application.Factories;
using Appointments.Application.Managers;
using Appointments.Application.Managers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using FluentValidation;
using Appointments.Application.Helpers;
using Microsoft.AspNetCore.Http;
using Appointments.Application.Appoints.Commands.Shared;
using Appointments.Application.Jobs;
using Shared.Application.Settings;
using Shared.Application.Extensions;
using Appointments.Infrastructure.DBContexts;
using Appointments.Application.Consumers;

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
			.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
			.AddSingleton(sp =>
				sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

		services
			.AddMediatR(currentAssembly)
			.AddValidatorsFromAssembly(currentAssembly)
			.AddHangFire(configuration.GetConnectionString("AppointmentsDBConnection"))
			.AddHostedService<HangfireHostedService>()
			.AddMassTransit<AppointmentsDBContext>(busConfigurator =>
			{
				busConfigurator.AddConsumer<UserCreatedConsumer>();
			});


		return services;
	}
}
