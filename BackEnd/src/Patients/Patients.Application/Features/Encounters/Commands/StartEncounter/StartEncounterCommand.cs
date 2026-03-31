using Patients.Application.Features.Encounters.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.StartEncounter;

public sealed record StartEncounterCommand(
    string AppointmentId) : ICommand<EncounterCommandViewModel>;