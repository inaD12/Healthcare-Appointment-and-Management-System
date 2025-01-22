using Appointments.Domain.Enums;

namespace Appointments.Domain.DTOS.Request
{
	public class CreateAppointmentDTO
	{
		public string PatientEmail { get; set; }
		public string DoctorEmail { get; set; }
		public DateTime ScheduledStartTime { get; set; }
		public AppointmentDuration Duration { get; set; }
	}
}
