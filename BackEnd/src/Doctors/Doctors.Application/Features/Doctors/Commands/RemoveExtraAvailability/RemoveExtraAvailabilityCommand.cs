using Shared.Domain.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.RemoveExtraAvailability;

public sealed record RemoveExtraAvailabilityCommand(
    string UserId,
    DateTime Start,
    DateTime End ) : ICommand;
