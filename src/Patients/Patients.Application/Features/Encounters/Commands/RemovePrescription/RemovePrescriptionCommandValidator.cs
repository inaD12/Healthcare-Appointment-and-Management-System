using FluentValidation;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Encounters.Commands.RemovePrescription;

public sealed class RemovePrescriptionCommandValidator 
	: AbstractValidator<RemovePrescriptionCommand>
{
	public RemovePrescriptionCommandValidator()
	{
		RuleFor(x => x.EncounterId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
		
		RuleFor(x => x.PrescriptionId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
	}
}