using FluentValidation;
using Patients.Application.Features.Encounters.Commands.AddNote;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Encounters.Commands.FinalizeEncounter;

public sealed class FinalizeEncounterCommandValidator 
	: AbstractValidator<FinalizeEncounterCommand>
{
	public FinalizeEncounterCommandValidator()
	{
		RuleFor(x => x.EncounterId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
	}
}