using FluentValidation;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Patients.Commands.DeletePatient;

public sealed class DeletePatientCommandValidator 
	: AbstractValidator<DeletePatientCommand>
{
	public DeletePatientCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);
	}
}