using Appointments.Domain.Entities.Enums;

namespace Appointments.API.Appointments.Models.Requests;

public sealed record CreateAppointmentRequest(
	string PatientEmail,
	string DoctorEmail,
	DateTime ScheduledStartTime,
	AppointmentDuration Duration
);
