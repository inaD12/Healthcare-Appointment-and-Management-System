using Appointments.Application.Factories;
using Appointments.Application.Managers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Application.Managers
{
    internal class FactoryManager : IFactoryManager
	{
		private readonly IServiceProvider _serviceProvider;

		public FactoryManager(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public IAppointmentFactory Appointment => _serviceProvider.GetRequiredService<IAppointmentFactory>();
		public IUserDataFactory UserData => _serviceProvider.GetRequiredService<IUserDataFactory>();
		public ICreateAppointmentDTOFactory CreateAppointmentDTO => _serviceProvider.GetRequiredService<ICreateAppointmentDTOFactory>();
	}
}
