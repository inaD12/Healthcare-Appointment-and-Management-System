using Shared.Application.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.UnfinalizeEncounter;

public sealed record UnfinalizeEncounterCommand(
    string EncounterId) : ICommand;