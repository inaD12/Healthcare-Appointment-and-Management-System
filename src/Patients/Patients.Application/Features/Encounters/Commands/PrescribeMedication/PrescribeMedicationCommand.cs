using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.PrescribeMedication;

public sealed record PrescribeMedicationCommand(
    string UserId,
    string EncounterId,
    string Name,
    string Dosage,
    string Instructions) : ICommand;