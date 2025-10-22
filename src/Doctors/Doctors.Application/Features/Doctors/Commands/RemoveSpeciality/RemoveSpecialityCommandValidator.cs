using Doctors.Application.Features.Doctors.Commands.AddSpeciality;
using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Doctors.Commands.RemoveSpeciality;

public class RemoveSpecialityCommandValidator : AbstractValidator<RemoveSpecialityCommand>
{
	public RemoveSpecialityCommandValidator()
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
