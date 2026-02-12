using FluentValidation;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Patients.Commands.RemoveAllergy;

public sealed class RemoveAllergyCommandValidator 
	: AbstractValidator<RemoveAllergyCommand>
{
	public RemoveAllergyCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
		
		RuleFor(x => x.AllergyId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
	}
}