using Appointments.Domain.Entities;
using Appointments.Domain.Enums;

namespace Appointments.Domain.DTOS;

public class AppointmentWithDetailsDTO
{
	public string AppointmentId { get; set; }
	public DateTime ScheduledStartTime { get; set; }
	public DateTime ScheduledEndTime { get; set; }
	public AppointmentStatus Status { get; set; }
	public string DoctorEmail { get; set; }
	public string PatientEmail { get; set; }
	public string DoctorId { get; set; }
	public string PatientId { get; set; }
	public Appointment Appointment { get; set; }
}
