using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.LockEncounter;

public sealed record LockEncounterCommand(
    string UserId,
    string EncounterId) : ICommand;