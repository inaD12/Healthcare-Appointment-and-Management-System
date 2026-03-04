using FluentValidation;
using Patients.Application.Features.Encounters.Commands.StartEncounter;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Encounters.Commands.AddNote;

public sealed class AddNoteCommandValidator 
	: AbstractValidator<AddNoteCommand>
{
	public AddNoteCommandValidator()
	{
		RuleFor(x => x.EncounterId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
		
		RuleFor(x => x.Note)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.CLINICAL_NOTE_TEXT_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.CLINICAL_NOTE_TEXT_MAX_LENGTH);
	}
}