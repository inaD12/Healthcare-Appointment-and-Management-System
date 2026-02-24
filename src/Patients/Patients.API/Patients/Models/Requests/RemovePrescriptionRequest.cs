namespace Patients.API.Patients.Models.Requests;

public sealed record RemovePrescriptionRequest(
    string EncounterId,
    string PrescriptionId);