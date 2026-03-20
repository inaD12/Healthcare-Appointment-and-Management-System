using Appointments.Domain.Utilities;
using FluentValidation;

namespace Appointments.Application.Features.Appointments.Queries.GetBookingsByDoctorAndDate;

public class GetBookingsByDoctorAndDateQueryValidator : AbstractValidator<GetBookingsByDoctorAndDateQuery>
{
	public GetBookingsByDoctorAndDateQueryValidator()
	{
		RuleFor(q => q.DoctorUserId)
		  .NotEmpty()
		  .MinimumLength(AppointmentsBusinessConfiguration.ID_MIN_LENGTH)
		  .MaximumLength(AppointmentsBusinessConfiguration.ID_MAX_LENGTH);
	}
}
