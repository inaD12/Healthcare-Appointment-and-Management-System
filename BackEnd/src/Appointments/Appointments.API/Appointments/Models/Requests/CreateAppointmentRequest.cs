using Appointments.Domain.Enums;

namespace Appointments.API.Appointments.Models.Requests;

public sealed record CreateAppointmentRequest(
	string DoctorUserId,
	DateTime ScheduledStartTime,
	AppointmentDuration Duration
);
