using Appointments.Domain.Enums;

namespace Appointments.Domain.DTOS.Request;

public class RescheduleAppointmentDTO
{
	public string AppointmentID { get; set; }
	public DateTime ScheduledStartTime { get; set; }
	public AppointmentDuration Duration { get; set; }
}
