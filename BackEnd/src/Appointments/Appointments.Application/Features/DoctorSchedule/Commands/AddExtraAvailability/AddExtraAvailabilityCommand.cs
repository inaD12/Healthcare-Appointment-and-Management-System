using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.DoctorSchedule.Commands.AddExtraAvailability;

public sealed record AddExtraAvailabilityCommand(
    string DoctorId,
    DateTime Start,
    DateTime End,
    string Reason ) : ICommand;
