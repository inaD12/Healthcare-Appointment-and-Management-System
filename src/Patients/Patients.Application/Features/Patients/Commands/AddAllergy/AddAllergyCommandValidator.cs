using FluentValidation;
using Patients.Application.Features.Patients.Commands.DeletePatient;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Patients.Commands.AddAllergy;

public sealed class AddAllergyCommandValidator 
	: AbstractValidator<AddAllergyCommand>
{
	public AddAllergyCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
		
		RuleFor(x => x.Reaction)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.REACTION_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.REACTION_MAX_LENGTH);
		
		RuleFor(x => x.Substance)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.SUBSTANCE_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.SUBSTANCE_MAX_LENGTH);
	}
}