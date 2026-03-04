namespace Patients.API.Patients.Models.Requests;

public sealed record RemoveDiagnosisRequest(
    string EncounterId,
    string DiagnosisId);