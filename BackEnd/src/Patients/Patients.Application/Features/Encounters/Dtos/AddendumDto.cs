namespace Patients.Application.Features.Encounters.Dtos;

public sealed record AddendumDto(string Id, string EncounterId, string Text, DateTime CreatedAt, DateTime? DeletedAt);
