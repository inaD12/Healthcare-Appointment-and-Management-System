namespace Patients.Domain.Dtos;

public sealed record NoteDto(string Id, string Text, DateTime CreatedAt);
