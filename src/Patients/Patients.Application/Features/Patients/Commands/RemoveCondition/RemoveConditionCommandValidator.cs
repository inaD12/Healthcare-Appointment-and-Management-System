using FluentValidation;
using Patients.Application.Features.Patients.Commands.RemoveAllergy;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Patients.Commands.RemoveCondition;

public sealed class RemoveConditionCommandValidator 
	: AbstractValidator<RemoveConditionCommand>
{
	public RemoveConditionCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
		
		RuleFor(x => x.ConditionId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
	}
}