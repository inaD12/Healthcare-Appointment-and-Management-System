using Appointments.Domain.Enums;

namespace Appointments.Domain.Entities
{
	public class Appointment
	{
		public string AppointmentId { get; set; }
		public string PatientId { get; set; }
		public string DoctorId { get; set; }
		public DateTime ScheduledTime { get; set; }
		public DateTime? RescheduledTime { get; set; }
		public AppointmentStatus Status { get; set; }
	}
}
