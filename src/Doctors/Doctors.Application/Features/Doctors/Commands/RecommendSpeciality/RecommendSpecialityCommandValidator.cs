using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Doctors.Commands.RecommendSpeciality;

public class RecommendSpecialityCommandValidator : AbstractValidator<RecommendSpecialityCommand>
{
	public RecommendSpecialityCommandValidator()
	{
		RuleFor(x => x.Symptoms)
			.NotEmpty()
			.MinimumLength(DoctorsBusinessConfiguration.SYMPTOMS_MIN_LENGTH)
			.MaximumLength(DoctorsBusinessConfiguration.SYMPTOMS_MAX_LENGTH);
	}
}
