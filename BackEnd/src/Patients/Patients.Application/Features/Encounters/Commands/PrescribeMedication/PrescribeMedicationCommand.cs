using Patients.Application.Features.Encounters.Models;
using Shared.Application.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.PrescribeMedication;

public sealed record PrescribeMedicationCommand(
    string UserId,
    string EncounterId,
    string Name,
    string Dosage,
    string Instructions) : ICommand<PrescriptionCommandViewModel>;