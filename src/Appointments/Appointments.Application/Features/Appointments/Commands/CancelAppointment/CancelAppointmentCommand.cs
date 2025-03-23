using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.Commands.Appointments.CancelAppointment;

public sealed record CancelAppointmentCommand(
	string AppointmentId,
	string userId) : ICommand;

