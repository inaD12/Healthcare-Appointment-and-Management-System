namespace Patients.Domain.Dtos;

public sealed record AddendumDto(string Id, string EncounterId, string Text, DateTime CreatedAt);
