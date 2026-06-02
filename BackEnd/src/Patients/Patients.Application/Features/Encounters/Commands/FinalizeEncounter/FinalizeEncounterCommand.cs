using Shared.Application.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.FinalizeEncounter;

public sealed record FinalizeEncounterCommand(
    string UserId,
    string EncounterId) : ICommand;