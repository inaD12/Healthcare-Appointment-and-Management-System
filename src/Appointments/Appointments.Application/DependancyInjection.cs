using Appointments.Application.Factories;
using Appointments.Application.Managers;
using Appointments.Application.Managers.Interfaces;
using Appointments.Application.Services;
using Appointments.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using FluentValidation;

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
			services.AddTransient<IAppointentService, AppointentService>();

			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			return services;
		}
	}
}
