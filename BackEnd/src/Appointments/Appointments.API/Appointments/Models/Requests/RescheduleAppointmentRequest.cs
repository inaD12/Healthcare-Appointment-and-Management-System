using Appointments.Domain.Entities.Enums;

namespace Appointments.API.Appointments.Models.Requests;

public sealed record RescheduleAppointmentRequest(
	string AppointmentId,
	DateTime ScheduledStartTime,
	AppointmentDuration Duration
);
