using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Entities.Enums;
using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.Appointments.Commands.RescheduleAppointment;

public sealed record RescheduleAppointmentCommand(
	string AppointmentId,
	DateTime ScheduledStartTime,
	AppointmentDuration Duration) : ICommand<AppointmentCommandViewModel>;