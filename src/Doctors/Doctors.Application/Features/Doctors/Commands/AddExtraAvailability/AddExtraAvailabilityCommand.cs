using Shared.Domain.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.AddExtraAvailability;

public sealed record AddExtraAvailabilityCommand(
    string DoctorId,
    DateTime Start,
    DateTime End,
    string Reason ) : ICommand;
