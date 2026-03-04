using Patients.Application.Features.Encounters.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Encounters.Commands.AddNote;

public sealed record AddNoteCommand(
    string UserId,
    string EncounterId,
    string Note) : ICommand<NoteCommandViewModel>;