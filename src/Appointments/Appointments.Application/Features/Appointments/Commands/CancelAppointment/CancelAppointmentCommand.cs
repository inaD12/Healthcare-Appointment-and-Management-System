using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.Appointments.Commands.CancelAppointment;

public sealed record CancelAppointmentCommand(
	string AppointmentId) : ICommand;

