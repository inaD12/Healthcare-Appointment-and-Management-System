using Appointments.Domain.Abstractions.Repository;

namespace Appointments.Application.Features.Jobs.Managers.Interfaces;

public interface IRepositoryManager
{
	IAppointmentRepository Appointment { get; }
	IUserDataRepository UserData { get; }

}