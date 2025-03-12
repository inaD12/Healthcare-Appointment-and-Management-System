using Appointments.Domain.Entities.Enums;

namespace Appointments.Application.Features.Appointments.Models;

public sealed record AppointmentQueryViewModel(
	string Id, 
	string PatientId, 
	string DoctorId, 
	DateTime ScheduledStartTime, 
	DateTime ScheduledEndTime, 
	AppointmentStatus Status);

