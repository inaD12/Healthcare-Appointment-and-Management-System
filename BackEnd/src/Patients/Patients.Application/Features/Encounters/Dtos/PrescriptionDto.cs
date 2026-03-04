namespace Patients.Application.Features.Encounters.Dtos;

public sealed record PrescriptionDto(string Id, string MedicationName, string Dosage, string Instructions);
