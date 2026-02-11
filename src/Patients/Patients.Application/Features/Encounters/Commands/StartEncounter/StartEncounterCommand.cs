using Patients.Application.Features.Encounters.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.StartEncounter;

public sealed record StartEncounterCommand(
    string PatientId,
    string DoctorId,
    string AppointmentId) : ICommand<EncounterCommandViewModel>;