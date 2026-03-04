using Appointments.Domain.Entities.Enums;

namespace Appointments.API.Appointments.Models.Requests;

public sealed record CreateAppointmentRequest(
	string PatientId,
	string DoctorId,
	DateTime ScheduledStartTime,
	AppointmentDuration Duration
);
