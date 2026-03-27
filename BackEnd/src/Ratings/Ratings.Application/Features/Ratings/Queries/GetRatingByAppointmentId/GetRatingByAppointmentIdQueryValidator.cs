using FluentValidation;
using Ratings.Domain.Utilities;

namespace Ratings.Application.Features.Ratings.Queries.GetRatingByAppointmentId;

public class GetRatingByAppointmentIdQueryValidator : AbstractValidator<GetRatingByAppointmentIdQuery>
{
	public GetRatingByAppointmentIdQueryValidator()
	{
		RuleFor(q => q.AppointmentId)
		  .NotEmpty()
		  .MinimumLength(RatingsBusinessConfiguration.ID_MIN_LENGTH)
		  .MaximumLength(RatingsBusinessConfiguration.ID_MAX_LENGTH);
	}
}
