using Appointments.Domain.Entities.Enums;

namespace Appointments.API.Appointments.Models.Requests;

public class RescheduleAppointmentRequest
{
	public string AppointmentID { get; set; }
	public DateTime ScheduledStartTime { get; set; }
	public AppointmentDuration Duration { get; set; }
}
