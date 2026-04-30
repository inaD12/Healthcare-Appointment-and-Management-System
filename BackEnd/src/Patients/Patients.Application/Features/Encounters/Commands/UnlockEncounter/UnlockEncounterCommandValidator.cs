using FluentValidation;
using Patients.Application.Features.Encounters.Commands.LockEncounter;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Encounters.Commands.UnlockEncounter;

public sealed class UnlockEncounterCommandValidator 
	: AbstractValidator<UnlockEncounterCommand>
{
	public UnlockEncounterCommandValidator()
	{
		RuleFor(x => x.EncounterId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
	}
}