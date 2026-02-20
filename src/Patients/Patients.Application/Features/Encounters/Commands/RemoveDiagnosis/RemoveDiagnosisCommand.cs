using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.RemoveDiagnosis;

public sealed record RemoveDiagnosisCommand(
    string UserId,
    string EncounterId,
    string DiagnosisId) : ICommand;