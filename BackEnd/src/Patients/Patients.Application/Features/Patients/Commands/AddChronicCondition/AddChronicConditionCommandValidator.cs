using FluentValidation;
using Patients.Application.Features.Patients.Commands.AddAllergy;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Patients.Commands.AddChronicCondition;

public sealed class AddChronicConditionCommandValidator 
	: AbstractValidator<AddChronicConditionCommand>
{
	public AddChronicConditionCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
		
		RuleFor(x => x.Name)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.CHRONIC_CONDITION_NAME_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.CHRONIC_CONDITION_NAME_MAX_LENGTH);
	}
}