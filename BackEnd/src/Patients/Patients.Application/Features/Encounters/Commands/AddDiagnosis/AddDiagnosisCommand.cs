using Patients.Application.Features.Encounters.Models;
using Shared.Application.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.AddDiagnosis;

public sealed record AddDiagnosisCommand(
    string UserId,
    string EncounterId,
    string IcdCode,
    string Description) : ICommand<DiagnosisCommandViewModel>;