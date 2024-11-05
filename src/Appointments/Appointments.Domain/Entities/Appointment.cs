using Appointments.Domain.Entities.Base;
using Appointments.Domain.Enums;

namespace Appointments.Domain.Entities
{
	public class Appointment : BaseEntity
	{
		public string PatientId { get; set; }
		public string DoctorId { get; set; }
		public DateTime ScheduledStartTime { get; set; }
		public DateTime ScheduledEndTime { get; set; }
		public AppointmentStatus Status { get; set; }
	}
}
