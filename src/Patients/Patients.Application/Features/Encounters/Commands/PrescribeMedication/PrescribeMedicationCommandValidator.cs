using FluentValidation;
using Patients.Application.Features.Encounters.Commands.AddDiagnosis;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Encounters.Commands.PrescribeMedication;

public sealed class PrescribeMedicationCommandValidator 
	: AbstractValidator<PrescribeMedicationCommand>
{
	public PrescribeMedicationCommandValidator()
	{
		RuleFor(x => x.EncounterId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
		
		RuleFor(x => x.Dosage)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.PRESCRIPTION_DOSAGE_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.PRESCRIPTION_DOSAGE_MAX_LENGTH);
		
		RuleFor(x => x.Instructions)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.PRESCRIPTION_INSTRUCTIONS_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.PRESCRIPTION_INSTRUCTIONS_MAX_LENGTH);
		
		RuleFor(x => x.Name)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.PRESCRIPTION_NAME_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.PRESCRIPTION_NAME_MAX_LENGTH);
	}
}