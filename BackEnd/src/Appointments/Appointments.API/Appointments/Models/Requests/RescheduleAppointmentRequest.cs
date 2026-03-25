using Appointments.Domain.Entities.Enums;

namespace Appointments.API.Appointments.Models.Requests;

public sealed record RescheduleAppointmentRequest(
	DateTime ScheduledStartTime,
	AppointmentDuration Duration
);
