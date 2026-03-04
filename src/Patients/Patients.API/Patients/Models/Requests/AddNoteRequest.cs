namespace Patients.API.Patients.Models.Requests;

public sealed record AddNoteRequest(
    string EncounterId,
    string Note);