namespace Patients.API.Patients.Models.Requests;

public sealed record RemoveNoteRequest(
    string EncounterId,
    string NoteId);