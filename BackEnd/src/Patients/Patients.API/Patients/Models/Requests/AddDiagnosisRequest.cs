namespace Patients.API.Patients.Models.Requests;

public sealed record AddDiagnosisRequest(
    string IcdCode,
    string Description);