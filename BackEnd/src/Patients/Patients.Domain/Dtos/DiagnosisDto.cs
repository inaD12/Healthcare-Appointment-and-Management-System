namespace Patients.Domain.Dtos;

public sealed record DiagnosisDto(string Id, string EncounterId, string IcdCode, string Description, DateTime CreatedAt, DateTime? DeletedAt);
    