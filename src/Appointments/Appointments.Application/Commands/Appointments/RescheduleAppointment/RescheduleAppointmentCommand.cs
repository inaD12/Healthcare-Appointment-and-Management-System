using Appointments.Domain.Enums;
using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Appoints.Commands.RescheduleAppointment;

public sealed record RescheduleAppointmentCommand(
	 string AppointmentID,
	 DateTime ScheduledStartTime,
	 AppointmentDuration Duration) : ICommand;

