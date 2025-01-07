using Appointments.Domain.Enums;
using Contracts.Results;

namespace Appointments.Application.Appoints.Commands.Shared
{
	public interface IAppointmentCommandHandlerHelper
	{
		Task<Result> CreateAppointment(string doctorId, string patientId, DateTime startTime, AppointmentDuration duration);
	}
}