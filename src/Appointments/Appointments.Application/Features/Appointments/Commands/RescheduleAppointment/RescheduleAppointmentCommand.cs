using Appointments.Domain.Enums;
using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.Commands.Appointments.RescheduleAppointment;

public sealed record RescheduleAppointmentCommand(
	 string AppointmentID,
	 DateTime ScheduledStartTime,
	 AppointmentDuration Duration) : ICommand;

