namespace Patients.Application.Features.Encounters.Dtos;

public sealed record NoteDto(string Id, string Text, DateTime CreatedAt);
