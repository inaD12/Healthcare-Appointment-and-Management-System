using Appointments.Application.Features.Appointments.Factories.Abstractions;
using Appointments.Application.Features.Jobs.Managers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Application.Features.Jobs.Managers;

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
