using FluentValidation;
using Patients.Application.Features.Encounters.Commands.FinalizeEncounter;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Encounters.Commands.LockEncounter;

public sealed class LockEncounterCommandValidator 
	: AbstractValidator<LockEncounterCommand>
{
	public LockEncounterCommandValidator()
	{
		RuleFor(x => x.EncounterId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
	}
}