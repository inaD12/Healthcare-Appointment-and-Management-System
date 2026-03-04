using FluentValidation;
using Ratings.Domain.Utilities;

namespace Ratings.Application.Features.Ratings.Queries.GetRatingById;

public class GetRatingByIdQueryValidator : AbstractValidator<GetRatingByIdQuery>
{
	public GetRatingByIdQueryValidator()
	{
		RuleFor(q => q.Id)
		  .NotEmpty()
		  .MinimumLength(RatingsBusinessConfiguration.ID_MIN_LENGTH)
		  .MaximumLength(RatingsBusinessConfiguration.ID_MAX_LENGTH);
	}
}
