using Appointments.Domain.Enums;

namespace Appointments.API.Appointments.Models.Requests;

public class CreateAppointmentRequest
{
	public string PatientEmail { get; set; }
	public string DoctorEmail { get; set; }
	public DateTime ScheduledStartTime { get; set; }
	public AppointmentDuration Duration { get; set; }
}
