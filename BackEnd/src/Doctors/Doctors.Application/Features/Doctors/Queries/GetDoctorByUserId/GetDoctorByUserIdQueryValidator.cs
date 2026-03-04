using Doctors.Application.Features.Doctors.Queries.GetDoctorById;
using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Doctors.Queries.GetDoctorByUserId;

public class GetDoctorByUserIdQueryValidator : AbstractValidator<GetDoctorByIdQuery>
{
	public GetDoctorByUserIdQueryValidator()
	{
		RuleFor(q => q.Id)
		  .NotEmpty()
		  .MinimumLength(DoctorsBusinessConfiguration.ID_MIN_LENGTH)
		  .MaximumLength(DoctorsBusinessConfiguration.ID_MAX_LENGTH);
	}
}
