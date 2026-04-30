using Appointments.Domain.Utilities;
using FluentValidation;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentsByDoctorAndDate;

public class GetAppointmentsByDoctorAndDateQueryValidator : AbstractValidator<GetAppointmentsByDoctorAndDateQuery>
{
	public GetAppointmentsByDoctorAndDateQueryValidator()
	{
		RuleFor(q => q.DoctorUserId)
		  .NotEmpty()
		  .MinimumLength(AppointmentsBusinessConfiguration.ID_MIN_LENGTH)
		  .MaximumLength(AppointmentsBusinessConfiguration.ID_MAX_LENGTH);

		RuleFor(q => q.StartDate)
			.LessThanOrEqualTo(q => q.EndDate);

		RuleFor(q => q.EndDate)
			.GreaterThanOrEqualTo(q => q.StartDate);
	}
}
