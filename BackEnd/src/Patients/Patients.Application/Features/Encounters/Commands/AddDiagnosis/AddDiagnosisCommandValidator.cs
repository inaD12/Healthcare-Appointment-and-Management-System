using FluentValidation;
using Patients.Application.Features.Encounters.Commands.AddNote;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Encounters.Commands.AddDiagnosis;

public sealed class AddDiagnosisCommandValidator 
	: AbstractValidator<AddDiagnosisCommand>
{
	public AddDiagnosisCommandValidator()
	{
		RuleFor(x => x.EncounterId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
		
		RuleFor(x => x.IcdCode)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ICD_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ICD_MAX_LENGTH);
		
		RuleFor(x => x.Description)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.DIAGNOSIS_DESCTIPTION_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.DIAGNOSIS_DESCTIPTION_MAX_LENGTH);
	}
}