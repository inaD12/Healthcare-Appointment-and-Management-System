using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Appointments.Commands.CompleteAppointments;

public sealed record CompleteAppointmentsCommand() : ICommand;
