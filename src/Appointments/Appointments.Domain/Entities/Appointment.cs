using Appointments.Domain.Enums;
using Shared.Domain.Entities.Base;

namespace Appointments.Domain.Entities;

public class Appointment : BaseEntity
{
	public Appointment(string id, string patientId, string doctorId, DateTime scheduledStartTime, DateTime scheduledEndTime, AppointmentStatus status)
	{
		Id = id;
		PatientId = patientId;
		DoctorId = doctorId;
		ScheduledStartTime = scheduledStartTime;
		ScheduledEndTime = scheduledEndTime;
		Status = status;
	}
	public string PatientId { get; set; }
	public string DoctorId { get; set; }
	public DateTime ScheduledStartTime { get; set; }
	public DateTime ScheduledEndTime { get; set; }
	public AppointmentStatus Status { get; set; }
}
