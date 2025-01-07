using Appointments.Domain.Enums;
using Contracts.Abstractions.Messaging;

namespace Appointments.Application.Appoints.Commands.RescheduleAppointment;

public sealed record RescheduleAppointmentCommand(
	 string AppointmentID,
	 DateTime ScheduledStartTime,
	 AppointmentDuration Duration) : ICommand;

