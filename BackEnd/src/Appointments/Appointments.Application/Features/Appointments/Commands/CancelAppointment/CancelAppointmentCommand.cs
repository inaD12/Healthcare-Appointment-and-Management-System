using Shared.Application.Abstractions.Messaging;

namespace Appointments.Application.Features.Appointments.Commands.CancelAppointment;

public sealed record CancelAppointmentCommand(
	string AppointmentId,
	bool IsAdmin = false) : ICommand;

