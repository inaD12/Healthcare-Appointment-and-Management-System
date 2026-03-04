using FluentValidation;
using Patients.Application.Features.Encounters.Commands.AddNote;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Encounters.Commands.RemoveNote;

public sealed class RemoveNoteCommandValidator 
	: AbstractValidator<RemoveNoteCommand>
{
	public RemoveNoteCommandValidator()
	{
		RuleFor(x => x.EncounterId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
		
		RuleFor(x => x.NoteId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
	}
}