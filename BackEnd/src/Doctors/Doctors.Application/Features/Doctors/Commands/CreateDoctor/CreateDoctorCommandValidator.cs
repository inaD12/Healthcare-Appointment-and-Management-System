using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Doctors.Commands.CreateDoctor;

public class CreateDoctorCommandValidator : AbstractValidator<CreateDoctorCommand>
{
	public CreateDoctorCommandValidator()
	{
		RuleFor(x => x.UserId)
			.NotEmpty()
			.MinimumLength(DoctorsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(DoctorsBusinessConfiguration.ID_MAX_LENGTH);

	}
}
