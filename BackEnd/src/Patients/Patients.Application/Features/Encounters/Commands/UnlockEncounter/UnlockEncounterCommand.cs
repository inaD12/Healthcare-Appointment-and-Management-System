using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.UnlockEncounter;

public sealed record UnlockEncounterCommand(
    string EncounterId) : ICommand;