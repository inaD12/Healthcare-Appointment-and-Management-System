using Appointments.Application.Features.Appointments.Factories.Abstractions;

namespace Appointments.Application.Features.Jobs.Managers.Interfaces;

public interface IFactoryManager
{
	IAppointmentFactory Appointment { get; }
	IUserDataFactory UserData { get; }
	ICreateAppointmentDTOFactory CreateAppointmentDTO { get; }

}