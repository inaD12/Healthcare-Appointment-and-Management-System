using Appointments.Domain.Enums;

namespace Appointments.API.Appointments.Models.Requests;

public sealed record RescheduleAppointmentRequest(
	DateTime ScheduledStartTime,
	AppointmentDuration Duration
);
