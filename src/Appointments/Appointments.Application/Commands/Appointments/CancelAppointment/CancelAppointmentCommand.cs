using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Appoints.Commands.CancelAppointment;

public sealed record CancelAppointmentCommand(
	string AppointmentId) : ICommand;

