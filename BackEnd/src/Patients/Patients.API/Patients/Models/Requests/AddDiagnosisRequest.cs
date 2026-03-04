namespace Patients.API.Patients.Models.Requests;

public sealed record AddDiagnosisRequest(
    string EncounterId,
    string IcdCode,
    string Description);