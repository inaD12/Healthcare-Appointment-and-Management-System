using Appointments.Domain.DTOS.Request;
using Appointments.Domain.Enums;

namespace Appointments.Application.Factories
{
	public interface ICreateAppointmentDTOFactory
	{
		CreateAppointmentDTO Create(string patientEmail, string doctorEmail, DateTime ScheduledStartTime, AppointmentDuration duration);
	}
}