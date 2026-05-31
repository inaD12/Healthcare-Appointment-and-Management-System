using Appointments.Domain.Enums;

namespace Appointments.API.Appointments.Models.Requests;

public sealed record CreateAppointmentByAdminRequest(
	string DoctorUserId,
	string PatientUserId,
	DateTime ScheduledStartTime,
	AppointmentDuration Duration
);
