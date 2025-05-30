using Appointments.Domain.Entities.Enums;
using Appointments.Domain.Entities.ValueObjects;

namespace Appointments.API.Appointments.Models.Responses;

public sealed record AppointmentQueryResponse(
	string Id,
	string PatientId,
	string DoctorId,
	DateTimeRange Duration,
	AppointmentStatus Status);

