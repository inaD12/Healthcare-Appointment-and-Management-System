using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Doctors.Commands.AddSpeciality;

public class AddSpecialityCommandValidator : AbstractValidator<AddSpecialityCommand>
{
	public AddSpecialityCommandValidator()
	{
		RuleFor(x => x.UserId)
			.NotEmpty()
			.MinimumLength(DoctorsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(DoctorsBusinessConfiguration.ID_MAX_LENGTH);

		RuleFor(x => x.Speciality)
			.NotEmpty()
			.MinimumLength(DoctorsBusinessConfiguration.SPECIALITY_MIN_LENGTH)
			.MaximumLength(DoctorsBusinessConfiguration.SPECIALITY_MAX_LENGTH);
	}
}
