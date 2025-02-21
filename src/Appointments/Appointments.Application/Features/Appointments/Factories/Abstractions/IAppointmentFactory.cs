using Appointments.Domain.Entities;

namespace Appointments.Application.Features.Appointments.Factories.Abstractions;

public interface IAppointmentFactory
{
	Appointment Create(string patientId, string doctorId, DateTime scheduledStartTime, DateTime ScheduledEndTime);
}