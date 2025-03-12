using Appointments.Domain.Entities.Enums;

namespace Appointments.API.Appointments.Models.Responses;

public sealed record AppointmentQueryResponse(
	string Id,
	string PatientId,
	string DoctorId,
	DateTime ScheduledStartTime,
	DateTime ScheduledEndTime,
	AppointmentStatus Status);

