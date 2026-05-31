namespace Patients.Application.Features.Encounters.Dtos;

public sealed record NoteDto(string Id, string EncounterId, string Text, DateTime CreatedAt, DateTime? DeletedAt);
