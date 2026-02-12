using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.RemoveNote;

public sealed record RemoveNoteCommand(
    string EncounterId,
    string NoteId) : ICommand;