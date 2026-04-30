using FluentValidation;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Encounters.Commands.StartEncounter;

public sealed class StartEncounterCommandValidator 
	: AbstractValidator<StartEncounterCommand>
{
	public StartEncounterCommandValidator()
	{
		RuleFor(x => x.AppointmentId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
	}
}