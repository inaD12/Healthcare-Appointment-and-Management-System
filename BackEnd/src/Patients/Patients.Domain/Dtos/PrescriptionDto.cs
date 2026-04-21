namespace Patients.Domain.Dtos;

public sealed record PrescriptionDto(string Id, string EncounterId, string Name, string Dosage, string Instructions, DateTime CreatedAt, DateTime? DeletedAt);
