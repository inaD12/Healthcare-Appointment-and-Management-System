namespace Patients.Domain.Dtos;

public sealed record NoteDto(string Id, string EncounterId, string Text, DateTime CreatedAt, DateTime? DeletedAt);
