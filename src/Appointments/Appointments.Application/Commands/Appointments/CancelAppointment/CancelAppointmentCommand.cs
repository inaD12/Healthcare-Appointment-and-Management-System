using Contracts.Abstractions.Messaging;

namespace Appointments.Application.Appoints.Commands.CancelAppointment;

public sealed record CancelAppointmentCommand(
	string appointmentId) : ICommand;

