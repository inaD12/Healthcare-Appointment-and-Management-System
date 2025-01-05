using Appointments.Application.Factories;
using Appointments.Application.Managers;
using Appointments.Application.Managers.Interfaces;
using Appointments.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using FluentValidation;
using Appointments.Application.Helpers;
using Microsoft.AspNetCore.Http;
using Appointments.Application.Appoints.Commands.Shared;
using Appointments.Application.Jobs;

namespace Appointments.Application.DependancyInjection
{
	public static class DependancyInjection
	{
		public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<IFactoryManager, FactoryManager>();
			services.AddScoped<IRepositoryManager, RepositoryManager>();
			services.AddSingleton(sp =>
				sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
			services.AddSingleton<IAppointmentFactory, AppointmentFactory>();
			services.AddSingleton<IUserDataFactory, UserDataFactory>();
			services.AddSingleton<ICreateAppointmentDTOFactory, CreateAppointmentDTOFactory>();
			services.AddTransient<IJwtParser, JwtParser>();
			services.AddTransient<IJWTUserExtractor, JWTUserExtractor>();
			services.AddTransient<IAppointmentCommandHandlerHelper, AppointmentCommandHandlerHelper>();
			services.AddScoped<CompleteAppointmentsJob>();

			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddHostedService<HangfireHostedService>();

			return services;
		}
	}
}
