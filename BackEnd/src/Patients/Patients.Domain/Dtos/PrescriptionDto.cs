namespace Patients.Domain.Dtos;

public sealed record PrescriptionDto(string Id, string MedicationName, string Dosage, string Instructions);
