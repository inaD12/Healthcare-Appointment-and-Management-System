using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.RemovePrescription;

public sealed record RemovePrescriptionCommand(
    string EncounterId,
    string PrescriptionId) : ICommand;