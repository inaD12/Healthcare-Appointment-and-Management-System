using FluentValidation;
using Ratings.Application.Features.Ratings.Queries.GetRatingById;
using Ratings.Domain.Utilities;

namespace Ratings.Application.Features.Ratings.Queries.GetDoctorRatingStatsById;

public class GetDoctorRatingStatsByIdQueryValidator : AbstractValidator<GetDoctorRatingStatsByIdQuery>
{
	public GetDoctorRatingStatsByIdQueryValidator()
	{
		RuleFor(q => q.Id)
		  .NotEmpty()
		  .MinimumLength(RatingsBusinessConfiguration.ID_MIN_LENGTH)
		  .MaximumLength(RatingsBusinessConfiguration.ID_MAX_LENGTH);
	}
}
