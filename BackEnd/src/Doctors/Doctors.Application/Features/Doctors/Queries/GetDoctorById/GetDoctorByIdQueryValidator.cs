using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Doctors.Queries.GetDoctorById;

public class GetDoctorByIdQueryValidator : AbstractValidator<GetDoctorByIdQuery>
{
	public GetDoctorByIdQueryValidator()
	{
		RuleFor(q => q.Id)
		  .NotEmpty()
		  .MinimumLength(DoctorsBusinessConfiguration.ID_MIN_LENGTH)
		  .MaximumLength(DoctorsBusinessConfiguration.ID_MAX_LENGTH);
	}
}
