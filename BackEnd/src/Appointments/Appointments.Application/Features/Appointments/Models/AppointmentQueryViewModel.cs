using Appointments.Domain.Entities.Enums;
using Shared.Domain.Entities.ValueObjects;

namespace Appointments.Application.Features.Appointments.Models;

public sealed record AppointmentQueryViewModel(
	string Id, 
	string PatientId, 
	string DoctorId,
	DateTimeRange Duration, 
	AppointmentStatus Status);

