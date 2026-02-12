using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.RemoveDiagnosis;

public sealed record RemoveDiagnosisCommand(
    string EncounterId,
    string DiagnosisId) : ICommand;