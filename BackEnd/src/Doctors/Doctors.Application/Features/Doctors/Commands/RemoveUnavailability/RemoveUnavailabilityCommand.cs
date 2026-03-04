using Shared.Domain.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.RemoveUnavailability;

public sealed record RemoveUnavailabilityCommand(
    string UserId,
    DateTime Start,
    DateTime End ) : ICommand;
