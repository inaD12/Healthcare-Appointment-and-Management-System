namespace Patients.API.Patients.Models.Requests;

public sealed record PrescribeMedicationRequest(
    string EncounterId,
    string Name,
    string Dosage,
    string Instructions);