using FluentValidation;
using Patients.Domain.Utilities;

namespace Patients.Application.Features.Patients.Commands.RegisterPatient;

public sealed class RegisterPatientCommandValidator 
	: AbstractValidator<RegisterPatientCommand>
{
	public RegisterPatientCommandValidator()
	{
		RuleFor(x => x.UserId)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);

		RuleFor(x => x.FirstName)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.FIRSTNAME_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.FIRSTNAME_MAX_LENGTH);

		RuleFor(x => x.LastName)
			.NotEmpty()
			.MinimumLength(PatientsBusinessConfiguration.LASTTNAME_MIN_LENGTH)
			.MaximumLength(PatientsBusinessConfiguration.LASTNAME_MAX_LENGTH);
		
		RuleFor(x => x.BirthDate)
			.NotEmpty()
			.LessThan(DateOnly.FromDateTime(DateTime.Now));
	}
}