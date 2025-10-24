using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Entities.Enums;
using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.Appointments.Commands.RescheduleAppointment;

public sealed class RescheduleAppointmentCommand : ICommand<AppointmentCommandViewModel>
{
	public string AppointmentId { get; private set; }
	public DateTime ScheduledStartTime { get; private set; }
	public AppointmentDuration Duration { get; private set; }

	public RescheduleAppointmentCommand(
		string appointmentId,
		DateTime scheduledStartTime,
		AppointmentDuration duration)
	{
		AppointmentId = appointmentId;
		ScheduledStartTime = scheduledStartTime;
		Duration = duration;
	}
}