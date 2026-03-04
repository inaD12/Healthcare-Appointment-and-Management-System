using FluentValidation;
using Patients.Application.Features.Encounters.Commands.RemoveNote;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Encounters.Commands.RemoveDiagnosis;

public sealed class RemoveDiagnosisCommandValidator 
	: AbstractValidator<RemoveDiagnosisCommand>
{
	public RemoveDiagnosisCommandValidator()
	{
		RuleFor(x => x.EncounterId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
		
		RuleFor(x => x.DiagnosisId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
	}
}