using Shared.Domain.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.AddUnavailability;

public sealed record AddUnavailabilityCommand(
    string UserId,
    DateTime Start,
    DateTime End,
    string Reason ) : ICommand;
