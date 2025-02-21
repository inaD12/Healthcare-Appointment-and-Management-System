using Appointments.Application.Features.Appointments.Factories.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;

namespace Appointments.Application.Features.Appointments.Factories;

public class AppointmentFactory : IAppointmentFactory
{
	public Appointment Create(string patientId, string doctorId, DateTime scheduledStartTime, DateTime ScheduledEndTime)
	{
		return new Appointment(
			Guid.NewGuid().ToString(),
			patientId,
			doctorId,
			scheduledStartTime,
			ScheduledEndTime,
			AppointmentStatus.Scheduled);
	}
}
