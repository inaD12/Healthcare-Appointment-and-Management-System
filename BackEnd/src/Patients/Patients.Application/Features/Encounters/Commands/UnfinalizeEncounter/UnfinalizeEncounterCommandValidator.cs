using FluentValidation;
using Patients.Application.Features.Encounters.Commands.LockEncounter;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Encounters.Commands.UnfinalizeEncounter;

public sealed class UnfinalizeEncounterCommandValidator 
	: AbstractValidator<UnfinalizeEncounterCommand>
{
	public UnfinalizeEncounterCommandValidator()
	{
		RuleFor(x => x.EncounterId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
	}
}