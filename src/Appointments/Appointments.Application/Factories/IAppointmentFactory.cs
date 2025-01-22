using Appointments.Domain.Entities;

namespace Appointments.Application.Factories
{
	public interface IAppointmentFactory
	{
		Appointment Create(string patientId, string doctorId, DateTime scheduledStartTime, DateTime ScheduledEndTime);
	}
}