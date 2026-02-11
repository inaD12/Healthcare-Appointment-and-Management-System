using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.AddAddendum;

public sealed record AddAddendumCommand(
    string EncounterId,
    string Note) : ICommand;