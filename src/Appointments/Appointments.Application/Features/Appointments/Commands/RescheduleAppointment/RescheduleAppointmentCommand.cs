using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Entities.Enums;
using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.Commands.Appointments.RescheduleAppointment;

public sealed class RescheduleAppointmentCommand : ICommand<AppointmentCommandViewModel>
{
	public string AppointmentId { get; private set; }
	public string UserId { get; private set; }
	public DateTime ScheduledStartTime { get; private set; }
	public AppointmentDuration Duration { get; private set; }
	public bool IsAdmin { get; set; }

	public RescheduleAppointmentCommand(
		string appointmentID,
		string userId,
		DateTime scheduledStartTime,
		AppointmentDuration duration,
		bool isAdmin = false)
	{
		AppointmentId = appointmentID;
		UserId = userId;
		ScheduledStartTime = scheduledStartTime;
		Duration = duration;
		IsAdmin = isAdmin;
	}
}