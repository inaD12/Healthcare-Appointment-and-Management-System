using Patients.Application.Features.Encounters.Models;
using Shared.Application.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.StartEncounter;

public sealed record StartEncounterCommand(
    string AppointmentId) : ICommand<EncounterCommandViewModel>;