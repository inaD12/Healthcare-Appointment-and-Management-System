using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Entities.Enums;
using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.Commands.Appointments.RescheduleAppointment;

public sealed record RescheduleAppointmentCommand(
	 string AppointmentID,
	 string userId,
	 DateTime ScheduledStartTime,
	 AppointmentDuration Duration) : ICommand<AppointmentCommandViewModel>;

