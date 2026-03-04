using FluentValidation;
using Ratings.Application.Features.Ratings.Commands.AddRating;
using Ratings.Domain.Utilities;

namespace Ratings.Application.Features.RateableAppointments.Commands.AddAppointment;

public class AddAppointmentCommandValidator : AbstractValidator<AddAppointmentCommand>
{
	public AddAppointmentCommandValidator()
	{
		RuleFor(x => x.AppointmentId)
			.NotEmpty()
			.MinimumLength(RatingsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(RatingsBusinessConfiguration.ID_MAX_LENGTH);

		RuleFor(x => x.DoctorId)
			.NotEmpty()
			.MinimumLength(RatingsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(RatingsBusinessConfiguration.ID_MAX_LENGTH);
		
		RuleFor(x => x.PatientId)
			.NotEmpty()
			.MinimumLength(RatingsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(RatingsBusinessConfiguration.ID_MAX_LENGTH);
	}
}
