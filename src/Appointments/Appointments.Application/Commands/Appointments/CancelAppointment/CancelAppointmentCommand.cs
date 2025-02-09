using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Commands.Appointments.CancelAppointment;

public sealed record CancelAppointmentCommand(
	string AppointmentId) : ICommand;

